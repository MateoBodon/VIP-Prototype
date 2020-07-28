using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace VIP
{
    public class ThirdPersonMovement : MonoBehaviour
    {
        #region Variables
        //Controls the speed of the player and rate of acceleration     
        [SerializeField] float walkSpeed = 6f;
        [SerializeField] float sprintSpeed = 12f;
        [SerializeField] float speedUpRate = 5f;
        [SerializeField] float slowDownRate = 8f;
       
        //controls the speed of the descent
        [SerializeField] float gravity = -9.81f;
        //Jump height
        [SerializeField] float jumpHeight = 3f;

        //Used to check if grounded. Ground check on player/groundMask on enviroment/ground distance is radius
        [SerializeField] Transform groundCheck = null;
        [SerializeField] float groundDistance = 0.4f;
        [SerializeField] LayerMask groundMask;

        Transform mainCamera;
        CharacterController characterController;

        float speed = 6f;
        Vector3 velocity;
        bool isGrounded;
        bool isMoving;
        #endregion

        #region BuiltInMethods
        private void Awake()
        {
            //Accesses the main camera object and character controller on the player
            mainCamera = Camera.main.transform;
            characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            CursorStatus();
        }

        private void Update()
        {
            FallAndJump();
            Movement();

        }

        
        #endregion

        #region CustomMethods
        private void CursorStatus()
        {
            // Will disable the cursor and make it not visible to the player during runtime
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void FallAndJump()
        {
            //creates a sphere around the bottom of the player to see if they are touching the ground
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            //if player is grounded
            if (isGrounded && velocity.y < 0)
            {
                //Set to -2 because it prevents the player from slightly floating above the ground
                velocity.y = -2f;
            }

            //jump
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                //To find the velocity needed to jump a certain height velocity = Sqrt(height * -2 * gravity)
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }

            //adds gravity to the velocity
            velocity.y += gravity * Time.deltaTime;

            //moves the character with by the velocity and frame rate independent *Multiplied by time.deltatime twice because time is squared
            characterController.Move(velocity * Time.deltaTime);

        }

        private void Movement()
        {

            //Input axis
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            //Tells the direction of the key strokes -Horizontal/Right and Left/X axis - Vertical/Up and down/Z axis  * Y left zero so player doesnt float up
            //*Nomalized so diagonal movements are not faster
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            //Determines if the player should speed up or slow down
            Sprint();

            //Tells if the player is moving
            if (direction.magnitude >= .1f) isMoving = true;
            else isMoving = false;
            
            if (isMoving)
            {              
                //makes movement dependant on main camera and not world space
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;

                // sets the rotation of the player to the forward axis of the main camera
                transform.rotation = Quaternion.Euler(0f, mainCamera.eulerAngles.y, 0f);

                // tells the direction the player needs to go
                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

                //moves the character based on the moveDir, speed and multiplied by time.delta time to be framerate independent
                characterController.Move(moveDir.normalized * speed * Time.deltaTime);
            }
            //Tells if the player is idle
            else if (direction.magnitude == 0)
            {
                //Will rotate the players forward axis to the cameras forward axis to keep the player rotation with the main camera
                transform.rotation = Quaternion.Euler(0f, mainCamera.eulerAngles.y, 0f);
            }
        }

         void Sprint()
        {
            //Speeds up the player to the sprintspeed by a fixed rate
            if (Input.GetButton("Sprint") && isMoving)
            {
                speed = Mathf.MoveTowards(speed, sprintSpeed, speedUpRate * Time.deltaTime);
            }
            //slows down the player to the walkspeed by a fixed rate
            else if (speed != walkSpeed)
            {
                speed = Mathf.MoveTowards(speed, walkSpeed, slowDownRate * Time.deltaTime);
            }
        }
        #endregion

    }
}
