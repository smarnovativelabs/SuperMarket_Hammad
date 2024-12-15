using IAPProject;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
public class StoreManager : MonoBehaviour
{
 
    public static StoreManager Instance { get; private set; }
    Purchaser purchaserScript;
    public List<InAppProduct> inAppProducts;
    InAppProduct _adsProductInstance;
    public bool isIAPInitialized;

    PluginInitializationStatus initializationStatus;
    
    // Image LoadingPanel;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;  
            DontDestroyOnLoad(gameObject);
            purchaserScript = gameObject.GetComponent<Purchaser>();
            initializationStatus = PluginInitializationStatus.Loading;
            //initlilize all IAP based purchasable items
        }
        else
        {
            Destroy(gameObject);  
        }
    }

    private void Start()
    {
        
    }
    public void InitializeStore()
    {
        if (!Instance.isIAPInitialized)
        {
            purchaserScript.InitializAllProducts(inAppProducts);
        }
    }
    public void OnInitializationComplete(IStoreController _storeController)
    {
        isIAPInitialized = true;
        for (int i = 0; i < inAppProducts.Count; i++)
        {
            if (inAppProducts[i].purchaseMethod == PurchaseType.InAppPurchase)
            {
                inAppProducts[i].localizedPrice = _storeController.products.WithID(inAppProducts[i].productID).metadata.localizedPrice;
                inAppProducts[i].localizedPriceString = _storeController.products.WithID(inAppProducts[i].productID).metadata.localizedPriceString;
            }
        }
        initializationStatus = PluginInitializationStatus.Loaded;
    }
    public void OnUpdateRVCashPrice(int _val)
    {
        if (inAppProducts.Count < 2)
        {
            return;
        }
        inAppProducts[1].rewards[0].amount = _val;
    }
    public int GetCashRVAmount()
    {
        if (inAppProducts.Count < 2)
        {
            return 40;
        }
        return inAppProducts[1].rewards[0].amount;
    }
    public void OnInitializationFailed()
    {
        initializationStatus = PluginInitializationStatus.Failed;
    }
    public bool GetInitializationResponse()
    {
        return initializationStatus != PluginInitializationStatus.Loading;
    }
    public void OnRequestRestore()
    {
        if (!isIAPInitialized)
        {
            if (IAPUiManager.Instance != null)
            {
                IAPUiManager.Instance.OnFailPurchase("Unable To Purchase Item");
            }else if (MainMenu.instance != null)
            {
                MainMenu.instance.DisplayResponseText("Unable To Restore!");
            }
            return;
        }
        purchaserScript.RestorePurchases();
    }
    public void OnRequestPurchase(int _index)
    {
        PurchaseType purchaseMethod= inAppProducts[_index].purchaseMethod;

        if (purchaseMethod == PurchaseType.RewardedVideo)
        {
            //_adsProductInstance = new InAppProduct();
            _adsProductInstance = inAppProducts[_index];
           // AdsMediation.AdsMediationManager.instance.ShowRewardedVideo(OnRVSuccess, OnRVFailed);
        }
        else
        {
            if (!isIAPInitialized)
            {
                if (IAPUiManager.Instance != null)
                {
                    IAPUiManager.Instance.OnFailPurchase("Unable To Purchase Item");
                }
                return;
            }
            purchaserScript.OnPurchaseItem(inAppProducts[_index].productID);
        }
       // GameManager.instance.CallFireBase("IAPBtn_" + _index.ToString());
    }
    public void OnRequestPurchase(InAppProduct _product)
    {
        if (_product.purchaseMethod == PurchaseType.RewardedVideo)
        {
            //_adsProductInstance = new InAppProduct();
            _adsProductInstance = _product;
            //AdsMediation.AdsMediationManager.instance.ShowRewardedVideo(OnRVSuccess, OnRVFailed);
        }
        else
        {
            if (!isIAPInitialized)
            {
                if (IAPUiManager.Instance != null)
                {
                    IAPUiManager.Instance.OnFailPurchase("Unable To Purchase Item");
                }
                return;
            }
            purchaserScript.OnPurchaseItem(_product.productID);
        }
        for (int i = 0; i < inAppProducts.Count; i++)
        {
            if (_product.productID == inAppProducts[i].productID)
            {
              //  GameManager.instance.CallFireBase("IAPBtn_" + i.ToString());
                break;
            }
        }
    }
    public void OnSuccessPurchase(string _productId)
    {
        InAppProduct _product = inAppProducts.Find(p => p.productID == _productId);
    
        if (_product != null)
        {
            OnProccessPurchase(_product);

            if (IAPUiManager.Instance != null)
            {
                IAPUiManager.Instance.OnSuccessPurchase(_product);
            }else if (MainMenu.instance != null)
            {
                MainMenu.instance.DisplayResponseText("Purchase Successfull");
            }
        }
    }

    // Failure callback when the purchase fails
    public void OnPurchaseFailed(string message)
    {
        if (IAPUiManager.Instance != null)
        {
            IAPUiManager.Instance.OnFailPurchase(message);
        }
    }

    public void OnRVSuccess(string message)
    {
        if (_adsProductInstance != null)
        {
            OnProccessPurchase(_adsProductInstance);
            if (IAPUiManager.Instance != null)
            {
                IAPUiManager.Instance.OnSuccessPurchase(_adsProductInstance);
            }
        }
    }
    /// <summary>
    /// When user fail to watch ad inform him about failure reason
    /// </summary>
    /// <param name="_reason"></param>
    public void OnRVFailed(string _reason)
    {
        if (IAPUiManager.Instance != null)
        {
            IAPUiManager.Instance.OnFailPurchase(_reason);
        }
    }

    /// <summary>
    /// Give Successfully Purchased Items To Player 
    /// </summary>
    /// <param name="_product"></param>
    void OnProccessPurchase(InAppProduct _product)
    {
        if (_product != null)
        {
            for (int j = 0; j < _product.rewards.Count; j++)
            {
                // Check reward type and concatenate the reward text
                if (_product.rewards[j].rewardType == RewardType.Currency)
                {
                    PlayerDataManager.instance.UpdateCash(_product.rewards[j].amount);
                }
                else if (_product.rewards[j].rewardType == RewardType.Blitz)
                {
                    // Implement When Blitz Are Added
                }
                else if(_product.rewards[j].rewardType == RewardType.RemoveAds)
                {
                    //AdsMediation.AdsMediationManager.instance.OnRemoveAds();
                }
            }
        }
    }
}

// Enums for purchase method and reward type
public enum PurchaseType
{
    InAppPurchase,
    RewardedVideo
}

public enum RewardType
{
    Cashier = 0,
    Receptionist = 1,
    FuelAttendants = 2,
    Cleaner = 3,
    Currency=4,
    Blitz=5,
    RemoveAds=6
}
[System.Serializable]
public class RewardItem
{
    public RewardType rewardType;
    public Sprite rewardImg;
    public string rewardDesc;
    public int amount;

}
[System.Serializable]
public enum IAPCategory
{
    Package,
    Employees,
    SingleItem   
}

[System.Serializable]
public class InAppProduct
{
    public string productID;
    public string productName;
    [HideInInspector] public float price;
    [HideInInspector] public decimal localizedPrice;
    public string localizedPriceString;
    public ProductType type;
    public PurchaseType purchaseMethod;
    public IAPCategory inAppCategory;
    [HideInInspector]public GameObject displayBtnRef;
    public GameObject iapBtnRef;
    public Sprite icon;
    public Sprite bgImg;
    public List<RewardItem> rewards;
}
