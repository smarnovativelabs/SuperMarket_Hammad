//using AdsMediation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IAPUiManager : MonoBehaviour
{
    public static IAPUiManager Instance; 
    //public Image StorePanel;  
    public GameObject loadingPanel;
    public GameObject failureResponsePanel;
    public GameObject successResponsePanel;
    public GameObject promotionalPanel;
    public Image successResponseImage;
    public Text successResponseText;
    public Text failureResponseText;
    public Text promotionalPriceText;
    public AudioClip btnSound;
    public GameObject IAPPrefab;
    public Transform iapProductsContainer;
    public IAPSubPanelBtnDisplay[] btnDisplays;
    public IAPSubPanels[] iapSubPanels;
    public PromoPanels[] employeePromoPanels;

    [System.Serializable]
    public class PromoPanels
    {
        public GameObject promoPanel;
        public Button promoButton;
        public Text promoPriceText;
    }
    int currentSubPanelId = -1;
    [System.Serializable]
    public class IAPSubPanels
    {
        public Image subPanelBtn;
        public Text subPanelBtnTxt;
        public GameObject subPanel;
        public Transform iapProductContainer;
    }
    [System.Serializable]
    public class IAPSubPanelBtnDisplay
    {
        public Sprite btnImg;
        public Color textColor;
    }
    class ProductsContainerCount
    {
        public int productsCount = 0;
        public float deltaLength = -1;
    }

    private void Awake()
    {
        Instance = this;
    }
    public void InitializeIAPPanel()
    {
        loadingPanel.SetActive(false);
        failureResponsePanel.SetActive(false);
        successResponsePanel.SetActive(false);
        promotionalPanel.SetActive(false);
        for(int i = 0; i < iapSubPanels.Length; i++)
        {
            iapSubPanels[i].subPanelBtn.sprite = btnDisplays[0].btnImg;
            iapSubPanels[i].subPanelBtnTxt.color = btnDisplays[0].textColor;
            iapSubPanels[i].subPanel.SetActive(false);
        }
       /* for(int i = 0; i < employeePromoPanels.Length; i++)
        {
            employeePromoPanels[i].promoPanel.SetActive(false);
        }*/
        CreateIAPItems();
    }
    /// <summary>
    /// Creates and displays IAP items in the scroll view based on the list of products.
    /// It dynamically assigns the product name, price, icon, and rewards to the UI prefab.
    /// </summary>
    public void CreateIAPItems()
    {
        // Get the list of in-app products from the StoreManager
        List<ProductsContainerCount> _containersProductCount = new List<ProductsContainerCount>();
        for (int i = 0; i < iapSubPanels.Length; i++)
        {
            _containersProductCount.Add(new ProductsContainerCount());
        }
        List<InAppProduct> products = StoreManager.Instance.inAppProducts;

        // Iterate through each product in the inAppProducts list
        for (int i = 0; i < products.Count; i++)
        {
            // Instantiate the IAP item prefab and make it a child of scrollViewContent
            //GameObject newItem = Instantiate(IAPPrefab, iapProductsContainer.transform);
            int _cntnrIndex = (int)products[i].inAppCategory;
            products[i].displayBtnRef = Instantiate(products[i].iapBtnRef, iapSubPanels[_cntnrIndex].iapProductContainer);
            InAppProduct _product = products[i];
            //products[i].displayBtnRef = newItem;
            products[i].displayBtnRef.GetComponent<InAppBtn>().OnInitializeIAPBtn(_product);

            _containersProductCount[_cntnrIndex].productsCount++;
            if (_containersProductCount[_cntnrIndex].deltaLength < 0)
            {
                _containersProductCount[_cntnrIndex].deltaLength = products[i].displayBtnRef.GetComponent<RectTransform>().sizeDelta.x;
            }
            // Set the product name on the first child (assumed to be a Text component)

            //newItem.transform.GetChild(0).GetChild(0).gameObject.GetComponent<LocalizeText>().UpdateText(products[i].productName);

            //// Check the purchase method (InAppPurchase or RewardedVideo)
            //if (products[i].purchaseMethod == PurchaseType.InAppPurchase)
            //{
            //    // For in-app purchases, deactivate the ad-related button and display the price
            //    newItem.transform.GetChild(1).gameObject.SetActive(false);  // Deactivate Ad button
            //    newItem.transform.GetChild(5).gameObject.SetActive(false);  // Deactivate Watch Ads button
            //    newItem.transform.GetChild(4).gameObject.SetActive(true);
            //    // Set the price in the designated UI text
            //    newItem.transform.GetChild(4).transform.GetChild(0).gameObject.GetComponent<Text>().text = products[i].localizedPriceString;

            //    // Capture the current index to prevent closure issues in anonymous functions
            //    int k = i;
            //    // Assign click event for in-app purchase button
            //    newItem.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(() => OnIAPButtonClicked(k));
            //}
            //else
            //{
            //    // For ads-based purchases (RewardedVideo), deactivate price-related buttons and activate Ad button
            //    newItem.transform.GetChild(1).gameObject.SetActive(true);   // Activate Ad icon
            //    newItem.transform.GetChild(4).gameObject.SetActive(false);  // Deactivate Price button
            //    newItem.transform.GetChild(5).gameObject.SetActive(true);   // Activate Watch Ads button

            //    // Capture the current index for ads button click
            //    int k = i;
            //    // Assign click event for Watch Ads button
            //    newItem.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(() => OnWatchAdsBttonClick(k));
            //}

            //// Set the product icon for the item
            //newItem.transform.GetChild(2).GetComponent<Image>().sprite = products[i].icon;

            //// Handle the rewards text (if there is more than one reward, display them concatenated)
            //if (products[i].rewards.Count == 1)
            //{
            //    // Display single reward
            //    if (products[i].rewards[0].rewardType == RewardType.RemoveAds)
            //    {
            //        newItem.transform.GetChild(3).GetComponent<LocalizeText>().UpdateText("Remove Annoying Ads");
            //    }
            //    else
            //    {
            //        newItem.transform.GetChild(3).GetComponent<Text>().text = "+"+products[i].rewards[0].amount;
            //    }
            //}
            //else
            //{
            //    // If there are multiple rewards, concatenate them and display
            //    string reward = "";

            //    // Loop through each reward in the product
            //    for (int j = 0; j < products[i].rewards.Count; j++)
            //    {
            //        // Check reward type and concatenate the reward text
            //        if (products[i].rewards[j].rewardType == RewardType.Currency)
            //        {
            //            reward += products[i].rewards[j].amount + " Cash";
            //        }
            //        else if (products[i].rewards[j].rewardType == RewardType.Blitz)
            //        {
            //            reward += products[i].rewards[j].amount + " Blitz";
            //        }

            //        else if (products[i].rewards[j].rewardType == RewardType.RemoveAds)
            //        {
            //            reward +=" Remove Ads";
            //        }


            //        if (j < products[i].rewards.Count - 1)
            //        {
            //            reward += " + "; 
            //        }
            //    }
            //    newItem.transform.GetChild(3).GetComponent<LocalizeText>().UpdateText(reward);  // Set concatenated rewards
            //}
        }

        for (int i = 0; i < _containersProductCount.Count; i++)
        {
            HorizontalLayoutGroup _layoutGroup = iapSubPanels[i].iapProductContainer.GetComponent<HorizontalLayoutGroup>();
            float _containerLength = ((_containersProductCount[i].deltaLength + _layoutGroup.spacing) * _containersProductCount[i].productsCount);
            _containerLength += _layoutGroup.padding.left;
            iapSubPanels[i].iapProductContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(_containerLength,
                iapSubPanels[i].iapProductContainer.GetComponent<RectTransform>().sizeDelta.y);
        }
        //Need To Adjust This Price For Promotional IAP
        //promotionalPriceText.GetComponent<LocalizeText>().UpdateText("REMOVE ADS (" + iapProductsContainer.transform.GetChild(0).GetChild(4).transform.GetChild(0).gameObject.GetComponent<Text>().text + ")");

        //HorizontalLayoutGroup _layoutGroup = iapProductsContainer.GetComponent<HorizontalLayoutGroup>();
        //float _containerLength = ((IAPPrefab.GetComponent<RectTransform>().sizeDelta.x + _layoutGroup.spacing) * products.Count);
        //_containerLength += _layoutGroup.padding.left;
        //iapProductsContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(_containerLength,
        //    iapProductsContainer.GetComponent<RectTransform>().sizeDelta.y);
        //promotionalPriceText.GetComponent<LocalizeText>().UpdateText("REMOVE ADS (" + iapProductsContainer.transform.GetChild(0).GetChild(4).transform.GetChild(0).gameObject.GetComponent<Text>().text + ")");
    }

    public void OnSubPanelBtnPressed(int _id)
    {
        SoundController.instance.OnPlayInteractionSound(btnSound);
        if (currentSubPanelId >= 0)
        {
            iapSubPanels[currentSubPanelId].subPanel.SetActive(false);
            iapSubPanels[currentSubPanelId].subPanelBtn.sprite = btnDisplays[0].btnImg;
            iapSubPanels[currentSubPanelId].subPanelBtnTxt.color = btnDisplays[0].textColor;
        }
        currentSubPanelId = _id;
        iapSubPanels[currentSubPanelId].subPanel.SetActive(true);
        iapSubPanels[currentSubPanelId].subPanelBtn.sprite = btnDisplays[1].btnImg;
        iapSubPanels[currentSubPanelId].subPanelBtnTxt.color = btnDisplays[1].textColor;
    }
    /// <summary>
    /// Handles in-app purchase button click by sending a request to StoreManager for the selected product.
    /// </summary>
    /// <param name="index">Index of the product in the inAppProducts list.</param>
    public void OnIAPButtonClicked(int index)
    {
        ShowLoadingPanel();

        StoreManager.Instance.OnRequestPurchase(index);  // Request purchase from StoreManager
        SoundController.instance.OnPlayInteractionSound(btnSound);
    }
    public void EnablePromotionalPanel()
    {
        if (StoreManager.Instance.isIAPInitialized)
        {
            promotionalPanel.SetActive(true);
           // GameManager.instance.CallFireBase("PrmPnlDsplyd");
        }
    }
    public void OnPromotionalRemoveAdsPressed()
    {
        UIController.instance.OnPressCashContainer();
        OnSubPanelBtnPressed(2);
        OnIAPButtonClicked(0);
        promotionalPanel.SetActive(false);
       // GameManager.instance.CallFireBase("PrmRmvPrsd");
    }
    public void OnClosePromotionalPanel()
    {
        promotionalPanel.SetActive(false);
        SoundController.instance.OnPlayInteractionSound(btnSound);
        //GameManager.instance.CallFireBase("PrmRmvClsd");
    }
    public void DisplayEmployeePromoPanel(int _departmentType)
    {
        print(_departmentType);
        employeePromoPanels[_departmentType].promoPanel.SetActive(true);
        List<InAppProduct> _products = StoreManager.Instance.inAppProducts;
        for (int i = 0; i < _products.Count; i++)
        {
            if (_products[i].inAppCategory == IAPCategory.Package)
            {
                for(int j = 0; j < _products[i].rewards.Count; j++)
                {
                    if (((int)_products[i].rewards[j].rewardType) == _departmentType)
                    {
                        employeePromoPanels[_departmentType].promoPriceText.text = _products[i].localizedPriceString;
                        int _k = i;
                        employeePromoPanels[_departmentType].promoButton.onClick.AddListener(()=>OnEmployeePromoPressed(_k, _departmentType));
                        return;
                    }
                }
            }
        }
    }
   
    public void OnEmployeePromoPressed(int _id,int _departmentId)
    {
        UIController.instance.OnPressCashContainer();
        OnSubPanelBtnPressed(0);
        OnIAPButtonClicked(_id);
        employeePromoPanels[_departmentId].promoPanel.SetActive(false);
        //GameManager.instance.CallFireBase("PromoEmp_" + _departmentId.ToString() + "_Prsd");
    }
    public void OnCloseEmpPromoPanel()
    {
        for (int i = 0; i < employeePromoPanels.Length; i++)
        {
            employeePromoPanels[i].promoPanel.SetActive(false);
        }
    }
    /// <summary>
    /// Handles Watch Ads button click by sending a request to StoreManager to trigger a rewarded video.
    /// </summary>
    /// <param name="index">Index of the product in the inAppProducts list.</param>
    public void OnWatchAdsBttonClick(int index)
    {
        //ShowLoadingPanel();
        StoreManager.Instance.OnRequestPurchase(index);  // Request to watch an ad for rewards
        SoundController.instance.OnPlayInteractionSound(btnSound);

    }
    public void CloseSuccessResponsePanel()
    {
        successResponsePanel.SetActive(false);
        SoundController.instance.OnPlayInteractionSound(btnSound);
    }

    public void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }
    public void HideLoadingPnael()
    {
        loadingPanel.SetActive(false);
    }
    public void PlayBtnSound()
    {
        SoundController.instance.OnPlayInteractionSound(btnSound);
    }
    public void OnSuccessPurchase(InAppProduct _product)
    {
        HideLoadingPnael();
        if (_product.displayBtnRef != null)
        {
            _product.displayBtnRef.GetComponent<InAppBtn>().OnSuccessPurchase();
        }
        return;
        successResponsePanel.SetActive(true);
        successResponseImage.sprite = _product.icon;
        string _rewardMsg = "";
        for (int j = 0; j < _product.rewards.Count; j++)
        {
            if (_product.rewards.Count>1 && j >= (_product.rewards.Count-1))
            {
                _rewardMsg += " and ";
            }

            if (_product.rewards[j].rewardType == RewardType.Currency)
            {
                _rewardMsg += (_product.rewards[j].amount.ToString());
                _rewardMsg += " Cash ";
                if (j >= (_product.rewards.Count - 1))
                {
                    _rewardMsg += " received ";
                }
                //  UIController.instance.CashText.text = PlayerDataManager.instance.playerData.playerCash.ToString();
                UIController.instance.UpdateCurrency(_product.rewards[j].amount);
            }
            else if (_product.rewards[j].rewardType == RewardType.Blitz)
            {
                _rewardMsg += (_product.rewards[j].amount.ToString());
                _rewardMsg += " Blitz ";
                if (j >= (_product.rewards.Count - 1))
                {
                    _rewardMsg += " received ";
                }
            }
            else if (_product.rewards[j].rewardType == RewardType.RemoveAds)
            {
                _rewardMsg = "Ads Removed ";
            }
        }
        _rewardMsg += " Successfully";
        successResponseText.GetComponent<LocalizeText>().UpdateText(_rewardMsg);
    }
    public void EnableSuccessResponsePanel(string _msg,Sprite _icon)
    {
        successResponsePanel.SetActive(true);
        successResponseImage.sprite = _icon;
        successResponseText.GetComponent<LocalizeText>().UpdateText(_msg);
    }
    bool isFailurePanelDisplayed = false;

    public void OnFailPurchase(string _msg)
    {
        HideLoadingPnael();
        if (isFailurePanelDisplayed)
            return;
        isFailurePanelDisplayed = true;
        failureResponsePanel.SetActive(true);
        failureResponseText.text = _msg;
        StartCoroutine(DisableFailurePanel());
    }
    IEnumerator DisableFailurePanel()
    {
        yield return new WaitForSeconds(2f);
        isFailurePanelDisplayed = false;
        failureResponsePanel.SetActive(false);
    }
}
