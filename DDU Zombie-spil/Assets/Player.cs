using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerStates { Idle, Walking, Running, Crouching }

public class Player : MonoBehaviour
{

    [Header("Status")]
    public PlayerStates ActiveState;
    public float Stamina;



    [Header("Config")]
    public float MaxStamina;
    public float StaminaRecoveryRate;
    public float SecondsBeforeStaminaRecovery;

    public float CrouchSpeed;
    public float WalkSpeed;
    public float RunSpeed;



    private float Speed;


    Rigidbody2D rb;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }



    // Start is called before the first frame update
    void Start()
    {
        Speed = WalkSpeed;
    }

    // Update is called once per frame
    void Update()
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


    private void FixedUpdate()
    {
        
        if (ActiveState == PlayerStates.Running)
        {
            Stamina--;
            if (Stamina <= 0)
            {
                ActiveState = PlayerStates.Walking;
            }
        }
        // If the player can gain stamina without gaining more than MaxStamina
        else if (Stamina + StaminaRecoveryRate <= MaxStamina)
        {
            Stamina += StaminaRecoveryRate;
        }
        // The player's staminabar has room for more stamina, but less than StaminaRecoveryRate
        else if (Stamina < MaxStamina)
        {
            Stamina = MaxStamina;
        }




    }


}
