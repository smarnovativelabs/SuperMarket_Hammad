//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.Events;

//public class ReceptionCamera : MonoBehaviour
//{
//    // Start is called before the first frame update
//    public ReceptionComputer ReceptionComputer;
//    public float rotationSpeed = 1.0f;
//    public float transitionSpeed = 1f;
//    // Current camera state, determines behavior of the camera
//    public ReceptionCounterCameraState receptionCounterCameraState;

//    // Currently interacting object in the scene
//    Vector3 targetPosition;
//    Quaternion targetRotation;
//    float targetFOV;
//    float transitionVal = 0;
//    UnityAction transitionCompleteAction;
//    public bool startTransition;
//    private GameObject currentInteractingObj;
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (startTransition)
//        {
//            ApplyTransitionToCamera();
//        }
//    }

//    void ApplyTransitionToCamera()
//    {
//        transform.position = Vector3.Lerp(transform.position, targetPosition, transitionVal);
//        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, transitionVal);
//        GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, targetFOV, transitionVal);
//        transitionVal += (Time.deltaTime * transitionSpeed);
//        if (transitionVal >= 1f)
//        {
//            transitionVal = 0f;
//            GetComponent<Camera>().fieldOfView = targetFOV;
//            transform.position = targetPosition;
//            transform.rotation = targetRotation;
//            transitionCompleteAction?.Invoke();
//            transitionCompleteAction = null;
//            startTransition = false;
//           // print("Bool disabled");
//        }
//    }

//    public void SetCameraState(ReceptionCounterCameraState state, UnityAction _completeAction = null)
//    {
//        if (receptionCounterCameraState == state)
//            return;
//        receptionCounterCameraState = state;
//        transitionCompleteAction = _completeAction;
//        startTransition = true;
       
//        switch (receptionCounterCameraState)
//        {
//            case ReceptionCounterCameraState.Enter:
//                HandleEnterState();
//                break;

//            case ReceptionCounterCameraState.Exit:
//                HandleExitState();
//                break;

//        }


//    }

//    void HandleEnterState()
//    {
//        targetFOV = ReceptionComputer.focusCoustomerFieldOfView;
//        targetPosition = ReceptionComputer.initialPos;
//        targetRotation = ReceptionComputer.initialRotation;
//    }

//    void HandleExitState()
//    {
//        targetFOV = ReceptionComputer.initialFieldOfView;
//        targetPosition = ReceptionComputer.playerStartPosition;
//        targetRotation = ReceptionComputer.playerStartRotation;
//    }


//}

//public enum ReceptionCounterCameraState
//{
//   Default,
//   Enter,
//   Exit
//}
