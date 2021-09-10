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
    /*
    public GameObject bulletsUI;
    public TextMeshProUGUI bulletAmmoText;
    public GameObject grenadesUI;
    public TextMeshProUGUI grenadeAmmoText;
    public GameObject rocketsUI;
    public TextMeshProUGUI rocketAmmoText;
    public GameObject laserUI;
    public TextMeshProUGUI laserFuelText;
    public GameObject bouncingBladeUI;
    public TextMeshProUGUI bouncingBladeText;

    public Color32 filledClipColor;
    public Color32 emptyClipColor;
    */
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

    #endregion

    void Awake()
    {
        current = this;
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
        playerGoldUI.richText = true;
        playerGoldUI.text = "$: " + player.gold;
        UpdatePlayerAmmoUI();

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

    void UpdatePlayerAmmoUI()
    {
        int xIndex = 0;

        for(int i=0; i<bulletSprites.Length; i++)
        {
            if (i < player.bulletStats.maxClip.value)
            {
                bulletSprites[i].gameObject.SetActive(true);
                //bulletSprites[i].gameObject.transform.position = new Vector3(xIndex * spacing.x + defaultOffset.x, 0 + defaultOffset.y, 0);
                xIndex++;
                if (i < player.currentBulletClip)
                {
                    bulletSprites[i].color = bulletColour;
                }
                else
                {
                    bulletSprites[i].color = defaultAmmoColour;
                }
            }
            else
            {
                bulletSprites[i].gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < grenadeSprites.Length; i++)
        {
            if (i < player.grenadeStats.maxClip.value)
            {
                grenadeSprites[i].gameObject.SetActive(true);
                //grenadeSprites[i].gameObject.transform.position = new Vector3(xIndex * spacing.x + defaultOffset.x, 0 + defaultOffset.y, 0);
                xIndex++;
                if (i < player.currentGrenadeClip)
                {
                    grenadeSprites[i].color = grenadeColour;
                }
                else
                {
                    grenadeSprites[i].color = defaultAmmoColour;
                }
            }

        }

        for (int i = 0; i < rocketSprites.Length; i++)
        {
            if (i < player.rocketStats.maxClip.value)
            {
                rocketSprites[i].gameObject.SetActive(true);
                //rocketSprites[i].gameObject.transform.position = new Vector3(xIndex * spacing.x + defaultOffset.x, 0 + defaultOffset.y, 0);
                xIndex++;
                if (i < player.currentRocketClip)
                {
                    rocketSprites[i].color = rocketColour;
                }
                else
                {
                    rocketSprites[i].color = defaultAmmoColour;
                }
            }
        }

        for (int i = 0; i < laserSprites.Length; i++)
        {
            if (i < player.laserStats.maxClip.value)
            {
                laserSprites[i].gameObject.SetActive(true);
                //laserSprites[i].gameObject.transform.position = new Vector3(xIndex * spacing.x + defaultOffset.x, 0 + defaultOffset.y, 0);
                xIndex++;
                if (i < player.currentLaserClip)
                {
                    laserSprites[i].color = laserColour;
                }
                else
                {
                    laserSprites[i].color = defaultAmmoColour;
                }
            }
        }

        for (int i = 0; i < player.bouncingBladeStats.maxClip.value; i++)
        {
            bouncingBladeSprites[i].gameObject.SetActive(true);
            //bouncingBladeSprites[i].gameObject.transform.position = new Vector3(xIndex * spacing.x + defaultOffset.x, 0 + defaultOffset.y, 0);
            xIndex++;
            if (i < player.currentBouncingBladeClip)
            {
                bouncingBladeSprites[i].color = bouncingBladeColour;
            }
            else
            {
                bouncingBladeSprites[i].color = defaultAmmoColour;
            }

        }

        /*
        if (player.bulletStats.maxClip.value > 0)
        {
            bulletsUI.SetActive(true);
            bulletAmmoText.text = player.currentBulletClip.ToString();
            if (player.currentBulletClip == 0)
            {
                bulletAmmoText.color = emptyClipColor;
            }
            else
            {
                bulletAmmoText.color = filledClipColor;
            }
        }
        else
        {
            bulletsUI.SetActive(false);
        }

        if (player.grenadeStats.maxClip.value > 0)
        {
            grenadesUI.SetActive(true);
            grenadeAmmoText.text = player.currentGrenadeClip.ToString();
            if (player.currentGrenadeClip == 0)
            {
                grenadeAmmoText.color = emptyClipColor;
            }
            else
            {
                grenadeAmmoText.color = filledClipColor;
            }
        }
        else
        {
            grenadesUI.SetActive(false);
        }

        if (player.rocketStats.maxClip.value > 0)
        {
            rocketsUI.SetActive(true);
            rocketAmmoText.text = player.currentRocketClip.ToString();
            if (player.currentRocketClip == 0)
            {
                rocketAmmoText.color = emptyClipColor;
            }
            else
            {
                rocketAmmoText.color = filledClipColor;
            }
        }
        else
        {
            rocketsUI.SetActive(false);
        }

        if (player.laserStats.maxClip.value > 0)
        {
            laserUI.SetActive(true);
            laserFuelText.text = player.currentLaserClip.ToString();
            if (player.currentLaserClip == 0)
            {
                laserFuelText.color = emptyClipColor;
            }
            else
            {
                laserFuelText.color = filledClipColor;
            }
        }
        else
        {
            laserUI.SetActive(false);
        }

        if (player.bouncingBladeStats.maxClip.value > 0)
        {
            bouncingBladeUI.SetActive(true);
            bouncingBladeText.text = player.currentBouncingBladeClip.ToString();
            if (player.currentGrenadeClip == 0)
            {
                bouncingBladeText.color = emptyClipColor;
            }
            else
            {
                bouncingBladeText.color = filledClipColor;
            }
        }
        else
        {
            bouncingBladeUI.SetActive(false);
        }

         */

    }
}
