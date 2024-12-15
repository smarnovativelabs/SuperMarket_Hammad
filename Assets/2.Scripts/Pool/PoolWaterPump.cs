//using System.Collections;
//using UnityEngine;

//public class PoolWaterPump : MonoBehaviour, InteractableObjects
//{
//    [SerializeField] bool isThisWaterFillPump;
//    [SerializeField] float rotationSpeed = 50f;

//    public void OnHoverItems()
//    {
//        if (isThisWaterFillPump)
//        {
//            UIController.instance.DisplayHoverObjectName("Tap To Fill Water In The Pool", true, HoverInstructionType.General);
//        }
//        else
//        {
//            UIController.instance.DisplayHoverObjectName("Tap To Empty The Pool", true, HoverInstructionType.General);
//        }

//        UIController.instance.OnChangeInteraction(0, true);

//        if (GetComponent<Outline>())
//        {
//            GetComponent<Outline>().enabled = true;
//        }
//    }

//    public void OnInteract()
//    {
//        if (isThisWaterFillPump)
//        {
//            HandleFillPumpInteraction();
//        }
//        else
//        {
//            HandleEmptyPumpInteraction();
//        }
//    }

//    private void HandleFillPumpInteraction()
//    {
//        int _waterStatus = PoolManager.instance.GetPoolWaterStatus();
//        if (_waterStatus == (int)PoolWaterStatus.Empty)
//        {
//            if(!PoolManager.instance.AreAllPoolInnerTilesPainted())
//            {
//                UIController.instance.DisplayInstructions("First Paint Inner Tiles");
//            }
//            else
//            {
//                PoolManager.instance.DoFillThePool();
//            }
//        }
//        else if (_waterStatus == (int)PoolWaterStatus.Dirty)
//        {
//            UIController.instance.DisplayInstructions("The Pool Is Filled With Dirty Water.");
//        }
//        else
//        {
//            UIController.instance.DisplayInstructions("The Pool Is Filled With Clean Water.");
//        }
//    }

//    private void HandleEmptyPumpInteraction()
//    {
//        if (PoolManager.instance.GetPoolWaterStatus() == (int)PoolWaterStatus.Empty)
//        {
//            UIController.instance.DisplayInstructions("The Pool Is Already Empty.");
//        }
//        else if (PoolManager.instance.GetPoolWaterStatus() == (int)PoolWaterStatus.Dirty)  // next time for dirty implementation remove this and below if statement
//        {
//            PoolManager.instance.DoEmptyThePool();
//        }
//        else if (PoolManager.instance.GetPoolWaterStatus() == (int)PoolWaterStatus.Filled)
//        {
//            UIController.instance.DisplayInstructions("Pool Is Already Filled With Clean Water.");
//        }

//    }

//    public void TurnOffOutline()
//    {
//        if (GetComponent<Outline>())
//        {
//            GetComponent<Outline>().enabled = false;
//        }
//    }

//    public void WaterFillPumpRotation()
//    {
//        // Rotate from 0 to 360
//        StartCoroutine(RotateToAngle(0, 360));
//    }

//    public void WaterOutPumpRotation()
//    {
//        StartCoroutine(RotateToAngle(360, 0));

//    }

//    private IEnumerator RotateToAngle(float _fromZAngle, float _toZAngle)
//    {
//        float _elapsed = 0f;
//        float _duration = 1f;
//        Vector3 _tempAngle = transform.localEulerAngles;
//        transform.localEulerAngles = new Vector3(_tempAngle.x, _tempAngle.y, _fromZAngle);

//        while (_elapsed < _duration)
//        {
//            _elapsed += Time.deltaTime;
//            float progress = Mathf.Clamp01(_elapsed / _duration);

//            float zRotation = Mathf.Lerp(_fromZAngle, _toZAngle, progress);
//            _tempAngle.z = zRotation;
//            // Apply the rotation
//            transform.localEulerAngles = _tempAngle;
//            yield return null;
//        }

//        transform.localEulerAngles = new Vector3(_tempAngle.x, _tempAngle.y, _toZAngle);
//    }
//}
