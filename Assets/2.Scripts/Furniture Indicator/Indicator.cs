using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Parent Class*/
public class Indicator : ItemPickandPlace
{

    public bool isOccupied;
    public GameObject placedItem;
    void Start()
    {
        //gameObject.SetActive(false);

    }
    public virtual void OnOccupyIndicator(GameObject _placedItem)
    {
        placedItem = _placedItem;
        isOccupied = true;
        gameObject.SetActive(false);
    }
    public virtual void ResetIndicator(GameObject _heldObject)
    {
        if (!isOccupied)
            return;
        if (_heldObject != null)
        {
            if (_heldObject.GetInstanceID() == placedItem.GetInstanceID())
            {
                print("Calling To remove Item");
                isOccupied = false;
                placedItem = null;
                /*if (machine != null)
                {
                    machine.GetComponent<Machines>().OnUndoPreReq(itemProps.id);
                }*/
            }
        }
        else
        {
            isOccupied = false;
            placedItem = null;
            /*if (machine != null)
            {
                machine.GetComponent<Machines>().OnUndoPreReq(itemProps.id);
            }*/
        }

    }

    public void RemoveItemFromRoom()
    {
       // RoomManager.instance.OnRemoveItemFromRoom(placedRoomIndex, indexInRoomList);
        
    }
    public virtual void EnableIndicator()
    {
        if (isOccupied)
            return;
        gameObject.SetActive(true);

    }
    public bool IsAlreadyOccupied()
    {
        return isOccupied;
    }

    public virtual void DisablePlacedItem()
    {
        if (placedItem == null)
            return;
        placedItem.SetActive(false);
        placedItem.GetComponent<Collider>().enabled = true;

    }
    public virtual void DecreaseUsedItem()
    {
        /*if (placedItem.GetComponent<InteractableObjects>())
        {
            KitchenInventory.instance.DecreaseKitchenBuyableItem(placedItem.GetComponent<InteractableObjects>().itemProps.id);
        }*/
    }
    public virtual void ChangePlacedItemCollider(bool _enable)
    {
        if (placedItem == null)
            return;
        placedItem.GetComponent<Collider>().enabled = _enable;
    }

    public virtual void ProcessItem()
    {

    }
}
