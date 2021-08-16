using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AugmentUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI descriptions;
    public TextMeshProUGUI cost;
    public Image background;
    public Outline outline;
    public Shop shop;
    int id;

    // Start is called before the first frame update
    void Start()
    {
        //title = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Click()
    {
        AugmentManager augmentManager = AugmentManager.current;
        int state = shop.player.BuyAugment(id);
        if (state == 1)
        {
            // obtained one that owned but remains the same level
            gameObject.SetActive(false);
            shop.PopulateAugmentListUI();
        }
        else if (state == 2)
        {
            // obtained a new augment
            augmentManager.OnAttached(id, 0);
            gameObject.SetActive(false);
            shop.PopulateAugmentListUI();
        }
        else if (state == 3)
        {
            shop.player.RemoveAllModifiers();

            for (int i = 0; i < shop.player.augments.Count; i++)
            {
                augmentManager.OnAttached(shop.player.augments[i].id, shop.player.augments[i].level);
            }
            gameObject.SetActive(false);
            shop.PopulateAugmentListUI();
        }
        shop.UpdateText();
    }

    public void Populate(int id)
    {
        this.id = id;
        UpdateText();
    }

    public void UpdateText()
    {
        AugmentManager augmentManager = AugmentManager.current;
        int level = 0;
        for (int i = 0; i < shop.player.augments.Count; i++)
        {
            if (shop.player.augments[i].id == id)
            {
                level = shop.player.augments[i].level;
                break;
            }
        }
        title.text = augmentManager.augmentDatas[id].name;
        descriptions.richText = true;
        descriptions.text = augmentManager.augmentDatas[id].descriptions[level];
        cost.text = "$ " + augmentManager.costs[augmentManager.augmentDatas[id].rarity];
        outline.effectColor = augmentManager.colors[augmentManager.augmentDatas[id].rarity];
    }
}
