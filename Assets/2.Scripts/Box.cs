//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class Box : ItemPickandPlace, InteractableObjects
//{
//    public ItemData itemData;
//    public Image[] boxItemImages;

//    public void RefrenceBoxItemData(ItemData _ItemData)
//    {
//        itemData = _ItemData;
//        for(int i = 0; i < boxItemImages.Length; i++)
//        {
//            boxItemImages[i].sprite = itemData.itemSprite;
//        }
//    }
//    public void OnHoverItems()
//    {
//        UIController.instance.DisplayHoverObjectName("Tap to Pick Box!", true, HoverInstructionType.General);
//        UIController.instance.OnChangeInteraction(0, true);

//        //code for outline object

//        if (gameObject.GetComponent<Outline>())
//        {
//            gameObject.GetComponent<Outline>().enabled = true;
//        }
//    }

//    public void OnInteract()
//    {
//        var toolPicked = GameController.instance.currentPickedTool;
//        var itemPicked = GameController.instance.currentPicketItem;
//        if (toolPicked == null && itemPicked == null)
//        {
//            SetObjectToCam();
//            UIController.instance.boxClickPanel.SetActive(true);
//            GameController.instance.UpdateCurrentPickedItem(gameObject);
//            UIController.instance.DisplayHoverObjectName("", false);

//        }
//        else
//        {
//            UIController.instance.DisplayInstructions("Item Already Picked");
//        }
//    }

//    public void SpawnFurnitureItems()
//    {
//        GameController.instance.currentPicketItem = null;
//        UIController.instance.boxClickPanel.SetActive(false);
//        GameObject _furniture = Instantiate(itemData.itemPrefab);
//        _furniture.gameObject.SetActive(true);
//        _furniture.GetComponent<ItemPickandPlace>().itemId = itemData.itemID;
//        _furniture.GetComponent<ItemPickandPlace>().itemName = itemData.itemName;

//        ItemSavingProps _props = new ItemSavingProps();
//        _props.mainCatId = (int)itemData.mainCatID;
//        _props.subCatId = itemData.subCatID;
//        _props.itemId = itemData.itemID;
//        _props.itemCount = itemData.itemquantity;
//        _furniture.GetComponent<ItemPickandPlace>().UpdateItemSavingData(_props);
//        _furniture.GetComponent<ItemPickandPlace>().AddItemToSavingList();

//        _furniture.GetComponent<IRuntimeSpawn>().OnNewSpawnItem();
//        Destroy(gameObject);
//    }
//    public void TurnOffOutline()
//    {
//        if (gameObject.GetComponent<Outline>())
//        {
//            gameObject.GetComponent<Outline>().enabled = false;
//        }
//    }
//}

using System.Collections;
using System.Collections.Generic;
//using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;
using static TriggerArea;

public class Box : ItemPickandPlace, InteractableObjects
{
    public ItemData itemData;
    public Image[] boxItemImages;
    public bool isPlayerInteractingBox;
    public int storageRackShelfIndex = -1;


    public bool isAtTrolley; // Flag to indicate if the box is on a trolley

