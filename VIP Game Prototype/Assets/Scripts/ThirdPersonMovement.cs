using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VIP
{
    public class ThirdPersonMovement : MonoBehaviour
    {
        //Controls the speed of the player * Can be changed to simulate walking and sprinting
        [SerializeField] float speed = 6f;

        Transform mainCamera;
        CharacterController characterController;

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

        //(COULD BE MORE OPTIMIZED BUT JUST A SIMPLE SOLUTION FOR NOW)
        private void Update()
        {
            //Input axis
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            //Tells the direction of the key strokes -Horizontal/Right and Left/X axis - Vertical/Up and down/Z axis  * Y left zero so player doesnt float up
            //*Nomalized so diagonal movements are not faster
            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

            //Tells if the player is moving
            if (direction.magnitude >= .1f)
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

        private void CursorStatus()
        {
            // Will disable the cursor and make it not visible to the player during runtime
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
