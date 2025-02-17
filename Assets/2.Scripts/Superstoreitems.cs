using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;
using TMPro;
using CandyCoded.HapticFeedback;
//using System.Diagnostics;
public class Superstoreitems : MonoBehaviour
{
    // Price of the item, randomly assigned between 1 and 5
    public float Price = 0;

    // Start is called before the first frame update
    private void Start()
    {
        // Assign a random price to the item between 1 and 5
        StartCoroutine(EnableOutline());
    }
    IEnumerator EnableOutline()
    {
        yield return new WaitForSeconds(0.1f);
        if (GetComponent<Outline>())
        {
            GetComponent<Outline>().enabled = true;
        }
    }
    /// <summary>
    /// Called when the item is interacted with (e.g., picked up for billing).
    /// Handles the animation and pricing of the item.
    /// </summary>
    public void OnInteract(int _id)
    {
        SoundController.instance.OnPlayInteractionSound(UIController.instance.uiButtonSound);

        // Set the animation state of the item to 'AfterBill'
        gameObject.GetComponent<Billingitemsanimation>().animationState = BillingItemsAnimationState.AfterBill;

        if (gameObject.GetComponent<BoxCollider>() != null)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        // Instantiate the price tag at the item's position
        GameObject _price = Instantiate(SuperStoreManager.instance.GetItemPricePrefab(_id), transform.position, SuperStoreManager.instance.GetItemPricePrefab(_id).transform.rotation);

        // Set the price text on the price tag
        _price.gameObject.GetComponent<TextMeshPro>().text = "$" + Price.ToString("###.00");

        // Adjust the price tag's position to match the item's position
        _price.transform.position = transform.position;

        // Add the price of this item to the total bill in the cash counter manager
      //  CashCounterManager.instance.AddTotalBill(Price);
        SuperStoreManager.instance.AddToBill(_id,Price);
        if (GetComponent<Outline>())
        {
            GetComponent<Outline>().enabled = false;
        }
        HapticFeedback.LightFeedback();
        print("me calling");
        // Start a coroutine to check if all items have been scanned
        StartCoroutine(CheckBillingComplete(_id));
    }

    public void OnCashierInteract(int _id)
    {
       
        SuperStoreManager.instance.PlayCashCounterSound(_id);
        // CashCounterManager.instance.PlayCashCounterSound(UIController.instance.uiButtonSound);
        // Set the animation state of the item to 'AfterBill'
       
        gameObject.GetComponent<Billingitemsanimation>().animationState = BillingItemsAnimationState.AfterBill;
        
        // Instantiate the price tag at the item's position
        GameObject _price = Instantiate(SuperStoreManager.instance.GetItemPricePrefab(_id), transform.position, SuperStoreManager.instance.GetItemPricePrefab(_id).transform.rotation);
       
        // Set the price text on the price tag
        _price.gameObject.GetComponent<TextMeshPro>().text = "$" + Price.ToString("###.00");
        
        // Adjust the price tag's position to match the item's position
        _price.transform.position = transform.position;
      
        // Add the price of this item to the total bill in the cash counter manager
        // CashCounterManager.instance.AddTotalBill(Price);

        SuperStoreManager.instance.AddToBill(_id, Price);
       
        if (GetComponent<Outline>())
        {
            GetComponent<Outline>().enabled = false;
        }
        // Start a coroutine to check if all items have been scanned
        print("Cashier calling");
        StartCoroutine(CheckBillingComplete(_id));
    }
    /// <summary>
    /// Coroutine to check if all items have been scanned after a short delay.
    /// If all items have been scanned, it triggers the completion of the billing process.
    /// </summary>
    IEnumerator CheckBillingComplete(int _id)
    {
        // Wait for 0.2 seconds to allow for animation and UI updates
        yield return new WaitForSeconds(0.6f);

        // Decrement the total number of items left to scan
        //  CashCounterManager.instance.totalItemsToScan--;

        // If no more items are left to scan, trigger the final billing step
        //  if (CashCounterManager.instance.totalItemsToScan <= 0)
        //  {
        //CashCounterManager.instance.UponAllItemsScanned();
        // }
        print("Scanning item of counter:" + _id);
        SuperStoreManager.instance.UpdateAndCheckTotalItemsScanned(_id);
    }

}
