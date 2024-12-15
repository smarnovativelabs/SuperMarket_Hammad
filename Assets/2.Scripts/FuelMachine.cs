//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;

//public class FuelMachine : MonoBehaviour,InteractableObjects
//{
//    public TextMeshPro reserveGasText;
//    public TextMeshPro currentFuelText;
//    public TextMeshPro fuelPriceText;
//    public TextMeshPro currentPriceText;
//    public TextMeshPro orderedFuelText;
//    public GameObject fuelBar;
//    public GameObject fuelNozel;
//    public GameObject fuelNozelPoint;
//    public int connectedFillingPointIndex;
//    float maxFuel;
//    public bool isFueling = false;
//    public bool isUnlocked = false;

//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }
//    void Update()
//    {
//        if (isFueling)
//        {
//            GasStationManager.instance.AddFuelToVehicle(connectedFillingPointIndex);
//        }
//    }
//    public void UpdateGasReserve()
//    {
//        reserveGasText.text = GasStationManager.instance.gasData.reserveGas.ToString("##0.0#");
//    }
//    public void UpdateFuelPriceText()
//    {
//        fuelPriceText.text = GasStationManager.instance.gasData.gasPrice.ToString();
//    }
//    public void UpdateCurrentFuelText(float _val)
//    {
//        if (_val > maxFuel)
//            _val = maxFuel;

//        currentFuelText.text = _val.ToString("#0.0#");
//        currentPriceText.text = (_val * GasStationManager.instance.gasData.gasPrice).ToString("####.##");
//        float _percent = 1 - ((maxFuel - _val) / maxFuel);
//        Vector3 _targetScale = Vector3.one * _percent;
//        _targetScale.z = 1f;
//        fuelBar.transform.localScale = _targetScale;
//    }
//    public void ResetValues()
//    {
//        fuelBar.transform.localScale = new Vector3(0f, 0f, 1f);
//        currentFuelText.text = "00.00";
//        currentPriceText.text = "00.00";
//        orderedFuelText.text = "00.00";
//    }
//    public void SetmaxFuel(float _val)
//    {
//        maxFuel = _val;
//    }
//    public void SetOrderedFuelText(float _val)
//    {
//        orderedFuelText.text = _val.ToString("#0.00");
//    }
//    public void OnInteract()
//    {
//        if (!isUnlocked)
//        {
//            int _price= GasStationManager.instance.GetFillingPointPrice(connectedFillingPointIndex);
//            if (_price > PlayerDataManager.instance.playerData.playerCash)
//            {
//                UIController.instance.EnableNoCashPanel();
//                GameManager.instance.CallFireBase("NoCshFlMchn");
//                return;
//            }
//            GasStationManager.instance.UnlockFillingPoint(connectedFillingPointIndex);
//            PlayerDataManager.instance.UpdateCash(-1 * _price);
//            UIController.instance.UpdateCurrency(-1 * _price);
//          //  StartCoroutine(UIController.instance.AddCurrancy());
//            UIController.instance.DisplayInstructions("Fuel Machine Unlocked!");
//            return;
//        }
//        if (isFueling)
//        {
//            GameManager.instance.CallFireBase("FuelingStopped");
//            GasStationManager.instance.UpdateFuelingStatus(connectedFillingPointIndex,false);
//            GetComponent<AudioSource>().Stop();
//            isFueling = false;
//            GasStationManager.instance.UpdateGameProgressText();
//            return;
//        }
        
//        if (!GasStationManager.instance.IsNozelAttachedToVehicle(connectedFillingPointIndex))
//        {
//            if (fuelNozel.GetComponent<FuelNozel>().employeeHaveNozel)
//            {
//                return;
//            }
//            if (!fuelNozel.GetComponent<FuelNozel>().IsNozelSelected())
//            {
//                fuelNozel.GetComponent<FuelNozel>().OnInteract();
//                return;
//            }
//            fuelNozelPoint.GetComponent<FuelMachineNozelPoint>().OnInteract();
//            return;
//        }
//        if (GasStationManager.instance.gasData.reserveGas <= 0)
//        {
//            UIController.instance.DisplayInstructions("Do Not Have Fuel Order Fuel From PC!");
//            return;
//        }
//        if(GasStationManager.instance.UpdateFuelingStatus(connectedFillingPointIndex, true))
//        {
//            GameManager.instance.CallFireBase("FuelingStarted");
//            isFueling = true;
//            GasStationManager.instance.UpdateGameProgressText();
//            if (SoundController.instance.isSoundOn)
//                GetComponent<AudioSource>().Play();
//        }
//    }
//    public void OnEmployeeStartFueling()
//    {
//        if (isFueling)
//        {
//            return;
//        }
//        if (GasStationManager.instance.IsNozelAttachedToVehicle(connectedFillingPointIndex))
//        {
//            if (GasStationManager.instance.gasData.reserveGas <= 0)
//            {
//                return;
//            }
//            if(GasStationManager.instance.UpdateFuelingStatus(connectedFillingPointIndex, true))
//            {
//                isFueling = true;
//                if (SoundController.instance.isSoundOn)
//                    GetComponent<AudioSource>().Play();
//            }
//        }
//    }
//    public void OnEmployeeStopFueling()
//    {
//        if (!isFueling)
//        {
//            return;
//        }
//        if (GasStationManager.instance.UpdateFuelingStatus(connectedFillingPointIndex, false))
//        {
//            print("Called To Stop Fueling!");
//            GetComponent<AudioSource>().Stop();
            
//        }
//        isFueling = false;
//    }
//    public void TurnOffOutline()
//    {
//        if (GetComponent<Outline>())
//        {
//            GetComponent<Outline>().enabled = false;
//        }
//    }
//    public bool IsFueling()
//    {
//        return isFueling;
//    }

//    public void OnHoverItems()
//    {
//        if (GetComponent<Outline>())
//        {
//            GetComponent<Outline>().enabled = true;
//        }
//        UIController.instance.OnChangeInteraction(0, true);
//        if (!isUnlocked)
//        {
//            UIController.instance.DisplayHoverObjectName("Unlock Fuel Machine!",true,HoverInstructionType.Warning);
//            return;
//        }

//        if (isFueling)
//        {
//            UIController.instance.DisplayHoverObjectName("Stop Fueling", true,HoverInstructionType.Warning);
//        }
//        else
//        {
//            if (!fuelNozel.GetComponent<FuelNozel>().IsNozelSelected())
//            {
//                UIController.instance.DisplayHoverObjectName("Tap to pick up the fuel pump nozzle", true,HoverInstructionType.General);

//                return;
//            }
//            UIController.instance.DisplayHoverObjectName("Start Fueling", true,HoverInstructionType.General);
//        }
//    }
//}
