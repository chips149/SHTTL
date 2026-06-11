using System.Collections.Generic;
using UnityEngine;


public class FollowerPool : MonoBehaviour
{
    private static FollowerPool _instance;

    [SerializeField] private FollowerPoolEntry[] entries;

    private readonly Dictionary<int, Queue<GameObject>> _pools = new();

    private void Awake()
    {
        _instance = this;
        foreach (var entry in entries)
        {
            if (entry.prefab == null) continue;
            int id = entry.prefab.GetInstanceID();
            if (!_pools.ContainsKey(id))
                _pools[id] = new Queue<GameObject>();
        }
    }

    public static GameObject Get(GameObject prefab)
    {
        int id = prefab.GetInstanceID();
        if (_instance._pools.TryGetValue(id, out var queue) && queue.Count > 0)
        {
            var go = queue.Dequeue();
            go.SetActive(true);
            return go;
        }
        return Instantiate(prefab);
    }

    public static void Return(GameObject go, GameObject prefab)
    {
        go.SetActive(false);
        go.transform.SetParent(_instance.transform);
        int id = prefab.GetInstanceID();
        if (_instance._pools.TryGetValue(id, out var queue))
        {
            queue.Enqueue(go);
        }
        else
        {
            Destroy(go);
        }
    }
}

[System.Serializable]
public class FollowerPoolEntry
{
    public string key;
    public GameObject prefab;
}
