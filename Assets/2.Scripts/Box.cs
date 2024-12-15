using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box : ItemPickandPlace, InteractableObjects
{
    public ItemData itemData;
    public Image[] boxItemImages;

    public void RefrenceBoxItemData(ItemData _ItemData)
    {
        itemData = _ItemData;
        for(int i = 0; i < boxItemImages.Length; i++)
        {
            boxItemImages[i].sprite = itemData.itemSprite;
        }
    }
    public void OnHoverItems()
    {
        UIController.instance.DisplayHoverObjectName("Tap to Pick Box!", true, HoverInstructionType.General);
        UIController.instance.OnChangeInteraction(0, true);

        //code for outline object

        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }

    public void OnInteract()
    {
        var toolPicked = GameController.instance.currentPickedTool;
        var itemPicked = GameController.instance.currentPicketItem;
        if (toolPicked == null && itemPicked == null)
        {
            SetObjectToCam();
            UIController.instance.boxClickPanel.SetActive(true);
            GameController.instance.UpdateCurrentPickedItem(gameObject);
            UIController.instance.DisplayHoverObjectName("", false);

        }
        else
        {
            UIController.instance.DisplayInstructions("Item Already Picked");
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
    public void TurnOffOutline()
    {
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }
}
