using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreRack : ItemPickandPlace, InteractableObjects,IRuntimeSpawn
{
 //   public string rackName;
  //  public int rackId;
    public bool isDoorRack;
    public List<CategoryName> placableCategories;
    public List<int> placableSubCatIds;
    public Transform placersContainer;
    public List<RackItemPlacers> rackItemPlacers;
    public Transform rackCustomerPoint;
    public bool isPlacingDynamically;

    #region Rack Initialization
    public void InitializeRack()
    {
        rackItemPlacers = new List<RackItemPlacers>();
        for(int i = 0; i < placersContainer.childCount; i++)
        {
            RackItemPlacers _rackPlacer = new RackItemPlacers();
            int _temp = i;
            _rackPlacer.itemPlacerPoint = placersContainer.GetChild(_temp);
            rackItemPlacers.Add(_rackPlacer);
        }
    }
    public void OnSpawnInitialRack(Transform _placingPoint)
    {
        isPlacingDynamically = false;
        transform.position = _placingPoint.position;
        transform.rotation = _placingPoint.rotation;
        itemsSavingProps.itemPosition = _placingPoint.position;
        itemsSavingProps.itemRotation = _placingPoint.rotation;
        itemsSavingProps.isPlacedRight = true;
        SetDefaultValues();
        InitializeRack();
        ResetDynamicItemColliders(true, true);
        StartCoroutine(ResetRackBaseCollider());

    }
    #endregion
    #region Rack Items Spawning
    public void SpawnItemToPlace(SuperStoreItems _item)
    {
        if(_item.rackPlaceId<0 || _item.rackPlaceId>= rackItemPlacers.Count)
        {
            return;
        }
        ItemData _data = GameManager.instance.GetItem((CategoryName)_item.catId, _item.subCatId, _item.itemId);
        if (_data != null)
        {
            if (_data.singleItemPrefab != null)
            {
                GameObject _obj= Instantiate(_data.singleItemPrefab, placersContainer);
                _obj.transform.localPosition = rackItemPlacers[_item.rackPlaceId].itemPlacerPoint.transform.localPosition;
                _obj.transform.localRotation = rackItemPlacers[_item.rackPlaceId].itemPlacerPoint.transform.localRotation;
                _obj.transform.localScale = rackItemPlacers[_item.rackPlaceId].itemPlacerPoint.transform.localScale;
                _obj.isStatic = true;
                if (_obj.GetComponent<Collider>())
                {
                    _obj.GetComponent<Collider>().enabled = false;
                }
                rackItemPlacers[_item.rackPlaceId].placedItemRef = _obj;
                rackItemPlacers[_item.rackPlaceId].placedItemProps = _item;
                rackItemPlacers[_item.rackPlaceId].isOccupied = true;
            }
            else
            {
                Debug.LogError("Non Interactable Mesh Required For " + _data.itemName);
            }
        }
        else
        {
            Debug.LogError("Invalid Item Saved!");
        }
    }
    public int GetEmptyPlacerPointIndex()
    {
        for(int i = 0; i < rackItemPlacers.Count; i++)
        {
            if (rackItemPlacers[i].isOccupied == false)
            {
                return i;
            }
        }
        return -1;
    }
    public Transform GetRackItemPlacerPoint(int _index)
    {
        if (_index < 0 || _index >= rackItemPlacers.Count)
            return null;
        return rackItemPlacers[_index].itemPlacerPoint;
    }
    public void AddItemToRack(int _rackPlacerIndex,GameObject _itemRef,ItemData _item)
    {
        if(_rackPlacerIndex<0 || _rackPlacerIndex >= rackItemPlacers.Count)
        {
            Debug.LogError("Invalid Placer index Given");
            return;
        }
        SuperStoreItems _newItem = new SuperStoreItems();
        _newItem.catId = (int)_item.mainCatID;
        _newItem.subCatId = _item.subCatID;
        _newItem.itemId = _item.itemID;
        _newItem.rackId = itemsSavingProps.itemUniqueId;
        _newItem.rackPlaceId = _rackPlacerIndex;
        _newItem.sellingPrice = _item.itemSellingPrice;

        rackItemPlacers[_rackPlacerIndex].placedItemRef = _itemRef;
        rackItemPlacers[_rackPlacerIndex].isOccupied = true;
        rackItemPlacers[_rackPlacerIndex].placedItemProps = _newItem;
        SuperStoreManager.instance.OnAddItem(_newItem);
    }
    public void RemoveItemFromRack(int _itemPlacerIndex)
    {
        if(_itemPlacerIndex<0 || _itemPlacerIndex >= rackItemPlacers.Count)
        {
            Debug.LogError("Invalid Index");
            return;
        }
        rackItemPlacers[_itemPlacerIndex].isOccupied = false;
        rackItemPlacers[_itemPlacerIndex].placedItemProps = null;
        if (rackItemPlacers[_itemPlacerIndex].placedItemRef != null)
        {
            Destroy(rackItemPlacers[_itemPlacerIndex].placedItemRef);
        }
    }
    #endregion

    public void OnHoverItems()
    {
        if(GameController.instance.currentPicketItem==null && GameController.instance.currentPickedTool == null)
        {
            UIController.instance.DisplayHoverObjectName("Tap to Reposition Rack", true, HoverInstructionType.General);
            UIController.instance.OnChangeInteraction(0, true);

            if (GetComponent<Outline>())
            {
                GetComponent<Outline>().enabled = true;
            }
            return;
        }
        if (GameController.instance.currentPicketItem != null)
        {
            CategoryName _pickedItemMainCat = GameController.instance.currentPicketItem.GetComponent<ItemPickandPlace>().mainCat;
            int _itemSubCatId = GameController.instance.currentPicketItem.GetComponent<ItemPickandPlace>().SubCatId;

            if (placableCategories.Contains(_pickedItemMainCat) && placableSubCatIds.Contains(_itemSubCatId) &&
                isPlacingDynamically && itemsSavingProps.isPlacedRight)
            {
                UIController.instance.DisplayHoverObjectName("Tap to Place Items In Rack", true, HoverInstructionType.Warning);
                if (GetComponent<Outline>())
                {
                    GetComponent<Outline>().enabled = true;
                }
                return;
            }
        }
        UIController.instance.DisplayHoverObjectName(itemName, true);
    }
    public void TurnOffOutline()
    {
        if (GetComponent<Outline>())
        {
            GetComponent<Outline>().enabled = false;
        }
    }

    public void OnInteract()
    {
        if (GameController.instance.currentPicketItem != null)
        {
            print("Picked Item Details====" + GameController.instance.currentPicketItem.GetComponent<ItemPickandPlace>().mainCat +
                "=======" + GameController.instance.currentPicketItem.GetComponent<ItemPickandPlace>().SubCatId);
            CategoryName _pickedItemMainCat = GameController.instance.currentPicketItem.GetComponent<ItemPickandPlace>().mainCat;
            if (!placableCategories.Contains(_pickedItemMainCat))
            {
                
                UIController.instance.DisplayInstructions("Can Not Place Item In This Rack");
                return;
            }
            int _itemSubCatId = GameController.instance.currentPicketItem.GetComponent<ItemPickandPlace>().SubCatId;
            if (!placableSubCatIds.Contains(_itemSubCatId))
            {
                UIController.instance.DisplayInstructions("Can Not Place Item In This Rack");
                return;
            }
            if (GameController.instance.currentPicketItem.GetComponent<ISuperStoreItem>()!=null)
            {
                if (GameController.instance.currentPicketItem.GetComponent<ISuperStoreItem>().IsPlacingProducts())
                {
                    print("Already Placing Items");
                    return;
                }
                if(isPlacingDynamically || !itemsSavingProps.isPlacedRight)
                {
                    UIController.instance.DisplayInstructions("Can Not Place Item In This Rack");
                    return;
                }
                GameController.instance.currentPicketItem.GetComponent<ISuperStoreItem>().PlaceItemsInRack(gameObject);
            }
        }
        else if(GameController.instance.currentPickedTool==null)
        {
            if (canPlaceDynamically)
            {
                isPlacingDynamically = true;
                OnStartItemPlacement();
                GameController.instance.UpdateCurrentPickedItem(gameObject);
                UIController.instance.DisplayHoverObjectName("", false);
            }
            else
            {
                Debug.LogError("Can Not Be Placed Dynamically");
            }
        }
    }
    public void OnNewSpawnItem()
    {
        SetDefaultValues();
        saveInitialParent = false;
        isPlacingDynamically = true;
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
                    Collider[] _childColliders = transform.GetChild(i).GetComponents<Collider>();
                    for (int j = 0; j < _childColliders.Length; j++)
                    {
                        _childColliders[j].enabled = false;
                    }
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
            isPlacingDynamically = false;

            SetDefaultValues();
            if (itemsSavingProps != null)
            {
                if (itemsSavingProps.isPlacedRight)
                {

                    ResetDynamicItemColliders(true,true);

                    StartCoroutine(ResetRackBaseCollider());
    

                 //   rackId = itemsSavingProps.itemUniqueId;
                    if (objectRelatedTo == ObjectRelavance.SuperStore)
                    {
                        // Will Register Itself to SuperStore Data
                        SuperStoreManager.instance.OnPlacedRackSpawned(gameObject);
                    }
                    return;
                }
            }

            ResetDynamicItemColliders(false, true);
            StartCoroutine(ResetRackBaseCollider());

        }
    }
    public override void OnPlaceItemDynamically()
    {
        base.OnPlaceItemDynamically();
        if (itemsSavingProps.isPlacedRight)
        {
            isPlacingDynamically = false;
            //Register Itself to StoreData
            StartCoroutine(ResetRackBaseCollider());

            if (itemsSavingProps.itemUniqueId < 0)
            {
                itemsSavingProps.itemUniqueId = SuperStoreManager.instance.OnNewRackPlaced(gameObject);
            }
        }
    }
    /// <summary>
    /// Make Each Racks Base Collider A Trigger Since Its Base Collider Is Extended To Cover its customer Spawn Point
    /// </summary>
    IEnumerator ResetRackBaseCollider()
    {
        yield return null;
        yield return null;
        yield return null;
        if (!isPlacingDynamically)
        {
            Collider[] _colliders = GetComponents<Collider>();
            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].isTrigger = true;
            }
        }
        
    }
    public override void ThrowPickedObjects()
    {
        if (canPlaceDynamically)
        {
            isPlacingDynamically = false;
            if (itemsSavingProps.isPlacedRight)
            {
                ResetDynamicItemColliders(true,true);
                StartCoroutine(ResetRackBaseCollider());

                gameObject.layer = defaultLayer;
                transform.position = itemsSavingProps.itemPosition;
                transform.rotation = itemsSavingProps.itemRotation;
                GameController.instance.currentPicketItem = null;
                OnEndPlacement();
                return;
            }
            transform.parent = GameController.instance.parentOFPickedObj.transform;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<Collider>())
                {
                    Collider[] _childColliders = transform.GetChild(i).GetComponents<Collider>();
                    for (int j = 0; j < _childColliders.Length; j++)
                    {
                        _childColliders[j].enabled = false;
                    }
                }
            }
            Collider[] _colliders = GetComponents<Collider>();
            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].enabled = false;
            }
            StartCoroutine(ResetRackBaseCollider());

            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }
            transform.localPosition = pickedPosition;
            transform.localRotation = Quaternion.Euler(pickedAngle);
            transform.localScale = Vector3.one * camChildScale;
            StartCoroutine(StartThrowRoutine(true));
            //Disable Throw And Rotation Btns
            OnEndPlacement();
        }
        else
        {
            base.ThrowPickedObjects();
        }
    }
}

[System.Serializable]
public class RackItemPlacers
{
    public Transform itemPlacerPoint;
    public bool isOccupied;
    public SuperStoreItems placedItemProps;
    public GameObject placedItemRef;
}
