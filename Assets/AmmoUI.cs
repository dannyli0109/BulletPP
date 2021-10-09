using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public List<Image> ammoImages;
    public Image ammoImagePrefab;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < player.inventory.capacity; i++)
        {
            ammoImages.Add(Instantiate(ammoImagePrefab, transform));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
