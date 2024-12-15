using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;

    public PlayerData playerData;
    public PurchasedCategory purchasedCategories;
    bool isDataInitialized=false;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    #region Currancy
    public void UpdateCash(int _val)
    {
        playerData.playerCash += _val;
        if (_val < 0)
        {
            UpdatePlayerSpending(Mathf.Abs(_val));
        }
    }
    public void UpdatePlayerSpending(int _val)
    {
        if (playerData.playerTotalSpending > -1)
        {
            playerData.playerTotalSpending += _val;

            if (playerData.playerTotalSpending >= 500)
            {
              playerData.playerTotalSpending = -1;
              StartCoroutine(MonetizationManager.instance.StartDroneDelivery());
            }
        }
    }
    public void UpdateBlitz(int _val)
    {
        playerData.playerBlitz += _val;
    }
    public void UpdateXP(int _val)
    {
        playerData.playerXP += _val;

    }
    public void UpdateLevel(int _val)
    {
        playerData.playerLevel += _val;
    }
    #endregion
    public void LoadPlayerData()
    {
        playerData = (PlayerData)SerializationManager.LoadFile("Player");
        if (playerData == null)
        {
            playerData = (PlayerData)SerializationManager.Load("Player");
            if (playerData == null)
            {
                playerData = new PlayerData();
            }
        }
        //for existing user
        if (playerData.playerLevel <= 0) playerData.playerLevel = 1;
        if (playerData.playerBlitz < 0) playerData.playerBlitz = 100;
        if(playerData.playerXP<=0) playerData.playerXP = 0;
        isDataInitialized = true;
    }
    public void SavePlayerData()
    {
        if (isDataInitialized)
        {
            SerializationManager.Save(playerData, "Player");
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            if (GameController.instance == null)
            {
                SavePlayerData();
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (GameController.instance == null)
        {
            SavePlayerData();
        }
    }
    public bool IsItemPurchased(ItemData _item)
    {
        if (purchasedCategories.purchasedCategories.ContainsKey((int)_item.mainCatID))
        {
            PurchasedSubCategory _subCat = purchasedCategories.purchasedCategories[(int)_item.mainCatID];
            if (_subCat.purcahsedSubCategories.ContainsKey(_item.subCatID))
            {
                if (_subCat.purcahsedSubCategories[_item.subCatID].itemIds.Contains(_item.itemID))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        return false;
    }
}

[System.Serializable]
public class PlayerData
{
    public string playerName = "Enter Name";
    public int playerCash;
    public int playerBlitz;
    public int playerXP;
    public int playerLevel;
    public int playerOrderRecieveCount;
    public int playerRewardGivenState;  //Used Only To Identify Users before new env
    public int selectedQualitySettings;
    public int selectedLanguage;
    public int playerTotalSpending;
    public bool qualityPnlDisplayed;
    public bool initialLocalizationSet;
    public Vector3 playerPosition;
    public Quaternion playerRotation;
    public string utcforwelcomebackreward;
    public PlayerData()
    {
  ///revised cash according to the document
        playerCash = 1000;
        playerBlitz = 100;
        playerXP = 0;
        playerLevel = 1;
        selectedLanguage = 0;
        playerRewardGivenState = 1;
        playerOrderRecieveCount = 0;
        selectedQualitySettings = 0;
        qualityPnlDisplayed = false;
        initialLocalizationSet = true;
        playerPosition = new Vector3(0, 0, 0);
        playerRotation = Quaternion.identity;
    }
}
[System.Serializable]
public class PurchasedCategory
{
    public Dictionary<int, PurchasedSubCategory> purchasedCategories = new Dictionary<int, PurchasedSubCategory>();
}
[System.Serializable]
public class PurchasedSubCategory
{
    public Dictionary<int, PurchasedItems> purcahsedSubCategories = new Dictionary<int, PurchasedItems>();
}
[System.Serializable]
public class PurchasedItems
{
    public List<int> itemIds = new List<int>();
}