using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billingitemsanimation : MonoBehaviour
{
    // Defines the current state of the item's animation
    public BillingItemsAnimationState animationState;

    // Speed at which the item moves when being billed
    public float speed;

    public int counterId;

    Vector3 destructionPoint;
    //
    private void Start()
    {
        destructionPoint = SuperStoreManager.instance.GetCustomerCartItemsDestructionPoint(counterId).position;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle the item movement after it has been billed
        if (animationState == BillingItemsAnimationState.AfterBill)
        {
            // Move the item towards the destruction point (end of the billing process)
            transform.position = Vector3.Lerp(transform.position, destructionPoint, speed * Time.deltaTime);

            // Check if the item has reached the destruction point
            if (Vector3.Distance(transform.position, destructionPoint) <= 0.1f)
            {
                // Log the item destruction and destroy the game object
                SuperStoreManager.instance.RemoveItemFromList(counterId, gameObject);
                Destroy(gameObject);
            }
        }
    }
}

// Enum to represent the different animation states for billing items
public enum BillingItemsAnimationState
{
    OnDesk,    // Item is on the desk, waiting to be billed
    AfterBill  // Item has been billed and is moving towards destruction
}