    public void RefrenceBoxItemData(ItemData _ItemData)
    {
        itemData = _ItemData;
        for (int i = 0; i < boxItemImages.Length; i++)
        {
            boxItemImages[i].sprite = itemData.itemSprite;
        }
        if (canPlaceDynamically)
        {
            SetDefaultValues();
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<BoxCollider>())
                    transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
            }
        }
       
    }

    public void OnHoverItems()
    {
        Debug.Log("Inside OnHoverItem()");
        UIController.instance.DisplayHoverObjectName("Tap to Pick Box!", true, HoverInstructionType.General);
        UIController.instance.OnChangeInteraction(0, true);

        // Enable outline if it exists
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }

    //public void OnInteract()
    //{
    //    Debug.Log("Inside OnInteract()");
    //    var toolPicked = GameController.instance.currentPickedTool;
    //    var itemPicked = GameController.instance.currentPicketItem;

    //    if (toolPicked == null && itemPicked == null && !GameController.instance.IsMountedOnTrolley())
    //    {
    //        SetObjectToCam();
    //        UIController.instance.boxClickPanel.SetActive(true);
    //        GameController.instance.UpdateCurrentPickedItem(gameObject);
    //        UIController.instance.DisplayHoverObjectName("", false);

    //        isAtTrolley = false; // Reset trolley flag
    //    }
    //    else
    //    {
    //        UIController.instance.DisplayInstructions("Item Already Picked");
    //    }
    //}
    public void OnInteract()
    {
        var toolPicked = GameController.instance.currentPickedTool;
        var itemPicked = GameController.instance.currentPicketItem;
        if (toolPicked == null && itemPicked == null && !GameController.instance.IsMountedOnTrolley())
        {
            if (canPlaceDynamically)
            {
                Debug.Log("3600");
                OnStartItemPlacement();
            }
            else
            {
                SetObjectToCam();
            }
            isAtTrolley = false;
           // isBoxInRoom = false;
            isPlayerInteractingBox = true;
            if (itemsSavingProps != null)
            {
                if (itemsSavingProps.itemUniqueId >= 0)
                {
                   // StorageRoomManager.instance.RemoveRackPlacedItem(itemsSavingProps.itemUniqueId
                       // , storageRackShelfIndex, gameObject);
                    AddItemToSavingList();
                }
            }

            UIController.instance.boxClickPanel.SetActive(true);
            GameController.instance.UpdateCurrentPickedItem(gameObject);
            UIController.instance.DisplayHoverObjectName("", false);
        }
        //else if (toolPicked != null && !isStockerOccupyBox)
        //{
        //    if (GameController.instance.currentPickedTool.gameObject.name == "Sell Machine")
        //    {
        //        if (itemsSavingProps != null)
        //        {
        //            if (itemsSavingProps.itemUniqueId >= 0)
        //            {
        //                StorageRoomManager.instance.RemoveRackPlacedItem(itemsSavingProps.itemUniqueId
        //                    , storageRackShelfIndex, gameObject);
        //            }
        //        }
        //        BoxManager.instance.OnRemoveBox(gameObject);
        //        OnSellItem();
        //    }
        //    else
        //    {
        //        UIController.instance.DisplayInstructions("Already Picked A Tool");
        //    }
        //}
        //else if (isStockerOccupyBox)
        //{
        //    UIController.instance.DisplayInstructions("Stocker Is Picking This Box");
        //}
        else
        {
            UIController.instance.DisplayInstructions("Item Already Picked");
        }
    }
    public override void Update()
    {
      //  Debug.Log("Inside overide void update()");
        PlaceDynamically();
        HandleObjectPlacement();
        if (isAtTrolley)
        {
            if (itemsSavingProps != null)
            {
                itemsSavingProps.itemPosition = transform.position;
            }
        }
    }
    void PlaceDynamically()
    {
        Debug.Log("Inside Place Dynamically() in Box");
        Debug.Log("3200 can place dynamcl" + canPlaceDynamically);
        Debug.Log("3300 isobject interacting" + isObjectInteracting);
        if (!canPlaceDynamically || !isObjectInteracting)
        {
            Debug.Log("Inside Not Dynamix");
            return;
        }
        Debug.Log("OutSide if of PlaceDynamically in Box() 75");
        List<PlacementRayHitData> _interactingObjs = PlayerInteraction.instance.GetItemPlacementColliders();

        bool _canPlace = false;
        Vector3 _position = Vector3.zero;
        bool _pointFound = false;
        if (_interactingObjs.Count > 0)
        {
            for (int i = 0; i < _interactingObjs.Count; i++)
            {
                if (placableTags.Contains(_interactingObjs[i].hitCollider.gameObject.tag))
                {
                    _position = _interactingObjs[i].hitPoint;
                    _canPlace = invalidTriggeredColliders.Count < 1;
                    _pointFound = true;
                    break;
                }
                if (_pointFound && _canPlace)
                {
                    break;
                }
            }
        }
        UIController.instance.EnableDynamicPlacingBtns(_pointFound);
        canPlaceObject = _canPlace;
        if (_pointFound)
        {
            //Call To Display UI Buttons from here
            gameObject.transform.parent = null;
            Collider[] _colliders = GetComponents<Collider>();

            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].enabled = true;
                _colliders[i].isTrigger = true;
            }
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                Collider[] _childColliders = transform.GetChild(i).GetComponents<Collider>();
                for (int j = 0; j < _childColliders.Length; j++)
                {
                    _childColliders[j].enabled = false;
                }
            }
            transform.position = _position;

            targetScale = Vector3.one * normalScale;
            transform.localScale = targetScale;

            UpdateItemVisibility(_canPlace);
            UpdatePreviewObjectRotation();
        }
        else
        {
            invalidTriggeredColliders.Clear();
            placableTriggerColliders.Clear();
            SetBoxToCamChild();
            UpdateItemVisibility(true);
        }
    }
    void SetBoxToCamChild()
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
    }
    public void OnPlaceInRack(int _rackId, int _shlefIndex, Transform _placerPoint)
    {
        ResetDynamicItemColliders(true, true);
        gameObject.layer = defaultLayer;
        GameController.instance.currentPicketItem = null;
        transform.position = _placerPoint.position;
        transform.rotation = _placerPoint.rotation;
        transform.localScale = _placerPoint.localScale;
        storageRackShelfIndex = _shlefIndex;
        if (itemsSavingProps != null)
        {
            itemsSavingProps.itemPosition = transform.position;
            itemsSavingProps.itemRotation = transform.rotation;
            itemsSavingProps.itemUniqueId = _rackId;
        }
        if (areaTriggers.Count > 0)
        {
            if (areaTriggers[areaTriggers.Count - 1].GetComponent<TriggerArea>().triggerAreaType == AreaType.Trolley)
            {
                transform.parent = areaTriggers[areaTriggers.Count - 1].gameObject.transform;
                //    isAtTrolley = true;

            }
        }
        UIController.instance.boxClickPanel.SetActive(false);
        itemsSavingProps.isPlacedRight = true;
        isPlayerInteractingBox = false;
        RemoveItemFromSavingList();
        OnEndPlacement();
    }
    public override void OnPlaceItemDynamically()
    {
        if (!isObjectInteracting || !canPlaceDynamically)
        { return; }

        if (!canPlaceObject)
        {
            UIController.instance.DisplayInstructions("Selected Place Is Occupied");
            return;
        }
        ResetDynamicItemColliders(true, true);
        gameObject.layer = defaultLayer;
        GameController.instance.currentPicketItem = null;
        isPlayerInteractingBox = false;
        if (itemsSavingProps != null)
        {
            itemsSavingProps.itemPosition = transform.position;
            itemsSavingProps.itemRotation = transform.rotation;
        }
        if (areaTriggers.Count > 0)
        {
            if (areaTriggers[areaTriggers.Count - 1].GetComponent<TriggerArea>().triggerAreaType == AreaType.Trolley)
            {
                transform.parent = areaTriggers[areaTriggers.Count - 1].gameObject.transform;
                //    isAtTrolley = true;

            }
        }
        UIController.instance.boxClickPanel.SetActive(false);
        itemsSavingProps.isPlacedRight = true;
        OnEndPlacement();
    }

    public void OnSpawnSavedItem(Vector3 _defaultPosition)
    {
        if (canPlaceDynamically)
        {
            transform.position = _defaultPosition;
            if (itemsSavingProps != null)
            {
                if (itemsSavingProps.isPlacedRight || itemsSavingProps.positionSaved)
                {
                    transform.position = itemsSavingProps.itemPosition;
                    transform.rotation = itemsSavingProps.itemRotation;
                }

            }

        }
    }
    public override void ThrowPickedObjects()
    {
        if (canPlaceDynamically)
        {
          //  Debug.Log("Area Trigger Count" + areaTriggers.Count);
            SetBoxToCamChild();
            StartCoroutine(StartThrowRoutine(true));
            //Disable Throw And Rotation Btns
            OnEndPlacement();
            UIController.instance.boxClickPanel.SetActive(false);
            isPlayerInteractingBox = false;
           // base.ThrowPickedObjects();
            if (areaTriggers.Count > 0)
            {
                if (areaTriggers[areaTriggers.Count - 1].GetComponent<TriggerArea>().triggerAreaType == AreaType.StorageRoom)
                {
                    if (itemsSavingProps != null)
                    {
                        itemsSavingProps.positionSaved = true;
                        itemsSavingProps.itemPosition = transform.position;
                        itemsSavingProps.itemRotation = transform.rotation;
                    }
                }
            }
            //if (RoomManager.instance.currentRoomNumber >= 0)
            //{
            //    isBoxInRoom = true;
            //}
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
    public void SpawnFurnitureItems()
    {
        GameController.instance.currentPicketItem = null;
        UIController.instance.boxClickPanel.SetActive(false);
        GameObject _furniture = Instantiate(itemData.itemPrefab);
        _furniture.gameObject.SetActive(true);
        _furniture.GetComponent<ItemPickandPlace>().itemId = itemData.itemID;
        _furniture.GetComponent<ItemPickandPlace>().itemName = itemData.itemName;

        ItemSavingProps _props = new ItemSavingProps();
        _props.mainCatId = (int)itemData.mainCatID;
        _props.subCatId = itemData.subCatID;
        _props.itemId = itemData.itemID;
        _props.itemCount = itemData.itemquantity;
        _furniture.GetComponent<ItemPickandPlace>().UpdateItemSavingData(_props);
        _furniture.GetComponent<ItemPickandPlace>().AddItemToSavingList();

        _furniture.GetComponent<IRuntimeSpawn>().OnNewSpawnItem();
        Destroy(gameObject);
    }
}
