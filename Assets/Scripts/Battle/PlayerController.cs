using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //[SerializeField]
    Rigidbody2D playerRigidBody;
    [SerializeField]
    Transform playerSpriteTransform;
    [SerializeField]
    GameObject waveObject;
    [SerializeField]
    Image chargeRing;
    [SerializeField]
    Image energyRing;


    [SerializeField]
    float maxSpeed;
    [SerializeField]
    float acceleration;
    [SerializeField]
    float jumpPower;
    [SerializeField]
    float gravity;
    [SerializeField]
    float rotateSpeed = 360;
    [SerializeField]
    float chargeInitAmount = 1;
    [SerializeField]
    float chargeSpeed = 3;
    [SerializeField]
    float energyRecoverSpeed = 0.4f;
    [SerializeField]
    float energyMaxAmount = 10;


    public string controlTag = "";

    float jumpHeight;
    float jumpVelocity;

    float chargeAmount = 0;
    float energy = 0;


    float facingDirectionAngle = 180;


    // Use this for initialization
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveControl();
        JumpControl();
        //能量控制

        energy += energyRecoverSpeed * Time.deltaTime;


        bool charging = Input.GetButton(controlTag + "_Fire");
        if (charging)
        {
            if (chargeAmount == 0)
            {
                chargeAmount = chargeInitAmount;
            }
            else {
                chargeAmount += chargeSpeed * Time.deltaTime;
            }

            chargeAmount = Mathf.Clamp(chargeAmount,0, energy);
        }
        else
        {
            if (chargeAmount != 0)
            {
                GameObject shootedWave = GameObject.Instantiate(waveObject);
                shootedWave.transform.position = transform.position;
                shootedWave.GetComponent<WaveController>().Shoot(facingDirectionAngle,chargeAmount);
                energy -= chargeAmount * 0.25f;
            }
            chargeAmount = 0;
        }

        UIControl();

        //jumpVelocity



        //Input.GetKey(KeyCode.LeftArrow)
    }

    private void MoveControl()
    {
        float XAxis = Input.GetAxis(controlTag + "_Horizontal") * acceleration;
        float YAxis = Input.GetAxis(controlTag + "_Vertical") * acceleration;
        playerRigidBody.AddForce(new Vector2(XAxis, YAxis));
        playerRigidBody.velocity = Vector2.ClampMagnitude(playerRigidBody.velocity, maxSpeed);
        if (XAxis != 0 || YAxis != 0)
        {
            facingDirectionAngle = Mathf.MoveTowardsAngle(facingDirectionAngle, Mathf.Atan2(YAxis, XAxis) * Mathf.Rad2Deg, rotateSpeed * Time.deltaTime); //;
        }
    }

    private void JumpControl()
    {
        bool jumped = Input.GetButtonDown(controlTag + "_Jump");

        if (jumped)
        {
            if (jumpHeight == 0)
            {
                jumpVelocity = jumpPower;
            }
        }
        //計算重力與跳躍
        jumpHeight += jumpVelocity * Time.deltaTime;
        jumpVelocity -= gravity * Time.deltaTime;
        if (jumpHeight < 0) { jumpHeight = 0; }
        //更新圖片位置
        var newPosition = playerSpriteTransform.localPosition;
        newPosition.y = jumpHeight;
        playerSpriteTransform.localPosition = newPosition;
    }


    private void UIControl()
    {
        //控制能量條
        energyRing.fillAmount = energy / energyMaxAmount;
        energyRing.transform.position = transform.position;
        Quaternion energyRingQuaternion = chargeRing.transform.localRotation;
        Vector3 energyRingAngle = energyRingQuaternion.eulerAngles;
        energyRingAngle.z = facingDirectionAngle - energyRing.fillAmount * 180;// - 0.25f * 180;
        energyRingQuaternion.eulerAngles = energyRingAngle;
        energyRing.transform.localRotation = energyRingQuaternion;


        //控制集氣條
        chargeRing.fillAmount = chargeAmount / energyMaxAmount;
        chargeRing.transform.position = transform.position;
        Quaternion chargeRingQuaternion = chargeRing.transform.localRotation;
        Vector3 chargeRingAngle = chargeRingQuaternion.eulerAngles;
        chargeRingAngle.z = facingDirectionAngle - chargeRing.fillAmount * 180;// - 0.25f * 180;
        chargeRingQuaternion.eulerAngles = chargeRingAngle;
        chargeRing.transform.localRotation = chargeRingQuaternion;
        

        //float newZRotation = Mathf.Atan2()
    }
}
