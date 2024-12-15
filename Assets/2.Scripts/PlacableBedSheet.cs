//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlacableBedSheet : ItemPickandPlace, InteractableObjects, IRuntimeSpawn
//{
//    public GameObject placedBedSheet;
//    public void OnHoverItems()
//    {
//        if(GameController.instance.currentPicketItem==null && GameController.instance.currentPickedTool == null)
//        {
//            UIController.instance.DisplayHoverObjectName("Tap To Pick " + itemName, true, HoverInstructionType.General);
//            UIController.instance.OnChangeInteraction(0, true);
//            if (gameObject.GetComponent<Outline>())
//            {
//                gameObject.GetComponent<Outline>().enabled = true;
//            }
//        }
//    }

//    public void OnInteract()
//    {
//        var toolPicked = GameController.instance.currentPickedTool;
//        var itemPick = GameController.instance.currentPicketItem;

//        if (itemPick == null && toolPicked == null)
//        {
//            if (canPlaceDynamically)
//            {
//                if (itemsSavingProps.placedAreaId >= 0)
//                {
//                    RoomManager.instance.rooms[itemsSavingProps.placedAreaId].OnRemoveItem(mainCat, SubCatId, itemId);
//                    itemsSavingProps.placedAreaId = -1;
//                    itemsSavingProps.isPlacedRight = false;
//                }
//                OnStartItemPlacement();
//            }
//            else
//            {
//                SetObjectToCam();
//            }
//            GameController.instance.UpdateCurrentPickedItem(gameObject);
//            UIController.instance.DisplayHoverObjectName("", false);
//        }
//        else
//        {
//            UIController.instance.DisplayInstructions("Item is already Selected");
//        }
//    }
//    public override void ThrowPickedObjects()
//    {
//        if (canPlaceDynamically)
//        {
//            placedBedSheet.SetActive(false);
//            gameObject.layer = defaultLayer;
//            gameObject.transform.parent = GameController.instance.parentOFPickedObj.transform;
//            for (int i = 0; i < transform.childCount; i++)
//            {
//                if (transform.GetChild(i).GetComponent<BoxCollider>())
//                    transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
//            }
//            Collider[] _colliders = GetComponents<Collider>();
//            for (int i = 0; i < _colliders.Length; i++)
//            {
//                _colliders[i].enabled = false;
//            }
//            if (GetComponent<Rigidbody>())
//            {
//                GetComponent<Rigidbody>().isKinematic = true;
//            }
//            transform.localPosition = pickedPosition;
//            transform.localRotation = Quaternion.Euler(pickedAngle);
//            transform.localScale = Vector3.one * camChildScale;

//            StartCoroutine(StartThrowRoutine());

//            //Disable Throw And Rotation Btns
//            OnEndPlacement();


//            //OnPlaceItemDynamically();
//        }
//        else
//        {
//            base.ThrowPickedObjects();
//        }
//        //RoomManager.instance.EnablePlacableItemIndicater(mainCat, SubCatId, false);
//    }
//    public void TurnOffOutline()
//    {
//        if (gameObject.GetComponent<Outline>())
//        {
//            gameObject.GetComponent<Outline>().enabled = false;
//        }
//    }

//    public void OnNewSpawnItem()
//    {
//        SetDefaultValues();
//        var toolPicked = GameController.instance.currentPickedTool;
//        var itemPick = GameController.instance.currentPicketItem;
//        saveInitialParent = false;

//        if (itemPick == null && toolPicked == null)
//        {
//            gameObject.SetActive(true);
//            placedBedSheet.SetActive(false);
//            if (canPlaceDynamically)
//            {
//                OnStartItemPlacement();
//            }
//            else
//            {
//                gameObject.transform.parent = GameController.instance.parentOFPickedObj.transform;
//                for (int i = 0; i < transform.childCount; i++)
//                {
//                    if (transform.GetChild(i).GetComponent<BoxCollider>())
//                        transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
//                }
//                Collider[] _colliders = GetComponents<Collider>();
//                for (int i = 0; i < _colliders.Length; i++)
//                {
//                    _colliders[i].enabled = false;
//                }
//                if (GetComponent<Rigidbody>())
//                {
//                    GetComponent<Rigidbody>().isKinematic = true;
//                }
//                itemCondition = ItemState.ready;
//                transform.localPosition = pickedPosition;
//                transform.localRotation = Quaternion.Euler(pickedAngle);
//                transform.localScale = Vector3.one * camChildScale;
//                UIController.instance.OnChangeInteraction(1, true);
//            }
//            GameController.instance.UpdateCurrentPickedItem(gameObject);
//        }
//        else
//        {
//            UIController.instance.DisplayInstructions("Item is already picked");
//        }
//    }
//    public override void UpdateItemVisibility(bool _canBePlaced)
//    {
//        base.UpdateItemVisibility(_canBePlaced);
//    }
//    public override void OnPlaceItemDynamically()
//    {
//        base.OnPlaceItemDynamically();
//        if (itemsSavingProps.isPlacedRight)
//        {
//            if (placableTriggerColliders.Count > 0)
//            {
//                gameObject.layer = 22;
//                ObjectPlacingPoint _sheetPlacePoint = placableTriggerColliders[placableTriggerColliders.Count - 1].GetComponent<ObjectPlacingPoint>();
//                if (_sheetPlacePoint != null)
//                {
//                    transform.parent = _sheetPlacePoint.placingPoint;
//                    transform.localPosition = Vector3.zero;
//                    transform.localRotation = Quaternion.identity;
//                    transform.localScale = Vector3.one;
//                    placedBedSheet.SetActive(true);
//                }
//                itemsSavingProps.itemPosition = transform.position;
//                itemsSavingProps.itemRotation = transform.rotation;
//                transform.parent = RoomManager.instance.rooms[itemsSavingProps.placedAreaId].roomProperties.placedItemParent.transform;
//            }
//        }
//    }

//    public void OnSpawnSavedItem()
//    {
//        if (canPlaceDynamically)
//        {
//            SetDefaultValues();
//            if (itemsSavingProps != null)
//            {
//                if (itemsSavingProps.isPlacedRight)
//                {
//                    ResetDynamicItemColliders(true,true);
//                    gameObject.layer = 22;
//                    placedBedSheet.SetActive(true);
//                }
//                if (itemsSavingProps.placedAreaId >= 0)
//                {
//                    if (objectRelatedTo == ObjectRelavance.Room)
//                    {
//                        RoomManager.instance.rooms[itemsSavingProps.placedAreaId].OnPlaceItemInRoom(mainCat, SubCatId, itemId);
//                        transform.parent = RoomManager.instance.rooms[itemsSavingProps.placedAreaId].roomProperties.placedItemParent.transform;
//                    }
//                }
//            }
//        }
//    }
//}
