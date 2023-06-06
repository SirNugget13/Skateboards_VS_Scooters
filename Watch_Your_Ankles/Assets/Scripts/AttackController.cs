using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    //Ribigbody
    private Rigidbody2D myRB;

    //Player 1 or 2
    public bool player2;

    //Attack Cooldown
    public float atkCool = .5f;

    //colliders, inputs, and controller
    private PlayerController Contr;
    public ContactFilter2D Hitboxes;
    public Collider2D[] attackHitboxes;
    public Collider2D[] hitEnemies;
    public InputAction Hit1;
    public InputAction Hit2;

    private void OnEnable()
    {
        Hit1.Enable();
        Hit2.Enable();
    }

    void Start()
    {
        //Assigns Rigidbody/Controller
        myRB = GetComponent<Rigidbody2D>();
        Contr = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if(atkCool > 0)
        atkCool -= Time.deltaTime;

        //Attack Keys
        if (Hit1.WasPressedThisFrame() && atkCool <= 0)
        {
            LaunchAttack(attackHitboxes[0]);
            atkCool = .5f;
        }
        if (Hit2.WasPressedThisFrame() && atkCool <= 0)
        {
            LaunchAttack(attackHitboxes[1]);
            atkCool = .5f;
        }

    }

    private void LaunchAttack(Collider2D other)
    {
        Physics2D.OverlapCollider(other, Hitboxes, hitEnemies);
        foreach(Collider2D c in hitEnemies)
        {
            if (c.transform.parent == transform)
                continue;
           
            Debug.Log(Contr.momentum);
            
        }
    }
}
