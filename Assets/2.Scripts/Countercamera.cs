using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Events;
public class Countercamera : MonoBehaviour
{
    // Camera rotation speed
    public float rotationSpeed = 1.0f;
    public float transitionSpeed = 1f;
    // Current camera state, determines behavior of the camera
    public CashCounterCameraState cashCounterCameraState;

    // Currently interacting object in the scene
    public Vector3 targetPosition;
    public Quaternion targetRotation;
    float targetFOV;
    float transitionVal = 0;
    UnityAction transitionCompleteAction;
    bool startTransition;
    private GameObject currentInteractingObj;
    [SerializeField]
    int counterId=-1;

   /* void Start()
    {
        // Set the initial field of view when the camera starts
      //  counterId=transform.parent.GetComponent<CashCounterManager>().GettCounterID();
      //  print("Current Counter Id:" + counterId);
        // SetCameraFieldOfViewForCash(SuperStoreManager.instance.GetinitialFieldOfView(counterId));
     //   StartCoroutine(InitilizeAtStaret());
    }*/

    public void InitializeData(int _id)
    {
        counterId = _id;
        SetCameraFieldOfViewForCash(SuperStoreManager.instance.GetinitialFieldOfView(counterId));
       //  print("Current Counter Id:" + counterId);

    }

    // Update is called once per frame
    void Update()
    {
        // Handle different camera states
        //switch (cashCounterCameraState)
        //{
        //    case CashCounterCameraState.Default:
        //        HandleDefaultState();
        //        break;

        //    case CashCounterCameraState.FocusCustomer:
        //        HandleFocusCustomerState();
        //        break;

        //    case CashCounterCameraState.GiveChange:
        //        HandleGiveChangeState();
        //        break;

        //    case CashCounterCameraState.SwpieCard:
        //        HandleSwipeCardState();
        //        break;

        //    case CashCounterCameraState.LeaveCounter:
        //        HandleLeaveCounterState();
        //        break;
        //}

        if (startTransition)
        {
            ApplyTransitionToCamera();
        }

        // Handle mouse click interaction with various objects
        if (Input.GetMouseButtonDown(0) && !startTransition)
        {
            print("20a");
            HandleObjectInteraction();
        }
    }
    void ApplyTransitionToCamera()
    {
        transform.position = Vector3.Lerp(transform.position, targetPosition, transitionVal);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, transitionVal);
        GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, targetFOV, transitionVal);
        transitionVal += (Time.deltaTime * transitionSpeed);
        if (transitionVal >= 1f)
        {
            transitionVal = 0f;
            GetComponent<Camera>().fieldOfView = targetFOV;
            transform.position = targetPosition;
            transform.rotation = targetRotation;
            transitionCompleteAction?.Invoke();
            transitionCompleteAction = null;
            startTransition = false;
        }
    }
    /// <summary>
    /// Handles the Default state of the camera, resetting it to its initial position and rotation.
    /// </summary>
    private void HandleDefaultState()
    {
        //SetCameraFieldOfViewForCash(CashCounterManager.instance.initialFieldOfView);
        //transform.position = Vector3.Lerp(transform.position, CashCounterManager.instance.initialPos, rotationSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, CashCounterManager.instance.initialRotation, rotationSpeed * Time.deltaTime);
        
        targetFOV = SuperStoreManager.instance.GetinitialFieldOfView(counterId);
        targetPosition = SuperStoreManager.instance.GetInitialPos(counterId);
        targetRotation = SuperStoreManager.instance.GetInitialRotation(counterId);
    }

    /// <summary>
    /// Handles the FocusCustomer state of the camera, focusing the view on the customer.
    /// </summary>
    private void HandleFocusCustomerState()
    {
        //SetCameraFieldOfViewForCash(CashCounterManager.instance.focusCoustomerFieldOfView);
        //transform.position = Vector3.Lerp(transform.position, CashCounterManager.instance.initialPos, rotationSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, CashCounterManager.instance.initialRotation, rotationSpeed * Time.deltaTime);

      //  targetFOV = CashCounterManager.instance.focusCoustomerFieldOfView;
       // targetPosition = CashCounterManager.instance.initialPos;
       // targetRotation = CashCounterManager.instance.initialRotation;
    }

    /// <summary>
    /// Handles the GiveChange state of the camera, focusing on the cash box.
    /// </summary>
    private void HandleGiveChangeState()
    {
        //SetCameraFieldOfViewForCash(CashCounterManager.instance.giveChangeFieldOfView);
        //transform.localPosition = Vector3.Lerp(transform.localPosition, CashCounterManager.instance.cashBoxPos, rotationSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, CashCounterManager.instance.cashBoxRotation, rotationSpeed * Time.deltaTime);

       // targetFOV = CashCounterManager.instance.giveChangeFieldOfView;
       // targetPosition = CashCounterManager.instance.cashBoxPos;
        //targetRotation = CashCounterManager.instance.cashBoxRotation;
    }

    /// <summary>
    /// Handles the SwpieCard state of the camera, focusing on the card swipe position.
    /// </summary>
    private void HandleSwipeCardState()
    {
        //SetCameraFieldOfViewForCash(CashCounterManager.instance.initialFieldOfView);
        //transform.localPosition = Vector3.Lerp(transform.localPosition, CashCounterManager.instance.cardPos, rotationSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, CashCounterManager.instance.swipeCardRotation, rotationSpeed * Time.deltaTime);

      //  targetFOV = CashCounterManager.instance.focusCoustomerFieldOfView;
      //  targetPosition = CashCounterManager.instance.cardPos;
       // targetRotation = CashCounterManager.instance.swipeCardRotation;
    }

    /// <summary>
    /// Handles the LeaveCounter state of the camera, moving it to the player's starting position and rotation.
    /// Once the camera reaches this position, it triggers the OnCounterLeft method.
    /// </summary>
    private void HandleLeaveCounterState()
    {
        //transform.position = Vector3.Lerp(transform.position, CashCounterManager.instance.playerStartPosition, rotationSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, CashCounterManager.instance.playerStartRotation, rotationSpeed * Time.deltaTime);

        targetFOV = SuperStoreManager.instance.GetinitialFieldOfView(counterId);
        targetPosition = SuperStoreManager.instance.GetInitialPos(counterId);
        targetRotation = SuperStoreManager.instance.GetInitialRotation(counterId);


        //// Trigger OnCounterLeft when the camera reaches the desired rotation
        //if (Quaternion.Angle(transform.rotation, CashCounterManager.instance.playerStartRotation) < 0.1f)
        //{
        //    CashCounterManager.instance.OnCounterLeft();
        //}
    }

    /// <summary>
    /// Handles the interaction with objects when the user clicks on the screen.
    /// It casts a ray from the camera and triggers the appropriate interaction based on what it hits.
    /// </summary>
    private void HandleObjectInteraction()
    {
        print("20b");
        // Generate a ray from the camera based on the mouse click position
        Ray _ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

        // Raycast to detect what the ray hits
        if (Physics.Raycast(_ray, out RaycastHit _hit))
        {
            // Handle interaction based on the type of object hit
            if (_hit.collider.gameObject.GetComponent<Currancynote>())
            {
                currentInteractingObj = _hit.collider.gameObject;
                currentInteractingObj.GetComponent<Currancynote>().OnInteract();
            }
            else if (_hit.collider.gameObject.GetComponent<Superstoreitems>())
            {
                currentInteractingObj = _hit.collider.gameObject;
                currentInteractingObj.GetComponent<Superstoreitems>().OnInteract(counterId);
            }
            else if (_hit.collider.gameObject.GetComponent<Calculatorbuttons>())
            {
                currentInteractingObj = _hit.collider.gameObject;
                currentInteractingObj.GetComponent<Calculatorbuttons>().OnInteract(counterId);
            }
            else if (_hit.collider.gameObject.GetComponent<Paymentmethod>())
            {
                print("20c");
                currentInteractingObj = _hit.collider.gameObject;
                currentInteractingObj.GetComponent<Paymentmethod>().OnInteract(counterId);
            }
        }
    }

    /// <summary>
    /// Sets the camera's field of view to the given value.
    /// </summary>
    /// <param name="fieldOfView">The desired field of view for the camera.</param>
    public void SetCameraFieldOfViewForCash(float fieldOfView)
    {
        gameObject.GetComponent<Camera>().fieldOfView = fieldOfView;
    }

    /// <summary>
    /// Changes the camera state, which controls its rotation, position, and field of view.
    /// </summary>
    /// <param name="state">The desired camera state to switch to.</param>
    public void SetCameraState(CashCounterCameraState state, UnityAction _completeAction = null)
    {
        if (cashCounterCameraState == state)
            return;
        cashCounterCameraState = state;
        transitionCompleteAction = _completeAction;
        startTransition = true;

        switch (cashCounterCameraState)
        {
            case CashCounterCameraState.Default:
                HandleDefaultState();
                break;

          /*  case CashCounterCameraState.FocusCustomer:
                HandleFocusCustomerState();
                break;

            case CashCounterCameraState.GiveChange:
                HandleGiveChangeState();
                break;

            case CashCounterCameraState.SwpieCard:
                HandleSwipeCardState();
                break;*/

            case CashCounterCameraState.LeaveCounter:
                HandleLeaveCounterState();
                break;
        }


    }
}

    /// <summary>
    /// Enum to define different camera states for the cash counter.
    /// </summary>
    public enum CashCounterCameraState
    {
        Idle,
        Default,
        FocusCustomer,
        GiveChange,
        SwpieCard,
        LeaveCounter,
    }