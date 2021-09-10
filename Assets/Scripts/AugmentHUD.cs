using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AugmentHUD : MonoBehaviour
{
    public TooltipTrigger tooltipTrigger;
    public Image icon;
    public TextMeshProUGUI levelText;
    public Outline outline;
    public GameObject expContainer;
    public GameObject expPrefab;


    public void Populate(int id, int level, int count)
    {
        AugmentManager augmentManager = AugmentManager.current;
        string name = augmentManager.augmentDatas[id].title;
        string description = augmentManager.augmentDatas[id].descriptions[level];
        tooltipTrigger.header = name + " Lv." + (level + 1); ;
        tooltipTrigger.content = description;
        icon.sprite = augmentManager.augmentDatas[id].iconSprite;
        outline.effectColor = augmentManager.colors[augmentManager.augmentDatas[id].rarity];
        //levelText.text = "Lv." + (level + 1);


        if (level == 0)
        {
            for (int i = 0; i < expContainer.transform.childCount; i++)
            {
                GameObject expObject = expContainer.transform.GetChild(i).gameObject;
                if (i >= 3) expObject.SetActive(false);
                else expObject.SetActive(true);
                if (count > i)
                {
                    expObject.GetComponent<Image>().color = new Color(0, 1, 0);
                }
                else
                {
                    expObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                }
            }
        }
        else if (level == 1)
        {
            for (int i = 0; i < expContainer.transform.childCount; i++)
            {
                GameObject expObject = expContainer.transform.GetChild(i).gameObject;
                expObject.SetActive(true);

                if (count - 3 >= i)
                {
                    expObject.GetComponent<Image>().color = new Color(0, 1, 0);
                }
                else
                {
                    expObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                }
            }
        }
        else
        {
            for (int i = 0; i < expContainer.transform.childCount; i++)
            {
                GameObject expObject = expContainer.transform.GetChild(i).gameObject;
                expObject.SetActive(false);

            }
        }
    }
}
