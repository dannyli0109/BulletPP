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

    #endregion

    void Update()
    {
        playerGoldUI.richText = true;
        playerGoldUI.text = "$: " + player.gold;
        UpdatePlayerAmmoUI();
    }

    void UpdatePlayerAmmoUI()
    {
        int xIndex = 0;

        for(int i=0; i<bulletSprites.Length; i++)
        {
            if (i < player.bulletStats.maxClip.value)
            {
                bulletSprites[i].gameObject.SetActive(true);
                bulletSprites[i].gameObject.transform.position = new Vector3(xIndex * spacing.x + defaultOffset.x, 0 + defaultOffset.y, 0);
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

        for (int i = 0; i < player.grenadeStats.maxClip.value; i++)
        {
            grenadeSprites[i].gameObject.SetActive(true);
            grenadeSprites[i].gameObject.transform.position = new Vector3(xIndex * spacing.x + defaultOffset.x, 0 + defaultOffset.y, 0);
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

        for (int i = 0; i < player.rocketStats.maxClip.value; i++)
        {
            rocketSprites[i].gameObject.SetActive(true);
            rocketSprites[i].gameObject.transform.position = new Vector3(xIndex * spacing.x + defaultOffset.x, 0 + defaultOffset.y, 0);
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

        for (int i = 0; i < player.laserStats.maxClip.value; i++)
        {
            laserSprites[i].gameObject.SetActive(true);
            laserSprites[i].gameObject.transform.position = new Vector3(xIndex * spacing.x + defaultOffset.x, 0 + defaultOffset.y, 0);
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

        for (int i = 0; i < player.bouncingBladeStats.maxClip.value; i++)
        {
            bouncingBladeSprites[i].gameObject.SetActive(true);
            bouncingBladeSprites[i].gameObject.transform.position = new Vector3(xIndex * spacing.x + defaultOffset.x, 0 + defaultOffset.y, 0);
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
