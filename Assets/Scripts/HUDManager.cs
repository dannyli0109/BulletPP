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

    void Update()
    {
        playerGoldUI.richText = true;
        playerGoldUI.text = "$: " + player.gold;
        UpdatePlayerAmmoUI();
    }

    void UpdatePlayerAmmoUI()
    {
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


    }
}
