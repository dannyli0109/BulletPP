using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Shop : MonoBehaviour
{
    public List<AugmentUI> augmentUIs;
    public Character player;
    public GameObject augmentListUIContainer;
    public GameObject augmentUIPrefab;
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
            augmentUIs[i].Populate(index);
        }
    }

    public void UpdateText()
    {
        for (int i = 0; i < augmentUIs.Count; i++)
        {
            augmentUIs[i].UpdateText();
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

    public void PopulateAugmentListUI()
    {
        foreach (Transform child in augmentListUIContainer.transform)
        {
            Destroy(child.gameObject);
        }

        AugmentManager augmentManager = AugmentManager.current;
        for (int i = 0; i < player.augments.Count; i++)
        {
            GameObject augmentUI = Instantiate(augmentUIPrefab);
            augmentUI.GetComponent<TextMeshProUGUI>().text = augmentManager.augmentDatas[player.augments[i].id].name + ": " +  player.augments[i].count;
            augmentUI.transform.SetParent(augmentListUIContainer.transform);
            augmentUI.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
