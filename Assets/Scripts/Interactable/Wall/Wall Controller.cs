using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WallController : MonoBehaviour
{
    [SerializeField] private List<WallTarget> _wallTargets = new List<WallTarget>();
    [SerializeField] private List<Wall> _walls = new List<Wall>();
    [SerializeField] private Material _origin;
    [SerializeField] private Material _Silhouette;

    [SerializeField] private XRDirectInteractor _leftGrip;
    [SerializeField] private XRDirectInteractor _rightGrip;

    void Start()
    {
        InitWall();
    }

    private void InitWall()
    {
        GetComponentsInChildren(_walls);
        GetComponentsInChildren(_wallTargets);

        foreach (var wall in _walls)
        {
            wall.Init(_leftGrip, _rightGrip, _origin, _Silhouette);
        }
    }

    public void ResetWalls()
    {
        foreach (var wallTarget in _wallTargets)
        {
            wallTarget.gameObject.SetActive(true);
        }
    }

    public void SetInfinityMode()
    {
        foreach (var wall in _walls)
        {
            wall.SetFill(true);
        }

        foreach (var wallTarget in _wallTargets)
        {
            wallTarget.SetWall();
        }
    }
}
