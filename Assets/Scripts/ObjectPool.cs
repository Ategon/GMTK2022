using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GameObject Pool
public class ObjectPool
{
    // Total active and inactive objects
    public int totalCount { get; private set; }
    public int activeCount { get { return totalCount - inactiveCount; } }
    // Inactive objects / Number of objects in pool
    public int inactiveCount { get { return stack.Count; } }

    // Stack of all of the inactive gameobjects ready to be used
    Stack<GameObject> stack;

    GameObject prefab;
    Transform poolParent; // To organize all the prefabs and not make messy in the scene

    // If check the collection
    public bool ifCheckBeforeReleasing;

    public void InitPool(string poolName, GameObject prefab, int startCount, bool ifCheckBeforeReleasing = true)
    {
        GameObject poolParentGameObject = new GameObject(poolName);
        poolParent = poolParentGameObject.transform;

        this.prefab = prefab;

        stack = new Stack<GameObject>(startCount);

        for (int i = 0; i < startCount; ++i)
        {
            bool ifActive = prefab.activeSelf;
            prefab.SetActive(false);
            GameObject gameObject = GameObject.Instantiate(prefab, poolParent);
            stack.Push(gameObject);
            ++totalCount;
            prefab.SetActive(ifActive);

            //Get();

            //GameObject gameObject = GameObject.Instantiate(prefab, poolParent);
            //gameObject.SetActive(false);
            //stack.Push(gameObject);
        }

        this.ifCheckBeforeReleasing = ifCheckBeforeReleasing;
    }

    // Called to get an instance of the prefab from the pool
    public GameObject Get()
    {
        Debug.Log("Getting obj");
        // If no more inactive objects, add new objects to the stack and return the new object
        if (stack.Count == 0)
        {
            GameObject gameObject = GameObject.Instantiate(prefab, poolParent);
            //stack.Push(gameObject);
            ++totalCount;
            return gameObject;
        }
        else
        {
            return stack.Pop();
        }
    }

    // Called to release the prefab from the pool
    public void Release(GameObject gameObject)
    {
        if (ifCheckBeforeReleasing && stack.Contains(gameObject))
        {
            Debug.LogError("Releasing a object that's already in the pool. GameObject name: " + gameObject.name);
            return;
        }

        stack.Push(gameObject);

        gameObject.SetActive(false);
    }
}