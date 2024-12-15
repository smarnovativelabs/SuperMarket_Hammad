using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableItem : ItemPickandPlace, InteractableObjects,IRuntimeSpawn
{
    public void OnHoverItems()
    {
        if(GameController.instance.currentPicketItem==null && GameController.instance.currentPickedTool == null)
        {
            UIController.instance.DisplayHoverObjectName("Tap To Pick " + itemName, true, HoverInstructionType.General);
            UIController.instance.OnChangeInteraction(0, true);
            if (gameObject.GetComponent<Outline>())
            {
                gameObject.GetComponent<Outline>().enabled = true;
            }
        }
        
    }

    public void OnInteract()
    {
        var toolPicked = GameController.instance.currentPickedTool;
        var itemPick = GameController.instance.currentPicketItem;

        if (itemPick == null && toolPicked == null)
        {
            if (canPlaceDynamically)
            {
                if (itemsSavingProps.placedAreaId >= 0)
                {
                  //  RoomManager.instance.rooms[itemsSavingProps.placedAreaId].OnRemoveItem(mainCat, SubCatId, itemId);
                    itemsSavingProps.placedAreaId = -1;
                    itemsSavingProps.isPlacedRight = false;
                }
                OnStartItemPlacement();
            }
            else
            {
                SetObjectToCam();
            }
            GameController.instance.UpdateCurrentPickedItem(gameObject);
            UIController.instance.DisplayHoverObjectName("", false);

            //RoomManager.instance.EnablePlacableItemIndicater(mainCat, SubCatId);
            //if (indicator[0] != null)
            //{
            //    indicator[currentIndicator].GetComponent<Indicator>().RemoveItemFromRoom();
            //    indicator[currentIndicator].GetComponent<Indicator>().ResetIndicator(gameObject);
            //    indicator[currentIndicator].GetComponent<Indicator>().EnableIndicator();
            //    AddItemToSavingList();
            //}
            //indicator[0] = null;
        }
        else
        {
            UIController.instance.DisplayInstructions("Item is already Selected");
        }
    }
    public override void ThrowPickedObjects()
    {
        if (canPlaceDynamically)
        {
            gameObject.transform.parent = GameController.instance.parentOFPickedObj.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<BoxCollider>())
                    transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
            }
            Collider[] _colliders = GetComponents<Collider>();
            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].enabled = false;
            }
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }
            transform.localPosition = pickedPosition;
            transform.localRotation = Quaternion.Euler(pickedAngle);
            transform.localScale = Vector3.one * camChildScale;
            StartCoroutine(StartThrowRoutine());

            //Disable Throw And Rotation Btns
            OnEndPlacement();
        }
        else
        {
            base.ThrowPickedObjects();
        }
        //RoomManager.instance.EnablePlacableItemIndicater(mainCat, SubCatId, false);
    }
    public void TurnOffOutline()
    {
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }
    public void OnNewSpawnItem()
    {
        SetDefaultValues();
        saveInitialParent = false;

        var toolPicked = GameController.instance.currentPickedTool;
        var itemPick = GameController.instance.currentPicketItem;

        if (itemPick == null && toolPicked == null)
        {
            gameObject.SetActive(true);
            if (canPlaceDynamically)
            {
                OnStartItemPlacement();
            }
            else
            {
                gameObject.transform.parent = GameController.instance.parentOFPickedObj.transform;
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).GetComponent<BoxCollider>())
                        transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
                }
                Collider[] _colliders = GetComponents<Collider>();
                for (int i = 0; i < _colliders.Length; i++)
                {
                    _colliders[i].enabled = false;
                }
                if (GetComponent<Rigidbody>())
                {
                    GetComponent<Rigidbody>().isKinematic = true;
                }
                itemCondition = ItemState.ready;
                transform.localPosition = pickedPosition;
                transform.localRotation = Quaternion.Euler(pickedAngle);
                transform.localScale = Vector3.one * camChildScale;
                UIController.instance.OnChangeInteraction(1, true);
            }
            GameController.instance.UpdateCurrentPickedItem(gameObject);
            UIController.instance.DisplayHoverObjectName("", false);

        }
        else
        {
            UIController.instance.DisplayInstructions("Item is already picked");
        }
    }
    public void OnSpawnSavedItem()
    {
        if (canPlaceDynamically)
        {
            SetDefaultValues();
            if (itemsSavingProps != null)
            {
                if (itemsSavingProps.isPlacedRight)
                {
                    ResetDynamicItemColliders(true,true);
                }
                if (itemsSavingProps.placedAreaId >= 0)
                {
                    //if (objectRelatedTo == ObjectRelavance.Room)
                    //{
                    //    //RoomManager.instance.rooms[itemsSavingProps.placedAreaId].OnPlaceItemInRoom(mainCat, SubCatId, itemId);
                    //    //transform.parent = RoomManager.instance.rooms[itemsSavingProps.placedAreaId].roomProperties.placedItemParent.transform;
                    //}
                }
            }
        }
    }
}