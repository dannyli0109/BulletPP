using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RoomPrefabInformation
{
    public Vector2 upperRoomDoorSpawnOffet;
    public Vector2 lowerRoomDoorSpawnOffet;
    public Vector2 leftRoomDoorSpawnOffet;
    public Vector2 rightRoomDoorSpawnOffet;

    // where you enter from in the new room.
    public Vector2 middleOffset;
}

[Serializable]
public class Room : MonoBehaviour
{
    public Vector2 offsetPos;
    public int Length = 0;

    public RoomPrefabInformation thisPrefabInfo;

    public int upperRoomRef =-1;
    public int lowerRoomRef =-1;
    public int leftRoomRef  =-1;
    public int rightRoomRef = -1;

  public  bool Completed;

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