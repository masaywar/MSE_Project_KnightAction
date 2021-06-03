using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour, ICompanion
{
    private SPUM_Prefabs prefab;

    public void AttackMotion()
    {
        prefab.PlayAnimation(6);
    }

    public void UltMotion()
    {
        prefab.PlayAnimation(9);
    }

    public void StunMotion()
    {
        prefab.PlayAnimation(3);
    }

    public void DeadMotion()
    {
        prefab.PlayAnimation(2);
    }

    public void RunMotion()
    {
        prefab.PlayAnimation(1);
    }

    public void SetPrefab(SPUM_Prefabs prefab)
    {
        if (prefab != null)
            this.prefab = prefab;
    }

    public SPUM_Prefabs GetPrefab()
    {
        return prefab;
    }
}
