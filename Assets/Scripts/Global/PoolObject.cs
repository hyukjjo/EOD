using UnityEngine;
using KeyType = System.String;

public class PoolObject : MonoBehaviour
{
    public KeyType Key;

    public PoolObject Clone()
    {
        GameObject go = Instantiate(gameObject);

        if (!go.TryGetComponent(out PoolObject po))
        {
            po = go.AddComponent<PoolObject>();
        }

        go.SetActive(false);
        return po;
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        transform.SetParent(ObjectPoolManager.Instance.transform);
        gameObject.SetActive(false);
    }
}