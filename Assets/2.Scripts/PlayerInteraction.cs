using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;
public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction instance;
    public float envInteractionDistance;
    public float toolDistance = 0f;
    public LayerMask interactingLayer;
    public LayerMask placingLayer;
    public Transform pickedObjectParent;
    public InteractableObjects currentInteractingObj;
    public GameObject interactingObj;
    public ParticleSystem dustParticle;
    //public GameObject currentInteractingObjectOnlyForInspector;
    bool gameStarted = false;
    // Start is called before the first frame update
    private void Awake()
    {
        InstanceCheck();
    }
    void Start()
    {
        //GameController.instance.ChangeGameStatus += StartGame;
    }
    void InstanceCheck()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void StartGame(bool _start)
    {
        gameStarted = _start;
    }
    // Update is called once per frame
    void Update()
    {
        InteractWithEnv();
        if (!gameStarted)
            return;

    }

    public void MoveDustParticle(GameObject _obj)
    {
        dustParticle.transform.position = _obj.transform.position;
        dustParticle.transform.rotation = _obj.transform.rotation;
        dustParticle.Play();
    }

    void InteractWithEnv()
    {
        /* Ray _ray = new Ray(transform.position, transform.forward);
         RaycastHit _hit;
         if (Physics.Raycast(_ray, out _hit, envInteractionDistance))
         {
             if (_hit.collider.GetComponent<InteractableObjects>()!=null)
             {
                 currentInteractingObj = _hit.collider.GetComponent<InteractableObjects>();
                 _hit.collider.GetComponent<InteractableObjects>().OnHoverItems();/////////
             }
             else
             {
                 UIController.instance.DisplayHoverObjectName("", false);
                 if (GameController.instance.currentPicketItem == null)
                 {
                     UIController.instance.OnChangeInteraction(0, false);
                 }
                 if (currentInteractingObj != null)
                 {
                     currentInteractingObj.TurnOffOutline();
                 }
                 currentInteractingObj = null;
             }
         }
         else
         {
             if (GameController.instance.currentPicketItem == null)
             {
                 UIController.instance.OnChangeInteraction(0, false);
             }
             if (currentInteractingObj != null)
             {
                 currentInteractingObj.TurnOffOutline();
             }
             UIController.instance.DisplayHoverObjectName("", false);
             currentInteractingObj = null;
         }*/

        Ray _ray = new Ray(transform.position, transform.forward);
        RaycastHit _hit;
        if (Physics.Raycast(_ray, out _hit, envInteractionDistance+toolDistance,interactingLayer))
        {
            InteractableObjects _curObj = null;
            if (_hit.collider.GetComponentInParent<InteractableObjects>() != null)
            {
                _curObj = _hit.collider.GetComponentInParent<InteractableObjects>();
            }
            if (_hit.collider.GetComponent<InteractableObjects>() != null)
            {
                _curObj = _hit.collider.GetComponent<InteractableObjects>();
            }
            if (_curObj != null)
            {
                if (currentInteractingObj != null && _curObj != currentInteractingObj)
                {
                    currentInteractingObj.TurnOffOutline();
                }
                currentInteractingObj = _curObj;
                 
                _curObj.OnHoverItems();
            }
            else
            {
                UIController.instance.DisplayHoverObjectName("", false);
                if (GameController.instance.currentPicketItem == null && !ToolsManager.instance.IsPaintBrushActive())
                {
                    UIController.instance.OnChangeInteraction(0, false);
                }
                if (currentInteractingObj != null)
                {
                    currentInteractingObj.TurnOffOutline();
                }
                currentInteractingObj = null;
            }
        }
        else
        {
            if (GameController.instance.currentPicketItem == null && !ToolsManager.instance.IsPaintBrushActive())
            {
                UIController.instance.OnChangeInteraction(0, false);
            }
            if (currentInteractingObj != null)
            {
                currentInteractingObj.TurnOffOutline();
            }
            UIController.instance.DisplayHoverObjectName("", false);
            currentInteractingObj = null;
        }

        //if (CrossPlatformInputManager.GetButtonUp("Interact"))
        //{
        //    UIController.instance.OnInteractBtnPressed();
        //}
    }
    public void SetToolDistance(float _val)
    {
        toolDistance = _val;
    }
    public void OnInteract()
    {

        /*if (_currentTool != null && _currentTool.gameObject.tag == "Tool")
        {//this check is useless repair this when all working comoplete
            print("tool interacted");
        }
        else
        {}*/
        var _currentItem = GameController.instance.currentPicketItem;
        var _currentTool = GameController.instance.currentPickedTool;
        if (_currentItem != null)
        {
            if (_currentTool!=null && !_currentTool.CompareTag("Tool"))
            {
                _currentItem.GetComponent<ItemPickandPlace>().ThrowPickedObjects();
            }else if (_currentTool == null && currentInteractingObj==null)
            {
                _currentItem.GetComponent<ItemPickandPlace>().ThrowPickedObjects();
            }else if (currentInteractingObj != null)
            {
                currentInteractingObj.OnInteract();
            }
        }
        else
        {
            if (currentInteractingObj != null)
            {
                currentInteractingObj.OnInteract();

            }
        }

    }

    public bool isAlreadyIntreact()
    {
        return (currentInteractingObj != null);
    }
    public List<PlacementRayHitData> GetItemPlacementColliders()
    {
        List<PlacementRayHitData> _collider = new List<PlacementRayHitData>();
        // Get the camera's forward and up directions
        Vector3 forwardDirection = transform.forward; // Forward direction (camera's facing direction)

        // Calculate the rays' directions based on the angles
        List<Vector3> _directions = new List<Vector3>();
        _directions.Add(forwardDirection); // Ray 1: Forward direction
        _directions.Add(Quaternion.Euler(-30, 0, 0) * forwardDirection); // Ray 2: 60 degrees upward
        _directions.Add(Quaternion.Euler(30, 0, 0) * forwardDirection); // Ray 3: 60 degrees downward
        for(int i = 0; i < _directions.Count; i++)
        {
            PlacementRayHitData _data = Raycast(_directions[i]);
            if (_data != null)
            {
                _collider.Add(_data);
            }
        }
        return _collider;
    }
    private PlacementRayHitData Raycast(Vector3 direction)
    {
        Ray ray = new Ray(transform.position, direction);

        // Debugging: Draw the ray in the scene view for visualization
        Debug.DrawRay(ray.origin, ray.direction*10f, Color.red);
        RaycastHit _hit;
        if (Physics.Raycast(ray, out _hit,7f,placingLayer))
        {
            PlacementRayHitData _data = new PlacementRayHitData();
            _data.hitPoint = _hit.point;
            _data.hitCollider = _hit.collider;
            return _data;
        }
        return null;
    }
}

[System.Serializable]
public class PlacementRayHitData
{
    public Vector3 hitPoint;
    public Collider hitCollider;
}