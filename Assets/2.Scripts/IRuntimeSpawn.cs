using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRuntimeSpawn
{
    public abstract void OnNewSpawnItem();
    public abstract void OnSpawnSavedItem();
}
