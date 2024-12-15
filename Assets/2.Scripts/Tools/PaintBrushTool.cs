using UnityEngine;

public class PaintBrushTool : MonoBehaviour, IToolAction
{
    public GameObject brushMesh;
    public Material defaultMaterial;

    public Material currentMaterial;
    public int paintCount;
    public CategoryName mainId;
    public int subCatIdOfPaint;
    public int ItemId;
    public AudioClip toolSound;
    bool isAutoPaintedOnce = false;
    public void PerformAction()
    {
        if (paintCount > 0)
        {
            ToolsManager.instance.SetAutoUseButton(!ToolsManager.instance.isAutoUse);
           // var _index = RoomManager.instance.currentRoomNumber;
            //if (_index > -1)
            //{
            //    SoundController.instance.OnPlayInteractionSound(toolSound);
            //    RoomManager.instance.rooms[_index].CheckRoomProgress();
            //    if (paintCount - 1 <= 0)
            //    {
            //        ResetPaintBrushMaterial();
            //    }
            //}
            TutorialManager.instance.OnCompleteTutorialTask(10);  
        }
        else
        {
            ResetPaintBrushMaterial();

            UIController.instance.DisplayInstructions("Paint is empty, buy a new one");
        }

        if (isAutoPaintedOnce)
        {
            TutorialManager.instance.OnCompleteTutorialTask(12);
        }
        // Add specific paint brush action logic here
    }
    void Start()
    {

    }
    public void AutoPerformAction()
    {
        if (!isAutoPaintedOnce)
        {
            isAutoPaintedOnce = true;
            TutorialManager.instance.OnCompleteTutorialTask(11);

        }
        if (paintCount > 0)
        {
           // var _index = RoomManager.instance.currentRoomNumber;
            //if (_index > -1)
            //{
            //    SoundController.instance.OnPlayInteractionSound(toolSound);
            //    RoomManager.instance.rooms[_index].CheckRoomProgress();
            //    if (paintCount - 1 <= 0)
            //    {
            //        ResetPaintBrushMaterial();
            //    }
            //}

        }
        else
        {
            ResetPaintBrushMaterial();

            UIController.instance.DisplayInstructions("Paint is empty, buy a new one");
        }
    }
    public void ResetPaintBrushMaterial()
    {
        Material[] _mats = brushMesh.GetComponent<MeshRenderer>().materials;
        _mats[1] = defaultMaterial;
        brushMesh.GetComponent<MeshRenderer>().materials = _mats;
       
    }
    public void SetPaintBrushAnim()
    {
        gameObject.GetComponent<Animator>().SetBool("PaintHit", ToolsManager.instance.isAutoUse);
    }
    public void SetPaintProperties(Material _mat, int _count, CategoryName _mainId, int _subId, int _itemId)
    {
        
        currentMaterial = _mat;
        paintCount = _count;
        mainId = _mainId;
        subCatIdOfPaint = _subId;
        ItemId = _itemId;
        Material[] _mats = brushMesh.GetComponent<MeshRenderer>().materials;
        
        _mats[1] = currentMaterial;
        brushMesh.GetComponent<MeshRenderer>().materials = _mats;
    }

}
