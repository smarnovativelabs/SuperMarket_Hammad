using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerMovement : MonoBehaviour,InteractableObjects
{
    public string customerName = "Customer";
    public float customerSpeed;
    public AudioClip cashSound;

    public GameObject customerVehicle;

    Transform[] roamPoints;
    NavMeshAgent agent;
    Animator anim;
    Transform targetPoint;
  //  Room assignedRoom;
    int traverseCount;
    public CustomerStatus customerStatus;
    public int queueIndex;
    bool isLeavningArea;
    bool customerReachedToQueue = false;
    int checkInXP=10;
    public enum CustomerStatus
    {
        Idle,
        MovingToCounter,
        InQueue,
        AtCounter,
        MovingToRoom,
        InRoom,
        Leaving
    }

    public void OnHoverItems()
    {
        UIController.instance.DisplayHoverObjectName(customerName, true);
       // UIController.instance.OnChangeInteraction(0, true);

        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }

    public void OnInteract()
    {
       /* var _toolPicked = GameController.instance.currentPickedTool;
        var _itemPick = GameController.instance.currentPicketItem;

        if (CustomerManager.instance.receptionist != null)
        {
            UIController.instance.DisplayInstructions("Fire Receptionist to interact with Customers");
            return;
        }
        if (_itemPick == null && _toolPicked == null)
        {
            if (customerStatus == CustomerStatus.AtCounter)
            {
                UIController.instance.EnableCustomerServingPanel(AssignRoom,OnRejectRoomRequest);
            }
        }
        else
        {
            UIController.instance.DisplayInstructions("Throw Item");
        }*/
    }
    public void TurnOffOutline()
    {
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }

    IEnumerator UpdateRoamPoint()
    {
        if (customerStatus == CustomerStatus.Idle)
        {
            if (roamPoints.Length < 2)
            {
                print("Not Enough Roam Points");
                yield break;
            }
            yield return new WaitForSeconds(Random.Range(1f, 4f));
            if (isLeavningArea)
                yield break;

            traverseCount++;
            if (traverseCount >= roamPoints.Length)
            {
                traverseCount = 0;
            }
            targetPoint = roamPoints[traverseCount];
            customerStatus = CustomerStatus.InRoom;
            anim.SetInteger("move", 1);
            SetAgentDestination(targetPoint.position);
        }
    }
    IEnumerator OnRoomAssigned()
    {
        customerStatus = CustomerStatus.Idle;
        anim.SetTrigger("takeKey");
        yield return new WaitForSeconds(3.1f);
        customerStatus = CustomerStatus.MovingToRoom;
        CustomerManager.instance.customersList.Remove(gameObject);
        anim.SetInteger("move", 1);
        targetPoint = roamPoints[0];
        SetAgentDestination(targetPoint.position);
        yield return new WaitForSeconds(1f);
        CustomerManager.instance.OnRemoveCustomerFromQueue();

        //if receptionist avaiable dont grant xps
        if (CustomerManager.instance.receptionist == null)
        {
            //assign xps on assigning room to the customer
            PlayerDataManager.instance.UpdateXP(checkInXP);
            UIController.instance.UpdateXP(checkInXP);
        }

    }
    IEnumerator LeaveArea()
    {
        yield return new WaitForSeconds(1f);
        customerVehicle.GetComponent<Vehicle>().OnVehicleLeavingArea();
        Destroy(gameObject);
    }
    IEnumerator MoveToCounter()
    {
        yield return new WaitForSeconds(1f);
        customerStatus = CustomerStatus.MovingToCounter;
        anim.SetInteger("move", 1);
        SetAgentDestination(targetPoint.position);

    }
    void SetAgentDestination(Vector3 _destination)
    {
        agent.enabled = true;
        agent.SetDestination(_destination);
    }
    private void Update()
    {
        
        //if (customerStatus == CustomerStatus.InRoom || customerStatus==CustomerStatus.MovingToRoom)
        //{
        //    if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f)
        //    {
        //        if (customerStatus == CustomerStatus.MovingToRoom)
        //        {
        //            if (assignedRoom != null)
        //            {
        //                assignedRoom.CustomerEntersRoom();
        //                assignedRoom.StartOccupiedTimer();
        //            }
        //        }
        //        customerStatus = CustomerStatus.Idle;
        //        anim.SetInteger("move", 0);
        //        agent.enabled = false;
        //        StartCoroutine(UpdateRoamPoint());

        //    }
        //}
         if (customerStatus == CustomerStatus.Leaving)
        {
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.5f)
            {
                customerStatus = CustomerStatus.Idle;
                anim.SetInteger("move", 0);
                agent.enabled = false;

                StartCoroutine(LeaveArea());
            }
        }
        else if (customerStatus == CustomerStatus.MovingToCounter)
        {
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
            {
            //    print("this one is"+ queueIndex +" name:"+gameObject.name);
                agent.enabled = false;
                anim.SetInteger("move", 0);
                customerStatus = queueIndex <= 0 ? CustomerStatus.AtCounter : CustomerStatus.InQueue;
                transform.position = targetPoint.position;
                transform.rotation = targetPoint.rotation;
                if (!customerReachedToQueue)
                {
                    customerReachedToQueue = true;
                    CustomerManager.instance.UpdateCustomersInWait(1);
                    UIController.instance.DisplayInstructions("New Customer At Motel Counter");
                }
            }
        }
    }
    public void SpawnNewCustomer(int _queueIndex,GameObject _vehicle, Transform _spawnPoint,Transform _targetPoint)
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = customerSpeed;
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
        customerVehicle = _vehicle;
        targetPoint = _targetPoint;
        queueIndex = _queueIndex;
        customerStatus = CustomerStatus.Idle;
        agent.enabled = false;
        StartCoroutine(MoveToCounter());
    }
    public void SpawnSavedCustomer(GameObject _vehicle,Transform _spawnPoint,bool _isOnTopFloor, Transform[] _roamPoints)
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        agent.speed = customerSpeed;
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;
        customerVehicle = _vehicle;
        roamPoints = _roamPoints;
        traverseCount = 0;
        customerStatus = CustomerStatus.Idle;
        agent.agentTypeID = _isOnTopFloor ? GetAgentId(1) : GetAgentId(0);

        agent.enabled = false;
        anim.SetInteger("move", 0);
        StartCoroutine(UpdateRoamPoint());
    }
    public void UpdateEnqueueCustomerTarget(int _queueIndex,Transform _target)
    {
        if(customerStatus==CustomerStatus.MovingToCounter || customerStatus == CustomerStatus.InQueue)
        {
            queueIndex = _queueIndex;
            anim.SetInteger("move", 1);
            targetPoint = _target;
            customerStatus = CustomerStatus.MovingToCounter;
            SetAgentDestination(targetPoint.position);
        }
    }
    public void OnRejectRoomRequest()
    {
        OnLeavingArea(-1);
        Invoke("UpdateQueueDelay", 1f);
        CustomerManager.instance.OnDeclineRoom();
    }

    public void OnRejectRoomRequestReceptionist()
    {
        OnLeavingArea(-1);
        Invoke("UpdateQueueDelay", 1f);
        CustomerManager.instance.OnDeclineRoom();
    }
    void UpdateQueueDelay()
    {
        CustomerManager.instance.OnRemoveCustomerFromQueue();

    }
    //public void AssignRoom()
    //{
    //    assignedRoom = RoomManager.instance.GetEmptyRoom();
    //    if (assignedRoom == null)
    //    {
    //        UIController.instance.DisplayInstructions("No Room Available");
    //        GameManager.instance.CallFireBase("NoRoomAvl", "noRoom", 1);
    //        UIController.instance.DisableCustomerServingPanel();
    //        return;
    //    }
    //    roamPoints = assignedRoom.AssignCustomer(customerVehicle.GetComponent<Vehicle>().customerIndex, customerVehicle.GetComponent<Vehicle>().vehicleIndex
    //        , customerVehicle.GetComponent<Vehicle>().parkingIndex, gameObject, 0.0209f);
    //    CustomerManager.instance.OnAllotRoom();
    //    UIController.instance.DisableCustomerServingPanel();
    //    agent.agentTypeID = assignedRoom.roomProperties.isOnTopFloor ? GetAgentId(1) : GetAgentId(0);
    //    StartCoroutine(OnRoomAssigned());
    //}

    //public void AssignRoomFromReception(Room _assignedRoom)
    //{
    //    assignedRoom = _assignedRoom;
    //    roamPoints = _assignedRoom.AssignCustomer(customerVehicle.GetComponent<Vehicle>().customerIndex, customerVehicle.GetComponent<Vehicle>().vehicleIndex
    //        , customerVehicle.GetComponent<Vehicle>().parkingIndex, gameObject, 0.03125f);
    //    CustomerManager.instance.OnAllotRoom();
    //    UIController.instance.DisableCustomerServingPanel();
    //    agent.agentTypeID = _assignedRoom.roomProperties.isOnTopFloor ? GetAgentId(1) : GetAgentId(0);
    //    StartCoroutine(OnRoomAssigned());
    //}

    //public void AssignRoomReceptionist()
    //{
    //    assignedRoom = RoomManager.instance.GetEmptyRoom();

    //    if (assignedRoom != null)
    //    {

    //        roamPoints = assignedRoom.AssignCustomer(customerVehicle.GetComponent<Vehicle>().customerIndex, customerVehicle.GetComponent<Vehicle>().vehicleIndex
    //            , customerVehicle.GetComponent<Vehicle>().parkingIndex, gameObject, 0.03125f);
    //        CustomerManager.instance.OnAllotRoom();
    //        UIController.instance.DisableCustomerServingPanel();
    //        agent.agentTypeID = assignedRoom.roomProperties.isOnTopFloor ? GetAgentId(1) : GetAgentId(0);
    //        StartCoroutine(OnRoomAssigned());
    //    }
    //}
    int GetAgentId(int _indexId)
    {
        if(_indexId<0 || _indexId >= NavMesh.GetSettingsCount())
        {
            Debug.LogError("Invalid Agent Index!");
            return -1;
        }
        return NavMesh.GetSettingsByIndex(_indexId).agentTypeID;
    }
    public void OnLeavingArea(int _roomNummber=-1)
    {
        isLeavningArea = true;
        customerStatus = CustomerStatus.Leaving;
        CustomerManager.instance.customersList.Remove(gameObject);
        targetPoint = customerVehicle.GetComponent<Vehicle>().customerSpawnPoint;
        anim.SetInteger("move", 1);
        SetAgentDestination(targetPoint.position);
        if (_roomNummber>=0)
        {
            UIController.instance.DisplayInstructions("Room "+_roomNummber.ToString() + " Need Cleaning");
        }
    }
}
