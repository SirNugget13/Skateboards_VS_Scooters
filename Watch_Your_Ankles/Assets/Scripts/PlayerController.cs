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

    //Move Variables
    public float moveSpeed;
    private Vector2 move;

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
        if (pushOn)
        {
            if (pushInterval <= 0)
            {
                myRB.AddForce(new Vector2(temp.x, 0));
                pushInterval = .5f;
            }
            else
                pushInterval -= Time.deltaTime;
        }
        
        //Rotate
        Vector3 targ = new Vector2(0, 4);
        targ.z = 0f;

        Vector3 objectPos = playerSprite.transform.position;
        targ.x = targ.x - objectPos.x;
        targ.y = targ.y - objectPos.y;
        playerSprite.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg));
    }
}
