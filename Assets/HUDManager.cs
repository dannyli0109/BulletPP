using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Character player;
    public TextMeshProUGUI playerGoldUI;
    void Update()
    {
        playerGoldUI.text = "$: " + player.gold;
    }
}
