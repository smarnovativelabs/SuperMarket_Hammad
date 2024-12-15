using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static PlayerVechicle;
//using UnityEditor.Build.Pipeline;

public class Moneyboxes : MonoBehaviour, InteractableObjects
{
   // public VechicleType vechicleType;
    public string boxName;
    public int cashReward;
    public int requiredRVToUnlocked;
    public int totalRVWatched;
    public TextMeshProUGUI totalAdsText;
    Animator animator;
    public GameObject lid;
    public GameObject canvas;
    public GameObject dropEffect;
    public GameObject glowEffect;
  
    void Start()
    {
        updateUI();
        StartCoroutine(EnableOutline());
        animator = lid.gameObject.GetComponent<Animator>();
     //   Invoke("startRotate",1.0f);
       
    }

    void startRotate()
    {
        gameObject.GetComponent<Rotatereward>().enabled = true;
    }

    void EnableGlow()
    {
        glowEffect.gameObject.SetActive(true);
        glowEffect.GetComponent<ParticleSystem>().Play();
    }

    

    IEnumerator EnableOutline()
    {
        yield return null;
        gameObject.GetComponent<Outline>().enabled = true;
    }
    void OnSuccessRV()
    {
        totalRVWatched++;
        if (totalRVWatched >= requiredRVToUnlocked)
        {
            canvas.SetActive(false);
            MonetizationManager.instance.totalMoneyBoxesAdsWatched++;
            MonetizationManager.instance.CheckForDronCoolDown();
            gameObject.GetComponent<Rotatereward>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.AddComponent<BillBoard>();
       
            UIController.instance.ShowMoneyBoxRVPanel(false);
          
            StartCoroutine(openRewardBox());
        }
        updateUI();
    }

    IEnumerator openRewardBox()
    {
        yield return new WaitForSeconds(1f);
        //give user reward
       // PlayerDataManager.instance.playerData.playerCash += cashReward;
        PlayerDataManager.instance.UpdateCash(cashReward);
        UIController.instance.UpdateCurrency(cashReward);
        UIController.instance.DisplayInstructions("$"+cashReward+" Reward collected!");
        Destroy(gameObject);
    }

    void updateUI()
    {
        int required = (requiredRVToUnlocked - totalRVWatched);
        print("required rv :" + required);
        totalAdsText.text = required.ToString();
    }

    void OnFailureRV()
    {

    }
    public void OnHoverItems()
    {
      
        int required = (requiredRVToUnlocked - totalRVWatched);
        UIController.instance.EnableMoneyBoxPanel(cashReward, requiredRVToUnlocked, OnSuccessRV, OnFailureRV);
    }

    public void OnInteract()
    {
     
        int required = (requiredRVToUnlocked - totalRVWatched);
        UIController.instance.EnableMoneyBoxPanel(cashReward, requiredRVToUnlocked, OnSuccessRV, OnFailureRV);
    }

    public void TurnOffOutline()
    {
        UIController.instance.ShowMoneyBoxRVPanel(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") return;
        
        dropEffect.gameObject.SetActive(true);
        animator.enabled = true;
    
        transform.rotation=Quaternion.Euler(0, 0, 0);
        startRotate();
        Destroy(gameObject.GetComponent<Rigidbody>());
        EnableGlow();
    }
}
