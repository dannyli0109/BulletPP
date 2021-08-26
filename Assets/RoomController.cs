using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public List<GameObject> doors;


    public void OpenDoors()
    {
        foreach(var door in doors)
        {
            LeanTween.moveLocalZ(door, 2, 1);
        }
    }

    public void CloseDoors()
    {
        foreach (var door in doors)
        {
            //door.SetActive(true);
            LeanTween.moveLocalZ(door, 0, 1);
        }
    }
}
