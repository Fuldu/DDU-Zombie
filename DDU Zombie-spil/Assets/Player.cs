using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerStates { Idle, Walking, Running, Crouching }

public class Player : MonoBehaviour
{

    [Header("Status")]
    public PlayerStates ActiveState;

    public int Health;
    
    public float Stamina;
    public float StartStaminaRecoveryTimer;

    public bool Hitstun;



    [Header("Shooting")]
    public float GunNoiseRadius;
    public GameObject ProjectileObj;
    public GameObject ShootPos;
    public float defProjectileSpeed;

    public float GunCooldownTimer;



    [Header("Config")]
    public int MaxHealth;
    public float MaxStamina;
    public float StaminaRecoveryRate;
    public float StartStaminaRecoveryTimerMax;

    public float CrouchNoiseRadius;
    public float WalkNoiseRadius;
    public float RunNoiseRadius;

    public bool Invincible;
    public float IFrameDuration;

    public float CrouchSpeed;
    public float WalkSpeed;
    public float RunSpeed;

    public float HitstunTime;


    [Header("Misc")]
    public CircleCollider2D PlayerNoiseCollider;

    [Header("UI")]
    public Slider HealthBar;
    public Slider StaminaBar;


    private float Speed;

    private Vector2 mouseDir;

    Rigidbody2D rb;
    NoiseMaker nm;
    SpriteRenderer sr;
    GameController gc;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        nm = GetComponent<NoiseMaker>();
        sr = GetComponent<SpriteRenderer>();
        gc = FindObjectOfType<GameController>();

    }



    // Start is called before the first frame update
    void Start()
    {




        Speed = WalkSpeed;

        UpdateMaxSliderValues();
    }




    private void Shoot()
    {


        GameObject newProj = Instantiate(ProjectileObj, ShootPos.transform.position, Quaternion.identity);


        newProj.transform.up = mouseDir;

        newProj.GetComponent<Rigidbody2D>().velocity = newProj.transform.up * defProjectileSpeed;



        nm.MakeNoise(GunNoiseRadius);


    }






    // Update is called once per frame
    void Update()
    {

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseDir = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;

        //mouseDir.Normalize();

        transform.up = mouseDir;




        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }




        // Player Movement
        if (!Hitstun)
        {
            Movement();
        }




        if (ActiveState == PlayerStates.Crouching)
        {
            PlayerNoiseCollider.radius = CrouchNoiseRadius;
        }
        else if (ActiveState == PlayerStates.Walking)
        {
            PlayerNoiseCollider.radius = WalkNoiseRadius;
        }
        else if (ActiveState == PlayerStates.Running)
        {
            PlayerNoiseCollider.radius = RunNoiseRadius;
        }
        else if (ActiveState == PlayerStates.Idle)
        {
            PlayerNoiseCollider.radius = 0;
        }
        {

        }









        // Update UI


        HealthBar.value = Health;
        StaminaBar.value = Stamina;


    }



    private void Movement()
    {

        Vector2 movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movementVector.Normalize();



        if (Input.GetKey(KeyCode.LeftControl))
        {
            ActiveState = PlayerStates.Crouching;
        }
        else
        {
            if (movementVector != Vector2.zero)
            {
                if (Input.GetKey(KeyCode.LeftShift) && Stamina > 0)
                {
                    ActiveState = PlayerStates.Running;
                }
                else // Moving but not running
                {
                    ActiveState = PlayerStates.Walking;
                }

            }
            else // Not crouching and not moving
            {
                ActiveState = PlayerStates.Idle;
            }
        }




        if (ActiveState == PlayerStates.Running) { Speed = RunSpeed; }
        else if (ActiveState == PlayerStates.Crouching) { Speed = CrouchSpeed; }
        else { Speed = WalkSpeed; }


        rb.velocity = movementVector * Speed;

    }




    private void UpdateMaxSliderValues()
    {
        HealthBar.maxValue = MaxHealth;
        StaminaBar.maxValue = MaxStamina;
    }



    private void FixedUpdate()
    {


        // Using Stamina for running
        if (ActiveState == PlayerStates.Running)
        {
            Stamina--;
            StartStaminaRecoveryTimer = StartStaminaRecoveryTimerMax;
            if (Stamina <= 0)
            {
                ActiveState = PlayerStates.Walking;
            }
        }
        // Recovering stamina
        else if (StartStaminaRecoveryTimer <= 0)
        {
            // If the player can gain stamina without gaining more than MaxStamina
            if (Stamina + StaminaRecoveryRate <= MaxStamina)
            {
                Stamina += StaminaRecoveryRate;
            }
            // The player's staminabar has room for more stamina, but less than StaminaRecoveryRate
            else if (Stamina < MaxStamina)
            {
                Stamina = MaxStamina;
            }
        }
        // Timer to start recovering stamina
        else
        {
            StartStaminaRecoveryTimer--;
        }




    }



    public void GainHealth(int health)
    {
        if (Health + health >= MaxHealth)
        {
            Health = MaxHealth;
        }
        else
        {
            Health += health;
        }
    }




    private void Die()
    {
        Invincible = true;

        gc.GameOver();
        gameObject.SetActive(false);
    }


    public void TakeDamage(int dmg)
    {
        if (Invincible) { return; }

        if (Health - dmg <= 0)
        {
            Health = 0;
            Die();
        }
        else
        {
            Health -= dmg;

            StartCoroutine(IFrames());
        }
    }


    public void TakeKnockback(Vector2 sourcePos, float mag)
    {

        if (Invincible) { return; }

        Vector2 dirVector = new Vector2(transform.position.x - sourcePos.x, transform.position.y - sourcePos.y).normalized;

        rb.velocity = dirVector * mag;

        StartCoroutine(StartHitstun());

    }



    public IEnumerator StartHitstun()
    {
        if (!Hitstun)
        {
            Hitstun = true;
            yield return new WaitForSeconds(HitstunTime);
            Hitstun = false;
        }
    }


    private IEnumerator IFramesAnimation()
    {
        while (Invincible)
        {
            yield return new WaitForSeconds(0.1f);
            sr.enabled = false;
            yield return new WaitForSeconds(0.1f);
            sr.enabled = true;
        }
    }

    private IEnumerator IFrames()
    {
        Invincible = true;

        StartCoroutine(IFramesAnimation());

        yield return new WaitForSeconds(IFrameDuration);
        Invincible = false;
    }



}
