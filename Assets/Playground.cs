using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playground : MonoBehaviour
{
    public GameObject roomPrefab;
    public Player player;
    public MapGeneration mapGeneration;
    public Room room;

    bool shouldStart = true;

    private void Start()
    {
        Init();
        //mapGeneration.StartEncounter(room);
    }

    private void Update()
    {
        if (
                GameManager.current.GetState() == GameState.Shop || 
                GameManager.current.GetState() == GameState.Pause
            )
        {
            shouldStart = true;
            return;
        }

        if ((shouldStart || GameManager.current.GetState() == GameState.Casual) && mapGeneration.EnemiesInEncounter == 0)
        {
            mapGeneration.StartEncounter(room);
            shouldStart = false;
            GameManager.current.ChangeStateImmdeiate(GameState.Game);
        }

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
        player.transform.position = mapGeneration.startingPos;
        transform.position = mapGeneration.startingPos;
        GameManager.current.ChangeState(GameState.Shop);
        mapGeneration.currentRoomInside = 1;
    }
}
