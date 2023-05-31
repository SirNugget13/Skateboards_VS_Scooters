using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D myRB;

    public bool player2;

    public float speed;
    private Vector2 move;

    // Start is called before the first frame update
    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!player2)
        {
            if (Gamepad.current == null)
                move.x = Input.GetAxis("Horizontal");
            else
                move.x = Gamepad.all[0].leftStick.value.x;
        }
        else
        {
            move.x = Gamepad.all[1].leftStick.value.x;
        }
        Vector2 temp = move * speed;
        if (Input.GetMouseButtonDown(0))
            myRB.AddForce(new Vector2(temp.x, 0));
    }
}
