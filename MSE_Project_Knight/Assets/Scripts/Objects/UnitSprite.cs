using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSprite : ScriptObject
{
    private SPUM_Prefabs prefab;

    private void Start()
    {
        prefab = GetComponent<SPUM_Prefabs>();
        prefab.PlayAnimation(0);
    }

    public void DespawnSprite()
    {
        transform.SetParent(ObjectManager.Instance.childrenTransform.Find(child => child.name == this.prefabName));
        ObjectManager.Instance.Despawn<UnitSprite>(this);
    }
}
