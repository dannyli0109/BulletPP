using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RoomPrefabInformation
{
    public Vector2 upperRoomDoorSpawnOffet = new Vector2(0, 8.5f);
    public Vector2 lowerRoomDoorSpawnOffet = new Vector2(0, -8.5f);
    public Vector2 leftRoomDoorSpawnOffet = new Vector2(-13, 0);
    public Vector2 rightRoomDoorSpawnOffet = new Vector2(13, 0);
    // where you enter from in the new room.

    public float cameraBoundryMaxX = 7;
    public float cameraBoundryMinX = -7;
    public float cameraBoundryMaxZ = 6.5f;
    public float cameraBoundryMinZ = -3.7f;

    public Vector2 middleOffset;

    public List<Vector2> enemySpawnPoint;


}

[Serializable]
public class Room
{
    public Vector2 offsetPos;
    public int length = 0;

    public RoomPrefabInformation thisPrefabInfo;

    public int upperRoomRef =-1;
    public int lowerRoomRef =-1;
    public int leftRoomRef  =-1;
    public int rightRoomRef = -1;

    public bool completed;

    public Room(Vector2 posInput,int lengthInput, int upInput, int downInput, int leftInput, int rightInput)
    {
        offsetPos = posInput;

        upperRoomRef = upInput;
        lowerRoomRef = downInput;
        leftRoomRef = leftInput;
        rightRoomRef = rightInput;
    }

    public void DebugSpitInformation()
    {
        
    }

    void Update()
    {

    }
}