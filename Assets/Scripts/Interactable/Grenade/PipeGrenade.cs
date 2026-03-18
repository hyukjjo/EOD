using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PipeGrenade : Grenade
{
    [SerializeField] private bool _isPipe = false;
    [SerializeField] private Collider _range;

    private List<Monster> _monsterList = new List<Monster>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            Monster monster = other.gameObject.GetComponentInParent<Monster>();

            if (!_monsterList.Contains(monster))
            {
                _monsterList.Add(monster);
                monster.SetMoveTarget(transform);
            }
        }
    }

    protected override void TurnOnGrenade(XRBaseInteractor interactor)
    {
        base.TurnOnGrenade(interactor);

        _range.enabled = true;
    }

    protected override void TriggerGrenade()
    {
        base.TriggerGrenade();

        _range.enabled = false;

        foreach (Monster monster in _monsterList)
        {
            monster.SetMoveTarget(GameManager.Instance.GetPlayer().transform);
        }
    }
}