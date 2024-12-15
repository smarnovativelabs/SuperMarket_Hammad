//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class GasStationTrigger : MonoBehaviour
//{
//    public int fillingPointIndex;
//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.gameObject.CompareTag("Player"))
//        {
//            GasStationManager.instance.OnPlayerEnterGasStation(fillingPointIndex);
//        }
//    }
//    private void OnTriggerExit(Collider other)
//    {
//        if (other.gameObject.CompareTag("Player"))
//        {
//            GasStationManager.instance.OnPlayerExitGasStation(fillingPointIndex);
//        }
//    }
//}
