using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public static VehicleController instance;

    public CustomerController customerController;
    public List<GameObject> carPrefabs; // List of car prefabs
    public List<Transform> startPositions; // List of start positions
    public List<CarPath> carPaths; // List of CarPath objects for each car
    public float spawnInterval = 120f; // Time between spawning cars (e.g., 2 minutes)
    public float speed = 5f;
    public float parkingTime = 10f; // Time to wait at the last position
    public float rotationSpeed = 10f; // Speed of rotation
    public List<bool> shouldLeaveParking; // List of bools to control whether each car should leave parking
    public List<bool> isCarReached; // List to track if each car has reached its destination


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        isCarReached = new List<bool>(new bool[carPrefabs.Count]);
    }

    public void SpawnCar()
    {
        StartCoroutine(SpawnCars());
    }

    IEnumerator SpawnCars()
    {
        yield return new WaitForSeconds(spawnInterval);

        StartCoroutine(SpawnAndMoveCar(0));

        yield return new WaitForSeconds(spawnInterval);
        /* for (int i = 0; i < carPrefabs.Count-1; i++)
         {
             int carIndex = i; // Capture the current index for the coroutine


             // Wait for the spawn interval before spawning the next car
             yield return new WaitForSeconds(spawnInterval);
         }*/
    }

    IEnumerator SpawnAndMoveCar(int carIndex)
    {
        ShouldLeaveparking(false, 0);
        GameObject car = Instantiate(carPrefabs[carIndex], startPositions[carIndex].position, Quaternion.identity);
        yield return StartCoroutine(MoveCarAlongPath(car, carPaths[carIndex].pathPositions));

        car.transform.rotation = Quaternion.Euler(0, 180, 0);
        isCarReached[carIndex] = true;

        Transform lastNode = carPaths[carIndex].pathPositions[carPaths[carIndex].pathPositions.Count - 1];
        customerController.SpawningCharacter(lastNode);
        customerController.currentSpawnCharacter.GetComponent<Customer>().lastNodePosition = lastNode;

        // Wait at the last position
        //yield return new WaitForSeconds(parkingTime);

        print(carIndex+ "    car Index");
        // Wait until shouldLeaveParking for this car is true
        yield return new WaitUntil(() => shouldLeaveParking[carIndex]);

        // Move back to the start position
        List<Transform> reversePath = new List<Transform>(carPaths[carIndex].pathPositions);
        reversePath.Reverse();
        yield return StartCoroutine(MoveCarAlongPath(car, reversePath));

        // Destroy the car at the start position
        Destroy(car);
    }

    public void ShouldLeaveparking(bool _enable, int _index)
    {
        shouldLeaveParking[_index] = _enable;
    }

    IEnumerator MoveCarAlongPath(GameObject car, List<Transform> positions)
    {
        foreach (Transform targetPosition in positions)
        {
            while (Vector3.Distance(car.transform.position, targetPosition.position) > 0.1f)
            {
                // Move the car
                car.transform.position = Vector3.MoveTowards(car.transform.position, targetPosition.position, speed * Time.deltaTime);

                // Rotate the car to face the target position
                Vector3 direction = (targetPosition.position - car.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                car.transform.rotation = Quaternion.Slerp(car.transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

                yield return null;
            }
        }
    }
}


[System.Serializable]
public class CarPath
{
    public List<Transform> pathPositions; // List of path positions for a car
}
