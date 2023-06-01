using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Players Rigidbody and Sprite
    private Rigidbody2D myRB;
    public GameObject playerSprite;

    //Player 1 or 2 Bool
    public bool player2;

    //Player Health and Respawn Variables
    public float hp;
    public float spawnMoveSpeed;
    public Vector2 spawnPos;

    //Move Variables
    public float moveSpeed;
    private Vector2 move;
    public bool canMove;

    //Push Variables
    private bool pushOn;
    public float pushInterval;

    //Brake Variables
    public float brakeSpeed;
    
    void Start()
    {
        //Assigns Rigidbody
        myRB = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        //Inputs
        if (!player2)
        {
            if (Gamepad.current == null)
            {
                move.x = Input.GetAxis("Horizontal");
                if (Input.GetMouseButton(0))
                    myRB.velocity = Vector2.MoveTowards(myRB.velocity, Vector2.zero, brakeSpeed * Time.deltaTime);
            }
            else
                move.x = Gamepad.all[0].leftStick.value.x;
        }
        else
        {
            move.x = Gamepad.all[1].leftStick.value.x;
        }

        //Move Variable
        Vector2 temp = move * moveSpeed;
        if (Mathf.Abs(temp.x) > 0.1f || Mathf.Abs(temp.y) > 0.1f)
            pushOn = true;
        else
            pushOn = false;

        //Move
        if (pushOn && canMove)
        {
            if (pushInterval <= 0)
            {
                myRB.AddForce(playerSprite.transform.TransformDirection(Vector2.right) * temp.x);
                pushInterval = .5f;
            }
            else
                pushInterval -= Time.deltaTime;
        }

        //Rotate
        playerSprite.transform.rotation = Physics2D.Raycast(playerSprite.transform.position + new Vector3(0, -.6f, 0), Vector2.down).transform.rotation;

        //Health Stuff
        if (hp <= 0)
        {
            canMove = false;
            myRB.simulated = false;
            transform.position = Vector2.MoveTowards(transform.position, spawnPos, spawnMoveSpeed * Time.deltaTime);
            if (transform.position == new Vector3(spawnPos.x, spawnPos.y, 0))
            {
                hp = 3;
                canMove = true;
                myRB.simulated = true;
            }
        }
    }
}
