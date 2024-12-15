using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RackPickableDetector : MonoBehaviour
{
    public GameObject rackReference;
    public void OnHoverItems()
    {
        if(GameController.instance.currentPickedTool==null && GameController.instance.currentPicketItem == null)
        {
            rackReference.GetComponent<InteractableObjects>().OnHoverItems();
        }
       
       
    }

    public void OnInteract()
    {
        if (GameController.instance.currentPickedTool == null && GameController.instance.currentPicketItem == null)
        {
            rackReference.GetComponent<InteractableObjects>().OnInteract();
        }

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
