using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.UI;

public class WaveController : MonoBehaviour {

    [SerializeField]
    float speed;
    [SerializeField]
    float lifetime = 5;
    [SerializeField]
    float pushForce = 100;
    //[SerializeField]
    //SpriteRenderer waveSpriteRenderer;
    [SerializeField]
    Image waveImage;
    [SerializeField]
    BoxCollider2D boxCollider;
    [SerializeField]
    RectTransform imageRectTransform;



    float reflectMultiplier = 1;

    FloatReactiveProperty lifeTimer = new FloatReactiveProperty(0);

    PlayerController shooter;

    //移動用的向量
    Vector3 moveVector;
    //波的擴張速度
    float ratio = 1;

    // Use this for initialization
    void Start () {
    }

    List<PlayerController> ignoreList = new List<PlayerController>();


    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("ASDF");
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            if (ignoreList.Contains(player)) { return; }
            player.Push(moveVector, pushForce,ratio,reflectMultiplier,shooter);
            ignoreList.Add(player);
        }
    }

    public void Shoot(float _directionAngle, float _ratio , PlayerController _shooter , float _reflectMultiplier)
    {
        //計算波的面向角度用的方向
        //Vector3 projectedDirection = new Vector3(-_direction.y, _direction.x,0);
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(0, 0, _directionAngle);
        transform.rotation = rotation;

        float directionRad = _directionAngle * Mathf.Deg2Rad;
        Vector2 normalizedDirection = new Vector2(Mathf.Cos(directionRad),Mathf.Sin(directionRad));

        reflectMultiplier = _reflectMultiplier;
        moveVector = normalizedDirection * speed * reflectMultiplier;
        //transform.localScale = new Vector3(1,_ratio,1);
        Vector2 newSizeDelta = new Vector2(64, _ratio * 12);
        imageRectTransform.sizeDelta = newSizeDelta;
        ratio = _ratio;
        lifeTimer.Throttle(TimeSpan.FromSeconds(5)).Subscribe(_ => Destroy(gameObject));
        ignoreList = new List<PlayerController> { _shooter};
        //根據玩家顏色變色
        waveImage.color = _shooter.color;

        shooter = _shooter;
    }

    void UpdateColliderSize()
    {
        float ySize = imageRectTransform.sizeDelta.y;
        float colliderSize = ySize >= 132 ? ySize - 40 : ySize * 0.6969697f;
        var newSize = boxCollider.size;
        newSize.y = ySize;
        boxCollider.size = newSize;
    }


    void DoMove()
    {
        transform.position = transform.position + moveVector * Time.deltaTime;
        Vector2 newSizeDelta = imageRectTransform.sizeDelta;
        float newSizeY = newSizeDelta.y + speed * ratio * reflectMultiplier * Time.deltaTime * 16;
        newSizeDelta.y = newSizeY;
        imageRectTransform.sizeDelta = newSizeDelta;
        //var newScale = transform.localScale;
        //newScale.y = newScale.y + speed * ratio * reflectMultiplier * Time.deltaTime;
        //transform.localScale = newScale;
        
        UpdateColliderSize();
    }

	// Update is called once per frame
	void Update () {
        DoMove();
    }
}
