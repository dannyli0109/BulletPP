using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    defaultRoom,
    eventRoom,
    bossRoom
}

public class Room
{

    public Vector2 offsetPos;
    public RoomType roomType;
    public bool hasUpperRoom;
    public bool hasLowerRoom;
    public bool hasLeftRoom;
    public bool hasRightRoom;

    public Room(Vector2 posInput, bool upInput, bool downInput, bool leftInput, bool rightInput)
    {
        offsetPos = posInput;

        hasUpperRoom = upInput;
        hasLowerRoom = downInput;
        hasLeftRoom = leftInput;
        hasRightRoom = rightInput;
    }

    public void DebugSpitInformation()
    {
        
    }
    void Update()
    {

    }
}
