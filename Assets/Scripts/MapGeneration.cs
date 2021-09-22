﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.AI;

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
    public List<Room> rooms;

    int roomsBeingProcessed = 0;
    public float wallReConnectChanceOfHundred;

    public Vector2 roomMultiplyValue;

    public Vector3 startingPos;

    #region map generate
    public List<float> chanceOfHundredToSpawnNewRoom;
    public List<int> atHowManyCurrentRooms;
    public List<float> roomSpawnChanceModifierBasedOnLength;

    #endregion

    #region roomPrefab
    public int startingRoomID;
    public List<StartingRoom> startingRoomDoorLayouts;

    public List<GameObject> startingRooms;
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

    #endregion

    #region BetweenRoomMovement
    public int currentRoomInside = 0;

    public Player playerTarget;
    public Transform camTarget;

    public bool needToSetPos;
    public Vector3 desiredSetPos; // used in late update because character controllers have a problem with 

    #endregion

    #region Encounter
    public List<GameObject> enemiesTypes;
    public List<GameObject> sniperTypes;
    public List<GameObject> swarmEnemiesType;

    public int minSwarmSpawnAmount;
    public int maxSwarmSpawnAmount;

    public float yEnemyHeight;
    public Vector2 enemyRandOffset;

    public bool inCombat;

    public List<Enemy> EnemiesInEncounter;
    public List<int> numberOfWaves;
    int currentWave;
    public float waveWaitingTime;
    public float currentWaveWaitingTime;
    bool roomClear;
    #endregion

    #region MiniMap
    public GameObject miniMapCenterRoom;
    public GameObject miniMapUpperRoom;
    public GameObject miniMapLowerRoom;
    public GameObject miniMapLeftRoom;
    public GameObject miniMapRightRoom;

    public Vector2 miniMapStartingPos;
    public Vector2 miniMapRoomMultiplier;

    public Color completedMiniMapRoomColour;
    public Color CurrentMiniMapRoomColour;
    public Color DefaultMiniMapRoomColour;
    #endregion

    #region difficulty
    public float totalRoomDiffculty;
    public float singleEnemyCutOff;
    public float secondWaveCutOff;
    public float roomFinishMultiplier;
    public float difficultyNoDamageMultiplier;

    #endregion

    #region healthPickup
    public float chanceOfSpawnOutOfOneHundredHealthPickup;
    public float damagedChanceToSpawnHealthPickup; // * recently Taken Damage

   public GameObject healthPickUpObject;

    #endregion

    public BTSManager thisBTSManager;

    public GameEvent beginEnounter;
    public GameEvent finishEnounter;

    public AmmoPool ammoPool;
    void Start()
    {
        GenerateMap();
        //[] lights = (Light[])GameObject.FindObjectsOfType(typeof(Light
        refreshMiniMapUI();
        playerTarget.transform.position = startingPos;
    }

    private void Update()
    {
        if (CheckIfMapCompleted())
        {
            thisBTSManager.LoadWinGameScene();
        }
        if (inCombat)
        {
            UpdateEncounter(rooms[currentRoomInside]);
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
                    rooms.Add(new Room(rooms[roomsBeingProcessed].offsetPos + new Vector2(0, 1),rooms[roomsBeingProcessed].length + 1, -1, roomsBeingProcessed, -1, -1));
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
        GameObject holdingObject = null;
        for (int i = 0; i < rooms.Count; i++)
        {
            if (i == 0)
            {
                holdingObject= Instantiate(startingRooms[startingRoomID], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), startingRooms[startingRoomID].transform.rotation);
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
                                holdingObject = Instantiate(allDoorRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), allDoorRoom[holding].transform.rotation);                             
                            }
                            else
                            {
                                int holding = UnityEngine.Random.Range(0, upDownLeftRoom.Count);
                                holdingObject = Instantiate(upDownLeftRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upDownLeftRoom[holding].transform.rotation); ;
                              
                            }
                        }
                        else if (rooms[i].rightRoomRef != -1)
                        {
                            int holding = UnityEngine.Random.Range(0, upDownRightRoom.Count);
                            holdingObject = Instantiate(upDownRightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upDownRightRoom[holding].transform.rotation);
                         
                        }
                        else
                        {
                            int holding = UnityEngine.Random.Range(0, upDownRoom.Count);
                            holdingObject = Instantiate(upDownRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upDownRoom[holding].transform.rotation);
                          
                        }
                    }
                    else if (rooms[i].leftRoomRef != -1)
                    {
                        if (rooms[i].rightRoomRef != -1)
                        {
                            int holding = UnityEngine.Random.Range(0, upLeftRightRoom.Count);
                            holdingObject = Instantiate(upLeftRightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upLeftRightRoom[holding].transform.rotation);
                           
                        }
                        else
                        {
                            int holding = UnityEngine.Random.Range(0, upLeftRoom.Count);
                            holdingObject = Instantiate(upLeftRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upLeftRoom[holding].transform.rotation);
                         
                        }
                    }
                    else if (rooms[i].rightRoomRef != -1)
                    {
                        int holding = UnityEngine.Random.Range(0, upRightRoom.Count);
                        holdingObject = Instantiate(upRightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upRightRoom[holding].transform.rotation);
                     
                    }
                    else
                    {
                        int holding = UnityEngine.Random.Range(0, upRoom.Count);
                        holdingObject = Instantiate(upRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), upRoom[holding].transform.rotation);
                      
                    }
                }
                else if (rooms[i].lowerRoomRef != -1)
                {
                    if (rooms[i].leftRoomRef != -1)
                    {
                        if (rooms[i].rightRoomRef != -1)
                        {
                            int holding = UnityEngine.Random.Range(0, downLeftRightRoom.Count);
                            holdingObject = Instantiate(downLeftRightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), downLeftRightRoom[holding].transform.rotation);
                          
                        }
                        else
                        {
                            int holding = UnityEngine.Random.Range(0, downLeftRoom.Count);
                            holdingObject = Instantiate(downLeftRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), downLeftRoom[holding].transform.rotation);
                          
                        }
                    }
                    else if (rooms[i].rightRoomRef != -1)
                    {
                        int holding = UnityEngine.Random.Range(0, downRightRoom.Count);
                        holdingObject = Instantiate(downRightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), downRightRoom[holding].transform.rotation);
                     
                    }
                    else
                    {
                        int holding = UnityEngine.Random.Range(0, downRoom.Count);
                        holdingObject = Instantiate(downRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), downRoom[holding].transform.rotation);
                  
                    }
                }
                else if (rooms[i].leftRoomRef != -1)
                {
                    if (rooms[i].rightRoomRef != -1)
                    {
                        int holding = UnityEngine.Random.Range(0, leftRightRoom.Count);
                        holdingObject = Instantiate(leftRightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), leftRightRoom[holding].transform.rotation);
                     
                    }
                    else
                    {
                        int holding = UnityEngine.Random.Range(0, leftRoom.Count);
                        holdingObject = Instantiate(leftRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), leftRoom[holding].transform.rotation);
                    
                    }
                }
                else
                {
                    int holding = UnityEngine.Random.Range(0, rightRoom.Count);
                    holdingObject = Instantiate(rightRoom[holding], new Vector3(roomMultiplyValue.x * rooms[i].offsetPos.x, 0, roomMultiplyValue.y * rooms[i].offsetPos.y), rightRoom[holding].transform.rotation);
                }
            }

            RoomPrefabInformation  holdingPrefabInfo = holdingObject.GetComponent<RoomPrefabInformation>();

            rooms[i].thisPrefabInfo = holdingPrefabInfo;
          //  Debug.Log(i);
        }
        // starting room
        // if first room do all
        // else have chance to add more rooms
        // process each room until they don't add more 
        finishEnounter?.Invoke();
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
                if (rooms[i].offsetPos != new Vector2(0, 0) && rooms[k].offsetPos != new Vector2(0, 0))
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
                else
                {
                 //   Debug.Log("Start room");
                }
            }
        }
    }

    public void SpawnSwarms(Room room, int swarmIndex)
    {
        List<Transform> holdingPossibleEnemySpawnPoints = room.thisPrefabInfo.enemySpawnPoint;
        int swarmSpawnAmount = UnityEngine.Random.Range(minSwarmSpawnAmount, maxSwarmSpawnAmount);
        for (int j = 0; j < swarmSpawnAmount; j++)
        {

            // swarm
            if (holdingPossibleEnemySpawnPoints.Count == 0)
            {
                Debug.Log("Refill");
                // holdingPossibleEnemySpawnPoints = room.thisPrefabInfo.enemySpawnPoint;
            }
            int holdingSpawnInt = UnityEngine.Random.Range(0, holdingPossibleEnemySpawnPoints.Count);

            Vector3 holdingPosition = new Vector3(holdingPossibleEnemySpawnPoints[holdingSpawnInt].position.x, yEnemyHeight, holdingPossibleEnemySpawnPoints[holdingSpawnInt].position.z);

            GameObject holdingGameObject = Instantiate(swarmEnemiesType[swarmIndex], holdingPosition, enemiesTypes[0].transform.rotation);
            holdingGameObject.GetComponent<Enemy>().Init(playerTarget.gameObject, camTarget, ammoPool);
            EnemiesInEncounter.Add(holdingGameObject.GetComponent<Enemy>());
            // holdingPossibleEnemySpawnPoints.RemoveAt(holdingSpawnInt);
        }
    }

    public void SpawnSniper(Room room, int sniperIndex)
    {
        // sniper
        List<Transform> holdingPossibleSniperSpawnPoints = room.thisPrefabInfo.sniperSpawnPoint;
        if (holdingPossibleSniperSpawnPoints.Count == 0)
        {
            Debug.Log("refill");
            //  holdingPossibleSniperSpawnPoints = room.thisPrefabInfo.sniperSpawnPoint;
        }
        int holdingSpawnInt = UnityEngine.Random.Range(0, holdingPossibleSniperSpawnPoints.Count);

        Vector3 holdingPosition = new Vector3(holdingPossibleSniperSpawnPoints[holdingSpawnInt].position.x, yEnemyHeight, holdingPossibleSniperSpawnPoints[holdingSpawnInt].position.z);
        holdingPossibleSniperSpawnPoints.RemoveAt(holdingSpawnInt);
        GameObject holdingGameObject = Instantiate(sniperTypes[sniperIndex], holdingPosition, enemiesTypes[0].transform.rotation);
        holdingGameObject.GetComponent<Enemy>().Init(playerTarget.gameObject, camTarget, ammoPool);
        EnemiesInEncounter.Add(holdingGameObject.GetComponent<Enemy>());
        //  holdingPossibleSniperSpawnPoints.RemoveAt(holdingSpawnInt);
    }

    public void SpawnNormalEnemy(Room room, int enemyIndex)
    {
        List<Transform> holdingPossibleEnemySpawnPoints = room.thisPrefabInfo.enemySpawnPoint;
        // not sniper
        if (holdingPossibleEnemySpawnPoints.Count == 0)
        {
            Debug.Log("Refill");
            // holdingPossibleEnemySpawnPoints = rooms[currentRoomInside].thisPrefabInfo.enemySpawnPoint;
        }
        int holdingSpawnInt = UnityEngine.Random.Range(0, holdingPossibleEnemySpawnPoints.Count);

        Vector3 holdingPosition = new Vector3(holdingPossibleEnemySpawnPoints[holdingSpawnInt].position.x, yEnemyHeight, holdingPossibleEnemySpawnPoints[holdingSpawnInt].position.z);
        if (totalRoomDiffculty < singleEnemyCutOff)
        {
            enemyIndex = 0;
        }

        GameObject holdingGameObject = Instantiate(enemiesTypes[enemyIndex], holdingPosition, enemiesTypes[0].transform.rotation);
        holdingGameObject.GetComponent<Enemy>().Init(playerTarget.gameObject, camTarget, ammoPool);
        EnemiesInEncounter.Add(holdingGameObject.GetComponent<Enemy>());
        // holdingPossibleEnemySpawnPoints.RemoveAt(holdingSpawnInt);
    }

    public void StartEncounter(Room room)
    {
        beginEnounter?.Invoke();
        currentWave = 0;
        numberOfWaves = new List<int>();
        if (playerTarget.RecentlyTakenDamage > 0)
        {
            playerTarget.RecentlyTakenDamage--;
        }
        // lock doors
        inCombat = true;

        //Debug.Log((totalRoomDiffucluty / 3) + " " + totalRoomDiffucluty / 2);
        int holdingRand = (int)UnityEngine.Random.Range((int)(totalRoomDiffculty / 3), (int)(totalRoomDiffculty / 2));
        holdingRand++;
        //  Debug.Log(holdingRand);
        if (totalRoomDiffculty < singleEnemyCutOff)
        {
            holdingRand = 1;
        }
        if (totalRoomDiffculty > secondWaveCutOff)
        {
            holdingRand = holdingRand / 2;
            numberOfWaves.Add(holdingRand + 1);
        }

        numberOfWaves.Add(holdingRand);

        GameManager.current.ChangeStateImmdeiate(GameState.Game);

        for (int i = 0; i < numberOfWaves[0]; i++)
        {
            int holdingRandomEnemType = UnityEngine.Random.Range(0, enemiesTypes.Count + sniperTypes.Count + swarmEnemiesType.Count);
            if (totalRoomDiffculty < singleEnemyCutOff)
            {
                holdingRandomEnemType = 0;
            }

            if (holdingRandomEnemType >= enemiesTypes.Count + sniperTypes.Count)
            {
                int swarmIndex = holdingRandomEnemType - enemiesTypes.Count - sniperTypes.Count;
                SpawnSwarms(room, swarmIndex);
            }
            else if (holdingRandomEnemType >= enemiesTypes.Count)
            {
                int sniperIndex = holdingRandomEnemType - enemiesTypes.Count;
                SpawnSniper(room, sniperIndex);
            }
            else
            {
                int enemyIndex = holdingRandomEnemType;
                SpawnNormalEnemy(room, enemyIndex);
            }

        }
    }

    public void CheckToStartEncounter()
    {
        if (!rooms[currentRoomInside].completed && currentRoomInside != 0)
        {
            StartEncounter(rooms[currentRoomInside]);
        }
    }

    public Vector3 ClampCameraVectorToCameraBoundsOfCurrentRoom(Room room, Vector3 input)
    {
        float holdingX= Mathf.Clamp(input.x, room.offsetPos.x * roomMultiplyValue.x + room.thisPrefabInfo.cameraBoundryMinX, room.offsetPos.x * roomMultiplyValue.x + room.thisPrefabInfo.cameraBoundryMaxX);
        float holdingZ = Mathf.Clamp(input.z, room.offsetPos.y * roomMultiplyValue.y + room.thisPrefabInfo.cameraBoundryMinZ, room.offsetPos.y * roomMultiplyValue.y + room.thisPrefabInfo.cameraBoundryMaxZ);

        Vector3 holdingOutPut = new Vector3(
            holdingX,
            0,
            holdingZ
        );
        return holdingOutPut;
    }

    public Vector3 ClampCameraVectorToCameraBoundsOfCurrentRoom(Vector3 input)
    {
        return ClampCameraVectorToCameraBoundsOfCurrentRoom(rooms[currentRoomInside], input);
    }

    public void UpdateRoom(Room room)
    {
        if (currentWaveWaitingTime > waveWaitingTime)
        {
            roomClear = false;

            for (int i = 0; i < numberOfWaves[currentWave]; i++)
            {
                int holdingRandomEnemType = UnityEngine.Random.Range(0, enemiesTypes.Count + sniperTypes.Count + swarmEnemiesType.Count);
                if (totalRoomDiffculty < singleEnemyCutOff)
                {
                    holdingRandomEnemType = 0;
                }

                if (holdingRandomEnemType >= enemiesTypes.Count + sniperTypes.Count)
                {
                    int swarmIndex = holdingRandomEnemType - enemiesTypes.Count - sniperTypes.Count;
                    SpawnSwarms(room, swarmIndex);
                }
                else if (holdingRandomEnemType >= enemiesTypes.Count)
                {
                    int sniperIndex = holdingRandomEnemType - enemiesTypes.Count;
                    SpawnSniper(room, sniperIndex);
                }
                else
                {
                    int enemyIndex = holdingRandomEnemType;
                    SpawnNormalEnemy(room, enemyIndex);
                }
            }

        }
        else
        {
            currentWaveWaitingTime += Time.deltaTime;
        }
    }

    public void UpdateEncounter(Room room)
    {
        if (roomClear)
        {
            UpdateRoom(room);
        }
        else
        {
            // next wave
            if (EnemiesInEncounter.Count <= 0)
            {
                currentWave++;

                Debug.Log("current wave: " + currentWave + " " + numberOfWaves.Count);
                if (currentWave < numberOfWaves.Count)
                {
                    Debug.Log("Current wave");
          
                    roomClear = true;
                    currentWaveWaitingTime = 0;
                }
                else
                {
                    totalRoomDiffculty += roomFinishMultiplier;
                    inCombat = false;
                    room.completed = true;
                    refreshMiniMapUI();
                    if (currentRoomInside != 0 && GameManager.current.GetState() != GameState.Casual)
                    {
                        finishEnounter?.Invoke();
                        GameManager.current.ChangeState(GameState.Shop);
                        GameManager.current.shop.Refresh();
                    }
                }
            }
            else
            {
                Vector2 holdingLastEnemy = new Vector2(0,0);
                for (int i = 0; i < EnemiesInEncounter.Count; i++)
                {
                    if (EnemiesInEncounter[i].hp <= 0)
                    {
                        holdingLastEnemy = new Vector2(EnemiesInEncounter[i].transform.position.x, EnemiesInEncounter[i].transform.position.z);
                        EnemiesInEncounter.RemoveAt(i);
                    }
                    else if (EnemiesInEncounter[i] == null)
                    {
                        Debug.Log("doesn't exist");
                        EnemiesInEncounter.RemoveAt(i);
                    }
                    else if (EnemiesInEncounter[i].gameObject == null)
                    {
                        EnemiesInEncounter.RemoveAt(i);
                    }
                }

                if(EnemiesInEncounter.Count <= 0)
                {
                   // Debug.Log("health");

                    TryToSpawnHealthPickup(new Vector3(holdingLastEnemy.x, healthPickUpObject.transform.position.y, holdingLastEnemy.y));
                   
                }
            }
        }
    }

    bool CheckIfMapCompleted()
    {
        for(int i =0; i < rooms.Count; i++)
        {
            if (!rooms[i].completed)
            {
                return false;
            }

        }
        return true;
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
                desiredSetPos = new Vector3(rooms[currentRoomInside].thisPrefabInfo.lowerRoomDoorSpawnOffet.position.x, playerTarget.transform.position.y, rooms[currentRoomInside].thisPrefabInfo.lowerRoomDoorSpawnOffet.position.z);
                   // desiredSetPos = new Vector3(rooms[currentRoomInside].offsetPos.x * roomMultiplyValue.x + rooms[currentRoomInside].thisPrefabInfo.lowerRoomDoorSpawnOffet.position.x, playerTarget.transform.position.y, rooms[currentRoomInside].offsetPos.y * roomMultiplyValue.y + rooms[currentRoomInside].thisPrefabInfo.lowerRoomDoorSpawnOffet.position.z);
                    playerTarget.transform.position = desiredSetPos;
                    //Debug.Log(desiredSetPos);
                break;
            case Direction.Lower:
                currentRoomInside = rooms[currentRoomInside].lowerRoomRef;
                needToSetPos = true;
                desiredSetPos = new Vector3(rooms[currentRoomInside].thisPrefabInfo.upperRoomDoorSpawnOffet.position.x, playerTarget.transform.position.y, rooms[currentRoomInside].thisPrefabInfo.upperRoomDoorSpawnOffet.position.z);
                playerTarget.transform.position = desiredSetPos;
                    Debug.Log(desiredSetPos);
                    break;
            case Direction.Left:
                currentRoomInside = rooms[currentRoomInside].leftRoomRef;
                needToSetPos = true;
                desiredSetPos = new Vector3(rooms[currentRoomInside].thisPrefabInfo.rightRoomDoorSpawnOffet.position.x, playerTarget.transform.position.y,rooms[currentRoomInside].thisPrefabInfo.rightRoomDoorSpawnOffet.position.z);
                playerTarget.transform.position = desiredSetPos;
                    Debug.Log(desiredSetPos);
                    break;
            case Direction.Right:
                currentRoomInside = rooms[currentRoomInside].rightRoomRef;
                needToSetPos = true;
                desiredSetPos = new Vector3(rooms[currentRoomInside].thisPrefabInfo.leftRoomDoorSpawnOffet.position.x, playerTarget.transform.position.y,  rooms[currentRoomInside].thisPrefabInfo.leftRoomDoorSpawnOffet.position.z);
                playerTarget.transform.position = desiredSetPos;
                    Debug.Log(desiredSetPos);
                    break;
        }

            CheckToStartEncounter();
            refreshMiniMapUI();
            //Debug.Log(directionInput);
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

    public void TryToSpawnHealthPickup(Vector3 pos)
    {
        float holdingRandom = UnityEngine.Random.Range(0, 100);

        if (holdingRandom < chanceOfSpawnOutOfOneHundredHealthPickup +  playerTarget.RecentlyTakenDamage * damagedChanceToSpawnHealthPickup)
        {
            Debug.Log("Spawn " + (chanceOfSpawnOutOfOneHundredHealthPickup + playerTarget.RecentlyTakenDamage * damagedChanceToSpawnHealthPickup)+ "  " + playerTarget.RecentlyTakenDamage);
            healthPickUpObject.SetActive(true);
            healthPickUpObject.transform.position = pos;
        }
    }

    public void refreshMiniMapUI()
    {
        if (currentRoomInside >= rooms.Count) return;
        int holdingPos = 1;
        //miniMapRooms[0].transform.position = miniMapStartingPos;
        if (rooms[currentRoomInside].completed)
        {
            miniMapCenterRoom.GetComponent<Image>().color = CurrentMiniMapRoomColour;
        }
        else
        {
            miniMapCenterRoom.GetComponent<Image>().color = DefaultMiniMapRoomColour;
        }

        if (rooms[currentRoomInside].upperRoomRef > -1)
        {
            miniMapUpperRoom.SetActive(true);
         
            if (rooms[rooms[currentRoomInside].upperRoomRef].completed)
            {
                miniMapUpperRoom.GetComponent<Image>().color = completedMiniMapRoomColour;
            }
            else
            {
                miniMapUpperRoom.GetComponent<Image>().color = DefaultMiniMapRoomColour;
            }
            holdingPos++;
        }
        else
        {
            miniMapUpperRoom.SetActive(false);
        }

        if (rooms[currentRoomInside].lowerRoomRef > -1)
        {
            miniMapLowerRoom.SetActive(true);
            if (rooms[rooms[currentRoomInside].lowerRoomRef].completed)
            {
                miniMapLowerRoom.GetComponent<Image>().color = completedMiniMapRoomColour;
            }
            else
            {
                miniMapLowerRoom.GetComponent<Image>().color = DefaultMiniMapRoomColour;
            }
            holdingPos++;
        }
        else
        {
            miniMapLowerRoom.SetActive(false);
        }

        if (rooms[currentRoomInside].leftRoomRef > -1)
        {
            miniMapLeftRoom.SetActive(true);
            if (rooms[rooms[currentRoomInside].leftRoomRef].completed)
            {
                miniMapLeftRoom.GetComponent<Image>().color = completedMiniMapRoomColour;
            }
            else
            {
                miniMapLeftRoom.GetComponent<Image>().color = DefaultMiniMapRoomColour;
            }
            holdingPos++;
        }
        else
        {
            miniMapLeftRoom.SetActive(false);
        }

        if (rooms[currentRoomInside].rightRoomRef > -1)
        {
            miniMapRightRoom.SetActive(true);
            if (rooms[rooms[currentRoomInside].rightRoomRef].completed)
            {
                miniMapRightRoom.GetComponent<Image>().color = completedMiniMapRoomColour;
            }
            else
            {
                miniMapRightRoom.GetComponent<Image>().color = DefaultMiniMapRoomColour;
            }
            holdingPos++;
        }
        else
        {
            miniMapRightRoom.SetActive(false);
        }
    }
}