using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneration : MonoBehaviour
{
    public List<Room> AllRooms;

    int roomsBeingProcessed=0;
    public float roomSpawnChanceOfHundred;
    public float WallReConnectChanceOfHundred;

    public Vector2 roomMultiplyValue;

    #region roomPrefab
    public GameObject AllDoorRoom;

    public GameObject upRoom;
    public GameObject downRoom;
    public GameObject leftRoom;
    public GameObject rightRoom;

    public GameObject upDownRoom;
    public GameObject upLeftRoom;
    public GameObject upRightRoom;
    public GameObject downLeftRoom;
    public GameObject downRightRoom;
    public GameObject leftRightRoom;

    public GameObject UDLRoom;
    public GameObject UDRRoom;
    public GameObject ULRRoom;
    public GameObject DLRRoom;

    #endregion

    void Start()
    {
        roomSpawnChanceOfHundred = Mathf.Clamp(roomSpawnChanceOfHundred,0, 50);
        GenerateMap();
    }

    public void GenerateMap()
    {
        AllRooms = new List<Room>();
        AllRooms.Add(new Room(new Vector2(0, 0), false, false, false, false));

        while(roomsBeingProcessed<AllRooms.Count)
        {
         //   Debug.Log(roomsBeingProcessed);
            if (roomsBeingProcessed == 0)
            {
               if(!AllRooms[roomsBeingProcessed].hasUpperRoom)
                {
                    AllRooms.Add(new Room(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(0, 1), false, true, false, false));
                    AllRooms[roomsBeingProcessed].hasUpperRoom = true;
                }
                if (!AllRooms[roomsBeingProcessed].hasLowerRoom)
                {
                    AllRooms.Add(new Room(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(0, -1), true,false, false, false));
                    AllRooms[roomsBeingProcessed].hasLowerRoom = true;
                }
                if (!AllRooms[roomsBeingProcessed].hasLeftRoom)
                {
                    AllRooms.Add(new Room(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(-1, 0), false, false, false, true));
                    AllRooms[roomsBeingProcessed].hasLeftRoom = true;
                }
                if (!AllRooms[roomsBeingProcessed].hasRightRoom)
                {
                    AllRooms.Add(new Room(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(1, 0), false, false, true, false));
                    AllRooms[roomsBeingProcessed].hasRightRoom = true;
                }
            }
            else
            {
                if (!AllRooms[roomsBeingProcessed].hasUpperRoom)
                {
                    if(Random.Range(0, 100) < roomSpawnChanceOfHundred)
                    {
                      if(  AddRoomOrWall(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(0, 1), false, true, false, false))
                        {
                        AllRooms[roomsBeingProcessed].hasUpperRoom = true;

                        }
                    }
                }
                if (!AllRooms[roomsBeingProcessed].hasLowerRoom)
                {
                    if (Random.Range(0, 100) < roomSpawnChanceOfHundred)
                    {
                      if(  AddRoomOrWall(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(0, -1), true, false, false, false))
                        {

                        AllRooms[roomsBeingProcessed].hasLowerRoom = true;
                        }
                    }
                }
                if (!AllRooms[roomsBeingProcessed].hasLeftRoom)
                {
                    if (Random.Range(0, 100) < roomSpawnChanceOfHundred)
                    {
                        if(AddRoomOrWall(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(-1, 0), false, false, false, true))
                        {
                        AllRooms[roomsBeingProcessed].hasLeftRoom = true;

                        }
                    }
                }
                if (!AllRooms[roomsBeingProcessed].hasRightRoom)
                {
                    if (Random.Range(0, 100) < roomSpawnChanceOfHundred)
                    {
                     if(   AddRoomOrWall(AllRooms[roomsBeingProcessed].offsetPos + new Vector2(1, 0), false, false, true, false))
                        {
                        AllRooms[roomsBeingProcessed].hasRightRoom = true;

                        }
                    }
                }
            }
            roomsBeingProcessed++;
        }

        Debug.Log(AllRooms.Count);
        for(int i =0; i<AllRooms.Count; i++)
        {
            Debug.Log(i+ "   " +AllRooms[i].offsetPos + " " + AllRooms[i].hasUpperRoom + " " + AllRooms[i].hasLowerRoom + " " + AllRooms[i].hasLeftRoom + " " + AllRooms[i].hasRightRoom);
            if (AllRooms[i].hasUpperRoom)
            {
                if (AllRooms[i].hasLowerRoom)
                {
                    if (AllRooms[i].hasLeftRoom)
                    {
                       if (AllRooms[i].hasRightRoom)
                        {
                            Instantiate(AllDoorRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                        }
                        else
                        {
                            Instantiate(UDLRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                        }
                    }
                    else if (AllRooms[i].hasRightRoom)
                    {
                        Instantiate(UDRRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                    }
                    else
                    {
                        Instantiate(upDownRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                    }
                }
                else if (AllRooms[i].hasLeftRoom)
                {
                     if (AllRooms[i].hasRightRoom)
                    {
                        Instantiate(ULRRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                    }
                    else
                    {
                        Instantiate(upLeftRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                    }
                }
                else if (AllRooms[i].hasRightRoom)
                {
                    Instantiate(upRightRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                }
                else
                {
                    Instantiate(upRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                }
            }
            else if (AllRooms[i].hasLowerRoom)
            {
                if (AllRooms[i].hasLeftRoom)
                {
                    if (AllRooms[i].hasRightRoom)
                    {
                        Instantiate(DLRRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                    }
                    else
                    {
                        Instantiate(downLeftRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                    }
                }
                else if (AllRooms[i].hasRightRoom)
                {
                    Instantiate(downRightRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                }
                else
                {
                    Instantiate(downRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                }
            }
            else if (AllRooms[i].hasLeftRoom)
            {
               if (AllRooms[i].hasRightRoom)
                {
                    Instantiate(leftRightRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                }
                else
                {
                    Instantiate(leftRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
                }
            }
            else 
            {
                Instantiate(rightRoom, new Vector3(roomMultiplyValue.x * AllRooms[i].offsetPos.x, 0, roomMultiplyValue.y * AllRooms[i].offsetPos.y), AllDoorRoom.transform.rotation);
            }
         
        }

        // starting room
        // if first room do all
        // else have chance to add more rooms
        // process each room until they don't add more 


    }

    public bool AddRoomOrWall(Vector2 Input, bool U, bool B, bool L, bool R)
    {
      //  Debug.Log("checking " + Input);
        // check if empty, if no have a chance to make a wall between them
        int holdingID = -1;
        for (int i = 0; i < AllRooms.Count; i++)
        {
            if (AllRooms[i].offsetPos== Input)
            {
                holdingID = i;
            }
        }
        if (holdingID < 0)
        {
            // no hit
          //  Debug.Log("all g");
            AllRooms.Add(new Room(Input, U, B, L ,R));
            return true;
        }
        else
        {
           // Debug.Log("something there");
            // something is there

        }

        return false;
    }
    
    void Update()
    {
        
    }
}
