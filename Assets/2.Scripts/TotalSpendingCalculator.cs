using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotalSpendingCalculator : MonoBehaviour
{
    public int totalSpent = 0;
    public RoomIndicaters[] rooms;
    public RequiredID ceilingId;
    public RequiredID floorId;
    public RequiredID wallId;
    public int[] fillingPointsPrices;
    public List<Departments> departments;

    public static TotalSpendingCalculator instance;
    [System.Serializable]
    public class RoomIndicaters
    {
        public RequiredID[] indicatorIds;
    }
    [System.Serializable]
    public class RequiredID
    {
        public CategoryName mainCat;
        public int subCatId;
    }
    void Awake()
    {
        instance = this;
    }
    public IEnumerator CalculateTotalSpendings()
    {
        totalSpent = 0;
       // yield return CalculateRoomsSpendings();
       // yield return CalculateGasStationSpending();
        yield return CalculateSuperStoreSpendings();
        yield return CalculateUnplacedItemsSpending();
        yield return CalculateEmployeesSpendings();

        SerializationManager.DeleteFile("_gameData");
    }
    //IEnumerator CalculateRoomsSpendings()
    //{
    //    int _total = 0;
    //    for(int j = 0; j < rooms.Length; j++)
    //    {
    //        int _tempVal = j;
    //       // RoomSaveable _saved= (RoomSaveable)SerializationManager.Load("_RoomData_" + _tempVal);
    //        if (_saved != null)
    //        {
    //            for (int i = 0; i < rooms[j].indicatorIds.Length; i++)
    //            {
    //                if (i >= _saved.placedItemsIds.Count)
    //                {
    //                    break;
    //                }
    //                if (_saved.placedItemsIds[i] >= 0)
    //                {
    //                    ItemData _item = GameManager.instance.GetItem(rooms[j].indicatorIds[i].mainCat,
    //                        rooms[j].indicatorIds[i].subCatId, _saved.placedItemsIds[i]);
    //                    if (_item != null)
    //                    {
    //                        print("Room Item Name-------" + _item.itemName);
    //                        _total += _item.itemPrice;
    //                    }
    //                }
    //            }
    //            yield return null;
    //            List<int> _prevColorIds = new List<int>();
    //            for (int i = 0; i < _saved.floorTextureIds.Count; i++)
    //            {
    //                if (_saved.floorTextureIds[i] >= 0)
    //                {
    //                    if (!_prevColorIds.Contains(_saved.floorTextureIds[i]))
    //                    {
    //                        ItemData _item = GameManager.instance.GetItem(floorId.mainCat,
    //                        floorId.subCatId, _saved.floorTextureIds[i]);
    //                        if (_item != null)
    //                        {
    //                            print("Room Floor Color Name---" + _item.itemName);
    //                            _total += _item.itemPrice;
    //                            int _temp = i;
    //                            _prevColorIds.Add(_saved.floorTextureIds[_temp]);
    //                        }
    //                    }
    //                }
    //            }
    //            _prevColorIds.Clear();
    //            yield return null;

    //            for (int i = 0; i < _saved.wallTextureIds.Count; i++)
    //            {
    //                if (_saved.wallTextureIds[i] >= 0)
    //                {
    //                    if (!_prevColorIds.Contains(_saved.wallTextureIds[i]))
    //                    {
    //                        ItemData _item = GameManager.instance.GetItem(wallId.mainCat,
    //                        wallId.subCatId, _saved.wallTextureIds[i]);
    //                        if (_item != null)
    //                        {
    //                            print("Room Wall Color Name---" + _item.itemName);
    //                            _total += _item.itemPrice;
    //                            int _temp = i;
    //                            _prevColorIds.Add(_saved.wallTextureIds[_temp]);
    //                        }
    //                    }

    //                }
    //            }
    //            _prevColorIds.Clear();
    //            yield return null;

    //            for (int i = 0; i < _saved.ceilingTextureIds.Count; i++)
    //            {
    //                if (_saved.ceilingTextureIds[i] >= 0)
    //                {
    //                    if (!_prevColorIds.Contains(_saved.ceilingTextureIds[i]))
    //                    {
    //                        ItemData _item = GameManager.instance.GetItem(ceilingId.mainCat,
    //                       ceilingId.subCatId, _saved.ceilingTextureIds[i]);
    //                        if (_item != null)
    //                        {
    //                            print("Room Ceiling Color Name---" + _item.itemName);

    //                            _total += _item.itemPrice;
    //                            int _temp = i;
    //                            _prevColorIds.Add(_saved.ceilingTextureIds[_temp]);
    //                        }
    //                    }
    //                }
    //            }

    //            yield return null;
    //            SerializationManager.DeleteFile("_RoomData_" + _tempVal);
    //        }
    //    }
    //    totalSpent += (_total / 2);
    //}

    //IEnumerator CalculateGasStationSpending()
    //{
    //    GasStationData _stationData = (GasStationData)SerializationManager.Load("_GasData");
    //    if (_stationData != null)
    //    {
    //        print("GasStation Delivery---" + _stationData.inDeliveryFuel + "----Reserved----" + _stationData.reserveGas);

    //        totalSpent += (int)_stationData.inDeliveryFuel;
    //        totalSpent += (int)_stationData.reserveGas;
    //        //Get Filling Points prices
    //        if (_stationData.activeFillingPointsIndex != null)
    //        {
    //            for (int i = 0; i < _stationData.activeFillingPointsIndex.Count; i++)
    //            {
    //                int _temp = _stationData.activeFillingPointsIndex[i];
    //                if (_temp < 0 || _temp >= fillingPointsPrices.Length)
    //                {
    //                    continue;
    //                }
    //                print("Fuel Machine----" + _temp);
    //                totalSpent += fillingPointsPrices[_temp];
    //            }
    //        }
            
    //        yield return null;
    //        SerializationManager.DeleteFile("_GasData");
    //    }
    //}

    IEnumerator CalculateSuperStoreSpendings()
    {
        SuperStoreSavableData _storeData= (SuperStoreSavableData)SerializationManager.Load("_SuperSoreData");
        if (_storeData != null)
        {
            for(int i = 0; i < _storeData.placedItems.Count; i++)
            {
                SuperStoreItems _storeItem = _storeData.placedItems[i];
                ItemData _item = GameManager.instance.GetItem((CategoryName)_storeItem.catId, _storeItem.subCatId, _storeItem.itemId);
                if (_item != null)
                {
                    totalSpent += (int)_item.unitItemPurPrice;
                    print("SuperMarket Item " + _item.itemName);
                }
            }
            yield return null;
            SerializationManager.DeleteFile("_SuperSoreData");
        }
    }

    IEnumerator CalculateUnplacedItemsSpending()
    {
        PurchasedItemsData _purchasedData = (PurchasedItemsData)SerializationManager.Load("PurchasedItems");
        if (_purchasedData != null)
        {
            int _total = 0;
            for(int i = 0; i < _purchasedData.savedItems.Count; i++)
            {
                ItemSavingProps _prop = _purchasedData.savedItems[i];
                ItemData _item = GameManager.instance.GetItem((CategoryName)_prop.mainCatId, _prop.subCatId, _prop.itemId);
                if (_item != null)
                {
                    print("Unplaced Item----" + _item.itemName);
                    _total += _item.itemPrice;
                }
            }
            yield return null;

            SerializationManager.DeleteFile("PurchasedItems");
            totalSpent += (_total / 2);
        }
       
    }

    IEnumerator CalculateEmployeesSpendings()
    {
        EmplayeesSaveData _saveData = (EmplayeesSaveData)SerializationManager.Load("EmployeesData");
        if (_saveData != null)
        {
            for(int j = 0; j < _saveData.savedEmployees.Count; j++)
            {
                EmployeeSavableData _emp = _saveData.savedEmployees[j];
                EmployeeType _type = (EmployeeType)_emp.employeeType;
                Departments _curDept = null;
                for (int i = 0; i < departments.Count; i++)
                {
                    if (departments[i].deptEmployeeType == _type)
                    {
                        _curDept = departments[i];
                        break;
                    }
                }
                if (_curDept == null)
                {
                    print("Invalid Department");
                    continue;
                }
                if (_emp.employeeId >= _curDept.avlEmployees.Count)
                {
                    print("Invalid Employe ID");
                    continue;
                }
                EmployeeData _employee = _curDept.avlEmployees[_emp.employeeId];
                for(int k = 0; k < _employee.hiringCosts.Count; k++)
                {
                    if (_employee.hiringCosts[k].type == CostType.Cash)
                    {
                        print("Employeeee-----" + _employee.employeeType + "------" + _employee.employeeTag);
                        totalSpent += (int)_employee.hiringCosts[k].price;
                    }
                }
                yield return null;

            }
            SerializationManager.DeleteFile("EmployeesData");
        }
    }
}
