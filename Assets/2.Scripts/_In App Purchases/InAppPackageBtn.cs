using UnityEngine;
using UnityEngine.UI;

public class InAppPackageBtn : InAppBtn
{
    [System.Serializable]
    public class RewardDisplays
    {
        public Image rewardImg;
        public Text rewardtext;
    }
    public RewardDisplays[] rewardDisplays;

    public override void OnInitializeIAPBtn(InAppProduct _product)
    {
        productToBuy = _product;
        headingTxt.GetComponent<LocalizeText>().UpdateText(_product.productName);

        priceTxt.text = _product.localizedPriceString;
        displayImg.sprite = _product.icon;
        GetComponent<Image>().sprite = _product.bgImg;

        for (int j = 0; j < _product.rewards.Count; j++)
        {
            if (j >= rewardDisplays.Length)
            {
                Debug.LogError("Rward Exceeded");
                break;
            }

            rewardDisplays[j].rewardtext.GetComponent<LocalizeText>().UpdateText(_product.rewards[j].rewardDesc);
            rewardDisplays[j].rewardImg.sprite = (_product.rewards[j].rewardImg);
        }
    }
    public void OnPressPurchase()
    {
        IAPUiManager.Instance.PlayBtnSound();
        if (productToBuy == null)
        {
            IAPUiManager.Instance.OnFailPurchase("Failed To Process Request!");
            return;
        }
        bool _canPurchase = true;
        for (int i = 0; i < productToBuy.rewards.Count; i++)
        {
            if ((int)productToBuy.rewards[i].rewardType < 4)
            {
                _canPurchase = EmployeeManager.Instance.CanPurchaseEmployee((int)(productToBuy.rewards[i].rewardType));
                if (!_canPurchase)
                    break;
            }
        }

        if (!_canPurchase)
        {
            IAPUiManager.Instance.OnFailPurchase("No More Workplace Available For This Employee");
            return;
        }
        IAPUiManager.Instance.ShowLoadingPanel();
        StoreManager.Instance.OnRequestPurchase(productToBuy);
    }
    public override void OnSuccessPurchase()
    {
        IAPUiManager.Instance.EnableSuccessResponsePanel(productToBuy.productName + " Purchased Successfully!", productToBuy.icon);

        for (int j = 0; j < productToBuy.rewards.Count; j++)
        {
            if ((int)productToBuy.rewards[j].rewardType < 4)
            {
                EmployeeManager.Instance.SpawnPermanentEmployee((int)productToBuy.rewards[j].rewardType);
            }
            if (productToBuy.rewards[j].rewardType == RewardType.Currency)
            {
                UIController.instance.UpdateCurrency(productToBuy.rewards[j].amount);
            }

        }
       
    }
}
