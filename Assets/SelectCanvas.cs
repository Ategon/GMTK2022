using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCanvas : MonoBehaviour
{
    public int userIndex;
    int timesInThis = 0;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SelectIndex(int index)
    {
        userIndex = index;
        GameObject.Find("LevelLoader").GetComponent<LevelLoader>().StartTransition();
    }

    void OnLevelWasLoaded(int level)
    {
        if(level != 2)
        {
            if(level == 1)
            {
                timesInThis++;
                if (timesInThis == 2)
                {
                    Destroy(this.gameObject);
                }
            }else if (level == 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
