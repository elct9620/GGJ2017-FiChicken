using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour {

    public Collider2D fieldCollider;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    GameObject energyRing;
    [SerializeField]
    GameObject chargeRing;

	// Use this for initialization
	void Start () {
        //生成玩家

        GeneratePlayer("KeyPlayer",Color.cyan,"Key",new Vector3(-3.5f,2f,0));
        GeneratePlayer("Joy1Player", Color.magenta, "Joy1", new Vector3(3.5f, -2f, 0));
	}

    void GeneratePlayer(string playerName,Color playerColor,string playerControlTag,Vector3 revivePoint)
    {
        var playerGO = GameObject.Instantiate(playerPrefab);
        playerGO.name = playerName;
        var player = playerGO.GetComponent<PlayerController>();
        player.field = this;
        player.color = playerColor;
        player.controlTag = playerControlTag;
        player.revivePoint = revivePoint;
        player.transform.position = player.revivePoint;
        player.energyRing = GameObject.Instantiate(energyRing).GetComponent<Image>();
        player.energyRing.transform.SetParent(canvas.transform);
        player.chargeRing = GameObject.Instantiate(chargeRing).GetComponent<Image>();
        player.chargeRing.transform.SetParent(canvas.transform);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
