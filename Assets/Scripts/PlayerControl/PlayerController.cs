using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

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
        [InfoBox("�÷��̾�� ������ �浹���� �� �� �浹 ������ vector.up���� ������ ���ϱ� ���ؼ� ���")]
        public float groundCheckAngle = 45f;
        [Title("Animator")]
        public Animator animator;

        private Rigidbody rb;
        private bool isGround = false;

        private float _height;
        private float inputRL;
        private float inputFB;
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            _height = GetComponent<CapsuleCollider>().height / 2 + 0.05f;
        }

        private void Update()
        {
            // WASD�� �Է��� �޾ƿ���
            inputRL = Input.GetAxis("Horizontal"); // Right - Left (A - D)
            inputFB = Input.GetAxis("Vertical"); // Front - Back (W - S)

            //ground check
            RaycastHit groundCheck;
            if (Physics.Raycast(transform.position, Vector3.down, out groundCheck, _height, ground) && Vector3.Angle(groundCheck.normal, Vector3.up) < groundCheckAngle)//���� �浹 üũ(�ϰ� �� ������ ���ǰ� �΋H��)
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }

            if(isGround && Input.GetKeyDown(KeyCode.Space))
            {
                //jump
                //rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
            //animation
            animator.SetFloat("Speed", Mathf.Abs(inputFB) + Mathf.Abs(inputRL));
            animator.SetFloat("VelocityY", rb.velocity.y);
            animator.SetBool("IsJumping", !isGround);
        }

        // walk �ӵ��� ��ǻ�� ��翡 ���� �޶��� �� �����Ƿ� FixedUpdate���� ó���� �� �ֵ���
        private void FixedUpdate()
        {   
            //walk
            Vector3 movement = new Vector3(inputRL, 0, inputFB).normalized;
            movement = transform.TransformDirection(movement);
            if (isGround)
            {
                movement *= moveSpeed;

            }
            else
            {
                movement *= atAirSpeed;
            }
            //rb.AddForce(movement, ForceMode.Acceleration);
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
        }
    } 
}
