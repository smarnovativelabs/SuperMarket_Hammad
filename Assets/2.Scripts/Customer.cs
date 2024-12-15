using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Customer : ItemPickandPlace, InteractableObjects
{
    public Transform[] officePosition;
    public Transform roomPosition;
    public CustomerState state;
    public bool hasReachedOffice;
    public Transform lastNodePosition;
    public int StayTime;
    public int RoomRent = 100;
    public AudioClip cashSound;
    Animator anim;
    NavMeshAgent agent;
    
    public void OnHoverItems()
    {
        UIController.instance.DisplayHoverObjectName("Customer", true);
        UIController.instance.OnChangeInteraction(0, true);

        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }

    public void OnInteract()
    {
        var toolPicked = GameController.instance.currentPickedTool;
        var itemPick = GameController.instance.currentPicketItem;

        if (itemPick == null && toolPicked == null)
        {
            if (state == CustomerState.WaitingInOffice)
            {
                UIController.instance.customerRequestPanel.SetActive(true);
            }
        }
        else
        {
            UIController.instance.DisplayInstructions("Through Item");
        }
    }

    public void TurnOffOutline()
    {
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }

    public void OnPressTakeRoom()
    {
        UIController.instance.customerRequestPanel.SetActive(false);
    }
    public void InitilizeCustomer()
    {
        state = CustomerState.Walking;
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        MoveCharacter();
    }

    public void Update()
    {
        if (state == CustomerState.Walking)
        {
            if (Vector3.Distance(transform.position, officePosition[0].position) < 1f)
            {
                state = CustomerState.WaitingInOffice;
                hasReachedOffice = true;
            }

            if (state == CustomerState.WaitingInOffice)
            {
                anim.SetTrigger("Discuss");
            }
        }

        if(state == CustomerState.ReadyToGoBack)
        {
            if (Vector3.Distance(transform.position, lastNodePosition.position) < 0.2f)
            {
                VehicleController.instance.ShouldLeaveparking(true, 0);
                Destroy(gameObject); 
            }
        }
    }

    public void MoveCharacter()
    {
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on the character prefab.");
            return;
        }

        agent.SetDestination(officePosition[0].position);

        // Set the bool to true since the character has reached the office
        hasReachedOffice = true;
        UIController.instance.DisplayInstructions("Customer Reaching in office");

        if (hasReachedOffice)
        {

        }

        // Trigger discuss animation
        //anim.SetTrigger("Discuss");

        // Trigger walk animation again
        //anim.SetTrigger("Walk");

        // Move character to the room using NavMeshAgent
        //agent.SetDestination(roomPosition.position);
    }

    public void GoCustomerInAssignedRoom()
    {
        anim.SetTrigger("Walk");
        state = CustomerState.InRoom;
     //   agent.SetDestination(RoomManager.instance.rooms[0].roomProperties.roomPositions.position);
        StartCoroutine(LeaveRoom());
    }

    IEnumerator LeaveRoom()
    {
        print("Leaving Room");
        yield return new WaitForSeconds(StayTime);
        state = CustomerState.ReadyToGoBack;
        agent.SetDestination(lastNodePosition.position);
        PlayerDataManager.instance.playerData.playerCash += RoomRent;
        SoundController.instance.OnPlayInteractionSound(cashSound);
        UIController.instance.DisplayInstructions(RoomRent + "$ Room Rent Collected");

        //  UIController.instance.CashText.text = PlayerDataManager.instance.playerData.playerCash.ToString();
        UIController.instance.UpdateCurrency(RoomRent);
        yield return new WaitForSeconds(5);
        VehicleController.instance.SpawnCar();
    }
}

[System.Serializable]
public enum CustomerState
{
    Walking = 0,
    Queue = 1,
    WaitingInOffice = 2,
    InRoom = 3,
    ReadyToGoBack =4
}
