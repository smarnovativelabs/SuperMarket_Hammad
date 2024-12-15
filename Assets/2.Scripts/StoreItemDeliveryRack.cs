using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StoreItemDeliveryRack : ItemPickandPlace, InteractableObjects, ISuperStoreItem,IRuntimeSpawn
{
    public Transform productsContaienr;
    public AudioClip placeItemSound;
    bool isPlacingProducts = false;
    int movingItemsCount = 0;
    int XpOnItemPlaced = 0;
    class PlacingProductQueue
    {
        public Transform itemRef;
        public Transform rackPlacingPoint;
        public int rackPlacingIndex;
        public float transitionVal;
    }
    List<PlacingProductQueue> placingProductsQueue;
    

    public void OnHoverItems()
    {
        if (GameController.instance.currentPicketItem == null && GameController.instance.currentPickedTool==null)
        {
            UIController.instance.DisplayHoverObjectName("Tap To Pick" + itemName, true);
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
            SetObjectToCam();
            GameController.instance.UpdateCurrentPickedItem(gameObject);
        }
        else
        {
            UIController.instance.DisplayInstructions("Item is already picked");
        }
    }

    public void OnNewSpawnItem()
    {
        var toolPicked = GameController.instance.currentPickedTool;
        var itemPick = GameController.instance.currentPicketItem;
        saveInitialParent = false;
        if (itemPick == null && toolPicked == null)
        {
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
            itemCondition = ItemState.ready;
            transform.localPosition = pickedPosition;
            transform.localRotation = Quaternion.Euler(pickedAngle);
            transform.localScale = Vector3.one * camChildScale;
            UIController.instance.OnChangeInteraction(1, true);
            GameController.instance.UpdateCurrentPickedItem(gameObject);
            placingProductsQueue = new List<PlacingProductQueue>();
            
        }
        else
        {
            UIController.instance.DisplayInstructions("Item is already picked");
        }
    }
    public void OnSpawnSavedItem()
    {
        placingProductsQueue = new List<PlacingProductQueue>();
        StartCoroutine(UpdateRackItems());
    }
    IEnumerator UpdateRackItems()
    {
        yield return null;
        yield return null;

        ItemData _item = GameManager.instance.GetItem(mainCat, SubCatId, itemId);
        if (_item != null)
        {
            if (itemsSavingProps.itemCount < 0)
                itemsSavingProps.itemCount = 0;
            int _childsToRemove = _item.itemquantity - itemsSavingProps.itemCount;
            print(_childsToRemove + "-------- Remoinvg Childs");
            while (productsContaienr.childCount > 0 && _childsToRemove > 0)
            {
                Destroy(productsContaienr.GetChild(0).gameObject);
                _childsToRemove--;
                yield return null;
            }
        }
    }
    public void TurnOffOutline()
    {
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }
    public override void PlaceItem()
    {
        if(isPlacingProducts)
        {
            UIController.instance.DisplayInstructions("Products Placing On Rack");
            return;
        }
        base.PlaceItem();
    }
    
    public override void ThrowPickedObjects()
    {
        if (isPlacingProducts)
        {
            UIController.instance.DisplayInstructions("Products Placing On Rack");
            return;
        }
        base.ThrowPickedObjects();
    }
    public void PlaceItemsInRack(GameObject _rack)
    {
        if (isPlacingProducts)
        {
            return;
        }
        if (productsContaienr.childCount <= 0)
        {
            OnRemoveRack();
            return;
        }
        StartCoroutine(PlaceItemsOnRack(_rack));
    }
    public bool IsPlacingProducts()
    {
        return isPlacingProducts;
    }
    IEnumerator PlaceItemsOnRack(GameObject _rack)
    {
        isPlacingProducts = true;
        yield return null;
        bool _placingItems = true;
        while (_placingItems)
        {
            int _rackPlacingIndex = _rack.GetComponent<StoreRack>().GetEmptyPlacerPointIndex();
            if (_rackPlacingIndex < 0)
            {
                yield return new WaitForSeconds(0.2f);
                break;
            }
            if (productsContaienr.childCount <= 0)
            {
                yield return new WaitForSeconds(0.2f);
                break;
            }
            PlacingProductQueue _item = new PlacingProductQueue();
            _item.itemRef = productsContaienr.GetChild(0);
            _item.rackPlacingPoint = _rack.GetComponent<StoreRack>().GetRackItemPlacerPoint(_rackPlacingIndex);
            _item.rackPlacingIndex = _rackPlacingIndex;

            _item.itemRef.transform.parent = _item.rackPlacingPoint.parent;
            _rack.GetComponent<StoreRack>().AddItemToRack(_rackPlacingIndex, _item.itemRef.gameObject, GameManager.instance.GetItem(mainCat, SubCatId, itemId));
            SoundController.instance.OnPlayInteractionSound(placeItemSound);
            yield return null;
            placingProductsQueue.Add(_item);
            itemsSavingProps.itemCount--;
            if (productsContaienr.childCount <= 0)
            {
                _placingItems = false;
                yield return new WaitForSeconds(0.2f);
                break;
            }
            yield return new WaitForSeconds(0.2f);
        }
       // GameManager.instance.CallFireBase("StoreItmPlac_" + mainCat.ToString() + "_" + SubCatId.ToString() + "_" + itemId.ToString());
        if (placingProductsQueue.Count <= 0)
        {
            isPlacingProducts = false;
            if (productsContaienr.childCount <= 0)
            {
                OnRemoveRack();
                SuperStoreManager.instance.UpdateGameProgressBar(true);
            }
        }

        print("___-All items are placed-____");

        //   PlayerDataManager.instance.UpdateXP(xpvalueHere);
        //  UIController.instance.UpdateXP(xpvalueHere);
    }
    public override void Update()
    {
        if (isPlacingProducts)
        {
            if (placingProductsQueue.Count > 0)
            {
                for(int i=0;i<placingProductsQueue.Count;i++)
                {
                    
                    PlacingProductQueue _item = placingProductsQueue[i];
                    if (_item.transitionVal >= 1)
                    {
                        continue;
                    }
                    _item.itemRef.transform.position = Vector3.Lerp(_item.itemRef.transform.position, _item.rackPlacingPoint.transform.position, _item.transitionVal);
                    _item.itemRef.transform.rotation = Quaternion.Slerp(_item.itemRef.transform.rotation, _item.rackPlacingPoint.transform.rotation, _item.transitionVal);
                    _item.itemRef.transform.localScale = Vector3.Lerp(_item.itemRef.transform.localScale, _item.rackPlacingPoint.transform.localScale, _item.transitionVal);
                    _item.transitionVal += (Time.deltaTime * 1f);
                    if (_item.transitionVal >= 1)
                    {
                        _item.itemRef.transform.position = _item.rackPlacingPoint.transform.position;
                        _item.itemRef.transform.rotation = _item.rackPlacingPoint.transform.rotation;
                        _item.itemRef.transform.localScale = _item.rackPlacingPoint.transform.localScale;
                        if (_item.itemRef.GetComponent<Collider>())
                        {
                            _item.itemRef.GetComponent<Collider>().enabled = false;
                        }
                        _item.itemRef.gameObject.isStatic = true;
                        movingItemsCount++;

                    }
                }
                if (movingItemsCount >= placingProductsQueue.Count)
                {
                    placingProductsQueue.Clear();
                    isPlacingProducts = false;
                    movingItemsCount = 0;
                    if (productsContaienr.childCount <= 0)
                    {
                        SuperStoreManager.instance.UpdateGameProgressBar(true);
                        OnRemoveRack();
                    }
                }
            }
            
        }
        base.Update();
    }

    void OnRemoveRack()
    {
        ItemsSavingManager.instance.OnRemoveItem(itemsSavingProps);
        ItemData _item = GameManager.instance.GetItem(mainCat, SubCatId, itemId);
        if (_item != null)
        {
            int _xp = (_item.itemPrice / 2);
            PlayerDataManager.instance.UpdateXP(_xp);
            UIController.instance.UpdateXP(_xp);
        }
        Destroy(gameObject);
        GameController.instance.currentPicketItem = null;
    }
}
