using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<AugmentUI> augmentUIs;
    public Character player;
    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        AugmentManager augmentManager = AugmentManager.current;

        for (int i = 0; i < augmentUIs.Count; i++)
        {
            int index = Random.Range(0, augmentManager.augmentDatas.Count);
            augmentUIs[i].gameObject.SetActive(true);
            augmentUIs[i].Populate(augmentManager.augmentDatas[index]);
        }
    }

    public void ReRoll()
    {
        if (player.gold >=2)
        {
            player.gold -= 2;
            Refresh();
        }
    }

    public void Continue()
    {
        GameManager.current.gameState = GameState.Casual;
        // gameObject.SetActive(false);
    }
}
