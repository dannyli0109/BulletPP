using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum Direction
{
    Upper,
    Lower,
    Left,
    Right
}

[Serializable]
public class StartingRoom
{
    public bool needUpperRoom;
    public bool needLowerRoom;
    public bool needLeftRoom;
    public bool needRightRoom;

}

public class MapGeneration : MonoBehaviour
{
    public bool debug;

    public List<Room> rooms;

    int roomsBeingProcessed = 0;
    public float wallReConnectChanceOfHundred;

    public Vector2 roomMultiplyValue;

    #region map generate
    public List<float> chanceOfHundredToSpawnNewRoom;
    public List<int> atHowManyCurrentRooms;
    public List<float> roomSpawnChanceModifierBasedOnLength;

    #endregion

    #region roomPrefab
    public int startingRoomID;
    public List<StartingRoom> startingRoomDoorLayouts;
    public List<GameObject> startingRooms;
    public List<RoomPrefabInformation> startingRoomPrefabInfo;

    public List<GameObject> allDoorRoom;

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

    public List<GameObject> upDownLeftRoom;
    public List<GameObject> upDownRightRoom;
    public List<GameObject> upLeftRightRoom;
    public List<GameObject> downLeftRightRoom;


    public List<RoomPrefabInformation> allDoorRoomInfo;

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

    public List<RoomPrefabInformation> upDownLeftRoomInfo;
    public List<RoomPrefabInformation> upDownRightRoomInfo;
    public List<RoomPrefabInformation> upLeftRightRoomInfo;
    public List<RoomPrefabInformation> downLeftRightRoomInfo;

    #endregion

    #region BetweenRoomMovement
    public int currentRoomInside = 0;

    public GameObject playerTarget;
    public Transform camTarget;

    public bool needToSetPos;
    public Vector3 desiredSetPos; // used in late update because character controllers have a problem with 

    #endregion

    #region Encounter
    public GameObject Enemy;
    public float yEnemyHeight;
    public Vector2 enemyRandOffset;

    public bool inCombat;

    public List< Enemy> EnemiesInEncounter;
    #endregion

    #region MiniMap
    public List<GameObject> miniMapRooms;

    public Vector2 miniMapStartingPos;
    public Vector2 miniMapRoomMultiplier;

    public Color completedMiniMapRoomColour;
    public Color CurrentMiniMapRoomColour;
    public Color DefaultMiniMapRoomColour;
    #endregion

    public BTSManager thisBTSManager;

    void Start()
    {
        GenerateMap();
        refreshMiniMapUI();
    }

    private void Update()
    {
        if (inCombat)
        {
        UpdateEncounter();
        }
    }

