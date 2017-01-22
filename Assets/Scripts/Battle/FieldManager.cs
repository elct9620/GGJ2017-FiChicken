using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour {

    public Collider2D fieldCollider;
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
        
        

        GeneratePlayer("KeyPlayer",Color.white,"Key",new Vector3(-3.5f,2f,0),getRandomElementFromIntList(avatarIDs));
        GeneratePlayer("Joy1Player", Color.magenta, "Joy1", new Vector3(3.5f, -2f, 0), getRandomElementFromIntList(avatarIDs));
        if(playerCount == 3)
        {
            GeneratePlayer("Joy1Player", Color.cyan, "Joy2", new Vector3(3.5f, 2f, 0), getRandomElementFromIntList(avatarIDs));
        }
        if(playerCount >= 4)
        {
            GeneratePlayer("Joy1Player", Color.yellow, "Joy3", new Vector3(-3.5f, -2f, 0), getRandomElementFromIntList(avatarIDs));
        }
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
    }

	// Update is called once per frame
	void Update () {
		
	}
}
