using UnityEngine;
using UnityEngine.UI;

public class CartItemValues : MonoBehaviour
{
    public Text itemName;
    public Text cartTotalUnits;
    public Text cartItemPrice;
    public AudioClip uiBtnSound;
    public int itemCount;

    public ItemData itemData;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetCartItemData(ItemData _item,int _itemCount)
    {
        itemData = _item;
        itemCount = _itemCount;

        itemName.GetComponent<LocalizeText>().UpdateText(itemData.itemName);
        cartTotalUnits.text = itemCount.ToString();
        cartItemPrice.text = "$ " + (itemCount * itemData.itemPrice).ToString();
    }
    public void UpdateCartItemData(int _countToAdd)
    {
        itemCount += _countToAdd;
        cartTotalUnits.text = itemCount.ToString();
        cartItemPrice.text = "$ " + (itemCount * itemData.itemPrice).ToString();
    }

    public void OnRemoveItemFromCart()
    {
        StoreItemsValuse.totalBill -= (itemData.itemPrice * itemCount);
        if (StoreItemsValuse.totalBill < 0)
        {
            StoreItemsValuse.totalBill = 0;
        }
        UIController.instance.AdjustParentHeight(-1);
        UIController.instance.totalBillPriceText.text = "$ " + StoreItemsValuse.totalBill.ToString();
        Destroy(gameObject);
        SoundController.instance.OnPlayInteractionSound(uiBtnSound);
    }
    public void OnDeleteAllItems()
    {
        itemCount = 0;
    }
}
[System.Serializable]
public class CartItemFields
{
    public CategoryName itemCatId;
    public int itemSubCatId;
    public int itemId;
    public int itemPrice;
    public int itemQuantity;
    public string itemName;

}
