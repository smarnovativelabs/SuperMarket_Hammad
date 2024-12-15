using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currancynote :MonoBehaviour
{

    // Index representing the type of currency note (e.g., $1, $5, etc.)
    public int currancyIndex;

    // The value of the currency note (e.g., 1.0f for $1, 5.0f for $5)
    public float currancyValue;

    // Boolean flag to determine if the currency is in cents
    public bool isCent;

    /// <summary>
    /// Handles interaction with the currency note. 
    /// This method triggers when the player interacts with the currency note, creating cash at the current position.
    /// </summary>
    public void OnInteract()
    {
        SoundController.instance.OnPlayInteractionSound(UIController.instance.uiButtonSound);
        // Log when the currency note is interacted with
        print("Hovering on currency");

        // Call CashCounterManager to create cash at a slightly offset position
        //CashCounterManager.instance.CreateCash(
        //    currancyIndex,                     // The index/type of the currency
        //    currancyValue,                     // The value of the currency
        //    transform.position + new Vector3(0, 0.2f, 0), // Position slightly above the current position
        //    gameObject,                        // The current currency game object
        //    isCent                             // Whether it's in cents
        //);
    }
}
