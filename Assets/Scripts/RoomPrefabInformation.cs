using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPrefabInformation : MonoBehaviour
{
    public Transform upperRoomDoorSpawnOffet;
    public Transform lowerRoomDoorSpawnOffet;
    public Transform leftRoomDoorSpawnOffet;
    public Transform rightRoomDoorSpawnOffet;
    // where you enter from in the new room.

    public float cameraBoundryMaxX = 7;
    public float cameraBoundryMinX = -7;
    public float cameraBoundryMaxZ = 6.5f;
    public float cameraBoundryMinZ = -3.7f;

    public List<Transform> enemySpawnPoint;

    public List<Transform> sniperSpawnPoint;
}