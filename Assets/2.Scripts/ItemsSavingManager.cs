using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ItemsSavingManager : MonoBehaviour
{
    public Transform itemsSpawnPoint;

    public static ItemsSavingManager instance;
    public PurchasedItemsData saveData;
    bool isDataInitialized = false;

    private void Awake()
    {
        instance = this;
    }
    public IEnumerator LoadAndSpawnItems()
    {
        saveData = (PurchasedItemsData)SerializationManager.LoadFile("PurchasedItems");
        if (saveData == null)
        {
            saveData = new PurchasedItemsData();
        }
        for(int i = 0; i < saveData.savedItems.Count; i++)
        {
            ItemSavingProps _prop = saveData.savedItems[i];
            ItemData _item = GameManager.instance.GetItem((CategoryName)_prop.mainCatId, _prop.subCatId, _prop.itemId);
            if (_item == null)
            {
                continue;
            }

            Vector3 _position = (itemsSpawnPoint.position + (Random.insideUnitSphere * 1.5f));
            _position.y = itemsSpawnPoint.position.y;
            Quaternion _itemRotation = itemsSpawnPoint.rotation;
            if (_prop.isPlacedRight)
            {
                _position = _prop.itemPosition;
                _itemRotation = _prop.itemRotation;
            }

            GameObject _itemRef = Instantiate(_prop.isInBox ? _item.itemBoxPrefab : _item.itemPrefab, _position, _itemRotation);
            _itemRef.gameObject.SetActive(true);

            Rigidbody rb = _itemRef.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = _itemRef.AddComponent<Rigidbody>();
            }
            rb.useGravity = true;
            rb.isKinematic = false;
            if (_prop.isInBox)
            {
                _itemRef.GetComponent<Box>().RefrenceBoxItemData(_item);
            }
            else
            {
                _itemRef.GetComponent<ItemPickandPlace>().itemId = _item.itemID;
                _itemRef.GetComponent<ItemPickandPlace>().itemName = _item.itemName;
            }
            _itemRef.GetComponent<ItemPickandPlace>().UpdateItemSavingData(_prop);
            if (_itemRef.GetComponent<IRuntimeSpawn>()!=null)
            {
                _itemRef.GetComponent<IRuntimeSpawn>().OnSpawnSavedItem();
            }
            yield return new WaitForSeconds(0.1f);
        }
        isDataInitialized = true;
        GameController.instance.AddSavingAction(SaveGameData);
    }
    public void AddPurchasedItem(ItemSavingProps _item, bool _defaultPosition = true)
    {
        if (!isDataInitialized)
            return;
        if (saveData.savedItems.Contains(_item))
        {
            return;
        }
        if (_defaultPosition)
        {
            Vector3 _position = (itemsSpawnPoint.position + (Random.insideUnitSphere * 1.5f));
            _position.y = itemsSpawnPoint.position.y;
            _item.itemRotation = itemsSpawnPoint.rotation;
            _item.itemPosition = _position;
        }
        saveData.savedItems.Add(_item);
    }
    public void OnRemoveItem(ItemSavingProps _item)
    {
        if (!isDataInitialized)
            return;
        if (saveData.savedItems.Contains(_item))
        {
            print("Item Removed");
            saveData.savedItems.Remove(_item);
        }
    }


    void SaveGameData()
    {
        if (!isDataInitialized)
            return;
        if (saveData != null)
        {
            SerializationManager.Save(saveData, "PurchasedItems");
        }
    }
    //private void OnApplicationPause(bool pause)
    //{
    //    if (pause)
    //    {
    //        SaveGameData();
    //    }
    //}
    //private void OnApplicationQuit()
    //{
    //    SaveGameData();
    //}
    //private void OnDestroy()
    //{
    //    SaveGameData();
    //}
}

[System.Serializable]
public class PurchasedItemsData
{
    public List<ItemSavingProps> savedItems;
    public PurchasedItemsData()
    {
        savedItems = new List<ItemSavingProps>();
    }
}
[System.Serializable]
public class ItemSavingProps
{
    public bool positionSaved; // new
    public int mainCatId;
    public int subCatId;
    public int itemId;
    public bool isInBox;
    public int itemCount;
    public int placedAreaId;
    public int itemUniqueId;
    public bool isPlacedRight;
    public bool isXpGiven;
    public Vector3 itemPosition;
    public Quaternion itemRotation;
    public Quaternion itemScale;

    public ItemSavingProps()
    {
        isInBox = false;
        mainCatId = 0;
        subCatId = 0;
        itemId = 0;
        placedAreaId = -1;
        itemUniqueId = -1;
        itemPosition = Vector3.zero;
        itemRotation = Quaternion.identity;
    }
}