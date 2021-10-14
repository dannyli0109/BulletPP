using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Room
{
    public Vector2 offsetPos;
    public int length = 0;
   public bool bossRoom;

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