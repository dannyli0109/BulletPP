using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Direction
{
    Upper,
    Lower,
    Left,
    Right
}

[Serializable]
public class startingRoom
{
    public bool needUpperRoom;
    public bool needLowerRoom;
    public bool needLeftRoom;
    public bool needRightRoom;

}

public class MapGeneration : MonoBehaviour
{
    public bool debug;

    public List<Room> AllRooms;

    int roomsBeingProcessed = 0;
    public float WallReConnectChanceOfHundred;

    public Vector2 roomMultiplyValue;

    #region map generate
    public List<float> ChanceOfHundredToSpawnNewRoom;
    public List<int> atHowManyCurrentRooms;
    public List<float> RoomSpawnChanceModifierBasedOnLength;

    #endregion

    #region roomPrefab
    public int startingRoomID;
    public List<startingRoom> StartingRoomDoorLayouts;
    public List<GameObject> StartingRooms;
    public List<RoomPrefabInformation> StartingRoomPrefabInfo;

    public List<GameObject> AllDoorRoom;

    public List<GameObject> upRoom;
    public List<GameObject> downRoom;
    public List<GameObject> leftRoom;
    public List<GameObject> rightRoom;

    public List<GameObject> upDownRoom;
    public List<GameObject> upLeftRoom;
    public List<GameObject> upRightRoom;
    public List<GameObject> downLeftRoom;
    public List<GameObject> downRightRoom;
    public List<GameObject> leftRightRoom;

    public List<GameObject> UDLRoom;
    public List<GameObject> UDRRoom;
    public List<GameObject> ULRRoom;
    public List<GameObject> DLRRoom;


    public List<RoomPrefabInformation> AllDoorRoomInfo;

    public List<RoomPrefabInformation> upRoomInfo;
    public List<RoomPrefabInformation> downRoomInfo;
    public List<RoomPrefabInformation> leftRoomInfo;
    public List<RoomPrefabInformation> rightRoomInfo;

    public List<RoomPrefabInformation> upDownRoomInfo;
    public List<RoomPrefabInformation> upLeftRoomInfo;
    public List<RoomPrefabInformation> upRightRoomInfo;
    public List<RoomPrefabInformation> downLeftRoomInfo;
    public List<RoomPrefabInformation> downRightRoomInfo;
    public List<RoomPrefabInformation> leftRightRoomInfo;

    public List<RoomPrefabInformation> UDLRoomInfo;
    public List<RoomPrefabInformation> UDRRoomInfo;
    public List<RoomPrefabInformation> ULRRoomInfo;
    public List<RoomPrefabInformation> DLRRoomInfo;

    #endregion

    #region BetweenRoomMovement
    public int currentRoomInside = 0;

    public GameObject playerTarget;

    public bool needToSetPos;
    public Vector3 desiredSetPos; // used in late update because character controllers have a problem with 

    #endregion

    void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        startingRoomID = UnityEngine.Random.Range(0, StartingRooms.Count);
        AllRooms = new List<Room>();
        AllRooms.Add(new Room(new Vector2(0, 0),0, -1, -1, -1, -1));

