using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToolsManager : MonoBehaviour
{
    public static ToolsManager instance;

    [System.Serializable]
    public class ToolsInteractionDistance
    {
        public string toolName;
        public float distanceVal;
    }
    public ToolsInteractionDistance[] toolsDistance;
    //public Button actionButton;
    public bool isAutoUse = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //actionButton.onClick.AddListener(OnPressActionButton);
    }

    private void Start()
    {
        //actionButton.gameObject.SetActive(false); // Initially disable the action button
    }

    public void SelectTool(GameObject tool)
    {
        if (tool != null && tool.CompareTag("Tool"))
        {
            GameController.instance.UpdateCurrentPickedTool(tool);
            //actionButton.gameObject.SetActive(true); // Enable the action button

            // Enable the selected tool
            tool.SetActive(true);
            float _distance = 0f;
            for(int i = 0; i < toolsDistance.Length; i++)
            {
                if ((toolsDistance[i].toolName == tool.name))
                {
                    _distance = toolsDistance[i].distanceVal;
                    break;
                }
            }
            PlayerInteraction.instance.SetToolDistance(_distance);
            // Optionally, disable other tools if they should not be active at the same time
            DisableOtherTools(tool);
            if(tool.gameObject.name == "Paint Brush")
            {
                UIController.instance.paintCountContainer.gameObject.SetActive(true);
                UIController.instance.OnChangeInteraction(0, true);
            }
        }
        else
        {
            Debug.LogWarning("Invalid tool selected.");
            if (GameController.instance.currentPicketItem == null)
            {
                ///actionButton.gameObject.SetActive(false); // Disable the action button
            }
        }
    }
    private void DisableOtherTools(GameObject activeTool)
    {
        foreach (Transform tool in GameController.instance.toolParent.transform)
        {
            if (tool.gameObject != activeTool)
            {
                tool.gameObject.SetActive(false);
                UIController.instance.paintCountContainer.gameObject.SetActive(false);
            }
        }
    }
    public void DeactivateAllTools()
    {
        // Deactivate all tools
        foreach (Transform tool in GameController.instance.toolParent.transform)
        {
            tool.gameObject.SetActive(false);
        }
        PlayerInteraction.instance.SetToolDistance(0f);

        UIController.instance.paintCountContainer.gameObject.SetActive(false);
        // Clear the current picked item
        GameController.instance.UpdateCurrentPickedTool(null);
    }

    public void OnPressActionButton()
    {
        var currentTool = GameController.instance.currentPickedTool;
        if (currentTool != null && currentTool.CompareTag("Tool"))
        {
            var toolAction = currentTool.GetComponent<IToolAction>();
            if (toolAction != null)
            {
                toolAction.PerformAction();
            }
            else
            {
                Debug.LogWarning("No action defined for tool: " + currentTool.name);
            }
        }
        else
        {
            Debug.LogWarning("No valid tool selected for action.");
        }
    }
    public void OnAutoActionPressed()
    {
        var currentTool = GameController.instance.currentPickedTool;
        if (currentTool != null && currentTool.CompareTag("Tool"))
        {
            var toolAction = currentTool.GetComponent<IToolAction>();
            if (toolAction != null)
            {
                //   toolAction.AutoPerformAction();
                toolAction.AutoPerformAction();
            }
        }
    }
    public void SetAutoUseButton(bool _enable = true)
    {
        var currentTool = GameController.instance.currentPickedTool;
        if (currentTool != null && currentTool.CompareTag("Tool"))
        {
            PaintBrushTool toolAction = currentTool.GetComponent<PaintBrushTool>();
            if (toolAction != null)
            {
                isAutoUse = _enable;
                toolAction.SetPaintBrushAnim();
                UIController.instance.SetPickBtnColor(_enable ? Color.green : Color.white);
            }
        }
    }
    public bool IsPaintBrushActive()
    {
        bool _isActive = false;
        var currentTool = GameController.instance.currentPickedTool;
        if (currentTool != null && currentTool.CompareTag("Tool"))
        {
            _isActive = currentTool.GetComponent<PaintBrushTool>() != null;

        }
        return _isActive;
    }
}

[System.Serializable]
public enum PlayerTools
{
    EmpthHand=0,
    Dustbin=1,
    WallPaint=2,
    CeilPaint=3,
    Hammer=4,
    Broom=5
}