using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCanvas : MonoBehaviour
{
    int userIndex;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SelectIndex(int index)
    {
        userIndex = index;
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().StartTransition();
    }
}