        while (roomsBeingProcessed < AllRooms.Count)
        {
            //   Debug.Log(roomsBeingProcessed);
            if (roomsBeingProcessed == 0)
            {
                if (StartingRoomDoorLayouts[startingRoomID].needUpperRoom )
                {
                    AllRooms.Add(new Room(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(0, 1),AllRooms[roomsBeingProcessed].Length+1, -1, roomsBeingProcessed, -1, -1));
                    AllRooms[roomsBeingProcessed].upperRoomRef = AllRooms.Count - 1;
                }
                if (StartingRoomDoorLayouts[startingRoomID].needLowerRoom)
                {
                    AllRooms.Add(new Room(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(0, -1), AllRooms[roomsBeingProcessed].Length + 1, roomsBeingProcessed, -1, -1, -1));
                    AllRooms[roomsBeingProcessed].lowerRoomRef = AllRooms.Count - 1;
                }
                if (StartingRoomDoorLayouts[startingRoomID].needLeftRoom)
                {
                    AllRooms.Add(new Room(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(-1, 0), AllRooms[roomsBeingProcessed].Length + 1, -1, -1, -1, roomsBeingProcessed));
                    AllRooms[roomsBeingProcessed].leftRoomRef = AllRooms.Count - 1;
                }
                if (StartingRoomDoorLayouts[startingRoomID].needRightRoom)
                {
                    AllRooms.Add(new Room(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(1, 0), AllRooms[roomsBeingProcessed].Length + 1, -1, -1, roomsBeingProcessed, -1));
                    AllRooms[roomsBeingProcessed].rightRoomRef = AllRooms.Count - 1;
                }
            }
            else
            {
                if (AllRooms[roomsBeingProcessed].upperRoomRef == -1)
                {
                    if (UnityEngine.Random.Range(0, 100) < getRoomSpawnChance(AllRooms[roomsBeingProcessed].Length))
                    {
                        if (AddRoomOrWall(new Room(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(0, 1), AllRooms[roomsBeingProcessed].Length + 1, -1, roomsBeingProcessed, -1, -1)))
                        {
                            AllRooms[roomsBeingProcessed].upperRoomRef = AllRooms.Count - 1;
                        }
                    }
                }
                if (AllRooms[roomsBeingProcessed].lowerRoomRef == -1)
                {
                    if (UnityEngine.Random.Range(0, 100) < getRoomSpawnChance(AllRooms[roomsBeingProcessed].Length))
                    {
                        if (AddRoomOrWall(new Room(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(0, -1), AllRooms[roomsBeingProcessed].Length + 1, roomsBeingProcessed, -1, -1, -1)))
                        {
                            AllRooms[roomsBeingProcessed].lowerRoomRef = AllRooms.Count - 1;
                        }
                    }
                }
                if (AllRooms[roomsBeingProcessed].leftRoomRef == -1)
                {
                    if (UnityEngine.Random.Range(0, 100) < getRoomSpawnChance(AllRooms[roomsBeingProcessed].Length))
                    {
                        if (AddRoomOrWall(new Room(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(-1, 0), AllRooms[roomsBeingProcessed].Length + 1, -1, -1, -1, roomsBeingProcessed)))
                        {
                            AllRooms[roomsBeingProcessed].leftRoomRef = AllRooms.Count - 1;
                        }
                    }
                }
                if (AllRooms[roomsBeingProcessed].rightRoomRef == -1)
                {
                    if (UnityEngine.Random.Range(0, 100) < getRoomSpawnChance(AllRooms[roomsBeingProcessed].Length))
                    {
                        if (AddRoomOrWall(new Room(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(1, 0), AllRooms[roomsBeingProcessed].Length + 1, -1, -1, roomsBeingProcessed, -1)))
                        {
                            AllRooms[roomsBeingProcessed].rightRoomRef = AllRooms.Count - 1;
                        }
                    }
                }
            }
            roomsBeingProcessed++;
        }
        reconnectWalls();
        Debug.Log(AllRooms.Count);
        for (int i = 0; i < AllRooms.Count; i++)
        {
            if (i == 0)
            {
                Instantiate(StartingRooms[startingRoomID], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), StartingRooms[startingRoomID].transform.rotation);
                AllRooms[i].thisPrefabInfo = StartingRoomPrefabInfo[startingRoomID];
            }
            else
            {
               // Debug.Log(i + "  |  " + AllRooms[i].offsetPos + " | " + AllRooms[i].Length + " | " + AllRooms[i].upperRoomRef + " | " + AllRooms[i].lowerRoomRef + " | " + AllRooms[i].leftRoomRef + " | " + AllRooms[i].rightRoomRef);
                if (AllRooms[i].upperRoomRef != -1)
                {
                    if (AllRooms[i].lowerRoomRef != -1)
                    {
                        if (AllRooms[i].leftRoomRef != -1)
                        {
                            if (AllRooms[i].rightRoomRef != -1)
                            {
                                int holding = UnityEngine.Random.Range(0, AllDoorRoom.Count);
                                Instantiate(AllDoorRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom[holding].transform.rotation);
                                AllRooms[i].thisPrefabInfo = AllDoorRoomInfo[holding];
                            }
                            else
                            {
                                int holding = UnityEngine.Random.Range(0, UDLRoom.Count);
                                Instantiate(UDLRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), UDLRoom[holding].transform.rotation); ;
                                AllRooms[i].thisPrefabInfo = UDLRoomInfo[holding];
                            }
                        }
                        else if (AllRooms[i].rightRoomRef != -1)
                        {
                            int holding = UnityEngine.Random.Range(0, UDRRoom.Count);
                            Instantiate(UDRRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), UDRRoom[holding].transform.rotation);
                            AllRooms[i].thisPrefabInfo = UDRRoomInfo[holding];
                        }
                        else
                        {
                            int holding = UnityEngine.Random.Range(0, upDownRoom.Count);
                            Instantiate(upDownRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), upDownRoom[holding].transform.rotation);
                            AllRooms[i].thisPrefabInfo = upDownRoomInfo[holding];
                        }
                    }
                    else if (AllRooms[i].leftRoomRef != -1)
                    {
                        if (AllRooms[i].rightRoomRef != -1)
                        {
                            int holding = UnityEngine.Random.Range(0, ULRRoom.Count);
                            Instantiate(ULRRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), ULRRoom[holding].transform.rotation);
                            AllRooms[i].thisPrefabInfo = ULRRoomInfo[holding];
                        }
                        else
                        {
                            int holding = UnityEngine.Random.Range(0, upLeftRoom.Count);
                            Instantiate(upLeftRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), upLeftRoom[holding].transform.rotation);
                            AllRooms[i].thisPrefabInfo = upLeftRoomInfo[holding];
                        }
                    }
                    else if (AllRooms[i].rightRoomRef != -1)
                    {
                        int holding = UnityEngine.Random.Range(0, upRightRoom.Count);
                        Instantiate(upRightRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), upRightRoom[holding].transform.rotation);
                        AllRooms[i].thisPrefabInfo = upRightRoomInfo[holding];
                    }
                    else
                    {
                        int holding = UnityEngine.Random.Range(0, upRoom.Count);
                        Instantiate(upRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), upRoom[holding].transform.rotation);
                        AllRooms[i].thisPrefabInfo = upRoomInfo[holding];
                    }
                }
                else if (AllRooms[i].lowerRoomRef != -1)
                {
                    if (AllRooms[i].leftRoomRef != -1)
                    {
                        if (AllRooms[i].rightRoomRef != -1)
                        {
                            int holding = UnityEngine.Random.Range(0, DLRRoom.Count);
                            Instantiate(DLRRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), DLRRoom[holding].transform.rotation);
                            AllRooms[i].thisPrefabInfo = DLRRoomInfo[holding];
                        }
                        else
                        {
                            int holding = UnityEngine.Random.Range(0, downLeftRoom.Count);
                            Instantiate(downLeftRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), downLeftRoom[holding].transform.rotation);
                            AllRooms[i].thisPrefabInfo = downLeftRoomInfo[holding];
                        }
                    }
                    else if (AllRooms[i].rightRoomRef != -1)
                    {
                        int holding = UnityEngine.Random.Range(0, downRightRoom.Count);
                        Instantiate(downRightRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), downRightRoom[holding].transform.rotation);
                        AllRooms[i].thisPrefabInfo = downRightRoomInfo[holding];
                    }
                    else
                    {
                        int holding = UnityEngine.Random.Range(0, downRoom.Count);
                        Instantiate(downRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), downRoom[holding].transform.rotation);
                        AllRooms[i].thisPrefabInfo = downRoomInfo[holding];
                    }
                }
                else if (AllRooms[i].leftRoomRef != -1)
                {
                    if (AllRooms[i].rightRoomRef != -1)
                    {
                        int holding = UnityEngine.Random.Range(0, leftRightRoom.Count);
                        Instantiate(leftRightRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), leftRightRoom[holding].transform.rotation);
                        AllRooms[i].thisPrefabInfo = leftRightRoomInfo[holding];
                    }
                    else
                    {
                        int holding = UnityEngine.Random.Range(0, leftRoom.Count);
                        Instantiate(leftRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), leftRoom[holding].transform.rotation);
                        AllRooms[i].thisPrefabInfo = leftRoomInfo[holding];
                    }
                }
                else
                {
                    int holding = UnityEngine.Random.Range(0, rightRoom.Count);
                    Instantiate(rightRoom[holding], new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), rightRoom[holding].transform.rotation);
                    AllRooms[i].thisPrefabInfo = rightRoomInfo[holding];
                }
            }
        }
        // starting room
        // if first room do all
        // else have chance to add more rooms
        // process each room until they don't add more 
    }

    public bool AddRoomOrWall(Room ToAdd)
    {
        // check if empty, if no have a chance to make a wall between them
        int holdingID = -1;
        for (int i = 0; i < AllRooms.Count; i++)
        {
            if (AllRooms[i].offsetPos == ToAdd.offsetPos)
            {
                holdingID = i;
            }
        }

        if (holdingID < 0)
        {
            // no hit


            AllRooms.Add(ToAdd);
            return true;
        }

        return false;
    }

    public void reconnectWalls()
    {
        for (int i = 0; i < AllRooms.Count; i++)
        {
            for (int k = 0; k < AllRooms.Count; k++)
            {
                if (new Vector2(-1, 0) + AllRooms[i].offsetPos == AllRooms[k].offsetPos)
                {
                    if (UnityEngine.Random.Range(0, 100) < WallReConnectChanceOfHundred)
                    {
                        AllRooms[i].leftRoomRef = k;
                        AllRooms[k].rightRoomRef = i;
                    }
                }

                if (new Vector2(1, 0) + AllRooms[i].offsetPos == AllRooms[k].offsetPos)
                {
                    if (UnityEngine.Random.Range(0, 100) < WallReConnectChanceOfHundred)
                    {
                        AllRooms[i].rightRoomRef = k;
                        AllRooms[k].leftRoomRef = i;
                    }
                }

                if (new Vector2(0, 1) + AllRooms[i].offsetPos == AllRooms[k].offsetPos)
                {
                    if (UnityEngine.Random.Range(0, 100) < WallReConnectChanceOfHundred)
                    {
                        AllRooms[i].upperRoomRef = k;
                        AllRooms[k].lowerRoomRef = i;
                    }
                }

                if (new Vector2(0, -1) + AllRooms[i].offsetPos == AllRooms[k].offsetPos)
                {
                    if (UnityEngine.Random.Range(0, 100) < WallReConnectChanceOfHundred)
                    {
                        AllRooms[i].lowerRoomRef = k;
                        AllRooms[k].upperRoomRef = i;
                    }
                }

            }
        }
    }

    public void ReceiveDoorInput(Direction directionInput)
    {
        //playerTarget.transform.position = new Vector3(100, playerTarget.transform.position.y,100);

        switch (directionInput)
        {
            case Direction.Upper:
                currentRoomInside = AllRooms[currentRoomInside].upperRoomRef;
                needToSetPos = true;
                desiredSetPos = new Vector3(AllRooms[currentRoomInside].offsetPos.x * roomMultiplyValue.x + AllRooms[currentRoomInside].thisPrefabInfo.lowerRoomDoorSpawnOffet.x, playerTarget.transform.position.y, AllRooms[currentRoomInside].offsetPos.y * roomMultiplyValue.y + AllRooms[currentRoomInside].thisPrefabInfo.lowerRoomDoorSpawnOffet.y);
                if (debug)
                {
                    Debug.Log("Moving up " + desiredSetPos + "  " + AllRooms[currentRoomInside].thisPrefabInfo.lowerRoomDoorSpawnOffet);

                }
                break;
            case Direction.Lower:
                currentRoomInside = AllRooms[currentRoomInside].lowerRoomRef;
                needToSetPos = true;
                desiredSetPos = new Vector3(AllRooms[currentRoomInside].offsetPos.x * roomMultiplyValue.x + AllRooms[currentRoomInside].thisPrefabInfo.upperRoomDoorSpawnOffet.x, playerTarget.transform.position.y, AllRooms[currentRoomInside].offsetPos.y * roomMultiplyValue.y + AllRooms[currentRoomInside].thisPrefabInfo.upperRoomDoorSpawnOffet.y);
                break;
            case Direction.Left:
                currentRoomInside = AllRooms[currentRoomInside].leftRoomRef;
                needToSetPos = true;
                desiredSetPos = new Vector3(AllRooms[currentRoomInside].offsetPos.x * roomMultiplyValue.x + AllRooms[currentRoomInside].thisPrefabInfo.rightRoomDoorSpawnOffet.x, playerTarget.transform.position.y, AllRooms[currentRoomInside].offsetPos.y * roomMultiplyValue.y + AllRooms[currentRoomInside].thisPrefabInfo.rightRoomDoorSpawnOffet.y);
                break;
            case Direction.Right:
                currentRoomInside = AllRooms[currentRoomInside].rightRoomRef;
                needToSetPos = true;
                desiredSetPos = new Vector3(AllRooms[currentRoomInside].offsetPos.x * roomMultiplyValue.x + AllRooms[currentRoomInside].thisPrefabInfo.leftRoomDoorSpawnOffet.x, playerTarget.transform.position.y, AllRooms[currentRoomInside].offsetPos.y * roomMultiplyValue.y + AllRooms[currentRoomInside].thisPrefabInfo.leftRoomDoorSpawnOffet.y);
                break;
        }
        Debug.Log(directionInput);
    }

    private void LateUpdate()
    {
        if (needToSetPos)
        {
            if (playerTarget.transform.position == desiredSetPos)
            {
                needToSetPos = false;
            }
          //  Debug.Log(desiredSetPos);
            playerTarget.transform.position = desiredSetPos;
        }
    }

    public float getRoomSpawnChance(int lengthInput)
    {
        float holdingVal = 0;
        for (int i = 0; i < atHowManyCurrentRooms.Count; i++)
        {
            if (AllRooms.Count <= atHowManyCurrentRooms[i])
            {
              //  Debug.Log(ChanceOfHundredToSpawnNewRoom[i]);
                holdingVal += ChanceOfHundredToSpawnNewRoom[i];
            }
        }
        return holdingVal;
    }
}
