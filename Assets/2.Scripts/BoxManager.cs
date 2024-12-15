using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoxManager : MonoBehaviour
{
    public Transform spawnPoint;
    public float spawnInterval = 300f;  // 5 minutes in seconds
    public GameObject CurrentPickedBox;
    public int boxesToSpawn;  // Number of boxes to spawn in total
    public int boxesSpawned;  // Counter for boxes spawned so far
    public AudioClip boxDeliverSound;
    private bool isSpawningBoxes = false;
    private Queue<SpawnItemData> boxQueue = new Queue<SpawnItemData>();

    public void StartSpawnBoxes(List<BoxDataToSpawn> _items)
    {
        foreach (BoxDataToSpawn _box in _items)
        {
            SpawnItemData _data = new SpawnItemData();
            _data.item = _box.item;
            _data.savingProps = new List<ItemSavingProps>();
            for(int i = 0; i < _box.count; i++)
            {
                ItemSavingProps _prop = new ItemSavingProps();
                _prop.mainCatId = (int)_box.item.mainCatID;
                _prop.subCatId = _box.item.subCatID;
                _prop.itemId = _box.item.itemID;
                _prop.isInBox = true;
                _prop.itemCount = _box.item.itemquantity;
                ItemsSavingManager.instance.AddPurchasedItem(_prop);
                _data.savingProps.Add(_prop);
            }            
            _data.itemCount = _box.count;
            boxQueue.Enqueue(_data);
        }

        if (!isSpawningBoxes)
        {
            StartCoroutine(SpawnBoxesRoutine());
        }
    }

    private IEnumerator SpawnBoxesRoutine()
    {
        isSpawningBoxes = true;

        while (boxQueue.Count > 0)
        {
            SpawnItemData currentBox = boxQueue.Dequeue();

            for (int j = 0; j < currentBox.itemCount; j++)
            {
                SpawnBox(currentBox.item, currentBox.savingProps[j]);
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        isSpawningBoxes = false;
    }
    public class SpawnItemData
    {
        public int itemCount;
        public ItemData item;
        public List<ItemSavingProps> savingProps;
    }
    private void SpawnBox(ItemData _spawnData,ItemSavingProps _props)
    {
        Vector3 _position = (spawnPoint.position + (Random.insideUnitSphere * 1.5f));
        _position.y = spawnPoint.position.y;
        GameObject box = Instantiate(_spawnData.itemBoxPrefab, _position, spawnPoint.rotation);
        UIController.instance.DisplayInstructions("Box Delivered Outside Office");
        Rigidbody rb = box.GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = box.AddComponent<Rigidbody>();
        }
        rb.useGravity = true;
        rb.isKinematic = false;
        box.GetComponent<Box>().RefrenceBoxItemData(_spawnData);
        box.GetComponent<ItemPickandPlace>().UpdateItemSavingData(_props);
        SoundController.instance.OnPlayInteractionSound(boxDeliverSound);
    }

    public void OnPressOpenBoxPickedPanel()
    {
        if (GameController.instance.currentPicketItem.GetComponent<Box>())
        {
            GameController.instance.currentPicketItem.GetComponent<Box>().RemoveItemFromSavingList();
            GameController.instance.currentPicketItem.GetComponent<Box>().SpawnFurnitureItems();
            TutorialManager.instance.OnCompleteTutorialTask(9);
        }
    }
}

[System.Serializable]
public class BoxDataToSpawn
{

    public ItemData item;
    public int count;
}
