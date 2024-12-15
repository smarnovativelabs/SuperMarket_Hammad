using UnityEngine;
using UnityEngine.UI;
public class InAppEmployeeBtn : InAppBtn
{
    public override void OnInitializeIAPBtn(InAppProduct _product)
    {
        productToBuy = _product;
        headingTxt.GetComponent<LocalizeText>().UpdateText(_product.productName);
        priceTxt.text = _product.localizedPriceString;
        displayImg.sprite = _product.icon;
        GetComponent<Image>().sprite = _product.bgImg;
        string _reward = _product.rewards.Count < 1 ? "" : "Get ";
        //string _reward = "";


        // Loop through each reward in the product
        for (int j = 0; j < _product.rewards.Count; j++)
        {
            _reward += _product.rewards[j].rewardDesc;
            _reward += (j < (_product.rewards.Count - 2) ? ", " :
                (j < (_product.rewards.Count - 1) ? " and " : ""));
        }
        //_reward += " Permanently";
        rewardDesc.GetComponent<LocalizeText>().UpdateText(_reward);
    }
    public void OnPressPurchase()
    {
        IAPUiManager.Instance.PlayBtnSound();
        if (productToBuy == null)
        {
            IAPUiManager.Instance.OnFailPurchase("Failed To Process Request!");
            return;
        }
        bool _canPurchase = false;
        for (int i = 0; i < productToBuy.rewards.Count; i++)
        {
            _canPurchase = EmployeeManager.Instance.CanPurchaseEmployee((int)(productToBuy.rewards[i].rewardType));
            if (!_canPurchase)
                break;
        }

        if (!_canPurchase)
        {
            IAPUiManager.Instance.OnFailPurchase("Can Not Hire More Employees Of This Type!");
            return;
        }
        IAPUiManager.Instance.ShowLoadingPanel();
        StoreManager.Instance.OnRequestPurchase(productToBuy);
    }
    public override void OnSuccessPurchase()
    {
        string _reward = "";
        // Loop through each reward in the product
        for (int j = 0; j < productToBuy.rewards.Count; j++)
        {
            _reward += productToBuy.rewards[j].rewardDesc;
            _reward += (j < (productToBuy.rewards.Count - 2) ? ", " :
                (j < (productToBuy.rewards.Count - 1) ? " and " : ""));
            EmployeeManager.Instance.SpawnPermanentEmployee((int)productToBuy.rewards[j].rewardType);
        }
        _reward += " Hired Successfully!";

        IAPUiManager.Instance.EnableSuccessResponsePanel(_reward, productToBuy.icon);
    }
}
