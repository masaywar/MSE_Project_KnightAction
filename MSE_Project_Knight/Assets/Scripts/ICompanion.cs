using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICompanion 
{
    public void AttackMotion();
    public void UltMotion();
    public void StunMotion();
    public void DeadMotion();
    public void RunMotion();
    public void SetPrefab(SPUM_Prefabs prefab);
    public SPUM_Prefabs GetPrefab();
}
