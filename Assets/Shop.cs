using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<AugmentUI> augmentUIs;
    // Start is called before the first frame update
    void Start()
    {
        AugmentManager augmentManager = AugmentManager.current;
        
        for (int i = 0; i < augmentUIs.Count; i++)
        {
            augmentUIs[i].Populate(augmentManager.augmentDatas[i]);
        }

        //Time.timeScale = 0;
    }
}
