//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[System.Serializable]
//public class PoolCustomerManager : MonoBehaviour
//{
//    [Header("Customer Related")]
//    public GameObject[] avlCustomers;
//    public Transform[] customerSpawnPoints;

//    [SerializeField] float approxCustomerSpawnDelay;
//    [SerializeField] int maxCustomersInPool;
//    [SerializeField] Transform spawnedCustomerParent;
//    public Transform[] poolExitPaths; // Paths customers follow to exit the pool
//    public PoolPositionStatus[] poolPositions;
//    public PoolCustomerPath[] customerPoolPaths;
//    [Space(2)]

//    public int maxCustomersInInnerPool;
//    int currentCustomerInPool;
//    public bool startSpawning = false;
//    float spawningTimer;
//    int spawnedCustomerIndex = 0;
//    int spawnPointIndex = 0;
//    int customersInWaiting = 0;
//    int servedCustomerCount = 0;
//    int customerReachedAtPool = 0;

//    public Transform[] poolPoints; // Array of target points within the pool
//    public List<GameObject> poolCustomerPrefabs; // List of customer prefabs to instantiate
//    public int maxCustomers = 20; // Maximum number of customers allowed at once

//    [Header("Spawn Timers")]
//    public float spawnInterval = 5f; // Time between spawning customers
//    public float minStayDuration = 10f; // Minimum stay time in the pool
//    public float maxStayDuration = 30f; // Maximum stay time in the pool

//    List<GameObject> activeCustomers = new List<GameObject>(); // List of active customers
//    List<GameObject> customersInOuterArea = new List<GameObject>();
//    float spawnTimer; // Timer to track spawning intervals

//    private void Awake()
//    {
//        activeCustomers = new List<GameObject>();
//    }
//    public bool IsCustomersInThePool()
//    {
//        return true;
//    }

//    private void Update()
//    {
//        if (!PoolManager.instance.IsGameStarted())
//            return;
//        if (startSpawning)
//        {
//            // Reduce spawn timer
//            spawnTimer -= Time.deltaTime;

//            // If timer reaches 0, spawn a customer
//            if (spawnTimer <= 0f)
//            {
//                SpawnCustomer();
//            }
//        }
//    }

//    private void SpawnCustomer()
//    {
//        if (activeCustomers.Count >= maxCustomersInPool)
//        {
//            spawnTimer = 3f;
//            return;
//        }
//        int _customerPathIndex = GetPoolCustomerPath();
//        if (_customerPathIndex < 0)
//        {
//            spawnTimer = 3f;
//            return;
//        }
//        GameObject _customer = Instantiate(avlCustomers[spawnedCustomerIndex], customerSpawnPoints[spawnPointIndex].position, Quaternion.identity, spawnedCustomerParent);
//        // Call Pool customer To Move Do It's Chores

//        spawnPointIndex++;
//        spawnedCustomerIndex++;
//        if (spawnPointIndex >= customerSpawnPoints.Length)
//        {
//            spawnPointIndex = 0;
//        }
//        if (spawnedCustomerIndex >= avlCustomers.Length)
//        {
//            spawnedCustomerIndex = 0;
//        }
//        float _minDelay = approxCustomerSpawnDelay - 2f;
//        _minDelay = _minDelay < 0 ? approxCustomerSpawnDelay : _minDelay;
//        spawnTimer = Random.Range(_minDelay, approxCustomerSpawnDelay + 2f);
//    }
//    int GetPoolCustomerPath()
//    {
//        for(int i = 0; i < customerPoolPaths.Length; i++)
//        {
//            if (!customerPoolPaths[i].isOccupied)
//            {
//                return i;
//            }
//        }
//        return -1;
//    }

//    public void CustomerEnteredPool(GameObject _customer)
//    {
//        if (!activeCustomers.Contains(_customer))
//        {
//            activeCustomers.Add(_customer);
//            Debug.Log($"Customer entered the pool: {_customer.name}");
//        }
//    }

//    public void CustomerExitedPool(GameObject _customer)
//    {
//        if (activeCustomers.Contains(_customer))
//        {
//            activeCustomers.Remove(_customer);
//            Debug.Log($"Customer exited the pool: {_customer.name}");
//        }
//    }

//    public void CustomerInOuterPoolArea(GameObject _customer)
//    {
//        if (!customersInOuterArea.Contains(_customer))
//        {
//            customersInOuterArea.Add(_customer);
//            Debug.Log($"Customer entered outer pool area: {_customer.name}");
//        }
//    }

//    public void CustomerExitedOuterPoolArea(GameObject _customer)
//    {
//        if (customersInOuterArea.Contains(_customer))
//        {
//            customersInOuterArea.Remove(_customer);
//            Debug.Log($"Customer exited outer pool area: {_customer.name}");
//        }
//    }

//}

//[System.Serializable]
//public class PoolPositionStatus
//{
//    public Transform positionTransform; // Position in the pool
//    public bool occupiedStatus; // Whether the position is currently occupied
//}

//[System.Serializable]
//public class PoolCustomerPath
//{
//    public string pathName;
//    public bool isOccupied;
//    public Transform poolInnerPointsPrnt;
//    public Transform poolOuterPointsPrnt;
//}