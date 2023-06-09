using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    //Players Rigidbody, Sprite, and Raycast
    private Rigidbody2D myRB;
    public GameObject playerSprite;
    private RaycastHit2D below;

    //Player 1 or 2 Bool
    public bool player2;

    //Player Health and Respawn Variables
    public int lives;
    public float hp, hpMax;
    public float spawnMoveSpeed;
    public Vector2 spawnPos;
    public GameObject spawnBox;
    public Image hpBar;

    //Move Variables
    private bool jumpInput;
    public float jumpForce;
    public float moveSpeed;
    private Vector2 move;
    public bool canMove;
    public float momentum;
    public float momentumMultiplier;
    public GameObject ramp;

    //Push Variables
    private bool pushOn;
    public float pushInterval;

    //Brake Variables
    public float brakeSpeed;
    
    void Start()
    {
        //Assigns Rigidbody
        myRB = GetComponent<Rigidbody2D>();

        //Assigns Ramp
        ramp = GameObject.FindGameObjectWithTag("Ramp");
    }
    
    void Update()
    {
        //Inputs
        if (!player2)
            Inputs(0, "", Input.GetKeyDown(KeyCode.Space), Input.GetMouseButton(0));
        else
            Inputs(1, "Second ", Input.GetKeyDown(KeyCode.UpArrow), Input.GetKey(KeyCode.DownArrow));

        //Move Variable
        Vector2 temp = move * moveSpeed;
        momentum = (Mathf.Abs(myRB.velocity.x) + Mathf.Abs(myRB.velocity.y)) * momentumMultiplier;
        if (Mathf.Abs(temp.x) > 0.1f || Mathf.Abs(temp.y) > 0.1f)
            pushOn = true;
        else
            pushOn = false;

        //Below Raycast
        below = Physics2D.Raycast(playerSprite.transform.position + playerSprite.transform.TransformDirection(Vector2.down * .7f), playerSprite.transform.TransformDirection(Vector2.down), .15f);

        //Move
        if (canMove)
        {
            if (pushOn)
            {
                if (pushInterval <= 0)
                {
                    myRB.AddForce(playerSprite.transform.TransformDirection(Vector2.right) * temp.x);
                    pushInterval = .5f;
                }
                else
                    pushInterval -= Time.deltaTime;
            }
            if (jumpInput && below)
                myRB.AddForce(playerSprite.transform.TransformDirection(Vector2.up) * jumpForce);
            if (below)
            {
                if (myRB.velocity.x > .2f)
                    GetComponent<SpriteRenderer>().flipX = false;
                if (myRB.velocity.x < -.2f)
                    GetComponent<SpriteRenderer>().flipX = true;
            }
            if (move.y > .4f)
                Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), ramp.GetComponent<PolygonCollider2D>(), false);
            else
                Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), ramp.GetComponent<PolygonCollider2D>(), true);
        }

        //Rotate
        RaycastHit2D staticBelow = Physics2D.Raycast(playerSprite.transform.position + new Vector3(0, -.7f, 0), Vector2.down, .35f);
        if (below)
            playerSprite.transform.rotation = below.transform.rotation;
        else if (!below && staticBelow)
            playerSprite.transform.rotation = staticBelow.transform.rotation;
        
        //Health Stuff
        if (hp <= 0)
            Respawn();
    }

    private void Respawn()
    {
        canMove = false;
        myRB.simulated = false;
        transform.position = Vector2.MoveTowards(transform.position, spawnPos, spawnMoveSpeed * Time.deltaTime);
        Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), spawnBox.GetComponent<BoxCollider2D>(), true);
        if (transform.position == new Vector3(spawnPos.x, spawnPos.y, 0) && lives > 0)
        {
            lives--;
            hp = hpMax;
            canMove = true;
            myRB.simulated = true;
        }
    }

    private void Inputs(int controlIndex, string inputDirection, bool jump, bool brake)
    {
        if (Gamepad.all.Count >= controlIndex + 1)
        {
            move.x = Gamepad.all[controlIndex].leftStick.value.x + Input.GetAxis(inputDirection + "Horizontal");
            move.y = Gamepad.all[controlIndex].leftStick.value.y + Input.GetAxis(inputDirection + "Vertical");
            if (Gamepad.all[controlIndex].buttonSouth.wasPressedThisFrame || jump)
                jumpInput = true;
            else
                jumpInput = false;
            if (Gamepad.all[controlIndex].leftTrigger.isPressed || brake)
                myRB.velocity = Vector2.MoveTowards(myRB.velocity, Vector2.zero, brakeSpeed * Time.deltaTime);
        }
        else
        {
            move.x = Input.GetAxis(inputDirection + "Horizontal");
            move.y = Input.GetAxis(inputDirection + "Vertical");
            jumpInput = jump;
            if (brake)
                myRB.velocity = Vector2.MoveTowards(myRB.velocity, Vector2.zero, brakeSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Spawn")
            Physics2D.IgnoreCollision(GetComponent<CircleCollider2D>(), spawnBox.GetComponent<BoxCollider2D>(), false);
    }
}
