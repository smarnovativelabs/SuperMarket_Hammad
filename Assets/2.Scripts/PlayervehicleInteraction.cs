using JetBrains.Annotations;
using KinematicCharacterController.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerVechicle;

public class PlayervehicleInteraction : MonoBehaviour
{
    //refrence to the hoverboard disabled initilally and wil be enabled when player ride it
    public GameObject hoverBoard;
    GameObject tolleyActive;
    public Transform trollyParent;
    public GameObject ridingVehicle;
    public AudioClip hoverbaordSound;
    public AudioClip trollySound;
    [HideInInspector]
    public AudioClip currentClip;
    public AudioSource audioSource;
    //vehicle type can be hver board or scooter or anything else
    public PlayerVechicle.VechicleType VechicleType;
    public float acquireTime;
    public bool startTimer;
    public int ridingVehicleIndex = -1;
    public bool isTimeBasedVehicle;


    public void SetAndStartVehicleAcquiringTime(float _acquireTime)
    {
       // acquireTime = _acquireTime;
        acquireTime=((_acquireTime * GameController.instance.dayCycleMinutes) * 60f);
        startTimer = true;
    }

    private void Update()
    {
        if (startTimer && isTimeBasedVehicle)
        {
            acquireTime-=Time.deltaTime;
            if (acquireTime <= 0)
            {
                if (ridingVehicle != null)
                {
                    ridingVehicle.GetComponent<PlayerVechicle>().totalRVWatched = 0;
                }
                ExitVehicle();
                startTimer = false;
              //  MonetizationManager.instance.StartResponeTimer();
                return;
            }
            int _remSec = Mathf.FloorToInt(acquireTime);
            int _hours = (_remSec / 60);
            int _totalMinutes = _remSec % 60;
            //int _days = _hours / GameController.instance.dayCycleMinutes;
            int remHours = _hours;// % GameController.instance.dayCycleMinutes;
            string _timeString = string.Format("{0:00} : {1:00}", remHours.ToString() + "H", _totalMinutes.ToString() + "M");
            UIController.instance.playerVechicleTimerText.text = _timeString;
           //set this time to UI
        }
    }
    public void EnterVechicle(float _speed,PlayerVechicle.VechicleType _vechicleType,float _acquireTime,int _spawnedIndex,GameObject _ridingVehicle)//,GameObject _ridingVehicle)
    {
        Debug.Log("This is the Riding Vehicle Now ~~~~~" + _ridingVehicle);
        print("This is the Vehicle Type  Now ~~~~~" + _vechicleType);
        ridingVehicleIndex = _spawnedIndex;
        VechicleType = _vechicleType;
        ridingVehicle = _ridingVehicle;
        //enabling dummmy vehicle inside player
        if (VechicleType == PlayerVechicle.VechicleType.HoverBoard)
        {
            audioSource.clip = hoverbaordSound;
            Controlsmanager.instance.charactercontroller.setMaxSpeed(0);
            isTimeBasedVehicle = true;
            hoverBoard.gameObject.SetActive(true);
            hoverBoard.GetComponent<Animator>().SetBool("mount",true);
            StartCoroutine(EnableSpeedyMovement(_speed));
            Invoke("MountHoverBoard", 0.8f);
            acquireTime = ((_acquireTime * GameController.instance.dayCycleMinutes) * 60f);
            startTimer = true;
            UIController.instance.playerVehicleTimerContainer.gameObject.SetActive(true);
        }

       else if (VechicleType == PlayerVechicle.VechicleType.Trolly)
        {
            print("Inside if of Trolley~~~~~~~~");
            audioSource.clip = trollySound;
            print("This is the Riding Vehicle Now ~~~~~21" + _ridingVehicle);
            ridingVehicle.SetActive(true);
            print("This is Trolly Parent" + trollyParent);
            ridingVehicle.transform.SetParent(trollyParent);
            //ridingVehicle.SetActive(true);
            ridingVehicle.transform.localPosition = Vector3.zero;
            ridingVehicle.transform.localRotation =Quaternion.identity;
            ridingVehicle.GetComponent<Outline>().enabled = false;
            ridingVehicle.GetComponent<BoxCollider>().enabled = false;
            Controlsmanager.instance.UpdateCharacterSpeed(_speed);
        }
      
        UIController.instance.EnableVehicleControllerPanel(true);

    }

