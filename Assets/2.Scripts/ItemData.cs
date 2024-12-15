using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable/ItemData")]
[System.Serializable]
public class ItemData : ScriptableObject
{
    public CategoryName mainCatID;
    public int subCatID;
    public int itemID;
    public string itemName;
    public string subCatName;
    public ItemQuality itemQuality;
    public int unlockLevel;
    public int itemPrice;
    public float itemSellingPrice;
    public float unitItemPurPrice;
    public int itemquantity = 1;
    public bool oneTimePurchaseItem;
    public bool isSpawnOnSelection;
    public Sprite itemSprite;
    public GameObject itemBoxPrefab;
    public GameObject itemPrefab;
    public GameObject singleItemPrefab;
    public GameObject uiItemObject;
    public Material PaintMaterial;

    //  public bool isInstantiateObject;
    //   public GameObject itemMaterial;
    //   public GameObject itemTexture;
    //  public string firebaseEventName;
}

[System.Serializable]
public enum CategoryName
{
    //Furniture = 0,
    //Accessories = 1,
   // Paint = 2,
    Products=0,
    Racks=1,
    //Pool=5
}

[System.Serializable]
public enum ItemQuality
{
    Basic = 0,
    Average = 1,
    Premium = 2
}
