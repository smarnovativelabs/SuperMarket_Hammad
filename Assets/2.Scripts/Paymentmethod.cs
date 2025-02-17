using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paymentmethod : MonoBehaviour
{
    // The type of payment method (e.g., Cash or Card)
    public PaymentMethod _paymentMethod;

    /// <summary>
    /// Called when the payment method is interacted with (e.g., the user selects Cash or Card).
    /// Triggers the corresponding behavior in the CashCounterManager.
    /// </summary>
    public void OnInteract(int _id)
    {
        print("20d");
        // Log the interaction with the payment method for debugging purposes
        print("Interacted with payment style");

        // Notify the CashCounterManager about the selected payment method (Cash or Card)
      //  CashCounterManager.instance.OnInteractionWithCashOrCard(_paymentMethod);
        SuperStoreManager.instance.OnInteractionWithCashOrCard(_id, _paymentMethod);
    }
}
// Enum representing the different payment methods available in the system
public enum PaymentMethod
{
    Cash,  // Cash payment option
    Card   // Card payment option
}
