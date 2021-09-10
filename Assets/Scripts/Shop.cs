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

    public List<int> augmentIds;

    void Start()
    {
        AugmentManager augmentManager = AugmentManager.current;
        augmentIds = augmentManager.GetAugmentIdList();
        Refresh();
    }

    public void Refresh()
    {
        AugmentManager augmentManager = AugmentManager.current;

        for (int i = 0; i < augmentUIs.Count; i++)
        { 
            if (augmentIds.Count == 0) augmentIds = augmentManager.GetAugmentIdList();

            int rarity = GetRandomRarity();
            List<List<int>> augmentRarities = augmentManager.GetRarityList(augmentIds);
            List<int> augmentIndices = augmentRarities[rarity];

            // if there's no augment in a given rarity, generate another rarity
            while (augmentIndices.Count == 0)
            {
                rarity = GetRandomRarity();
                augmentIndices = augmentRarities[rarity];
            }
            int randIndex = Random.Range(0, augmentIndices.Count);
            int augmentIndex = augmentIndices[randIndex];

            augmentIds.Remove(augmentIndex);
            augmentUIs[i].gameObject.SetActive(true);
            augmentUIs[i].Populate(augmentIndex);
        }
    }

    public int GetRandomRarity()
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
        return rarity;
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
            augmentUI.transform.GetComponent<AugmentHUD>().Populate(player.augments[i].id, player.augments[i].level, player.augments[i].count);
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
            synergyUI.GetComponent<SynergyHUD>().Populate(player.synergies[i].id, player.synergies[i].breakPoint, player.synergies[i].count);
            synergyUI.transform.SetParent(synergyListUIContainer.transform);
            synergyUI.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
