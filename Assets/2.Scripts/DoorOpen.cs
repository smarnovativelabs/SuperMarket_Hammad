using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class DoorOpen : MonoBehaviour, InteractableObjects
{

    public bool isWindow;
    public bool isOpen;
    public bool isLock;
    public bool tapOpenOnly;

    public int doorRoomId = -1;
    public float doorOpenAngle;
    public float doorCloseAngle;
    public float smooth;
    Vector3 initialDoorAngle;
    public string interactName;
    public bool isCustomerEntering = false;

    [Header("Sliding Door")]
    public bool isSlidingDoor;
    public List<Vector3> doorClosePositions;
    public List<Vector3> doorsOpenPositions;

    List<Vector3> targetPositions;
    Vector3 targetScale;
    Quaternion targetRotation;
    float transition = 0f;
    bool isInInteraction;
    void Start()
    {
        initialDoorAngle = gameObject.transform.localEulerAngles;
        isInInteraction = false;
        transition = 0f;
        targetPositions = new List<Vector3>();
    }
     
    public void TurnOffOutline()
    {
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }
    public void OnHoverItems()
    {
        //if (doorRoomId >= 0)
        //{
        //    if (RoomManager.instance.IsRoomBeingCleaned(doorRoomId))
        //    {
        //        UIController.instance.DisplayHoverObjectName("Please wait! The room is being cleaned!", true);
        //        return;
        //    }
        //}
       
        UIController.instance.DisplayHoverObjectName(interactName, true);
        if (tapOpenOnly)
        {
            UIController.instance.OnChangeInteraction(0, true);
        }
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }
    public void OnInteract()
    {
        
        if (!isLock)
        {
            InteractWithDoor(!isOpen);
        }
        else if (isLock)
        {
            UIController.instance.DisplayHoverObjectName("Locked", true);
        }
    }
    void Update()
    {
        if (!isInInteraction)
            return;
        if (isSlidingDoor)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i >= targetPositions.Count)
                {
                    print("Invalid Door Close Position");
                    continue;
                }
                transform.GetChild(i).transform.localPosition = Vector3.Lerp(transform.GetChild(i).transform.localPosition, targetPositions[i], transition);
            }
        }
        else if (isWindow)
        {
            transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, targetScale, transition);
        }
        else
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, transition);
        }
        transition += (Time.deltaTime * smooth);
        if (transition >= 1)
        {
            transition = 0f;
            isInInteraction = false;
            if (isSlidingDoor)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (i >= targetPositions.Count)
                    {
                        print("Invalid Door Close Position");
                        continue;
                    }
                    transform.GetChild(i).transform.localPosition = targetPositions[i];
                }
            }
            else if (isWindow)
            {
                transform.GetChild(0).localScale = targetScale;
            }
            else
            {
                transform.localRotation = targetRotation;
            }
            //if(!isOpen && RoomManager.instance.currentRoomNumber < 0)
            //{
            //    if (doorRoomId >= 0)
            //    {
            //        RoomManager.instance.rooms[doorRoomId].EnableRoomObjects(false);
            //    }
            //}
        }
    }
    public void InteractWithDoor(bool _open)
    {
        if (isOpen == _open)
        {
            return;
        }
        isInInteraction = true;
        isOpen = _open;
        if (isOpen)
        {
            //if (doorRoomId >= 0)
            //{
            //    RoomManager.instance.rooms[doorRoomId].EnableRoomObjects(true);
            //}
        }
        if (isSlidingDoor)
        {
            if (_open)
            {
                targetPositions = doorsOpenPositions;
            }
            else
            {
                targetPositions = doorClosePositions;
            }
        }
        else
        {
            if (isWindow)
            {
                targetScale = transform.GetChild(0).localScale;
                if (_open)
                {
                    targetScale.y = doorOpenAngle;
                }
                else
                {
                    targetScale.y = doorCloseAngle;
                }
            }
            else
            {
                if (_open)
                {
                    targetRotation= Quaternion.Euler(initialDoorAngle.x, doorOpenAngle, initialDoorAngle.z);
                }
                else
                {
                    targetRotation = Quaternion.Euler(initialDoorAngle.x, doorCloseAngle, initialDoorAngle.z);
                }
            }
        }
    }
    public void OnStayInTrigger()
    {
        //if (!isOpen)
        //{
        //    InteractWithDoor(true);
        //}
    }
    public void OnCustomerOpenDoor()
    {
        if (isLock)
            return;
        InteractWithDoor(true);
    }
    public void OnCustomerCloseDoor()
    {
        InteractWithDoor(false);
    }
    public void OnPlayerEnterTrigger(bool _open)
    {
        if (_open)
        {
            if (isLock)
                return;
        }
        InteractWithDoor(_open);
    }
}



//if (!isCustomerEntering)
//{
//    if(isLock)
//    {
//        return;
//    }
//    if (isOpen == false && Vector3.Distance(PlayerController.instance.gameObject.transform.position, gameObject.transform.position) < 4f)
//    {
//        print("Called To Open Door---" + gameObject.name);
//        isOpen = true;
//    }
//    else if (isOpen == true && Vector3.Distance(PlayerController.instance.gameObject.transform.position, gameObject.transform.position) > 4f)
//    {
//        isOpen = false;
//    }
//}




//if (isOpen) //&& !isLock)
//{
//    for (int i = 0; i < transform.childCount; i++)
//    {
//        if (i >= doorsOpenPositions.Count)
//        {
//            print("Invalid Door Close Position");
//            continue;
//        }
//        transform.GetChild(i).transform.localPosition = Vector3.Lerp(transform.GetChild(i).transform.localPosition, doorsOpenPositions[i], smooth * Time.deltaTime);
//    }
//    interactName = "Door";

//}
//else if (!isOpen)// && !isLock)
//{
//    for (int i = 0; i < transform.childCount; i++)
//    {
//        if (i >= doorClosePositions.Count)
//        {
//            print("Invalid Door Close Position");
//            continue;
//        }
//        transform.GetChild(i).transform.localPosition = Vector3.Lerp(transform.GetChild(i).transform.localPosition, doorClosePositions[i], smooth * Time.deltaTime);
//    }
//    interactName = "Open";
//}


//Door


//if (this.isOpen && !isLock)
//{
//    Quaternion targetRotation = Quaternion.Euler(initialDoorAngle.x, doorOpenAngle, initialDoorAngle.z);
//    this.transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
//    interactName = "Close";

//}
//else if (!this.isOpen)
//{
//    Quaternion targetRotation2 = Quaternion.Euler(initialDoorAngle.x, doorCloseAngle, initialDoorAngle.z);
//    this.transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, smooth * Time.deltaTime);
//    interactName = "Open";
//}

//Window

//if (this.isOpen)
//{
//    Vector3 targetScale = new Vector3(transform.GetChild(0).localScale.x, doorOpenAngle, transform.GetChild(0).localScale.z);
//    interactName = "Close";

//}
//else if (!this.isOpen && !isLock)
//{
//    Vector3 targetScale = new Vector3(transform.GetChild(0).localScale.x, doorCloseAngle, transform.GetChild(0).localScale.z);
//    this.transform.GetChild(0).localScale = Vector3.Lerp(transform.GetChild(0).localScale, targetScale, smooth * Time.deltaTime);
//    interactName = "Open";
//}