    public void GenerateMap()
    {
        startingRoomID = UnityEngine.Random.Range(0, startingRooms.Count);
        rooms = new List<Room>();
        rooms.Add(new Room(new Vector2(0, 0),0, -1, -1, -1, -1));

        while (roomsBeingProcessed < rooms.Count)
        {
            //   Debug.Log(roomsBeingProcessed);
            if (roomsBeingProcessed == 0)
            {
                if (startingRoomDoorLayouts[startingRoomID].needUpperRoom )
                {
                    rooms.Add(new Room(rooms[roomsBeingProcessed].offsetPos + new Vector2(0, 1),rooms[roomsBeingProcessed].length+1, -1, roomsBeingProcessed, -1, -1));
                    rooms[roomsBeingProcessed].upperRoomRef = rooms.Count - 1;
                }
                if (startingRoomDoorLayouts[startingRoomID].needLowerRoom)
                {
                    rooms.Add(new Room(rooms[roomsBeingProcessed].offsetPos + new Vector2(0, -1), rooms[roomsBeingProcessed].length + 1, roomsBeingProcessed, -1, -1, -1));
                    rooms[roomsBeingProcessed].lowerRoomRef = rooms.Count - 1;
                }
                if (startingRoomDoorLayouts[startingRoomID].needLeftRoom)
                {
                    rooms.Add(new Room(rooms[roomsBeingProcessed].offsetPos + new Vector2(-1, 0), rooms[roomsBeingProcessed].length + 1, -1, -1, -1, roomsBeingProcessed));
                    rooms[roomsBeingProcessed].leftRoomRef = rooms.Count - 1;
                }
                if (startingRoomDoorLayouts[startingRoomID].needRightRoom)
                {
                    rooms.Add(new Room(rooms[roomsBeingProcessed].offsetPos + new Vector2(1, 0), rooms[roomsBeingProcessed].length + 1, -1, -1, roomsBeingProcessed, -1));
                    rooms[roomsBeingProcessed].rightRoomRef = rooms.Count - 1;
                }
            }
            else
            {
                if (rooms[roomsBeingProcessed].upperRoomRef == -1)
                {
                    if (UnityEngine.Random.Range(0, 100) < getRoomSpawnChance(rooms[roomsBeingProcessed].length))
                    {
                        if (AddRoomOrWall(new Room(rooms[roomsBeingProcessed].offsetPos + new Vector2(0, 1), rooms[roomsBeingProcessed].length + 1, -1, roomsBeingProcessed, -1, -1)))
                        {
                            rooms[roomsBeingProcessed].upperRoomRef = rooms.Count - 1;
                        }
                    }
                }
                if (rooms[roomsBeingProcessed].lowerRoomRef == -1)
                {
                    if (UnityEngine.Random.Range(0, 100) < getRoomSpawnChance(rooms[roomsBeingProcessed].length))
                    {
                        if (AddRoomOrWall(new Room(rooms[roomsBeingProcessed].offsetPos + new Vector2(0, -1), rooms[roomsBeingProcessed].length + 1, roomsBeingProcessed, -1, -1, -1)))
                        {
                            rooms[roomsBeingProcessed].lowerRoomRef = rooms.Count - 1;
                        }
                    }
                }
                if (rooms[roomsBeingProcessed].leftRoomRef == -1)
                {
                    if (UnityEngine.Random.Range(0, 100) < getRoomSpawnChance(rooms[roomsBeingProcessed].length))
                    {
                        if (AddRoomOrWall(new Room(rooms[roomsBeingProcessed].offsetPos + new Vector2(-1, 0), rooms[roomsBeingProcessed].length + 1, -1, -1, -1, roomsBeingProcessed)))
                        {
                            rooms[roomsBeingProcessed].leftRoomRef = rooms.Count - 1;
                        }
                    }
                }
                if (rooms[roomsBeingProcessed].rightRoomRef == -1)
                {
                    if (UnityEngine.Random.Range(0, 100) < getRoomSpawnChance(rooms[roomsBeingProcessed].length))
                    {
                        if (AddRoomOrWall(new Room(rooms[roomsBeingProcessed].offsetPos + new Vector2(1, 0), rooms[roomsBeingProcessed].length + 1, -1, -1, roomsBeingProcessed, -1)))
                        {
                            rooms[roomsBeingProcessed].rightRoomRef = rooms.Count - 1;
                        }
                    }
                }
            }
            roomsBeingProcessed++;
        }
        reconnectWalls();
       // Debug.Log(AllRooms.Count);
        for (int i = 0; i < rooms.Count; i++)
        {
            if (i == 0)
            {
                Instantiate(startingRooms[startingRoomID], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), startingRooms[startingRoomID].transform.rotation);
                rooms[i].thisPrefabInfo = startingRoomPrefabInfo[startingRoomID];
            }
            else
            {
               // Debug.Log(i + "  |  " + AllRooms[i].offsetPos + " | " + AllRooms[i].Length + " | " + AllRooms[i].upperRoomRef + " | " + AllRooms[i].lowerRoomRef + " | " + AllRooms[i].leftRoomRef + " | " + AllRooms[i].rightRoomRef);
                if (rooms[i].upperRoomRef != -1)
                {
                    if (rooms[i].lowerRoomRef != -1)
                    {
                        if (rooms[i].leftRoomRef != -1)
                        {
                            if (rooms[i].rightRoomRef != -1)
                            {
                                int holding = UnityEngine.Random.Range(0, allDoorRoom.Count);
                                Instantiate(allDoorRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), allDoorRoom[holding].transform.rotation);
                                rooms[i].thisPrefabInfo = allDoorRoomInfo[holding];
                            }
                            else
                            {
                                int holding = UnityEngine.Random.Range(0, upDownLeftRoom.Count);
                                Instantiate(upDownLeftRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upDownLeftRoom[holding].transform.rotation); ;
                                rooms[i].thisPrefabInfo = upDownLeftRoomInfo[holding];
                            }
                        }
                        else if (rooms[i].rightRoomRef != -1)
                        {
                            int holding = UnityEngine.Random.Range(0, upDownRightRoom.Count);
                            Instantiate(upDownRightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upDownRightRoom[holding].transform.rotation);
                            rooms[i].thisPrefabInfo = upDownRightRoomInfo[holding];
                        }
                        else
                        {
                            int holding = UnityEngine.Random.Range(0, upDownRoom.Count);
                            Instantiate(upDownRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upDownRoom[holding].transform.rotation);
                            rooms[i].thisPrefabInfo = upDownRoomInfo[holding];
                        }
                    }
                    else if (rooms[i].leftRoomRef != -1)
                    {
                        if (rooms[i].rightRoomRef != -1)
                        {
                            int holding = UnityEngine.Random.Range(0, upLeftRightRoom.Count);
                            Instantiate(upLeftRightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upLeftRightRoom[holding].transform.rotation);
                            rooms[i].thisPrefabInfo = upLeftRightRoomInfo[holding];
                        }
                        else
                        {
                            int holding = UnityEngine.Random.Range(0, upLeftRoom.Count);
                            Instantiate(upLeftRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upLeftRoom[holding].transform.rotation);
                            rooms[i].thisPrefabInfo = upLeftRoomInfo[holding];
                        }
                    }
                    else if (rooms[i].rightRoomRef != -1)
                    {
                        int holding = UnityEngine.Random.Range(0, upRightRoom.Count);
                        Instantiate(upRightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upRightRoom[holding].transform.rotation);
                        rooms[i].thisPrefabInfo = upRightRoomInfo[holding];
                    }
                    else
                    {
                        int holding = UnityEngine.Random.Range(0, upRoom.Count);
                        Instantiate(upRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upRoom[holding].transform.rotation);
                        rooms[i].thisPrefabInfo = upRoomInfo[holding];
                    }
                }
                else if (rooms[i].lowerRoomRef != -1)
                {
                    if (rooms[i].leftRoomRef != -1)
                    {
                        if (rooms[i].rightRoomRef != -1)
                        {
                            int holding = UnityEngine.Random.Range(0, downLeftRightRoom.Count);
                            Instantiate(downLeftRightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), downLeftRightRoom[holding].transform.rotation);
                            rooms[i].thisPrefabInfo = downLeftRightRoomInfo[holding];
                        }
                        else
                        {
                            int holding = UnityEngine.Random.Range(0, downLeftRoom.Count);
                            Instantiate(downLeftRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), downLeftRoom[holding].transform.rotation);
                            rooms[i].thisPrefabInfo = downLeftRoomInfo[holding];
                        }
                    }
                    else if (rooms[i].rightRoomRef != -1)
                    {
                        int holding = UnityEngine.Random.Range(0, downRightRoom.Count);
                        Instantiate(downRightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), downRightRoom[holding].transform.rotation);
                        rooms[i].thisPrefabInfo = downRightRoomInfo[holding];
                    }
                    else
                    {
                        int holding = UnityEngine.Random.Range(0, downRoom.Count);
                        Instantiate(downRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), downRoom[holding].transform.rotation);
                        rooms[i].thisPrefabInfo = downRoomInfo[holding];
                    }
                }
                else if (rooms[i].leftRoomRef != -1)
                {
                    if (rooms[i].rightRoomRef != -1)
                    {
                        int holding = UnityEngine.Random.Range(0, leftRightRoom.Count);
                        Instantiate(leftRightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), leftRightRoom[holding].transform.rotation);
                        rooms[i].thisPrefabInfo = leftRightRoomInfo[holding];
                    }
                    else
                    {
                        int holding = UnityEngine.Random.Range(0, leftRoom.Count);
                        Instantiate(leftRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), leftRoom[holding].transform.rotation);
                        rooms[i].thisPrefabInfo = leftRoomInfo[holding];
                    }
                }
                else
                {
                    int holding = UnityEngine.Random.Range(0, rightRoom.Count);
                    Instantiate(rightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), rightRoom[holding].transform.rotation);
                    rooms[i].thisPrefabInfo = rightRoomInfo[holding];
                }
            }
        }
        // starting room
        // if first room do all
        // else have chance to add more rooms
        // process each room until they don't add more 
        rooms[0].completed = true;
    }

    public bool AddRoomOrWall(Room ToAdd)
    {
        // check if empty, if no have a chance to make a wall between them
        int holdingID = -1;
        for (int i = 0; i < rooms.Count; i++)
        {
            if (rooms[i].offsetPos == ToAdd.offsetPos)
            {
                holdingID = i;
            }
        }

        if (holdingID < 0)
        {
            // no hit
            rooms.Add(ToAdd);
            return true;
        }

        return false;
    }

    public void reconnectWalls()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            for (int k = 0; k < rooms.Count; k++)
            {
                if (new Vector2(-1, 0) + rooms[i].offsetPos == rooms[k].offsetPos)
                {
                    if (UnityEngine.Random.Range(0, 100) < wallReConnectChanceOfHundred)
                    {
                        rooms[i].leftRoomRef = k;
                        rooms[k].rightRoomRef = i;
                    }
                }

                if (new Vector2(1, 0) + rooms[i].offsetPos == rooms[k].offsetPos)
                {
                    if (UnityEngine.Random.Range(0, 100) < wallReConnectChanceOfHundred)
                    {
                        rooms[i].rightRoomRef = k;
                        rooms[k].leftRoomRef = i;
                    }
                }

                if (new Vector2(0, 1) + rooms[i].offsetPos == rooms[k].offsetPos)
                {
                    if (UnityEngine.Random.Range(0, 100) < wallReConnectChanceOfHundred)
                    {
                        rooms[i].upperRoomRef = k;
                        rooms[k].lowerRoomRef = i;
                    }
                }

                if (new Vector2(0, -1) + rooms[i].offsetPos == rooms[k].offsetPos)
                {
                    if (UnityEngine.Random.Range(0, 100) < wallReConnectChanceOfHundred)
                    {
                        rooms[i].lowerRoomRef = k;
                        rooms[k].upperRoomRef = i;
                    }
                }
            }
        }
    }

    public void CheckToStartEncounter()
    {
        if (!rooms[currentRoomInside].completed && currentRoomInside != 0)
        {
            int holdingRand = UnityEngine.Random.Range(1, 4);
            if (currentRoomInside <= 4)
            {
                holdingRand = 1;
            }
            // lock doors
            Debug.Log("encounter start");
            Vector3 placement = new Vector3(rooms[currentRoomInside].offsetPos.x * roomMultiplyValue.x + rooms[currentRoomInside].thisPrefabInfo.middleOffset.x, yEnemyHeight, rooms[currentRoomInside].offsetPos.y * roomMultiplyValue.y + rooms[currentRoomInside].thisPrefabInfo.middleOffset.y);
            inCombat = true;

            for (int i = 0; i < holdingRand; i++)
            {
                GameObject holdingGameObject = Instantiate(Enemy, placement + new Vector3(UnityEngine.Random.Range(-enemyRandOffset.x, enemyRandOffset.x), 0, UnityEngine.Random.Range(-enemyRandOffset.y, enemyRandOffset.y)), Enemy.transform.rotation);
                holdingGameObject.GetComponent<Enemy>().target = playerTarget;
                holdingGameObject.transform.GetChild(0).gameObject.GetComponent<Billboard>().cam = camTarget;
                EnemiesInEncounter.Add(holdingGameObject.GetComponent<Enemy>());
                GameManager.current.gameState = GameState.Game;
            }

        }
    }

    public void UpdateEncounter()
    {
        if (EnemiesInEncounter.Count <= 0)
        {
            inCombat = false;
            rooms[currentRoomInside].completed = true;
            CheckIfMapCompleted();
            refreshMiniMapUI();
            if (currentRoomInside != 0 && GameManager.current.gameState != GameState.Casual)
            {
                GameManager.current.gameState = GameState.Shop;
                GameManager.current.shop.Refresh();
            }
        }
        else
        {
            for (int i = 0; i < EnemiesInEncounter.Count; i++)
            {
                if (EnemiesInEncounter[i].hp <= 0)
                {
                    Debug.Log(i + " Dead" + EnemiesInEncounter[i].hp);
                    EnemiesInEncounter.RemoveAt(i);
                }
                else
                {
                    //  Debug.Log(i + " Alive " + EnemiesInEncounter[i].hp);
                }
            }
        }
    }

    void CheckIfMapCompleted()
    {
        for(int i =0; i < rooms.Count; i++)
        {
            if (!rooms[i].completed)
            {
                Debug.Log(i + " room");
                return;
            }

        }
        Debug.Log("all done");
        thisBTSManager.LoadWinGameScene();
    }

    public void ReceiveDoorInput(Direction directionInput)
    {
        if (!inCombat)
        {
        switch (directionInput)
        {
            case Direction.Upper:
                currentRoomInside = rooms[currentRoomInside].upperRoomRef;
                needToSetPos = true;
                desiredSetPos = new Vector3(rooms[currentRoomInside].offsetPos.x * roomMultiplyValue.x + rooms[currentRoomInside].thisPrefabInfo.lowerRoomDoorSpawnOffet.x, playerTarget.transform.position.y, rooms[currentRoomInside].offsetPos.y * roomMultiplyValue.y + rooms[currentRoomInside].thisPrefabInfo.lowerRoomDoorSpawnOffet.y);
                playerTarget.transform.position = desiredSetPos;
                if (debug)
                {
                    Debug.Log("Moving up " + desiredSetPos + "  " + rooms[currentRoomInside].thisPrefabInfo.lowerRoomDoorSpawnOffet);
                }
                break;
            case Direction.Lower:
                currentRoomInside = rooms[currentRoomInside].lowerRoomRef;
                needToSetPos = true;
                desiredSetPos = new Vector3(rooms[currentRoomInside].offsetPos.x * roomMultiplyValue.x + rooms[currentRoomInside].thisPrefabInfo.upperRoomDoorSpawnOffet.x, playerTarget.transform.position.y, rooms[currentRoomInside].offsetPos.y * roomMultiplyValue.y + rooms[currentRoomInside].thisPrefabInfo.upperRoomDoorSpawnOffet.y);
                playerTarget.transform.position = desiredSetPos;
                break;
            case Direction.Left:
                currentRoomInside = rooms[currentRoomInside].leftRoomRef;
                needToSetPos = true;
                desiredSetPos = new Vector3(rooms[currentRoomInside].offsetPos.x * roomMultiplyValue.x + rooms[currentRoomInside].thisPrefabInfo.rightRoomDoorSpawnOffet.x, playerTarget.transform.position.y, rooms[currentRoomInside].offsetPos.y * roomMultiplyValue.y + rooms[currentRoomInside].thisPrefabInfo.rightRoomDoorSpawnOffet.y);
                playerTarget.transform.position = desiredSetPos;
                break;
            case Direction.Right:
                currentRoomInside = rooms[currentRoomInside].rightRoomRef;
                needToSetPos = true;
                desiredSetPos = new Vector3(rooms[currentRoomInside].offsetPos.x * roomMultiplyValue.x + rooms[currentRoomInside].thisPrefabInfo.leftRoomDoorSpawnOffet.x, playerTarget.transform.position.y, rooms[currentRoomInside].offsetPos.y * roomMultiplyValue.y + rooms[currentRoomInside].thisPrefabInfo.leftRoomDoorSpawnOffet.y);
                playerTarget.transform.position = desiredSetPos;
                break;
        }

            CheckToStartEncounter();
            refreshMiniMapUI();
            Debug.Log(directionInput);
        }
    }

    public float getRoomSpawnChance(int lengthInput)
    {
        float holdingVal = 0;
        for (int i = 0; i < atHowManyCurrentRooms.Count; i++)
        {
            if (rooms.Count <= atHowManyCurrentRooms[i])
            {
              //  Debug.Log(ChanceOfHundredToSpawnNewRoom[i]);
                holdingVal += chanceOfHundredToSpawnNewRoom[i];
            }
        }
        return holdingVal;
    }

    public void refreshMiniMapUI()
    {
        for(int i=1; i < 5;i++)
        {
            miniMapRooms[i].SetActive(false);

        }
        int holdingPos = 1;
        miniMapRooms[0].SetActive(true);
        miniMapRooms[0].transform.position = miniMapStartingPos;
        if (rooms[currentRoomInside].completed)
        {
            miniMapRooms[0].GetComponent<Image>().color = CurrentMiniMapRoomColour;
        }
        else
        {
            miniMapRooms[0].GetComponent<Image>().color = DefaultMiniMapRoomColour;
        }

        if (rooms[currentRoomInside].upperRoomRef > -1)
        {
            miniMapRooms[holdingPos].SetActive(true);
            miniMapRooms[holdingPos].transform.position = miniMapStartingPos + new Vector2(0, 1) * miniMapRoomMultiplier;
            if (rooms[rooms[currentRoomInside].upperRoomRef].completed)
            {
                miniMapRooms[holdingPos].GetComponent<Image>().color = completedMiniMapRoomColour;
            }
            else
            {
                miniMapRooms[holdingPos].GetComponent<Image>().color = DefaultMiniMapRoomColour;
            }
            holdingPos++;
        }

        if (rooms[currentRoomInside].lowerRoomRef> -1)
        {
            miniMapRooms[holdingPos].SetActive(true);
            miniMapRooms[holdingPos].transform.position = miniMapStartingPos + new Vector2(0, -1) * miniMapRoomMultiplier;
            if (rooms[rooms[currentRoomInside].lowerRoomRef].completed)
            {
                miniMapRooms[holdingPos].GetComponent<Image>().color = completedMiniMapRoomColour;
            }
            else
            {
                miniMapRooms[holdingPos].GetComponent<Image>().color = DefaultMiniMapRoomColour;
            }
            holdingPos++;
        }

        if (rooms[currentRoomInside].leftRoomRef > -1)
        {
            miniMapRooms[holdingPos].SetActive(true);
            miniMapRooms[holdingPos].transform.position = miniMapStartingPos + new Vector2(-1, 0) * miniMapRoomMultiplier;
            if (rooms[rooms[currentRoomInside].leftRoomRef].completed)
            {
                miniMapRooms[holdingPos].GetComponent<Image>().color = completedMiniMapRoomColour;
            }
            else
            {
                miniMapRooms[holdingPos].GetComponent<Image>().color = DefaultMiniMapRoomColour;
            }
            holdingPos++;
        }

        if (rooms[currentRoomInside].rightRoomRef > -1)
        {
            miniMapRooms[holdingPos].SetActive(true);
            miniMapRooms[holdingPos].transform.position = miniMapStartingPos + new Vector2(1, 0) * miniMapRoomMultiplier;
            if (rooms[rooms[currentRoomInside].rightRoomRef].completed)
            {
                miniMapRooms[holdingPos].GetComponent<Image>().color = completedMiniMapRoomColour;
            }
            else
            {
                miniMapRooms[holdingPos].GetComponent<Image>().color = DefaultMiniMapRoomColour;
            }
            holdingPos++;
        }

    }
}