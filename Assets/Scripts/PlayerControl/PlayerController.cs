using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Burst.CompilerServices;
using ground;

namespace PlayerControl {
    public class PlayerController : SerializedMonoBehaviour
    {
        [Title("이동 관련 변수")]
        [InfoBox("걷는 속도")]
        public float moveSpeed = 5f;
        [InfoBox("감속 변수: 멈췄을 때 관성이 줄어들도록")]
        public float deceleration = 8f;
        [DetailedInfoBox("공중 이동시 속도", "Jump 상태에서 WASD사용시 각 방향으로 움직이는 속도")]
        public float atAirSpeed = 3;
        [InfoBox("점프시 가해지는 힘")]
        public float jumpForce = 7f;
        [InfoBox("떨어지는 최고 속도")]
        public float highestFallSpeed = -3f;
        [InfoBox("떨어지는 속도 증가 보정값")]
        public float fallSpeedFilter = 1f;
        [Title("Ground Layer")]
        [InfoBox("잘 넣어주세요")]
        public LayerMask ground;
        [InfoBox("플레이어가 천장에서 부딫힐 수 있는 오브젝트")]
        public string groundTagName;
        [Title("측면 판단 기준 각도")]
        [InfoBox("플레이어가 서있을 수 있는 발판의 최대 각도")]
        public float groundCheckAngle = 45f;
        [Title("Animator")]
        public Animator animator;
        [Title("GroundRay")]
        [InfoBox("바닥을 판정하기 위해 쏘는 레이의 길이(0.2f 기준 scale 0.5 ~ 1.0에서 정상적으로 작동함")]
        public float groundRay = 0.2f;
        [InfoBox("소리 출력 용 - 사운드 클립 시간에 맞춰서")]
        public float maxFootStepTimer = 1f;

        private float footStepTimer;

        //private Rigidbody rb;
        private CharacterController controller;
        private bool isGround = false;
        private bool overGroundAngle = false;

        private float _height;
        private float inputRL;
        private float inputFB;

        private Vector3 normalVector = Vector3.up;
        private Vector3 velocity;

        private float sphereRadius = 0.1f;

        private bool jumpScareTriggered = false;


        private void Start()
        {
            //rb = GetComponent<Rigidbody>();
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            //_height = GetComponent<CapsuleCollider>().height / 2 + 0.05f;
            _height = controller.height*transform.localScale.y;
            controller.slopeLimit = groundCheckAngle;
            sphereRadius *= transform.localScale.y;
            Collider characterCollider = controller.GetComponent<Collider>();
            if (characterCollider != null)
            {
                Physics.IgnoreCollision(characterCollider, characterCollider, true);
            }
            footStepTimer = 0.05f;
        }

        private void Update()
        {
            // WASD로 입력을 받아오고
            inputRL = Input.GetAxisRaw("Horizontal"); // Right - Left (A - D)
            inputFB = Input.GetAxisRaw("Vertical"); // Front - Back (W - S)

            ////ground check
            //RaycastHit groundCheck;
            ////Vector3 rayPos = transform.position + Vector3.up*_height;
            //Debug.DrawRay(transform.position, Vector3.down*groundRay, Color.red);
            //Debug.DrawRay(transform.position + Vector3.up * _height, Vector3.up * groundRay, Color.blue);
            //if (Physics.SphereCast(transform.position, groundRay, Vector3.down, out groundCheck, 0f, ground))
            //{
            //    Debug.Log($"Hit: {groundCheck.collider.name}");
            //    normalVector = groundCheck.normal;//경사가 있는 발판이라면 경사의 법선 벡터 기록
            //    float angleCheck = Vector3.Angle(normalVector, Vector3.up);
            //    if (angleCheck < groundCheckAngle)
            //    {
            //        isGround = true;
            //        overGroundAngle = false;
            //    }
            //    else
            //    {
            //        isGround = false;
            //        overGroundAngle = true;
            //    }
            //}
            //else
            //{
            //    isGround = false;
            //    overGroundAngle = false;
            //}

            RaycastHit[] groundChecks;
            Debug.DrawRay(transform.position, Vector3.down * groundRay, Color.red);
            Debug.DrawRay(transform.position + Vector3.up * _height, Vector3.up * groundRay, Color.blue);
            groundChecks = Physics.SphereCastAll(
                    transform.position,    // 시작 위치
                    groundRay,          // 구의 반지름
                    Vector3.down,          // 방향
                    0.05f,             // 최대 거리
                    ground                 // 레이어 마스크
                    );
            isGround = false;
            overGroundAngle = false;

            foreach (RaycastHit hit in groundChecks)
            {
                // 자신이나 무시해야 할 충돌체를 필터링
                if (hit.collider.gameObject != gameObject)
                {
                    //Debug.Log($"Hit: {hit.collider.name}");
                    normalVector = hit.normal; // 경사 법선 벡터 기록
                    float angleCheck = Vector3.Angle(normalVector, Vector3.up);

                    if (angleCheck < groundCheckAngle)
                    {
                        isGround = true;
                        overGroundAngle = false;
                    }
                    else
                    {
                        isGround = false;
                        overGroundAngle = true;
                    }
                    break; // 가장 가까운 충돌만 처리
                }
            }

            // 충돌이 없는 경우 기본값으로 설정
            if (!isGround)
            {
                isGround = false;
                overGroundAngle = false;
            }

            if (isGround && Input.GetKeyDown(KeyCode.Space))
            {
                //jump
                //rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                velocity.y = jumpForce;
                isGround = false;
            }
            CheckFootStep();
            //animation
            animator.SetFloat("Speed", Mathf.Abs(inputFB) + Mathf.Abs(inputRL));
            animator.SetFloat("VelocityY", velocity.y);
            animator.SetBool("IsJumping", !isGround);
        }

