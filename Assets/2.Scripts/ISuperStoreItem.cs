using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISuperStoreItem 
{
    public abstract void PlaceItemsInRack(GameObject _rack);
    public abstract bool IsPlacingProducts();
}
