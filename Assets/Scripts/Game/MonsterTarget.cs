using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterTarget : MonoBehaviour
{
    [SerializeField] protected float hp;
    private float originHp = 100;

    // Start is called before the first frame update
    void Start()
    {
        hp = originHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Damage(float damage, Transform transform = null, Action action = null)
    {
        if (hp > 0)
            hp -= damage;
    }
}
