using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    public ObjectRelavance relatedTo;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(relatedTo == ObjectRelavance.SuperStore)
            {
                SuperStoreManager.instance.UpdateAroundStoreTrigger(true);
            }else if (relatedTo == ObjectRelavance.Reception)
            {
                //RoomManager.instance.UpdateReceptionAreaProgress(true);
            }
            else if (relatedTo == ObjectRelavance.Pool)
            {
              //  PoolManager.instance.UpdateAroundStoreTrigger(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (relatedTo == ObjectRelavance.SuperStore)
            {
                SuperStoreManager.instance.UpdateAroundStoreTrigger(false);
            }
            else if (relatedTo == ObjectRelavance.Reception)
            {
                //RoomManager.instance.UpdateReceptionAreaProgress(false);

            }
            else if (relatedTo == ObjectRelavance.Pool)
            {
                //PoolManager.instance.UpdateAroundStoreTrigger(false);
            }
        }
    }
}
