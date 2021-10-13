using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playground : MonoBehaviour
{
    public GameObject roomPrefab;
    public Player player;
    public Room room;
    public MapGeneration mapGeneration;

    bool shouldStart = true;

    private void Start()
    {
        Init();
        //mapGeneration.StartEncounter(room);
    }

    private void Update()
    {
        if (GameManager.current.GetState() == GameState.Shop)
        {
            shouldStart = true;
            return;
        }

        if (shouldStart && GameManager.current.GetState() == GameState.Casual)
        {
            mapGeneration.StartEncounter(room);
            shouldStart = false;
            mapGeneration.inCombat = true;
        }

        //if (mapGeneration.inCombat && GameManager.current.GetState() == GameState.Game)
        //{
        //    mapGeneration.UpdateEncounter(room);
        //}

        if (mapGeneration.inCombat)
        {
            mapGeneration.UpdateEncounter(room);
        }
    }

    public void Init()
    {
        GameObject roomObject = Instantiate(roomPrefab, mapGeneration.startingPos, Quaternion.identity);
        room = new Room(new Vector2(0, 0), 0, -1, -1, -1, -1);
        room.thisPrefabInfo = roomObject.GetComponent<RoomPrefabInformation>();
        player.transform.position = new Vector3(0, 1.12f, 9.0f);
        transform.position = mapGeneration.startingPos;
        GameManager.current.ChangeState(GameState.Shop);
        mapGeneration.currentRoomInside = 1;
        mapGeneration.InitEvents();
    }
}
