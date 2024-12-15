using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBox : ItemPickandPlace, InteractableObjects,IRuntimeSpawn
{
    public Material paintMaterial;
    public int paintCount = 30;

    public void Start()
    {
        paintCount = 30;
    }
    public void OnHoverItems()
    {
        UIController.instance.DisplayHoverObjectName(itemName, true);
        UIController.instance.OnChangeInteraction(0, true);

        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = true;
        }
    }

    public void OnInteract()
    {
        var toolPicked = GameController.instance.currentPickedTool;
        var itemPick = GameController.instance.currentPicketItem;

        if (itemPick == null && toolPicked == null)
        {
            SetObjectToCam();
            GameController.instance.UpdateCurrentPickedItem(gameObject);

        }
        else if (toolPicked.gameObject.name == "Paint Brush" )
        {
            if (toolPicked.GetComponent<PaintBrushTool>().paintCount <= 0)
            {
                toolPicked.GetComponent<PaintBrushTool>().SetPaintProperties(paintMaterial, paintCount, mainCat, SubCatId, itemId);
                var _count = toolPicked.GetComponent<PaintBrushTool>().paintCount;
                UIController.instance.SetPaintCountContainer(_count);
                gameObject.SetActive(false);
                //TutorialManager.instance.OnCompleteTutorialTask(11);

            }
            else
            {
                UIController.instance.DisplayInstructions("Use Existing Paint");
            }
        }
        else
        {
            UIController.instance.DisplayInstructions(GameController.instance.currentPickedTool.gameObject.name + " is already picked");
        }
    }

    public void TurnOffOutline()
    {
        if (gameObject.GetComponent<Outline>())
        {
            gameObject.GetComponent<Outline>().enabled = false;
        }
    }

    public void OnNewSpawnItem()
    {
        var toolPicked = GameController.instance.currentPickedTool;
        var itemPick = GameController.instance.currentPicketItem;

        if (itemPick == null && toolPicked == null)
        {
            gameObject.SetActive(true);
            gameObject.transform.parent = GameController.instance.parentOFPickedObj.transform;

            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<BoxCollider>())
                    transform.GetChild(i).GetComponent<BoxCollider>().enabled = false;

            }

            Collider[] _colliders = GetComponents<Collider>();

            for (int i = 0; i < _colliders.Length; i++)
            {
                _colliders[i].enabled = false;
            }
            if (GetComponent<Rigidbody>())
            {
                GetComponent<Rigidbody>().isKinematic = true;
            }
            itemCondition = ItemState.ready;

            transform.localPosition = pickedPosition;
            transform.localRotation = Quaternion.Euler(pickedAngle);
            transform.localScale = Vector3.one * camChildScale;
            UIController.instance.OnChangeInteraction(1, true);
            GameController.instance.UpdateCurrentPickedItem(gameObject);
           // RoomManager.instance.EnablePlacableItemIndicater(mainCat, SubCatId);
        }
        else
        {
            UIController.instance.DisplayInstructions("Item is already picked");
        }
    }
    public void OnSpawnSavedItem()
    {

    }
}

