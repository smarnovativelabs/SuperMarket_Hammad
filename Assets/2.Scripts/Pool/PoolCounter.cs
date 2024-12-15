//using UnityEngine;

//public class PoolCounter : MonoBehaviour, InteractableObjects
//{
//    public void OnHoverItems()
//    {
//        if(PoolManager.instance.IsCounterBuyed())
//        {
//            UIController.instance.DisplayHoverObjectName("Pool Counter", true, HoverInstructionType.General);
//        }
//        else
//        {
//            UIController.instance.DisplayHoverObjectName("Buy The Pool Counter", true, HoverInstructionType.General);
//            UIController.instance.OnChangeInteraction(0, true);
//        }
//        if (GetComponent<Outline>())
//        {
//            GetComponent<Outline>().enabled = true;
//        }

//    }

//    public void OnInteract()
//    {
//        if (!PoolManager.instance.IsCounterBuyed())
//        {
//            PoolManager.instance.OnCounterPurchase();
//        }
//    }

//    public void TurnOffOutline()
//    {
     
//    }
//}
