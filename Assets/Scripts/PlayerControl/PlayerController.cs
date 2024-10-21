using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Unity.Burst.CompilerServices;

namespace PlayerControl {
    public class PlayerController : SerializedMonoBehaviour
    {
        [Title("�̵� ���� ����")]
        [InfoBox("�ȴ� �ӵ�")]
        public float moveSpeed = 5f;
        [DetailedInfoBox("���� �̵��� �ӵ�", "Jump ���¿��� WASD���� �� �������� �����̴� �ӵ�")]
        public float atAirSpeed = 3;
        [InfoBox("������ �������� ��")]
        public float jumpForce = 7f;
        [Title("Ground Layer")]
        [InfoBox("�� �־��ּ���")]
        public LayerMask ground;
        [Title("���� �Ǵ� ���� ����")]
        [InfoBox("�÷��̾ ������ �� �ִ� ������ �ִ� ����")]
        public float groundCheckAngle = 45f;
        [Title("Animator")]
        public Animator animator;

        //private Rigidbody rb;
        private CharacterController controller;
        private bool isGround = false;
        private bool overGroundAngle = false;

        private float _height;
        private float inputRL;
        private float inputFB;

        private Vector3 normalVector = Vector3.up;
        private Vector3 velocity;

        private void Start()
        {
            //rb = GetComponent<Rigidbody>();
            controller = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            //_height = GetComponent<CapsuleCollider>().height / 2 + 0.05f;
            _height = controller.height/2 + 0.05f;
            controller.slopeLimit = groundCheckAngle;
        }

        private void Update()
        {
            // WASD�� �Է��� �޾ƿ���
            inputRL = Input.GetAxis("Horizontal"); // Right - Left (A - D)
            inputFB = Input.GetAxis("Vertical"); // Front - Back (W - S)

            //ground check
            RaycastHit groundCheck;
            if (Physics.Raycast(transform.position, Vector3.down, out groundCheck, _height, ground))
            {
                normalVector = groundCheck.normal;//��簡 �ִ� �����̶�� ����� ���� ���� ���
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
            }
            else
            {
                isGround = false;
                overGroundAngle = false;
            }

            if(isGround && Input.GetKeyDown(KeyCode.Space))
            {
                //jump
                //rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                velocity.y = jumpForce;
            }
            //animation
            animator.SetFloat("Speed", Mathf.Abs(inputFB) + Mathf.Abs(inputRL));
            animator.SetFloat("VelocityY", velocity.y);
            animator.SetBool("IsJumping", !isGround);
        }

        // walk �ӵ��� ��ǻ�� ��翡 ���� �޶��� �� �����Ƿ� FixedUpdate���� ó���� �� �ֵ���
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
                velocity += slopeDirection * Physics.gravity.magnitude * Time.fixedDeltaTime;
            }
            //rb.AddForce(movement, ForceMode.Acceleration);
            //rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
            velocity.y += Physics.gravity.y * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
            if (isGround && velocity.y < 0) velocity.y = -3f;
        }
    } 
}
