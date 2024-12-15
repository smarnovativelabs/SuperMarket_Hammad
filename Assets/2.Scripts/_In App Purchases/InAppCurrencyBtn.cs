using UnityEngine;
using UnityEngine.UI;
public class InAppCurrencyBtn : InAppBtn
{
    public override void OnInitializeIAPBtn(InAppProduct _product)
    {
        productToBuy = _product;
        headingTxt.GetComponent<LocalizeText>().UpdateText(_product.productName);
        priceTxt.text = _product.localizedPriceString;
        displayImg.sprite = _product.icon;
        GetComponent<Image>().sprite = _product.bgImg;
        string _reward = _product.rewards.Count < 1 ? "" : "Get ";
        if(_product.rewards.Count==1 && _product.rewards[0].amount < 0)
        {
            _reward = "";
        }
        // Loop through each reward in the product
        for (int j = 0; j < _product.rewards.Count; j++)
        {
            if (_product.rewards[j].amount > 0)
            {
                _reward += _product.rewards[j].amount.ToString() + " ";
            }
            _reward+= _product.rewards[j].rewardDesc;
            _reward += (j < (_product.rewards.Count - 2) ? ", " :
                (j < (_product.rewards.Count - 1) ? " and " : ""));
        }
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
        IAPUiManager.Instance.ShowLoadingPanel();
        StoreManager.Instance.OnRequestPurchase(productToBuy);
    }
    public override void OnSuccessPurchase()
    {
        string _reward = "";

        // Loop through each reward in the product
        for (int j = 0; j < productToBuy.rewards.Count; j++)
        {
            _reward += productToBuy.rewards[j].amount.ToString()
                + productToBuy.rewards[j].rewardDesc;
            _reward += (j < (productToBuy.rewards.Count - 2) ? ", " :
                (j < (productToBuy.rewards.Count - 1) ? " and " : ""));

            if (productToBuy.rewards[j].rewardType == RewardType.Currency)
            {
                UIController.instance.UpdateCurrency(productToBuy.rewards[j].amount);
            }
        }
        _reward += " received Successfully";

        IAPUiManager.Instance.EnableSuccessResponsePanel(_reward, productToBuy.icon);
    }
}
