using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEncounter : MonoBehaviour
{
    public GameObject roomPrefab;
    public Transform camTarget;

    public Player player;
    public Room room;
    public MapGeneration mapGeneration;
    public GameObject bossPrefab;
    public AmmoPool ammoPool;
    // Start is called before the first frame update
    void Start()
    {
        GameObject roomObject = Instantiate(roomPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        room = new Room(new Vector2(0, 0), 0, -1, -1, -1, -1);
        room.thisPrefabInfo = roomObject.GetComponent<RoomPrefabInformation>();
        player.transform.position = new Vector3(20, 0, -12);

        GameObject holdingGameObject = Instantiate(bossPrefab, new Vector3(20, 0, 0), Quaternion.identity);
        BossEnemy enemy = holdingGameObject.GetComponent<BossEnemy>();
        enemy.Init(player, camTarget, ammoPool,0,0);
        enemy.mapGenerationScript = mapGeneration;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