        // walk 속도가 컴퓨터 사양에 따라 달라질 수 있으므로 FixedUpdate에서 처리할 수 있도록
        private void FixedUpdate()
        {   
            //walk
            Vector3 movement = new Vector3(inputRL, 0, inputFB).normalized;
            movement = transform.TransformDirection(movement);
            if (isGround && !overGroundAngle)// true,false
            {                
                movement *= moveSpeed;
                velocity = new Vector3(movement.x, velocity.y, movement.z);
            }
            else if(!isGround && !overGroundAngle)// false,false
            {
                movement *= atAirSpeed;
                velocity = new Vector3(movement.x, velocity.y, movement.z);
            }
            else
            {
                float angle = Vector3.Angle(normalVector, Vector3.up);
                Vector3 slopeDirection = Vector3.ProjectOnPlane(Vector3.down, normalVector).normalized;
                velocity += slopeDirection * Physics.gravity.magnitude * Time.fixedDeltaTime * fallSpeedFilter;
            }
            //rb.AddForce(movement, ForceMode.Acceleration);
            //rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

            if (inputRL == 0 && inputFB == 0 && isGround)
            {
                velocity.x = 0;
                velocity.z = 0;
            }
            if(velocity.y > highestFallSpeed)
                velocity.y += Physics.gravity.y * Time.deltaTime * fallSpeedFilter;
            if(!jumpScareTriggered)
                controller.Move(velocity * Time.deltaTime);
            if (isGround && velocity.y < 0) velocity.y = highestFallSpeed;
        }

        private AudioClip GetFootStepSFX()
        {
            AudioClip result = null;
            RaycastHit[] groundChecks = Physics.SphereCastAll(
                    transform.position,    // 시작 위치
                    groundRay,          // 구의 반지름
                    Vector3.down,          // 방향
                    0.05f,             // 최대 거리
                    ground                 // 레이어 마스크
                    );

            foreach (RaycastHit hit in groundChecks)
            {
                GameObject g = hit.collider.gameObject;
                // 자신이나 무시해야 할 충돌체를 필터링
                if (g.TryGetComponent<Ground>(out var groundComponent))
                {
                    result = groundComponent.GetFootStep();
                }
            }

            return result;
        }

        private void CheckFootStep()
        {
            AnimatorStateInfo a = animator.GetCurrentAnimatorStateInfo(0);
            if(a.IsName("Base Layer.Walking") && (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0))
            {
                footStepTimer -= Time.deltaTime;
            }
            else
            {
                footStepTimer = maxFootStepTimer;
                return;
            }

            if(footStepTimer > 0f)
            {
                return;
            }
            AudioClip footStepSFX = GetFootStepSFX();
            if (footStepSFX != null)
            {
                SoundManager.Instance.PlaySFX(footStepSFX);
                footStepTimer = maxFootStepTimer;
            }
        }

        public void WhenJumpScareTriggered()
        {
            jumpScareTriggered= true;
        }
        public void WhenJumpScareReleased()
        {
            jumpScareTriggered= false;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.CompareTag(groundTagName) && velocity.y > 0)
            {
                velocity.y = 0;
            }
        }
        //private void OnTriggerEnter(Collider other)
        //{
        //    // Trigger 되는 이상현상은 여기에서 사용하면 됩니다.
        //}
    }
}
