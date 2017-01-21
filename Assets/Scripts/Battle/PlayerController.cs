using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public enum Directions {
        Up, Right, Down, Left
    };

    //[SerializeField]
    Rigidbody2D playerRigidBody;
    Collider2D playerCollider;
    [SerializeField]
    Transform playerSpriteTransform;
    SpriteRenderer playerSpriteRenderer;
    Animator playerAnimator;
    [SerializeField]
    SpriteRenderer shieldSprite;
    [SerializeField]
    GameObject waveObject;
    [SerializeField]
    ParticleSystem dieParticle;


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
    [SerializeField]
    float invincibleTime = 1;
    [SerializeField]
    float evadeHeight = 0.1f;
    [SerializeField]
    float shieldTime = 0.1f;
    [SerializeField]
    float shieldCostEnergy = 1;

    public Color color = Color.white;

    public Vector3 revivePoint = new Vector3();
    public AnimatorOverrideController Avatar;

    public Image chargeRing;
    public Image energyRing;
    public FieldManager field;

    bool alive = false;
    bool isInvincible = false;
    bool shielded = false;

    public string controlTag = "";

    bool onGround = false;

    float playerHeight;
    float jumpVelocity;

    float chargeAmount = 0;
    float energy = 0;

    float facingDirectionAngle = 180;
    Directions Direction = Directions.Right;

    List<MapBlock> touchedMapblocks = new List<MapBlock>();

    bool triggerStayExcuted = false;

    void FixedUpdate()
    {
        onGround = OnGround();
        triggerStayExcuted = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        var mapBlock = other.GetComponent<MapBlock>();
        if (mapBlock != null)
        {
            touchedMapblocks.Add(mapBlock);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        var mapBlock = other.GetComponent<MapBlock>();
        if (mapBlock != null)
        {
            touchedMapblocks.Remove(mapBlock);
        }
    }


    // Use this for initialization
    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerSpriteRenderer = playerSpriteTransform.GetComponent<SpriteRenderer>();
        playerSpriteRenderer.color = color;
        playerAnimator = playerSpriteTransform.GetComponent<Animator>();
        if(Avatar) {
            playerAnimator.runtimeAnimatorController = Avatar;
        }
        shieldSprite.enabled = false;
        alive = true;
        shielded = false;
    }

    // Update is called once per frame
    void Update()
    {



        if (isInvincible)
        {
            playerSpriteRenderer.enabled = !playerSpriteRenderer.enabled;
        }

        if (alive)
        {
            MoveControl();
            JumpControl();
            EnergyControl();
            ShieldControl();
        }
        UIControl();

        //jumpVelocity



        //Input.GetKey(KeyCode.LeftArrow)
    }


    bool OnGround()
    {
        /*
        bool ret = true;
        if (!field.fieldCollider.IsTouching(playerCollider))
        {
            return false;
        }
        */

        if (touchedMapblocks.Count == 0) { return false; }
        foreach (var block in touchedMapblocks)
        {
            if (block.Type == MapBlock.BlockType.Normal)
            {
                return true;
            }
        }
        return false;
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
            UpdateDirection(XAxis, YAxis);
        }
    }

    private void UpdateDirection(float XAxis, float YAxis) {
        float angleFix =  (Mathf.RoundToInt(Mathf.Atan2(XAxis, YAxis) * Mathf.Rad2Deg) + 360) % 360;
        Debug.Log(angleFix);
        if(angleFix >= 0 && angleFix < 90) {
            Direction = Directions.Up;
        }

        if(angleFix >= 90 && angleFix < 180) {
            Direction = Directions.Right;
        }

        if(angleFix >= 180 && angleFix < 270) {
            Direction = Directions.Down;
        }

        if(angleFix >= 270 && angleFix < 360) {
            Direction = Directions.Left;
        }
        playerAnimator.SetInteger("Direction", (int)Direction);
    }

    private void JumpControl()
    {
        bool jumped = Input.GetButtonDown(controlTag + "_Jump");

        if (jumped)
        {
            if (playerHeight == 0)
            {
                jumpVelocity = jumpPower;
            }
        }
        //計算重力與跳躍
        playerHeight += jumpVelocity * Time.deltaTime;
        jumpVelocity -= gravity * Time.deltaTime;
        if (playerHeight < 0)
        {
            if (onGround || isInvincible)
            {
                playerHeight = 0;
            }
            else
            {
                Die();
                return;
            }

        }
        //更新圖片位置
        var newPosition = playerSpriteTransform.localPosition;
        newPosition.y = playerHeight;
        playerSpriteTransform.localPosition = newPosition;
    }

    void ShieldControl()
    {
        if (Input.GetButtonDown(controlTag + "_Shield"))
        {
            if (energy > shieldCostEnergy)
            {
                EnableShield();
            }
        }
    }
    void EnableShield()
    {
        if (shielded) return;
        energy -= shieldCostEnergy;
        StartCoroutine(Shield());
    }

    IEnumerator Shield()
    {
        playerAnimator.SetBool("Sit", true);
        shieldSprite.enabled = true;
        shielded = true;
        yield return new WaitForSeconds(shieldTime);
        playerAnimator.SetBool("Sit", false);
        shieldSprite.enabled = false;
        shielded = false;
    }

    void Die()
    {
        alive = false;
        playerSpriteTransform.localPosition = Vector3.zero;
        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine()
    {
        for (float timer = 1; timer > 0; timer -= Time.deltaTime)
        {
            yield return null;
            playerSpriteTransform.localScale = new Vector3(timer, timer, 1);
        }
        yield return null;
        playerSpriteTransform.localScale = Vector3.zero;
        dieParticle.Emit(15);
        yield return new WaitForSeconds(2);
        Revive();
    }

    void Revive()
    {
        transform.position = revivePoint;
        playerSpriteTransform.localScale = Vector3.one;
        playerRigidBody.velocity = Vector2.zero;
        alive = true;
        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        playerSpriteRenderer.enabled = true;
        isInvincible = false;
    }

    public void RecoverEnergy(float amount)
    {
        energy = Mathf.Clamp(energy + amount, 0, energyMaxAmount);

    }


    private void EnergyControl()
    {
        //能量控制

        energy += energyRecoverSpeed * Time.deltaTime;
        energy = Mathf.Clamp(energy, 0, energyMaxAmount);


        bool charging = Input.GetButton(controlTag + "_Fire");
        if (charging)
        {
            if (chargeAmount == 0)
            {
                chargeAmount = chargeInitAmount;
            }
            else
            {
                chargeAmount += chargeSpeed * Time.deltaTime;
            }

            chargeAmount = Mathf.Clamp(chargeAmount, 0, energy);
        }
        else
        {
            if (chargeAmount != 0)
            {
                GameObject shootedWave = GameObject.Instantiate(waveObject);
                shootedWave.transform.position = transform.position;
                shootedWave.GetComponent<WaveController>().Shoot(facingDirectionAngle, chargeAmount, this, 1);
                energy -= chargeAmount * 0.25f;
            }
            chargeAmount = 0;
        }
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


    public void Push(Vector2 direction, float force, float ratio, float reflectMultiplier)
    {

        if (isInvincible) { return; }
        if (shielded)
        {
            GameObject shootedWave = GameObject.Instantiate(waveObject);
            shootedWave.transform.position = transform.position;
            float reflectAngle = Mathf.Atan2(direction.y, direction.x) + 180;
            shootedWave.GetComponent<WaveController>().Shoot(facingDirectionAngle, ratio, this, reflectMultiplier + 0.2f);
            RecoverEnergy(shieldCostEnergy);
            shielded = false;
            return;
        }
        if (!alive) { return; }
        if (playerHeight > evadeHeight) { return; }
        playerRigidBody.AddForce(direction.normalized * force * ratio * reflectMultiplier);

    }
}
