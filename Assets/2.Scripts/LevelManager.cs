using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;  
    public LevelData[] levels;           
    public LevelUpSaveData saveData;      
  //  private bool isDataInitialized;     

    void Awake()
    {
        Instance = this;
    }
 
    /// <summary>
    /// Checks if the player qualifies for a level-up based on current XP.
    /// </summary>
    public void CheckForLevelUp()
    {
   
        if (PlayerDataManager.instance.playerData.playerXP >= GetRequiredXP())
        {
            LevelUp();
        }

        UIController.instance.UpdateMotelLevelAndXPBar();
    }


    /// <summary>
    /// Executes the level-up process, showing rewards if not previously displayed.
    /// </summary>
    private void LevelUp()
    {
        int playerCurrentLevel = PlayerDataManager.instance.playerData.playerLevel;
        PlayerDataManager.instance.UpdateLevel(1);
      //  GameManager.instance.CallFireBase("Level_" + playerCurrentLevel.ToString() + "_Up");
        LevelUpUiManager.Instance.ShowLevelUpPanel();
       // RoomManager.instance.OnLevelUpdate();
       // GasStationManager.instance.UpdateFillingPointsLockState();
        SuperStoreManager.instance.UpdateCashCountersLockState();
        SuperStoreManager.instance.UpdateSuperMarketExpensionLockState();
        SuperStoreManager.instance.UpdateSuperStoreLocking();
       // PoolManager.instance.SetPoolLockStatus();
        //CleanerManager.instance.EnableAllCleanersUponRequiredLevelReached();
      //  CleanerManager.instance.UnlockAllCleanersAtRequiredLevel();
        UIController.instance.OnUpdateLevel();
    }


    /// <summary>
    /// Adds XP to the player and checks for level-up.
    /// </summary>
    public void AddXP()
    {
        PlayerDataManager.instance.UpdateXP(100);  
      //  CheckForLevelUp();
       // UIController.instance.UpdateMotelLevelAndXPBar();
        UIController.instance.UpdateXP(100);
    }

    public int GetRequiredXP()
    {
       int _xpRequired;

        int _playerCurrentLevel = PlayerDataManager.instance.playerData.playerLevel;
        _playerCurrentLevel=_playerCurrentLevel <= 0 ? 1 : _playerCurrentLevel;

        _xpRequired = (100 * _playerCurrentLevel) * (_playerCurrentLevel + 3);
      //  print("Required is_" + _xpRequired);
        return _xpRequired;
    }
    public int GetLeveRequiredXPs(int _level)
    {
        if (_level < 1)
        {
            return 0;
        }
        int _xpRequired;

        _xpRequired = (100 * _level) * (_level + 3);
        //  print("Required is_" + _xpRequired);
        return _xpRequired;
    }
    public string GetCurrentAndNextLevelRequiredXPS()
    {
        return PlayerDataManager.instance.playerData.playerXP + "/" + GetRequiredXP();

    }

    public float GetLevelBarFillValue()
    {
        int _prevLevelXps = GetLeveRequiredXPs(PlayerDataManager.instance.playerData.playerLevel - 1);

        return ((float)(PlayerDataManager.instance.playerData.playerXP - _prevLevelXps) / (float)(GetRequiredXP() - _prevLevelXps));
        
    }
}

[System.Serializable]
public class LevelUpSaveData
{
    public List<int> levelsPanelDisplayed;

    public LevelUpSaveData()
    {
        levelsPanelDisplayed = new List<int>();
    }
}
