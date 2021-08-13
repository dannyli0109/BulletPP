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
        Debug.Log("Clicked");
        AugmentManager augmentManager = AugmentManager.current;
        int state = shop.player.BuyAugment(id);
        if (state == 1)
        {
            gameObject.SetActive(false);
            shop.PopulateAugmentListUI();
        }
        else if (state == 2)
        {
            augmentManager.OnAttached(id);
            gameObject.SetActive(false);
            shop.PopulateAugmentListUI();
        }
    }

    public void Populate(AugmentData data)
    {
        AugmentManager augmentManager = AugmentManager.current;
        id = data.id;
        title.text = data.name;
        descriptions.text = data.descriptions;
        cost.text = "$ " + augmentManager.costs[data.rarity];
        background.color = augmentManager.colors[data.rarity];
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0.5f);
    }
}
