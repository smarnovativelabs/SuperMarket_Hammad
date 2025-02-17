using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreItemsValuse : MonoBehaviour
{
    public Text itemName;
    public Text itemPrice;
    public Text itemQuantaity;
    public Text totalPrice;
    public Text quantaityText;
    public Text reqLevelText;
    public Image itemImage;
    public GameObject lockImage;
    public Button minusButton;
    public Button plusButton;
    public Button addToCartButton;

    public int selectedUnits;
    public int cartUnitCount = 0;

    public static int totalBill;
    int reqLevel;
    ItemData itemProps;
    GameObject cartItemBar;
    [Header("Sound")]
    public AudioClip uiButtonSound;
    private void Start()
    {
        totalPrice.text = "$ "+ itemPrice.text;
    }

    public void SetItemUI(ItemData _itemInfo)
    {
        itemProps = _itemInfo;
        itemName.text = _itemInfo.itemName;
        itemPrice.text = "$" + _itemInfo.itemPrice.ToString();
        itemImage.sprite = _itemInfo.itemSprite;
        quantaityText.text = _itemInfo.itemquantity.ToString();
        selectedUnits = 1;

        //minusButton.GetComponent<Button>().onClick.AddListener(() =>  DecrementQuantity());
        //plusButton.GetComponent<Button>().onClick.AddListener(() => IncrementQuantity());

        totalPrice.text = "$ " + (selectedUnits * _itemInfo.itemPrice).ToString();
        addToCartButton.GetComponent<Button>().onClick.AddListener(() => OnAddToCartBtnPress());
        addToCartButton.transform.GetChild(0).GetComponent<Text>().text = GameManager.instance.selectedDeliveryMode == 0 ? "Add To Cart" : "Buy";
        Debug.Log("Pass categore 0 : " + _itemInfo.itemName + GameManager.instance.categoriesUIData.Count);
         Debug.Log((int)_itemInfo.mainCatID);
        Debug.Log(_itemInfo.subCatID);

        Debug.Log((int)_itemInfo.itemID);
        reqLevel = GameManager.instance.categoriesUIData[(int)_itemInfo.mainCatID].subCategoriesUIData[_itemInfo.subCatID].reqLevel;
        // Debug.Log("Pass categore");
        print("umumum");
        lockImage.SetActive(reqLevel > PlayerDataManager.instance.playerData.playerLevel);
        reqLevelText.text = "Unlock At Level " + reqLevel.ToString();
    }
    public void UpdateItemUI()
    {
        print("NSD 1@");
        print("damdam");
        
        addToCartButton.transform.GetChild(0).GetComponent<LocalizeText>().UpdateText(GameManager.instance.selectedDeliveryMode == 0 ? "Add To Cart" : "Buy");
        lockImage.SetActive(reqLevel > PlayerDataManager.instance.playerData.playerLevel);
      //  EnableGameObjectAfterDelay(5);
        Debug.Log("Lock Image is: " + lockImage.name + ", Is Active: " + lockImage.activeSelf);
        print("damdam2" + lockImage);
    }

    public void OnAddToCartBtnPress()
    {
        if (GameManager.instance.selectedDeliveryMode == 1)
        {
            UIController.instance.OnPurchaseItem(itemProps);
            return;
        }
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        cartUnitCount += selectedUnits;
        if (cartItemBar == null || !cartItemBar.activeSelf)
        {
            UIController.instance.AdjustParentHeight(1);
            cartItemBar = Instantiate(UIController.instance.itemPriceUnitBar, UIController.instance.parentItemUnitPriceBar.transform);
            cartItemBar.GetComponent<CartItemValues>().SetCartItemData(itemProps, selectedUnits);

        }
        else
        {
            cartItemBar.GetComponent<CartItemValues>().UpdateCartItemData(selectedUnits);
            cartItemBar.SetActive(true);
        }
        totalBill += (selectedUnits * itemProps.itemPrice);
        UIController.instance.totalBillPriceText.text = "$ " + totalBill.ToString();
    }


    void CalculateTotalPriceAdd()
    {
        totalPrice.text = "$ "+(selectedUnits * itemProps.itemPrice).ToString();
    }

    public void IncrementQuantity()
    {
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        selectedUnits++;
        UpdateQuantityUI();
        CalculateTotalPriceAdd();
    }

    public void DecrementQuantity()
    {
        SoundController.instance.OnPlayInteractionSound(uiButtonSound);
        if (selectedUnits > 1)
        {
            selectedUnits--;
            UpdateQuantityUI();
            CalculateTotalPriceAdd();
        }
    }

    void UpdateQuantityUI()
    {
        itemQuantaity.text = selectedUnits.ToString();
    }

    public void ResetValuesOnRemove()
    {
        cartUnitCount = 0;
    }
    //IEnumerator EnableGameObjectAfterDelay(float delay)
    //{
    //    print("ysb called");
    //    // Wait for the specified delay
    //    yield return new WaitForSeconds(delay);

    //    // Enable the GameObject after the delay
    //  lockImage.SetActive(true);
    //}

}
