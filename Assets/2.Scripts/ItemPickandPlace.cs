using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
//using UnityStandardAssets.Characters.FirstPerson;

public class ItemPickandPlace : MonoBehaviour
{
    public CategoryName mainCat;
    public int SubCatId;
    public int itemId;
    public string itemName;
    public int placedRoomIndex = -1;
    public int indexInRoomList = -1;
    public GameObject[] indicator;
    public int currentIndicator = 0;
    [Header("Pickable Items Positions/Scales/Rotation")]
    public Vector3 pickedPosition;
    public Vector3 pickedAngle;
    public float camChildScale;
    public float throwForceMul;
    public float normalScale;

    public Vector3 targetPosition;
    public Quaternion targetRotation;
    public Vector3 targetScale;
    public AudioClip itemPickSound;
    public AudioClip itemThrowSound;
    public ItemSavingProps itemsSavingProps;
    [Header("Dynamic Placement Region")]
    public bool canPlaceDynamically;
    public ObjectRelavance objectRelatedTo = ObjectRelavance.Room;
    public ObjectRotationAxis rotationAxis = ObjectRotationAxis.Y;
    public float initialplayerDistance = 4f;
    public List<string> validTags;
    public List<string> placableTags;
    public List<string> ignoredTags;
    public bool isObjectInteracting;
    public int areaId;
    protected int defaultLayer;
    protected bool saveInitialParent = true;
    float currentRotation = 0f;
    Quaternion objectRotation;
    Material[] defaultMaterials;
    List<Material> invalidPlacerMaterials;
    bool canPlaceObject;
    public List<Collider> invalidTriggeredColliders;
    public List<Collider> placableTriggerColliders;
    List<Collider> areaTriggers;
    Transform defaultParent;

    [System.Serializable]
    public enum ObjectRotationAxis
    {
        X, Y, Z
    }

