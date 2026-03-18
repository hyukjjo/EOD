using System;
using System.Collections.Generic;
using UnityEngine;

public class WallTarget : MonsterTarget
{
    [SerializeField] private List<Wall> _walls = new List<Wall>();

    private const float _originHp = 100;
    private Action _actionFillWall;

    void Start()
    {
        Init();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    hp = 0;

        //    foreach (var wall in _walls)
        //    {
        //        wall.SetFill(false);
        //    }
        //}
    }

    private void OnDestroy()
    {
        _actionFillWall -= () =>
        {
            hp += 25;
            GetComponent<Collider>().enabled = true;
        };
    }

    private void Init()
    {
        //GameManager.Instance.MaintenanceStart += () => GetComponent<Collider>().enabled = true;

        GetComponentsInChildren(_walls);

        _actionFillWall += () => 
        {
            hp += 25;
            GetComponent<Collider>().enabled = true;
        };

        foreach (var wall in _walls)
        {
            wall.SetHp(_actionFillWall);

            if (wall.IsFill)
                hp += 25;
        }

        if (hp <= 0)
            GetComponent<Collider>().enabled = false;
    }

    public override void Damage(float damage, Transform transform = null, Action action = null)
    {
        base.Damage(damage);
        //if (hp < _originHp / _walls.Count)
        //{
        //    float num = hp / (_originHp / _walls.Count);

        //    if (_walls[(int)num+1].IsFill)
        //        _walls[(int)num+1].SetFill(false);
        //}

        //Proto
        if (hp < 25 && hp > 0)
            _walls[1].SetFill(false);
        else if (hp <= 0)
        {
            _walls[0].SetFill(false);
            //action?.Invoke();
            Destroy();
            return;
        }


        //if (hp == 0)
        //{
        //    Destroy();
        //    return;
        //}
    }

    public void SetWall()
    {
        foreach (var wall in _walls)
        {
            wall.SetHp(_actionFillWall);

            if (wall.IsFill)
                hp += 25;
        }

        GetComponent<Collider>().enabled = true;
    }

    private void Destroy()
    {
        //gameObject.SetActive(false);
        GetComponent<Collider>().enabled = false;
    }
}
