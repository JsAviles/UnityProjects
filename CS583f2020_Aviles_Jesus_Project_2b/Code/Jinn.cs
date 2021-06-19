using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jinn : Enemy
{
    public float patLeft;
    public float patRight;
    public bool facingLeft = true;
    public float speed;
    public LayerMask Ground;
    public Collider2D jinnColl;
    public Rigidbody2D jinnRB;
    public Animator jinnAnimator;

    // Start is called before the first frame update
    protected override void Start()
    {
   
        jinnColl = GetComponent<Collider2D>();
        jinnRB = GetComponent<Rigidbody2D>();
        speed = 2f;
        jinnAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(jinnAnimator.GetBool("Patrolling"))
        {
            patrol();
        }
        
    }

    protected override void patrol()
    {
        jinnAnimator.SetBool("Patrolling", true);
        if (facingLeft)
        {
            if (transform.position.x > patLeft)
            {
                if (transform.localScale.x != -1)
                {
                    jinnAnimator.SetBool("Patrolling", false);
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                jinnRB.velocity = new Vector2(-speed, transform.position.y);
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < patRight)
            {
                if (transform.localScale.x != 1)
                {
                    jinnAnimator.SetBool("Patrolling", false);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                jinnRB.velocity = new Vector2(speed, transform.position.y);
            }
            else
            {
                facingLeft = true;
            }
        }
    }
  
}