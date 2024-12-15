using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class MonetizationManager : MonoBehaviour
{
    public static MonetizationManager instance;
    public MonetizingRewards[] monetizingRewardsList;
  //  public List<GameObject> spawnedRewardsObjects = new List<GameObject>();
   // public GameObject hoverBoard;
    public GameObject[] hoverboardlist;

    public float reSpawnTimerVehicle;
    float reSpawnTimerVehicleInitial;
    public bool startResponeVehicleTimer;
    public int totalHoverBoardsAdsWatched;

    public GameObject moneyBag;
    public int totalMoneyBoxesAdsWatched;
    public GameObject deliveryDrone;
    public List<DroneWavePoints> droneSpawnPoints=new List<DroneWavePoints> ();
    public int droneDeliveryIndex = -1;
    public int totalDroneArrived;
    public int moneyItemToSpawn;
    public GameObject briefcase;
    public GameObject safe;


     void Awake()
    {
        instance = this;
    }
    public void InitilizeMonetizationManager()
    {
      
        reSpawnTimerVehicleInitial = reSpawnTimerVehicle;

        //starting for second time
        print("Player total speninding:" + PlayerDataManager.instance.playerData.playerTotalSpending);
       
        if (PlayerDataManager.instance.playerData.playerTotalSpending < 0)
        {
            StartCoroutine(StartDroneDelivery());
        }
        StartCoroutine(EnableAllVehicle(true, 1f));
        reSpawnDroneTimerInitial = reSpawnDroneTimer;

    }

    void Update()
    {
        //respawning vehicles after cool down period
        VehicleCoolDownTimer();
        DroneCoolDownTimer();
    }

    void VehicleCoolDownTimer()
    {
        if (startResponeVehicleTimer)
        {
            reSpawnTimerVehicle -= Time.deltaTime;
            if (reSpawnTimerVehicle <= 0)
            {
                startResponeVehicleTimer = false;
                reSpawnTimerVehicle = reSpawnTimerVehicleInitial;
                if (totalHoverBoardsAdsWatched >= 3)
                {
                    totalHoverBoardsAdsWatched= 0;
                }
                EnableAllVehicle();
            }
        }
    }



    //Spawn
  /*  public IEnumerator MonetizingRewardItems()
    {
        yield return new WaitForSeconds(1f);

        //spawning vechicles
        for(int i = 0; i < 3; i++)
        {
            int emptyIndex = GetEmptySpotForSpawning();
            if (emptyIndex == -1) continue;
            GameObject spawnedItem = Instantiate(hoverBoard, monetizingRewardsList[emptyIndex].spawnPoint.position, monetizingRewardsList[emptyIndex].spawnPoint.rotation);
            spawnedItem.GetComponent<PlayerVechicle>().spawnedIndex = emptyIndex;
            spawnedRewardsObjects.Add(spawnedItem);
            monetizingRewardsList[emptyIndex].isOccupied = true;
           // totalHoverBoardSpawnd++;
            yield return null;
        }

        yield return new WaitForSeconds(.5f);
        //spawn money boxes
       // totalMoneyBoxSpawnd++;
    }*/

   /* public int GetEmptySpotForSpawning()
    {
        List<int> _availableIndexs = new List<int>();

        for (int i = 0; i < monetizingRewardsList.Length; i++)
        {
            if (!monetizingRewardsList[i].isOccupied)
            {
                _availableIndexs.Add(i);
            }
        }
        if (_availableIndexs.Count == 0)
            return -1;

        int _randomIndex = UnityEngine.Random.Range(0, _availableIndexs.Count);
        return _availableIndexs[_randomIndex];
    }


    public void FreeSpaceForNextSpawn(int _index)
    {
        for (int i = 0; i < monetizingRewardsList.Length; i++)
        {
            if (i==_index)
            {
                monetizingRewardsList[i].isOccupied = false;
                break;
            }
        }

    }

   */

    public IEnumerator EnableAllVehicle(bool _status,float time)
    {
        yield return new WaitForSeconds(time);

        for(int i=0;i< hoverboardlist.Length; i++)
        {
            hoverboardlist[i].gameObject.SetActive(_status);
           // hoverboardlist[i].GetComponent<PlayerVechicle>().totalRVWatched = 0;
        }
        yield return null;
        EnableOutLine();
    }

    void EnableOutLine()
    {
        for (int i = 0; i < hoverboardlist.Length; i++)
        {
            hoverboardlist[i].GetComponent<Outline>().enabled = true;
        }
    }

    public void DisableAllVehicle()
    {
        for (int i = 0; i < hoverboardlist.Length; i++)
        {
            hoverboardlist[i].gameObject.SetActive(false);
        
        }
    }
    public void EnableAllVehicle()
    {
        for (int i = 0; i < hoverboardlist.Length; i++)
        {
            hoverboardlist[i].gameObject.SetActive(true);
            hoverboardlist[i].GetComponent<Outline>().enabled = true;
            hoverboardlist[i].GetComponent<Animator>().enabled = true;
        
        }
        EnableOutLine();
    }
    public void StartResponeTimer()
    {
        //if one cycle 3 ads watched
        if (totalHoverBoardsAdsWatched >= 3)
        {
            reSpawnTimerVehicle = reSpawnTimerVehicleInitial;
        }
        else
        {
            reSpawnTimerVehicle = 15f;
        }
        startResponeVehicleTimer = true;
    }
    public void SpawnVehicle()
    {
            totalHoverBoardsAdsWatched = 0;
          /*  int _index = GetEmptySpotForSpawning();
            GameObject spawnedItem = Instantiate(hoverBoard, monetizingRewardsList[_index].spawnPoint.position, monetizingRewardsList[_index].spawnPoint.rotation);
            spawnedItem.GetComponent<PlayerVechicle>().spawnedIndex = _index;
            spawnedRewardsObjects.Add(spawnedItem);
            monetizingRewardsList[_index].isOccupied = true;
           
        */
    }


    #region DroneDelivery


    public IEnumerator StartDroneDelivery()
    {
       
        yield return new WaitForSeconds(1f);
        int _pathIndex = GetEmptySpotForSpawning();
        if(_pathIndex != -1)
        {
            Transform[] _path= droneSpawnPoints[_pathIndex].wavepoints;
            droneSpawnPoints[_pathIndex].isPathUsed = true;
            deliveryDrone.gameObject.SetActive(true);
            deliveryDrone.GetComponent<Drone>().SetWaypoints(_path);
           int randomCase= UnityEngine.Random.Range(0, 2);
            moneyItemToSpawn = randomCase;
            if (randomCase == 0)
            {
              
                deliveryDrone.GetComponent<Drone>().briefcase.gameObject.SetActive(true);
            }
            else
            {
                deliveryDrone.GetComponent<Drone>().Safe.gameObject.SetActive(true);
            }
            deliveryDrone.transform.position= droneSpawnPoints[_pathIndex].wavepoints[0].position;
            deliveryDrone.GetComponent<Drone>().SetMovement(true);
        }
    }

 public int GetEmptySpotForSpawning()
{
   List<int> _availableIndexs = new List<int>();

   for (int i = 0; i < droneSpawnPoints.Count; i++)
   {
       if (!droneSpawnPoints[i].isPathUsed)
       {
           _availableIndexs.Add(i);
       }
   }
   if (_availableIndexs.Count == 0)
       return -1;

   int _randomIndex = UnityEngine.Random.Range(0, _availableIndexs.Count);
   return _availableIndexs[_randomIndex];
}

    public void SpawnMoneyBox(Transform spawnPoint)
    {
  
        if (moneyItemToSpawn == 0)
        {
            
            GameObject _case= Instantiate(briefcase, spawnPoint.position, spawnPoint.rotation);
            _case.AddComponent<Rigidbody>();

            StartCoroutine(AddColliderToIgnoreList(_case.GetComponent<BoxCollider>()));
       
        }
        else
        {
           
            GameObject _safe = Instantiate(safe, spawnPoint.position, spawnPoint.rotation);
            _safe.AddComponent<Rigidbody>();
            StartCoroutine(AddColliderToIgnoreList(_safe.GetComponent<BoxCollider>()));

        }
        UIController.instance.DisplayInstructions("Free stash of cash arrived outside!");
    }

    IEnumerator AddColliderToIgnoreList(BoxCollider collider)
    {
        yield return null;
        Controlsmanager.instance.AddToIgnoreCollision(collider);
    }

    /// <summary>
    /// Rest drone delivery and ad watch counters
    /// </summary>
    public void ResetDroneMechanism()
    {
        for (int i = 0; i < droneSpawnPoints.Count; i++)
        {
            droneSpawnPoints[i].isPathUsed = false;
           
        }
        totalDroneArrived = 0;
        totalMoneyBoxesAdsWatched = 0;
        reSpawnDroneTimer = reSpawnDroneTimerInitial;
        startResponeDroneTimer = true;
    }

    public bool startResponeDroneTimer;
    public float reSpawnDroneTimer;
    public float reSpawnDroneTimerInitial;
    void DroneCoolDownTimer()
    {
        if (startResponeDroneTimer)
        {
            reSpawnDroneTimer -= Time.deltaTime;

            if (reSpawnDroneTimer <= 0)
            {
                if (totalDroneArrived >= 3)
                {
                    reSpawnDroneTimer = reSpawnDroneTimerInitial;
                    print("waiting for three ads watch");
                }
                else
                {
                    reSpawnDroneTimer = 40f;
                    print("waiting for next delivery");
                    StartCoroutine(StartDroneDelivery());
                }
                startResponeDroneTimer = false;
            }
        }
    }

    /// <summary>
    /// Check how many drones arrived in the environment
    /// </summary>
    public void StartNextDroneDeliveryTimer()
    {
        print("Total drone arrived" + totalDroneArrived);
        if (totalDroneArrived < 3)
        {
            //next drone arrival time
            reSpawnDroneTimer = 40f;
            startResponeDroneTimer = true;
        }
        else
        {
            reSpawnDroneTimer = reSpawnDroneTimerInitial;
        }
      
    }

    /// <summary>
    /// When three ads watched now wait for 2 minutes
    /// </summary>
    public void CheckForDronCoolDown()
    {
     
        if (totalMoneyBoxesAdsWatched >= 3)
        {
            ResetDroneMechanism();

        }
    }

    #endregion

    [System.Serializable]
public class MonetizingRewards
{
   public Transform spawnPoint;
   public bool isOccupied;
}
[System.Serializable]
public class DroneWavePoints
{
   public Transform[] wavepoints;
   public bool isPathUsed;
}
}
