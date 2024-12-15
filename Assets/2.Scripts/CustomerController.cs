using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerController : MonoBehaviour
{
    public GameObject[] characterPrefabs; // Assign the character prefabs in the inspector
    public GameObject currentSpawnCharacter;

    void Start()
    {
        // Initialize anything if needed
    }

    public void SpawningCharacter(Transform _position)
    {
        GameObject character = Instantiate(characterPrefabs[Random.Range(0, characterPrefabs.Length)], _position.position, Quaternion.identity);
        character.gameObject.SetActive(true);
        character.GetComponent<Customer>().InitilizeCustomer();
        currentSpawnCharacter = character;
     

    }

    // btn Ref
    //public void OnPressGoToRoom()
    //{
    //    if(RoomManager.instance.rooms[0].roomSaveable.isRoomReady)
    //    {
    //        currentSpawnCharacter.GetComponent<Customer>().GoCustomerInAssignedRoom();
    //        UIController.instance.customerRequestPanel.SetActive(false);
    //    }
    //    else
    //    {
    //        UIController.instance.DisplayInstructions("Room is Not Ready");
    //    }
    //}

    //public void PressSorryNoRoomAvail()
    //{
    //    UIController.instance.customerRequestPanel.SetActive(false);
    //}

    //public void PressHoldOn()
    //{
    //    UIController.instance.customerRequestPanel.SetActive(false);
    //}

}
