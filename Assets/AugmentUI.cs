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

    // Start is called before the first frame update
    void Start()
    {
        //title = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Populate(AugmentData data)
    {
        AugmentManager augmentManager = AugmentManager.current;
        title.text = data.Name;
        descriptions.text = data.Descriptions;
        Debug.Log(data.Rarity);
        cost.text = "$ " + augmentManager.costs[data.Rarity];
        background.color = augmentManager.colors[data.Rarity];
        background.color = new Color(background.color.r, background.color.g, background.color.b, 0.5f);
    }
}
