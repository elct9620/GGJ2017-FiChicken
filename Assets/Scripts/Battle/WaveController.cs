using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class WaveController : MonoBehaviour {

    [SerializeField]
    float speed;
    [SerializeField]
    float lifetime = 5;
    FloatReactiveProperty lifeTimer = new FloatReactiveProperty(0);
    
    //移動用的向量
    Vector3 moveVector;
    //波的擴張速度
    float ratio = 1;

    // Use this for initialization
    void Start () {
        //Shoot(45,1);
        
        /*
        Direction = new Vector3(1,1,0);
        projectedDirection.x = -Direction.y;
        projectedDirection.y = Direction.x;
        projectedDirection.z = 0;
        */
    }
	
    public void Shoot(float _directionAngle, float _ratio)
    {
        //計算波的面向角度用的方向
        //Vector3 projectedDirection = new Vector3(-_direction.y, _direction.x,0);
        Quaternion rotation = new Quaternion();
        rotation.eulerAngles = new Vector3(0, 0, _directionAngle);
        transform.rotation = rotation;

        float directionRad = _directionAngle * Mathf.Deg2Rad;
        Vector2 normalizedDirection = new Vector2(Mathf.Cos(directionRad),Mathf.Sin(directionRad));

        moveVector = normalizedDirection * speed;
        transform.localScale = new Vector3(1,_ratio,1);
        ratio = _ratio;
        lifeTimer.Throttle(TimeSpan.FromSeconds(5)).Subscribe(_ => Destroy(gameObject));
    }


    void DoMove()
    {
        transform.position = transform.position + moveVector * Time.deltaTime;
        var newScale = transform.localScale;
        newScale.y = newScale.y + speed * ratio * Time.deltaTime;
        transform.localScale = newScale;
    }

	// Update is called once per frame
	void Update () {
        DoMove();
    }
}
