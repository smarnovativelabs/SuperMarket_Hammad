using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InteractableObjects
{
    public abstract void OnHoverItems();
    public abstract void OnInteract();
    public abstract void TurnOffOutline();
}
