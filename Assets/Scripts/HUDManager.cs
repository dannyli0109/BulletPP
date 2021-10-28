using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI playerGoldUI;

    #region AmmoUI

    public AmmoUI ammoUI;
    #endregion

    #region Ammo Images
    public Vector2 defaultOffset;
    public Vector2 spacing;
    public Color defaultAmmoColour;

    public Image[] bulletSprites;
    public Color bulletColour;

    public Image[] grenadeSprites;
    public Color grenadeColour;

    public Image[] rocketSprites;
    public Color rocketColour;

    public Image[] laserSprites;
    public Color laserColour;

    public Image[] bouncingBladeSprites;
    public Color bouncingBladeColour;

    public TextMeshProUGUI dpsText;
    public TextMeshProUGUI bulletDpsText;
    public TextMeshProUGUI grenadeDpsText;
    public TextMeshProUGUI rocketDpsText;
    public TextMeshProUGUI laserDpsText;
    public TextMeshProUGUI bouncingBladeDpsText;
    public static HUDManager current;
    #endregion

    #region AugmentHUD
    public GameObject augmentListUIContainer;
    public GameObject augmentUIPrefab;

    public GameObject synergyListUIContainer;
    public GameObject synergyUIPrefab;

    public TextMeshProUGUI headerTextGUI;
    #endregion

    float time = 0;
    float dps = 0;

    float bulletDps = 0;
    float grenadeDps = 0;
    float rocketDps = 0;
    float laserDps = 0;
    float bouncingBladeDps = 0;

    public float damage = 0;

    public float bulletDamage = 0;
    public float grenadeDamage = 0;
    public float rocketDamage = 0;
    public float laserDamage = 0;
    public float bouncingBladeDamage = 0;

    public bool updatingDPS = false;


    public Sprite emptySlotUISprite;

    void Awake()
    {
        current = this;
    }


    private void Start()
    {
        //PopulateAugmentListUI(true);
    }

    public void ShouldUpdateDPS(bool val)
    {
        updatingDPS = val;
        if (val)
        {
            damage = 0;
            bulletDamage = 0;
            grenadeDamage = 0;
            rocketDamage = 0;
            laserDamage = 0;
            bouncingBladeDamage = 0;

            time = 0;
            dps = 0;
            bulletDps = 0;
            grenadeDps = 0;
            rocketDps = 0;
            laserDps = 0;
            bouncingBladeDps = 0;
        }
    }

    void Update()
    {
        if (GameManager.current.GetState() == GameState.Shop)
        {
            playerGoldUI.gameObject.SetActive(true);
            playerGoldUI.richText = true;
            playerGoldUI.text = "$: " + player.gold;
        }
        else
        {
            playerGoldUI.gameObject.SetActive(false);
        }
        UpdatePlayerAmmoUI();
    }

    void UpdateDPSGUI()
    {
        if (time > 0)
        {
            dps = damage / time;
            bulletDps = bulletDamage / time;
            grenadeDps = grenadeDamage / time;
            rocketDps = rocketDamage / time;
            laserDps = laserDamage / time;
            bouncingBladeDps = bouncingBladeDamage / time;
        }
        if (damage > 0)
        {
            time += Time.deltaTime;
        }

        if (updatingDPS)
        {
            if (player.bulletStats.maxClip.value > 0)
            {
                bulletDpsText.gameObject.SetActive(true);
            }

            if (player.grenadeStats.maxClip.value > 0)
            {
                grenadeDpsText.gameObject.SetActive(true);
            }

            if (player.rocketStats.maxClip.value > 0)
            {
                rocketDpsText.gameObject.SetActive(true);
            }

            if (player.laserStats.maxClip.value > 0)
            {
                laserDpsText.gameObject.SetActive(true);
            }

            if (player.bouncingBladeStats.maxClip.value > 0)
            {
                bouncingBladeDpsText.gameObject.SetActive(true);
            }


            dpsText.text = "DPS: " + dps.ToString("F");
            bulletDpsText.text = "Bullet DPS: " + bulletDps.ToString("F");
            grenadeDpsText.text = "Grenade DPS: " + grenadeDps.ToString("F");
            rocketDpsText.text = "Rocket DPS: " + rocketDps.ToString("F");
            laserDpsText.text = "Laser DPS: " + laserDps.ToString("F");
            bouncingBladeDpsText.text = "Bouncing Blade DPS: " + bouncingBladeDps.ToString("F");
        }
    }

    public void UpdatePlayerAmmoUI()
    {
        if (GameManager.current.GetState() != GameState.Shop)
        {
            for (int i = 0; i < player.inventory.Count; i++)
            {
                ammoUI.ammoImages[i].transform.gameObject.SetActive(true);
                if (i < player.inventoryIndex)
                {
                    ammoUI.ammoImages[i].color = new Color(0.5f, 0.5f, 0.5f);
                }
                else
                {
                    ammoUI.ammoImages[i].color = player.inventory[i].GetColor(player, i);
                }
            }

            for (int i = player.inventory.Count; i < player.inventory.capacity; i++)
            {
                ammoUI.ammoImages[i].transform.gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < player.inventory.Count; i++)
            {
                ammoUI.ammoImages[i].transform.gameObject.SetActive(true);
                ammoUI.ammoImages[i].color = player.inventory[i].GetColor(player, i);
            }

            for (int i = player.inventory.Count; i < player.inventory.capacity; i++)
            {
                ammoUI.ammoImages[i].transform.gameObject.SetActive(false);
            }
        }
    }

    public void RefreshPlayerStats()
    {
        //AugmentManager augmentManager = AugmentManager.current;

        //player.RemoveAllModifiers();

        //for (int i = 0; i < player.inventory.augments.Count; i++)
        //{
        //    augmentManager.OnAugmentAttached(player.inventory.augments[i].id, player.inventory.augments[i].level);
        //}

        //for (int i = 0; i < player.synergies.Count; i++)
        //{
        //    augmentManager.OnSynergyAttached(player.synergies[i].id, player.synergies[i].breakPoint);
        //}
    }

    public void UpdateAugmentMaxSizeUI()
    {
        //headerTextGUI.text = "Augments " + player.inventory.augments.Count.ToString() + "/" + player.inventory.capacity.ToString();
    }

    public void PopulateAugmentListUI(bool inShop)
    {
      ////  Debug.Log("Trigger");
      //  foreach (Transform child in augmentListUIContainer.transform)
      //  {
      //      Destroy(child.gameObject);
      //  }

      //  //UpdateAugmentMaxSizeUI();

      //  //AugmentManager augmentManager = AugmentManager.current;


      //  for (int i = 0; i < player.inventory.capacity; i++)
      //  {
      //      if (i < player.inventory.augments.Count)
      //      {

      //          GameObject augmentUI = Instantiate(augmentUIPrefab);
      //          AugmentHUD augmentHUD = augmentUI.transform.GetComponent<AugmentHUD>();

      //          augmentHUD.Populate(player.inventory.augments[i].id);
      //          augmentUI.transform.SetParent(augmentListUIContainer.transform);
      //          augmentUI.transform.localScale = new Vector3(1, 1, 1);
      //          augmentHUD.sellAugmentTrigger.Init(player, i);
      //      }
      //      else
      //      {
      //          GameObject augmentUI = Instantiate(augmentUIPrefab);
      //          AugmentHUD augmentHUD = augmentUI.transform.GetComponent<AugmentHUD>();

      //          //  augmentHUD.Populate(0, 0, 0);
      //          augmentHUD.PopulateGeneric(emptySlotUISprite);
      //          augmentUI.transform.SetParent(augmentListUIContainer.transform);
      //          augmentUI.transform.localScale = new Vector3(1, 1, 1);
      //          augmentHUD.sellAugmentTrigger.Init(player, i);
      //      }
      //  }
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
