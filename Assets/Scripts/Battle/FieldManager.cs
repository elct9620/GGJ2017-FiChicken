using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour {

    public Collider2D fieldCollider;
    public List<PlayerController> Players = new List<PlayerController>();

    Vector3 P1Spawn = new Vector3(-3.5f, 2f, 0);
    Vector3 P2Spawn = new Vector3(3.5f, -2f, 0);
    Vector3 P3Spawn = new Vector3(3.5f, 2f, 0);
    Vector3 P4Spawn = new Vector3(-3.5f, -2f, 0);


    [SerializeField]
    GameObject canvasGO;
    Canvas canvas;
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    GameObject energyRingPrefab;
    [SerializeField]
    GameObject chargeRingPrefab;

    public int playerCount = 2;
	// Use this for initialization
	void Start () {
        //生成Canvas
        canvas = GameObject.Instantiate(canvasGO).GetComponent<Canvas>();
        //生成玩家

        List<int> avatarIDs = new List<int> { 0, 1, 2, 3 };

        GeneratePlayer("P1",Color.white,"Key", P1Spawn, getRandomElementFromIntList(avatarIDs));
        GeneratePlayer("P2", Color.magenta, "Joy1", P2Spawn, getRandomElementFromIntList(avatarIDs));
        if(playerCount >= 3)
        {
            GeneratePlayer("P3", Color.cyan, "Joy2", P3Spawn, getRandomElementFromIntList(avatarIDs));
        }
        if(playerCount >= 4)
        {
            GeneratePlayer("{P4}", Color.yellow, "Joy3", P4Spawn, getRandomElementFromIntList(avatarIDs));
        }

        FindObjectOfType<LevelManager>().LoadUI();
	}

    static int getRandomElementFromIntList(List<int> listToGet)
    {
        if(listToGet.Count == 0) { return 0; }
        int randomIndex = Random.Range(0, listToGet.Count);
        int ret = listToGet[randomIndex];
        listToGet.RemoveAt(randomIndex);
        return ret;
    }

    void GeneratePlayer(string playerName,Color playerColor,string playerControlTag,Vector3 revivePoint,int avatarID)
    {
        var playerGO = GameObject.Instantiate(playerPrefab);
        playerGO.name = playerName;
        var player = playerGO.GetComponent<PlayerController>();
        player.field = this;
        player.color = playerColor;
        player.controlTag = playerControlTag;
        player.revivePoint = revivePoint;
        player.transform.position = player.revivePoint;
        player.energyRing = GameObject.Instantiate(energyRingPrefab).GetComponent<Image>();
        player.energyRing.transform.SetParent(canvas.transform);
        player.chargeRing = GameObject.Instantiate(chargeRingPrefab).GetComponent<Image>();
        player.chargeRing.transform.SetParent(canvas.transform);
        player.score = 0;
        player.SetAvatar(avatarID);
        player.canvasTransform = canvas.transform;

        Players.Add(player);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
