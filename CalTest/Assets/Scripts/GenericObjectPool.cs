using System.Collections.Generic;
using UnityEngine;

// Generic object pool that can be used for any MonoBehaviour, optimizing performance by reusing objects.
public class GenericObjectPool<T> where T : MonoBehaviour
{
    private readonly Queue<T> pool = new Queue<T>();
    private readonly T prefab;
    private readonly Transform parent;

    public GenericObjectPool(T prefab, int initialSize, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        Initialize(initialSize);
    }

    // Initializes the pool with a set number of inactive objects
    private void Initialize(int initialSize)
    {
        for (int i = 0; i < initialSize; i++)
        {
            T obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    // Retrieves an object from the pool, creating a new one if the pool is empty
    public T GetObject(Vector3 position)
    {
        T obj = pool.Count > 0 ? pool.Dequeue() : Object.Instantiate(prefab, parent);
        obj.transform.position = position;
        obj.gameObject.SetActive(true);
        return obj;
    }

    // Returns an object to the pool for reuse
    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
