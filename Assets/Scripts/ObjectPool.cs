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

    /// <summary>
    /// Initialises the pool, setting the parent of all the instantiated prefabs as a new gameobject with poolName
    /// </summary>
    /// <param name="poolName">
    ///     If poolPrefabParent == null, create a new Transform using poolName and set that as the poolPrefabParent
    /// </param>
    /// <param name="prefab">Prefab to spawn</param>
    /// <param name="startCount">Start number to instantiate</param>
    /// <param name="ifCheckBeforeReleasing">
    ///     For debugging, might have some errors when releasing a game object
    /// </param>
    public void InitPool(string poolName, GameObject prefab, int startCount, bool ifCheckBeforeReleasing = true)
    {
        InitPool(null, poolName, prefab, startCount, ifCheckBeforeReleasing);
    }

    /// <summary>
    /// Initialises the pool, setting the parent of all the instantiated prefabs as a poolPrefabParent
    /// </summary>
    /// <param name="poolName">
    ///     If poolPrefabParent == null, create a new Transform using poolName and set that as the poolPrefabParent
    /// </param>
    /// <param name="prefab">Prefab to spawn</param>
    /// <param name="startCount">Start number to instantiate</param>
    /// <param name="ifCheckBeforeReleasing">
    ///     For debugging, might have some errors when releasing a game object
    /// </param>
    public void InitPool(Transform poolPrefabParent, GameObject prefab, int startCount, bool ifCheckBeforeReleasing = true)
    {
        InitPool(poolPrefabParent, null, prefab, startCount, ifCheckBeforeReleasing);
    }

    /// <summary>
    /// Initialises the pool
    /// </summary>
    /// <param name="poolPrefabParent">
    ///     Instantiate all the gameobjects under the parent
    ///     If poolPrefabParent == null, create a new Transform using poolName and set that as the poolPrefabParent
    /// </param>
    /// <param name="poolName">If poolPrefabParent == null, create a new Transform using poolName and set that as the poolPrefabParent</param>
    /// <param name="prefab">Prefab to spawn</param>
    /// <param name="startCount">Start number to instantiate</param>
    /// <param name="ifCheckBeforeReleasing">
    ///     For debugging, might have some errors when releasing a game object
    /// </param>
    private void InitPool(Transform poolPrefabParent, string poolName, GameObject prefab, int startCount, bool ifCheckBeforeReleasing = true)
    {
        if (poolPrefabParent == null)
        {
            GameObject poolPrefabParentObj = new GameObject(poolName);
            poolParent = poolPrefabParentObj.transform;
        }
        else
        {
            poolParent = poolPrefabParent;
        }

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
        }

        this.ifCheckBeforeReleasing = ifCheckBeforeReleasing;
    }

    // Called to get an instance of the prefab from the pool
    public GameObject Get()
    {
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
            GameObject gameObject = stack.Pop();
            gameObject.SetActive(true);
            return gameObject;
        }
    }

    // Called to release the prefab from the pool
    public void Release(GameObject gameObject)
    {
        // Shouldn't happen but check for safety.
        // Set ifCheckBeforeReleasing to false when not debugging as it might be expensive since checking every element in the stack
        if (ifCheckBeforeReleasing && stack.Contains(gameObject))
        {
            Debug.LogError("Releasing a object that's already in the pool. GameObject name: " + gameObject.name);
            return;
        }

        stack.Push(gameObject);

        gameObject.SetActive(false);
    }
}