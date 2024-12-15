//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ReceptionComputer : MonoBehaviour, InteractableObjects
//{
//    public ReceptionCamera receptionCamScript;
//    public Vector3 playerStartPosition;
//    public Quaternion playerStartRotation;
//    public Camera ReceptionCounterCam;
//    public float initialFieldOfView;
//    public Vector3 initialPos;
//    public Quaternion initialRotation;
//    public float focusCoustomerFieldOfView;

//    public void OnHoverItems()
//    {
//        UIController.instance.DisplayHoverObjectName("Tap To Serve Customers!", true, HoverInstructionType.General);
//        UIController.instance.OnChangeInteraction(0, true);
//        if (GetComponent<Outline>())
//        {
//            GetComponent<Outline>().enabled = true;
//        }

//    }

//    public void OnInteract()
//    {
//        PlayerEntersCounter();

//        UIController.instance.DisplayHoverObjectName("Motel Reception", false);
//        if (GetComponent<Outline>())
//        {
//            GetComponent<Outline>().enabled = false;
//        }
//    }


//    void PlayerEntersCounter()
//    {

//        if (CustomerManager.instance.receptionist!=null)
//        {
//            CustomerManager.instance.receptionist.GetComponent<Receptionist>().ChangeEmployeeState(Employeestate.MovingToRestingPlace);
//          //  UIController.instance.DisplayInstructions("Fire Receptionist to interact with Computer");
//          //  return;
//        }
//        playerStartRotation = Controlsmanager.instance.transform.GetChild(1).transform.rotation;
//        playerStartPosition = Controlsmanager.instance.transform.GetChild(1).transform.position;

//        ReceptionCounterCam.transform.position = playerStartPosition;
//        ReceptionCounterCam.transform.rotation = playerStartRotation;
//        Controlsmanager.instance.ActivateControls(false, 0);
//        EnableReceptionCamera(true);
//        UIController.instance.EnableFPSPanel(false);

//        receptionCamScript.SetCameraState(ReceptionCounterCameraState.Enter, OnCompleteEnterToCounter);
//    }


//    void OnCompleteEnterToCounter()
//    {
//        ReceptionUIManager.Instance.EnableReceptionRoomManagementPanel();
//        UIController.instance.EnableCashContainers(true);
//    }

//    public void PlayerLeavesCounter()
//    {

//        ReceptionUIManager.Instance.DisableReceptionRoomManagementPanel();
//        UIController.instance.EnableCashContainers(false);

//        receptionCamScript.SetCameraState(ReceptionCounterCameraState.Exit, OnCompleteLeaveToCounter);
        
//    }

//    void OnCompleteLeaveToCounter()
//    {
//        Controlsmanager.instance.ActivateControls(true, 4.5f);
//        EnableReceptionCamera(false);
//        UIController.instance.EnableFPSPanel(true);
//        receptionCamScript.receptionCounterCameraState = ReceptionCounterCameraState.Default;
//        if (CustomerManager.instance.receptionist != null)
//        {
//            CustomerManager.instance.receptionist.GetComponent<Receptionist>().ChangeEmployeeState(Employeestate.MovingToWorkPlace);
//        }
//    }


//    void EnableReceptionCamera(bool state)
//    {
//        ReceptionCounterCam.gameObject.SetActive(state);
//    }

//    public void TurnOffOutline()
//    {
//        if (GetComponent<Outline>())
//        {
//            GetComponent<Outline>().enabled = false;
//        }
//    }
//}
