using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;
    public int instructionCounter;
    //public GameObject footStepsTools;
    public TutorialTasks[] tutorial;
    public TutorialTasks[] variantBTutorial;

    int firebaseQuarter = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Start()
    {
        //footStepsTools.SetActive(false);
        if (GameController.instance.gameData.motelOpenStatus)
        {
            return;
        }
        if (GameManager.instance.selectedDeliveryMode == 0)
        {
            if (PlayerPrefs.GetInt("completeIndex", 0) < tutorial.Length)
            {
                StartTutorial();
            }
            else
            {
                UIController.instance.UpdateTutorialPanel("");
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("completeIndex", 0) < variantBTutorial.Length)
            {
                StartTutorial();
            }
            else
            {
                UIController.instance.UpdateTutorialPanel("");
            }
        }
        
       
    }

    public void OnCompleteTutorialTask(int _index = 0)
    {
        if (_index != instructionCounter)
        {
            return;
        }
        if (GameManager.instance.selectedDeliveryMode == 0)
        {
            tutorial[instructionCounter].tutorialObjects.SetActive(false);
            instructionCounter++;
            PlayerPrefs.SetInt("completeIndex", instructionCounter);
            //GameManager.instance.CallFireBase("TutInsPress_" + instructionCounter.ToString(), "tutorial", _index);
            FirePercentEvent();

            if (instructionCounter >= tutorial.Length)
            {
                //Call GameController from here
                OnFinishTutorial();
                return;
            }

            tutorial[instructionCounter].tutorialObjects.SetActive(true);
            UIController.instance.UpdateTutorialPanel(tutorial[instructionCounter].taskText);

            if (tutorial[instructionCounter].tutorialObjects.GetComponent<Animator>())
            {
                tutorial[instructionCounter].tutorialObjects.GetComponent<Animator>().enabled = true;
                UIController.instance.UpdateTutorialPanel(tutorial[instructionCounter].taskText);
            }
        }
        else
        {
            variantBTutorial[instructionCounter].tutorialObjects.SetActive(false);
            instructionCounter++;
            PlayerPrefs.SetInt("completeIndex", instructionCounter);
          //  GameManager.instance.CallFireBase("TutInsPress_" + instructionCounter.ToString(), "tutorial", _index);
            FirePercentEvent();

            if (instructionCounter >= variantBTutorial.Length)
            {
                //Call GameController from here
                OnFinishTutorial();
                return;
            }

            variantBTutorial[instructionCounter].tutorialObjects.SetActive(true);
            UIController.instance.UpdateTutorialPanel(variantBTutorial[instructionCounter].taskText);

            if (variantBTutorial[instructionCounter].tutorialObjects.GetComponent<Animator>())
            {
                variantBTutorial[instructionCounter].tutorialObjects.GetComponent<Animator>().enabled = true;
                UIController.instance.UpdateTutorialPanel(variantBTutorial[instructionCounter].taskText);
            }
        }
        

    }

    void FirePercentEvent()
    {
        float _percent = 1f;
        if (GameManager.instance.selectedDeliveryMode == 0)
        {
            _percent = ((float)instructionCounter) / ((float)tutorial.Length);
        }
        else
        {
            _percent = ((float)instructionCounter) / ((float)variantBTutorial.Length);

        }

        _percent *= 100f;
        int _currenQuarter = (int)(_percent / 25);
        if (_currenQuarter > firebaseQuarter)
        {
            //GameManager.instance.CallFireBase("TutWatch_" + (_currenQuarter * 25).ToString(), "percent", _currenQuarter);
            firebaseQuarter = _currenQuarter;
        }
    }
    public void StartTutorial()
    {
        instructionCounter = PlayerPrefs.GetInt("completeIndex", 0);
        if (GameManager.instance.selectedDeliveryMode == 0)
        {
            tutorial[instructionCounter].tutorialObjects.SetActive(true);
            UIController.instance.UpdateTutorialPanel(tutorial[instructionCounter].taskText);
        }
        else
        {
            variantBTutorial[instructionCounter].tutorialObjects.SetActive(true);
            UIController.instance.UpdateTutorialPanel(variantBTutorial[instructionCounter].taskText);
        }
    }

    public void OnFinishTutorial()
    {
        UIController.instance.UpdateTutorialPanel("");
    }

    public void InstructionPopActive()
    {
        if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
        {
            if (GameManager.instance.selectedDeliveryMode == 0)
            {
                tutorial[instructionCounter].tutorialObjects.SetActive(false);
            }
            else
            {
                variantBTutorial[instructionCounter].tutorialObjects.SetActive(false);
            }
        }
        else
        {
            //UIController.instance.InstrutionPopup.SetActive(true);
        }
    }

    public void FootStepsEnable()
    {
        if (PlayerPrefs.GetInt("toolFootStep", 0) == 0)
        {
            //footStepsTools.SetActive(true);
        }
        else
        {
            return;
        }
    }

    public void TurnOffFootSteps()
    {
        if (PlayerPrefs.GetInt("toolFootStep", 0) == 1)
        {
            return;
        }
        PlayerPrefs.SetInt("toolFootStep", 1);
        //footStepsTools.SetActive(false);
    }
}


    [System.Serializable]
public class TutorialTasks
{
    public GameObject tutorialObjects;
    public string taskText;
}