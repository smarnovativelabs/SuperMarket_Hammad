using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;
public class Floor : ItemPickandPlace, InteractableObjects
{
    public void OnHoverItems()
    {
        if (ToolsManager.instance.isAutoUse)
        {
            var toolPicked = GameController.instance.currentPickedTool;

            if (toolPicked != null)
            {
                var paintBrushTool = toolPicked.GetComponent<PaintBrushTool>();

                if (paintBrushTool != null)
                {
                    if (paintBrushTool.paintCount > 0)
                    {
                        if (paintBrushTool.subCatIdOfPaint == 1)// && paintBrushTool.mainId == CategoryName.Paint && RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].AreAllTrashTasksComplete())
                        {
                            if (paintBrushTool.ItemId != itemId)
                            {
                                ApplyColor(paintBrushTool);
                                ToolsManager.instance.OnAutoActionPressed();
                            }
                        }
                    }
                }
            }
            UIController.instance.DisplayHoverObjectName("", false);

        }
        else
        {
            if (GameController.instance.currentPicketItem == null)
            {
                var currentTool = GameController.instance.currentPickedTool;
                if (currentTool == null)
                {
                    UIController.instance.DisplayHoverObjectName("Select Paint Brush To Paint Floors!", true, HoverInstructionType.Tools);
                }
                else if (currentTool.GetComponent<PaintBrushTool>() != null)
                {
                    if (currentTool.GetComponent<PaintBrushTool>().subCatIdOfPaint == 1)
                    {
                        UIController.instance.DisplayHoverObjectName("Tap To Paint Floors!", true, HoverInstructionType.General);
                        UIController.instance.OnChangeInteraction(0, true);
                    }
                    else
                    {
                        UIController.instance.DisplayHoverObjectName("Buy Floor Paint To Paint Floors!", true, HoverInstructionType.GameStore);
                    }
                }
                else
                {
                    UIController.instance.DisplayHoverObjectName("Select Paint Brush To Paint Floors!", true, HoverInstructionType.Tools);
                }
                if (gameObject.GetComponent<Outline>())
                {
                    gameObject.GetComponent<Outline>().enabled = true;
                }
            }

        }
    }

    public void OnInteract()
    {
        var toolPicked = GameController.instance.currentPickedTool;

        if (toolPicked == null)
        {
            UIController.instance.DisplayInstructions("Pick relevant tool to paint");
            return;
        }

        var paintBrushTool = toolPicked.GetComponent<PaintBrushTool>();

        if (paintBrushTool == null)
        {
            UIController.instance.DisplayInstructions("Pick brush to paint");
            return;
        }

        if (paintBrushTool.paintCount <= 0)
        {
            UIController.instance.DisplayInstructions("Paint is empty, buy a new one");
            return;
        }

        if (paintBrushTool.subCatIdOfPaint == 1)// && paintBrushTool.mainId == CategoryName.Paint && RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].AreAllTrashTasksComplete())
        {
            if (paintBrushTool.ItemId != itemId)
            {
                ApplyColor(paintBrushTool);
            }
            else
            {
                UIController.instance.DisplayInstructions("Already Painted");
            }
        }
        else
        {
            UIController.instance.DisplayInstructions($"This is not {itemName} Paint");
        }
    }
    void ApplyColor(PaintBrushTool paintBrushTool)
    {
        paintBrushTool.paintCount--;
        if (paintBrushTool.paintCount <= 0)
        {
            ToolsManager.instance.SetAutoUseButton(false);
        }
        if (gameObject.GetComponent<ItemPickandPlace>().itemId < 0)
        {
            //RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].inactiveCountofFloor++;
        }
        // RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].CheckRoomProgress();
        UIController.instance.SetPaintCountContainer(paintBrushTool.paintCount);
        ChangePaintMaterial();
        gameObject.GetComponent<ItemPickandPlace>().itemId = paintBrushTool.ItemId;
        // RoomManager.instance.SetFloorItemId(placedRoomIndex, indexInRoomList, paintBrushTool.ItemId);
        HapticFeedback.LightFeedback();
        // RoomManager.instance.rooms[RoomManager.instance.currentRoomNumber].OnFloorPaint(RoomManager.instance.currentRoomNumber);
    }
    public override void OnSpawnItem(ItemData _data)
    {
        var _material = _data.PaintMaterial;
        if (_material != null)
        {
            gameObject.GetComponent<Renderer>().material = _material;
        }
        else
        {
            print(_data.itemPrefab.gameObject.name);
            Debug.LogError("No material");
        }
    }

    public void ChangePaintMaterial()
    {
        var toolPicked = GameController.instance.currentPickedTool;
        gameObject.GetComponent<Renderer>().material = toolPicked.GetComponent<PaintBrushTool>().currentMaterial;
    }

    public void TurnOffOutline()
    {
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }
}
