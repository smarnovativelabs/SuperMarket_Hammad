//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;


//public class PaintGun : ItemPickandPlace, InteractableObjects
//{
//    public void OnHoverItems()
//    {
//        UIController.instance.DisplayHoverObjectName(gameObject.name, true);
//        UIController.instance.OnChangeInteraction(0, true);

//        //code for outline object

//        if (gameObject.GetComponent<Outline>())
//        {
//            gameObject.GetComponent<Outline>().enabled = true;
//        }
//    }

//    public void OnInteract()
//    {
//        print(gameObject.name + " Interacted");
//        SetObjectToCam();
//        GameController.instance.UpdateCurrentPickedItem(gameObject);
//    }

//    public void PlaceItems()
//    {

//    }

//    public void TurnOffOutline()
//    {
//        if (gameObject.GetComponent<Outline>())
//        {
//            gameObject.GetComponent<Outline>().enabled = false;
//        }
//    }
//}
