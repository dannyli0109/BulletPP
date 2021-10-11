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
    public SellAugmentTrigger sellAugmentTrigger;


    public void Populate(int id)
    {
        //AugmentManager augmentManager = AugmentManager.current;
        //string name = augmentManager.augments[id].augmentName;
        //string description = "<b>Selling Price: " + augmentManager.augments[id].cost + "</b>" + "\n";
        //description += augmentManager.augments[id].description;
        //description += "\n<b>Right click to sell</b>";

        //tooltipTrigger.header = name;
        //tooltipTrigger.content = description;
        //icon.sprite = augmentManager.augments[id].augmentIcon;
        //outline.effectColor = augmentManager.augments[id].color;
        ////levelText.text = "Lv." + (level + 1);


        //if (level == 0)
        //{
        //    for (int i = 0; i < expContainer.transform.childCount; i++)
        //    {
        //        GameObject expObject = expContainer.transform.GetChild(i).gameObject;
        //        if (i >= 3) expObject.SetActive(false);
        //        else expObject.SetActive(true);
        //        if (count > i)
        //        {
        //            expObject.GetComponent<Image>().color = new Color(0, 1, 0);
        //        }
        //        else
        //        {
        //            expObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        //        }
        //    }
        //}
        //else if (level == 1)
        //{
        //    for (int i = 0; i < expContainer.transform.childCount; i++)
        //    {
        //        GameObject expObject = expContainer.transform.GetChild(i).gameObject;
        //        expObject.SetActive(true);

        //        if (count - 3 >= i)
        //        {
        //            expObject.GetComponent<Image>().color = new Color(0, 1, 0);
        //        }
        //        else
        //        {
        //            expObject.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
        //        }
        //    }
        //}
        //else
        //{
        //    for (int i = 0; i < expContainer.transform.childCount; i++)
        //    {
        //        GameObject expObject = expContainer.transform.GetChild(i).gameObject;
        //        expObject.SetActive(false);

        //    }
        //}
    }

    public void PopulateGeneric(Sprite image)
    {
        //AugmentManager augmentManager = AugmentManager.current;

        //tooltipTrigger.header = "Empty slot";
        //tooltipTrigger.content = "Try buying more augments.";
        //icon.sprite = image;
        //outline.effectColor = augmentManager.colors[0];


        //for (int i = 0; i < expContainer.transform.childCount; i++)
        //{
        //    GameObject expObject = expContainer.transform.GetChild(i).gameObject;
        //    expObject.SetActive(false);

        //}
    }
}
