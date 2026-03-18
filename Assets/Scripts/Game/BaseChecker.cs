using System;
using UnityEngine;

public class BaseChecker : MonoBehaviour
{
    public Action CheckMonster;

    private void OnDestroy()
    {
        CheckMonster = null;
    }

    private void OnTriggerEnter(Collider coll)
    {
        Monster monster = coll.GetComponentInParent<Monster>();

        if (monster)
        {
            monster.InsideTheBase = true;
            CheckMonster?.Invoke();
        }
    }
}