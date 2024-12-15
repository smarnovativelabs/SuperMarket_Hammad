using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculatorbuttons : MonoBehaviour
{ // The value associated with this button (e.g., numbers, "ok", "remove", etc.)
    public string buttonValue;
   // public int counterId;
    /// <summary>
    /// Called when the button is interacted with (e.g., pressed by the user).
    /// Triggers the card swipe machine logic based on the button's value.
    /// </summary>
    public void OnInteract(int _id)
    {
        // Log the interaction and the button value to the console for debugging
        print("Interacted with " + buttonValue);

        // Pass the button value to the CashCounterManager to handle the card swipe machine logic
       // CashCounterManager.instance.OnInteractionWithCardSwipeMachine(buttonValue);
        SuperStoreManager.instance.OnInteractionWithCardSwipeMachine(_id, buttonValue);
        // SoundController.instance.OnPlayInteractionSound(UIController.instance.uiButtonSound);
    }
}
