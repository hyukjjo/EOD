using System.Collections;
using System.Collections.Generic;
using KeyType = System.String;
using UnityEngine;

[System.Serializable]
public class PoolObjectData
{
    public const int INITIAL_COUNT = 10;
    public const int MAX_COUNT = 50;

    public KeyType Key;
    public GameObject ObjectPrefab;
    public int InitialObjectCount = INITIAL_COUNT;
    public int MaxObjectCount = MAX_COUNT;
}