    private float lerpTransition;
    public enum ItemState
    {
        picking,
        placing,
        throwing,
        ready
    }
    private void Start()
    {
        if (saveInitialParent)
        {
            defaultParent = transform.parent;
        }
    }
    public ItemState itemCondition = ItemState.ready;
    UnityAction onCompleteLerpAction;
    public virtual void Update()
    {
        if (canPlaceDynamically && isObjectInteracting)
        {
            RepositionObject(PlayerInteraction.instance.GetItemPlacementColliders());
        }
        if (itemCondition == ItemState.picking)
        {
            gameObject.transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, lerpTransition);
            gameObject.transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, lerpTransition);
            gameObject.transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpTransition);
            lerpTransition += Time.deltaTime * 2;
            if (lerpTransition > 1)
            {
                //GameController.instance.CanPickItem(true);
                itemCondition = ItemState.ready;
                gameObject.transform.localPosition = targetPosition;
                gameObject.transform.localRotation = targetRotation;
                gameObject.transform.localScale = targetScale;
                lerpTransition = 0;
                if (onCompleteLerpAction != null)
                {
                    onCompleteLerpAction();
                    onCompleteLerpAction = null;
                }
            }
        }
        else if (itemCondition == ItemState.placing)
        {
            gameObject.transform.position = Vector3.Lerp(transform.position, targetPosition, lerpTransition);
            gameObject.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lerpTransition);
            gameObject.transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpTransition);
            lerpTransition += Time.deltaTime * 2;
            if (lerpTransition > 1)
            {
                //GameController.instance.CanPickItem(true);
                itemCondition = ItemState.ready;
                gameObject.transform.position = targetPosition;
                gameObject.transform.rotation = targetRotation;
                gameObject.transform.localScale = targetScale;
                lerpTransition = 0;
                if (onCompleteLerpAction != null)
                {
                    onCompleteLerpAction();
                    onCompleteLerpAction = null;
                }
            }
        }
        else if (itemCondition == ItemState.throwing)
        {
            gameObject.transform.localScale = Vector3.Lerp(transform.localScale, targetScale, lerpTransition);
            lerpTransition += Time.deltaTime * 50;
            if (lerpTransition > 1)
            {
                //GameController.instance.CanPickItem(true);
                itemCondition = ItemState.ready;
                gameObject.transform.localScale = targetScale;
                lerpTransition = 0;
                gameObject.layer = defaultLayer;

                if (itemsSavingProps != null)
                {
                    itemsSavingProps.itemPosition = transform.position;
                    itemsSavingProps.itemRotation = transform.rotation;
                }
                if (onCompleteLerpAction != null)
                {
                    onCompleteLerpAction();
                    onCompleteLerpAction = null;
                }
            }
        }

    }

    public virtual void OnSpawnItem(ItemData _data)
    {

    }
    public void SetIndexes(int _roomIndex, int _indicaterIndex)
    {
        placedRoomIndex = _roomIndex;
        indexInRoomList = _indicaterIndex;
    }

    public void SetOutSideRoomIndex(int _indicaterIndex)
    {
        indexInRoomList = _indicaterIndex;
    }

    public void SetSpawnedItemProp(ItemData _data, GameObject _spawnedItem)
    {
        ItemSavingProps _item = new ItemSavingProps();
        _item.mainCatId = (int)_data.mainCatID;
        _item.subCatId = _data.subCatID;
        _item.itemId = _data.itemID;
        _item.itemCount = _data.itemquantity;
        _spawnedItem.GetComponent<ItemPickandPlace>().UpdateItemSavingData(_item);
    }
    public void SetObjectToCam()
    {
        //make it child of cam
        //boxcolliderDisble
        //make rigidbody to kinematic if applied
        //set position at cam child point
        //set object scale to cam point
        //set object rotation when pick
        //set button to picking
        // if (PlayerInteraction.instance.currentInteractingObj.tag != "Indicator")

        gameObject.SetActive(true);
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
        itemCondition = ItemState.picking;
        targetPosition = pickedPosition;
        targetRotation = Quaternion.Euler(pickedAngle);
        targetScale = Vector3.one * camChildScale;
        UIController.instance.OnChangeInteraction(1, true);
        if (itemPickSound != null)
        {
            SoundController.instance.OnPlayInteractionSound(itemPickSound);
        }
    }
    public virtual void PlaceItem()
    {
        Collider[] _colliders = GetComponents<Collider>();

        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = true;
        }

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
       // int _currentRoom = RoomManager.instance.currentRoomNumber;
        //if (_currentRoom >= 0)
        //{
        //    GameController.instance.currentPicketItem.transform.parent = RoomManager.instance.rooms[_currentRoom].roomProperties.placedItemParent.transform;

        //}
        //else
        //{
        //    GameController.instance.currentPicketItem.transform.parent = GameController.instance.FurnitureParent.transform;
        //}
        GameController.instance.currentPicketItem.GetComponent<ItemPickandPlace>().OnItemRightPlaced(indicator[currentIndicator].transform);
        GameController.instance.currentPicketItem = null;
        //SoundController.instance.OnPlayInteractionSound(placeObjectSound);

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<BoxCollider>())
                transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
        }
    }
    public virtual void PlaceItem(GameObject _itemRef)
    {

    }
    public void OnItemRightPlaced(Transform _placingIndicaterRef)
    {
        //isPlacedRight = true;
        targetPosition = _placingIndicaterRef.position;
        targetRotation = _placingIndicaterRef.rotation;
        targetScale = _placingIndicaterRef.lossyScale;
        itemCondition = ItemState.placing;
    }

    public virtual void ThrowPickedObjects()
    {
        gameObject.transform.parent = defaultParent;

        Collider[] _colliders = GetComponents<Collider>();

        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = true;
            _colliders[i].isTrigger = false;
        }

        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
        if (!canPlaceDynamically)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<BoxCollider>())
                    transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
            }
        }
        else
        {
            GetComponent<Renderer>().materials = defaultMaterials;
        }
        Vector3 _forceDir = (PlayerController.instance.transform.forward * 100f * throwForceMul) + (Controlsmanager.instance.transform.up * 0f);
        GetComponent<Rigidbody>().AddForce(_forceDir);

        targetScale = Vector3.one * normalScale;
        GameController.instance.currentPicketItem = null;
        UIController.instance.OnChangeInteraction(0, true);
        UIController.instance.boxClickPanel.SetActive(false);
        if (indicator[0] != null && indicator.Length > 0)
        {
            indicator[currentIndicator].SetActive(false);
        }
        itemCondition = ItemState.throwing;
        if (itemThrowSound != null)
        {
            SoundController.instance.OnPlayInteractionSound(itemThrowSound);
        }
    }
    public void RewardXps(int _val = 0)
    {
        if (itemsSavingProps.isXpGiven)
        {
            return;
        }
        ItemData _item = GameManager.instance.GetItem(mainCat, SubCatId, itemId);
        if (_item != null)
        {
            itemsSavingProps.isXpGiven = true;
            int _xp = (_item.itemPrice / 2);
            PlayerDataManager.instance.UpdateXP(_xp);
            UIController.instance.UpdateXP(_xp);
        }
    }
    public virtual void UpdateItemSavingData(ItemSavingProps _prop)
    {
        itemsSavingProps = _prop;
    }
    public virtual void AddItemToSavingList()
    {
        ItemsSavingManager.instance.AddPurchasedItem(itemsSavingProps);
    }
    public virtual void RemoveItemFromSavingList()
    {
        ItemsSavingManager.instance.OnRemoveItem(itemsSavingProps);
    }
    #region Dynamic Placement Implementation
    public IEnumerator StartThrowRoutine(bool _enableChildColliders = false)
    {
        yield return null;
        yield return null;
        //ThrowPickedObjects();

        gameObject.transform.parent = null;
        ResetDynamicItemColliders(false,_enableChildColliders);

        Vector3 _forceDir = (PlayerController.instance.transform.forward * 100f * throwForceMul) + (Controlsmanager.instance.transform.up * 0f);
        GetComponent<Rigidbody>().AddForce(_forceDir);
        targetScale = Vector3.one * normalScale;
        GameController.instance.currentPicketItem = null;
        UIController.instance.OnChangeInteraction(0, true);
        itemCondition = ItemState.throwing;
        if (itemThrowSound != null)
        {
            SoundController.instance.OnPlayInteractionSound(itemThrowSound);
        }
    }

    public void SetDefaultValues()
    {
        defaultLayer = gameObject.layer;
        defaultMaterials = GetComponent<Renderer>().materials;
        objectRotation = transform.rotation;
        invalidTriggeredColliders = new List<Collider>();
        placableTriggerColliders = new List<Collider>();
        invalidPlacerMaterials = new List<Material>();
        areaTriggers = new List<Collider>();

        for(int i = 0; i < defaultMaterials.Length; i++)
        {
            invalidPlacerMaterials.Add(GameController.instance.invalidPlacerMaterial);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<BoxCollider>())
                transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;
        }
    }
    public void OnStartItemPlacement()
    {
        isObjectInteracting = true;
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
        invalidTriggeredColliders.Clear();
        placableTriggerColliders.Clear();
        UIController.instance.OnChangeInteraction(0, true);
        gameObject.layer = 2;
        UpdatePreviewObjectRotation();
        UIController.instance.EnableDynamicPlacingBtns(true);

    }
    public void OnEndPlacement()
    {
        isObjectInteracting = false;
        //Call To Display UI Buttons from here
        UIController.instance.EnableDynamicPlacingBtns(false);
    }

    public virtual void OnPlaceItemDynamically()
    {
        if (!isObjectInteracting || !canPlaceDynamically)
        { return; }
        if (!canPlaceObject)
        {
            UIController.instance.DisplayInstructions("Selected Place Is Occupied");
            return;
        }
        ResetDynamicItemColliders(true,true);
        gameObject.layer = defaultLayer;
        GameController.instance.currentPicketItem = null;
        if (itemsSavingProps != null)
        {
            itemsSavingProps.itemPosition = transform.position;
            itemsSavingProps.itemRotation = transform.rotation;
        }
        if (areaTriggers.Count > 0)
        {
            //if (objectRelatedTo == ObjectRelavance.Room)
            //{
            //    itemsSavingProps.placedAreaId = areaTriggers[areaTriggers.Count - 1].GetComponent<RoomTrigger>().roomId;
            //    RoomManager.instance.rooms[itemsSavingProps.placedAreaId].OnPlaceItemInRoom(mainCat, SubCatId, itemId);
            //    transform.parent = RoomManager.instance.rooms[itemsSavingProps.placedAreaId].roomProperties.placedItemParent.transform;
            //    RewardXps();
            //    if (itemsSavingProps.placedAreaId == RoomManager.instance.currentRoomNumber)
            //    {
            //        RoomManager.instance.rooms[itemsSavingProps.placedAreaId].CheckRoomProgress();
            //    }
            //}
        }
        itemsSavingProps.isPlacedRight = true;
        GameManager.instance.CallFireBase("ItmPlcd_" + itemsSavingProps.mainCatId.ToString() + "_" + itemsSavingProps.subCatId.ToString()
            + "_" + itemsSavingProps.itemId.ToString());
        OnEndPlacement();
    }

    public void ResetDynamicItemColliders(bool _isPlacedRight, bool _enableChildColliders=false)
    {
        Collider[] _colliders = GetComponents<Collider>();
        for (int i = 0; i < _colliders.Length; i++)
        {
            _colliders[i].enabled = true;
            _colliders[i].isTrigger = false;
        }
        if (GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().isKinematic = _isPlacedRight;
            GetComponent<Rigidbody>().useGravity = true;
        }
        if (_enableChildColliders)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<Collider>())
                {
                    Collider[] _childColliders = transform.GetChild(i).GetComponents<Collider>();
                    for (int j = 0; j < _childColliders.Length; j++)
                    {
                        _childColliders[j].enabled = true;
                    }
                }
            }
        }
        GetComponent<Renderer>().materials = defaultMaterials;
        
    }
    public void UpdateItemRotation(int _direction)
    {
        if (!canPlaceDynamically || !isObjectInteracting)
            return;

        currentRotation += (_direction * 45f);
        UpdatePreviewObjectRotation();
    }
    public virtual void UpdateItemVisibility(bool _canBePlaced)
    {
        if (_canBePlaced)
        {
            GetComponent<MeshRenderer>().materials = defaultMaterials;
        }
        else
        {
            GetComponent<MeshRenderer>().materials = invalidPlacerMaterials.ToArray();
        }
    }
    private void UpdatePreviewObjectRotation()
    {
        Vector3 _angle = Vector3.zero;//objectRotation.eulerAngles;

        switch (rotationAxis)
        {
            case ObjectRotationAxis.X:
                _angle.x = currentRotation;
                break;
            case ObjectRotationAxis.Y:
                _angle.y = currentRotation;
                break;
            case ObjectRotationAxis.Z:
                _angle.z = currentRotation;
                break;
        }
        transform.rotation = Quaternion.Euler(_angle);
    }
    void RepositionObject(List<PlacementRayHitData>_interactingObjs)
    {
        bool _canPlace = false;
        Vector3 _position = Vector3.zero;
        bool _pointFound = false;
        if (_interactingObjs.Count > 0)
        {
            for(int i = 0; i < _interactingObjs.Count; i++)
            {
                if (placableTags.Contains(_interactingObjs[i].hitCollider.gameObject.tag))
                {
                    _position = _interactingObjs[i].hitPoint;
                    _canPlace = invalidTriggeredColliders.Count < 1;
                    _pointFound = true;
                    break;
                }
                if (_pointFound)
                    continue;

                if (validTags.Contains(_interactingObjs[i].hitCollider.gameObject.tag))
                {
                    _position = _interactingObjs[i].hitPoint;
                    _pointFound = true;
                }
            }

        }
        if (!_pointFound)
        {
            _canPlace = false;
            _position = PlayerInteraction.instance.gameObject.transform.position +
                (PlayerInteraction.instance.gameObject.transform.forward * initialplayerDistance);
        }
        canPlaceObject = _canPlace;
        transform.position = _position;
        UpdateItemVisibility(_canPlace);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!canPlaceDynamically && !isObjectInteracting)
        {
            return;
        }
        if (other.tag == "AreaTrigger")
        {
            if (!areaTriggers.Contains(other))
            {
                areaTriggers.Add(other);
            }
            return;
        }
      
        if (ignoredTags.Contains(other.tag))
        {
            return;
        }
        if (!placableTags.Contains(other.gameObject.tag))
        {
            invalidTriggeredColliders.Add(other);
        }
        else
        {
            if (!placableTriggerColliders.Contains(other))
            {
                placableTriggerColliders.Add(other);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!canPlaceDynamically && !isObjectInteracting)
        {
            return;
        }
        if (ignoredTags.Contains(other.tag))
        {
            return;
        }
        if (!placableTags.Contains(other.gameObject.tag))
        {
            if (!invalidTriggeredColliders.Contains(other))
            {
                invalidTriggeredColliders.Add(other);

            }
        }
        else
        {
            if (!placableTriggerColliders.Contains(other))
            {
                placableTriggerColliders.Add(other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!canPlaceDynamically)
        {
            return;
        }
        if (other.tag == "AreaTrigger")
        {
            if (areaTriggers.Contains(other))
            {
                areaTriggers.Remove(other);
            }
            return;
        }
        if (ignoredTags.Contains(other.tag))
        {
            return;
        }
        if (invalidTriggeredColliders.Contains(other))
        {
            invalidTriggeredColliders.Remove(other);
            invalidTriggeredColliders.Clear();
        }
        else
        {
            if (placableTriggerColliders.Contains(other))
            {
                placableTriggerColliders.Remove(other);
            }
        }

    }
    #endregion
}