    public void EnterVechicleAgain(float _speed, PlayerVechicle.VechicleType _vechicleType, int _spawnedIndex, GameObject _ridingVehicle)
    {
        Debug.Log("why76");
        Debug.Log("This is the Riding Vehicle Now" + _ridingVehicle.name);
        print("This is the Vehicle Type  Now ~~~~~" + _vechicleType);
        ridingVehicleIndex = _spawnedIndex;
        VechicleType = _vechicleType;
        ridingVehicle = _ridingVehicle;
       

        //enabling dummmy vehicle inside player
        if (VechicleType == PlayerVechicle.VechicleType.HoverBoard)
        {
            audioSource.clip = hoverbaordSound;
            ridingVehicle.SetActive(false);
            Controlsmanager.instance.charactercontroller.setMaxSpeed(0);
            isTimeBasedVehicle = false;
            hoverBoard.gameObject.SetActive(true);
            hoverBoard.GetComponent<Animator>().SetBool("mount", true);
            StartCoroutine(EnableSpeedyMovement(_speed));
            Invoke("MountHoverBoard", 0.8f);
        }

        else if (VechicleType == PlayerVechicle.VechicleType.Trolly)
        {
            audioSource.clip = trollySound;
            //  trolly.gameObject.SetActive(true);
            ridingVehicle.SetActive(true);
            ridingVehicle.transform.SetParent(trollyParent);
            ridingVehicle.transform.localPosition = Vector3.zero;
            ridingVehicle.transform.localRotation = Quaternion.identity;
            ridingVehicle.GetComponent<Outline>().enabled = false;
            ridingVehicle.GetComponent<BoxCollider>().enabled = false;
            Controlsmanager.instance.UpdateCharacterSpeed(_speed);
        }
     
        startTimer = true;
        UIController.instance.playerVehicleTimerContainer.gameObject.SetActive(true);
        UIController.instance.EnableVehicleControllerPanel(true);
    }
    void MountHoverBoard()
    {
        Controlsmanager.instance.gameObject.transform.GetChild(0).gameObject.GetComponent<Controllerinputmanager>().MakePlayerJump();
        Invoke("StopJumpForHoverBoard", 0.2f);
    }

    void StopJumpForHoverBoard()
    {
        Controlsmanager.instance.gameObject.transform.GetChild(0).gameObject.GetComponent<Controllerinputmanager>().StopJump();
    }

    IEnumerator EnableSpeedyMovement(float vehicleSpeed)
    {
        yield return new WaitForSeconds(1.0f);
        Controlsmanager.instance.UpdateCharacterSpeed(vehicleSpeed);
    }

    public void ExitVehicle() {
        print("Exit button is called");
        startTimer = false;
        print("Vehicle Type in Exit Vehicle button" + VechicleType);
    //    VechicleType = PlayerVechicle.VechicleType.HoverBoard;
     //   print("Vehicle Type in Exit Vehicle button After Set Hard Code" + VechicleType);

        if (VechicleType == PlayerVechicle.VechicleType.HoverBoard)
        {
            print("Exit button is called in hoverboard");
            hoverBoard.GetComponent<Animator>().SetBool("unmount", true);
            hoverBoard.gameObject.SetActive(false);
            Vector3 spawnPosition = Controlsmanager.instance.kinematicCharacterMotor.gameObject.transform.position + Controlsmanager.instance.kinematicCharacterMotor.gameObject.transform.forward * 1f;
            if (ridingVehicle == null) return;
            ridingVehicle.transform.position = new Vector3(spawnPosition.x, 0, spawnPosition.z);
            ridingVehicle.SetActive(true);
          
            ridingVehicle.GetComponent<Outline>().enabled = false;
            ridingVehicle.GetComponent<Animator>().enabled = false;
        }

       else if (VechicleType == PlayerVechicle.VechicleType.Trolly)
        {
            print("Exit button is called in trolley");
            // trolly.gameObject.SetActive(false);
            if (ridingVehicle == null) return;
            ridingVehicle.transform.SetParent(null);
           // Vector3 spawnPosition = Controlsmanager.instance.kinematicCharacterMotor.gameObject.transform.position + Controlsmanager.instance.kinematicCharacterMotor.gameObject.transform.forward * 1f;
          //  ridingVehicle.transform.position = new Vector3(spawnPosition.x, 0, spawnPosition.z);
          //  ridingVehicle.SetActive(true);
            ridingVehicle.GetComponent<Outline>().enabled = false;
          ////  ridingVehicle.transform.localPosition = trollyParent.transform.localPosition;
           // ridingVehicle.transform.localRotation = trollyParent.transform.localRotation;
            ridingVehicle.GetComponent<BoxCollider>().enabled = true;


        }
        ridingVehicle = null;
        UIController.instance.playerVehicleTimerContainer.gameObject.SetActive(false);
        //change character speed to normal again
        Controlsmanager.instance.UpdateCharacterSpeed(Controlsmanager.instance.characterMoveSpeed);
        UIController.instance.EnableVehicleControllerPanel(false);
        MonetizationManager.instance.StartResponeTimer();

    }

    public GameObject GetRidingVehicle()
    {
        return ridingVehicle;
    }
    public bool IsRiding()
    {
       if(ridingVehicle!=null) return true;
       else return false;
    }

    public void OnExitVehicle(){

        if (ridingVehicle != null && VechicleType == PlayerVechicle.VechicleType.Trolly)
        {
           if(ridingVehicle.transform.GetChild(0).gameObject.GetComponent<TrolleyCollision>().collisionCount!=0)
            {
                UIController.instance. DisplayInstructions("Move the Trolley to a proper spot first!");
                return;
            }
        }
        ExitVehicle();
        //call method here realted to trolly stuff
    }
}
