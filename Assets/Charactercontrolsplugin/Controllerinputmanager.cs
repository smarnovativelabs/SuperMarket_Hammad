
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinematicCharacterController;
using KinematicCharacterController.Examples;
using TouchControlsKit;
using static PlayerVechicle;
using UnityEngine.Audio;

namespace KinematicCharacterController.Examples
{
    public class Controllerinputmanager : MonoBehaviour
    {
        public Charactercontroller Character;
        public Camerafollow CharacterCamera;

        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";
        private Vector3 _smoothedLookInput;
        public FloatingJoystick joystick;
        private float _smoothingFactor = 0.1f; // Adjust for more or less smoothing

        public AudioSource audioSource;
        public AudioClip footstep1;
        public AudioClip footstep2;

        public float footstepInterval = 0.2f;
        private bool isWalking = false;
        private bool isFootstep1 = true;
        private float footstepTimer = 0f;
        PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();
        bool vehicleSoundPlayed;
        private void Start()
        {
            //  Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            CharacterCamera.SetFollowTransform(Character.CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            CharacterCamera.IgnoredColliders.Clear();
            CharacterCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }

        private void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //{
            //    // Cursor.lockState = CursorLockMode.Locked;
            //}
            HandleCharacterInput();
        }
        private void FixedUpdate()
        {
            if (IsPlayerWalking())
            {
                if (!isWalking)
                {
                    isWalking = true;  // Start walking
                }

                footstepTimer += Time.deltaTime;

                if (footstepTimer >= footstepInterval)
                {
                    PlayFootstepSound();
                    footstepTimer = 0f;  // Reset the timer
                }
                if (Controlsmanager.instance.playervehicleInteraction.ridingVehicle == null)
                {
                    vehicleSoundPlayed = false;
                    Controlsmanager.instance.playervehicleInteraction.audioSource.Stop();
                }
            }
            else
            {
                isWalking = false;  // Stop walking
                audioSource.Stop();
                vehicleSoundPlayed = false;
                Controlsmanager.instance.playervehicleInteraction.audioSource.Stop();
            }



        }


        private void PlayFootstepSound()
        {

            if (Controlsmanager.instance.playervehicleInteraction.ridingVehicle !=null)
                {
                    if (!vehicleSoundPlayed)
                    {
                      Controlsmanager.instance.playervehicleInteraction.audioSource.Play();
                     
                      vehicleSoundPlayed = true;
                    }

                    return;

                }
       
            // Alternate between footstep1 and footstep2
            if (isFootstep1)
            {
                audioSource.clip = footstep1;
            }
            else
            {
                audioSource.clip = footstep2;
            }

            // Play the selected footstep sound
            audioSource.Play();
            isFootstep1 = !isFootstep1;  // Alternate the footstep sound for the next step
        }


        private void LateUpdate()
        {
            // Handle rotating the camera along with physics movers
            if (CharacterCamera.RotateWithPhysicsMover && Character.Motor.AttachedRigidbody != null)
            {
                CharacterCamera.PlanarDirection = Character.Motor.AttachedRigidbody.GetComponent<PhysicsMover>().RotationDeltaFromInterpolation * CharacterCamera.PlanarDirection;
                CharacterCamera.PlanarDirection = Vector3.ProjectOnPlane(CharacterCamera.PlanarDirection, Character.Motor.CharacterUp).normalized;
            }

           
            Character.PostInputUpdate(Time.deltaTime, CharacterCamera.transform.forward);
            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = TCKInput.GetAxis("Touchpad").y;
            float mouseLookAxisRight = TCKInput.GetAxis("Touchpad").x; 
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            /*    if (Cursor.lockState != CursorLockMode.Locked)
                {
                    lookInputVector = Vector3.zero;
                }

                // Input for zooming the camera (disabled in WebGL because it can cause problems)
                float scrollInput = -Input.GetAxis(MouseScrollInput);
    #if UNITY_WEBGL
            scrollInput = 0f;
    #endif
            */
            // Apply inputs to the camera
            CharacterCamera.UpdateWithInput(Time.deltaTime, 0, lookInputVector);

            // Handle toggling zoom level
            /*  if (Input.GetMouseButtonDown(1))
              {
                  CharacterCamera.TargetDistance = (CharacterCamera.TargetDistance == 0f) ? CharacterCamera.DefaultDistance : 0f;
              }*/
        }

        private void HandleCharacterInput()
        {
            //  PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
#if UNITY_EDITOR
            characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
            characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);

#else
            characterInputs.MoveAxisForward = joystick.Vertical;
            characterInputs.MoveAxisRight = joystick.Horizontal;
#endif
            characterInputs.CameraRotation = CharacterCamera.Transform.rotation;
            characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
            characterInputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
            characterInputs.CrouchUp = Input.GetKeyUp(KeyCode.C);

            // print(characterInputs.MoveAxisForward);
            //  print(characterInputs.MoveAxisRight);
            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }


        private bool IsPlayerWalking()
        {

            if (characterInputs.MoveAxisForward!= 0 || characterInputs.MoveAxisRight!=0)
            {
              //  print("return true");
                return true;
            }
            else
            {
                return false;
              
            }
        }

        public void MakePlayerJump()
        {
            Character._jumpRequested = true;
        }
        public void StopJump()
        {
            Character._jumpRequested = false;
        }
    }
}
