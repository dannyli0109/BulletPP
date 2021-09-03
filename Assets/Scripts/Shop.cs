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

    public GameObject synergyListUIContainer;
    public GameObject synergyUIPrefab;

    public int level = 0;
    public List<List<float>> percents = new List<List<float>>() { 
        new List<float>() { 0.7f, 0.3f, 0.0f, 0.0f, 0.0f },
        new List<float>() { 0.5f, 0.35f, 0.1f, 0.05f, 0.0f },
        new List<float>() { 0.4f, 0.3f, 0.2f, 0.1f, 0.0f },
        new List<float>() { 0.2f, 0.3f, 0.3f, 0.2f, 0.0f },
        // new List<float>() { 0.2f, 0.25f, 0.30f, 0.2f, 0.05f },
        new List<float>() { 0.15f, 0.2f, 0.25f, 0.3f, 0.1f}
    };

    public bool hasBoughtAnAugment;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        AugmentManager augmentManager = AugmentManager.current;

        for (int i = 0; i < augmentUIs.Count; i++)
        {
            // Generate random index based on percents
            float randNum = Random.Range(0.0f, 1.0f);
            int rarity = 0;

            float accumulated = 0;
            for (int j = 0; j < percents[level].Count; j++)
            {
                accumulated += percents[level][j];
                if (randNum <= accumulated)
                {
                    rarity = j;
                    break;
                }
            }

            List<int> augmentIndices = augmentManager.augmentRarities[rarity];
            int randIndex = Random.Range(0, augmentIndices.Count);
            int augmentIndex = augmentIndices[randIndex];
            augmentUIs[i].gameObject.SetActive(true);

            augmentUIs[i].Populate(augmentIndex);
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
        if (hasBoughtAnAugment)
        {

            if (player.gold >= 2)
            {
                player.gold -= 2;
                Refresh();
            }
        }
    }

    public void Continue()
    {
        if (hasBoughtAnAugment)
        {

        GameManager.current.ChangeState(GameState.Casual);
            player.reloading = true;
        }
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
            augmentUI.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = augmentManager.augmentDatas[player.augments[i].id].iconSprite;
            augmentUI.transform.GetChild(0).GetComponent<AugmentHUD>().Populate(player.augments[i].id, player.augments[i].level);
            augmentUI.transform.GetChild(0).GetComponent<Outline>().effectColor = augmentManager.colors[augmentManager.augmentDatas[player.augments[i].id].rarity];
            augmentUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Lv." + (player.augments[i].level + 1);
            augmentUI.transform.SetParent(augmentListUIContainer.transform);
            augmentUI.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void PopulateSynergyListUI()
    {
        foreach (Transform child in synergyListUIContainer.transform)
        {
            Destroy(child.gameObject);
        }

        AugmentManager augmentManager = AugmentManager.current;
        for (int i = 0; i < player.synergies.Count; i++)
        {
            GameObject synergyUI = Instantiate(synergyUIPrefab);
            synergyUI.GetComponent<TextMeshProUGUI>().text = augmentManager.synergyDatas[player.synergies[i].id].title + ": " + player.synergies[i].count;
            synergyUI.transform.SetParent(synergyListUIContainer.transform);
            synergyUI.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
