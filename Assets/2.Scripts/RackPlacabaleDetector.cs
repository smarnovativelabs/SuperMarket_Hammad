using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackPlacabaleDetector : MonoBehaviour,InteractableObjects
{
    public GameObject rackReference;
    public void OnHoverItems()
    {
        rackReference.GetComponent<InteractableObjects>().OnHoverItems();

    }

    public void OnInteract()
    {
        rackReference.GetComponent<InteractableObjects>().OnInteract();
    }

    public void TurnOffOutline()
    {
        rackReference.GetComponent<InteractableObjects>().TurnOffOutline();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
