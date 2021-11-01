using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : PooledItem
{
    public Queue<T> available;

    public Pool(T prefab, int count)
    {
        available = new Queue<T>();
        for (int i = 0; i < count; i++)
        {
            T entity = GameObject.Instantiate(prefab);
            entity.gameObject.SetActive(false);
            entity.onDestroy += (x) => available.Enqueue(x as T);
            available.Enqueue(entity);
        }
    }

    public bool TryInstantiate(out T instantiateEntity, Vector3 position, Quaternion rotation)
    {
        if (available.Count > 0)
        {
            instantiateEntity = available.Dequeue();
            instantiateEntity.transform.SetPositionAndRotation(position, rotation);
            instantiateEntity.gameObject.SetActive(true);
            return true;
        }
        instantiateEntity = null;
        return false;
    }
}

public class PooledItem : MonoBehaviour
{
    public event System.Action<PooledItem> onDestroy;

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
        onDestroy?.Invoke(this);
    }
}
