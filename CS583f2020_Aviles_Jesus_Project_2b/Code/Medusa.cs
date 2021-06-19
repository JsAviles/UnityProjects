using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medusa : Enemy
{
    public float patLeft;
    public float patRight;
    public bool facingLeft = true;
    public float speed;
    public LayerMask Ground;
    public Collider2D dusaColl;
    public Rigidbody2D dusaRB;
    public Animator dusaAnimator;

    // Start is called before the first frame update
    protected override void Start()
    {
        dusaColl = GetComponent<Collider2D>();
        dusaRB = GetComponent<Rigidbody2D>();
        dusaAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (dusaAnimator.GetBool("Patrolling"))
        {
            patrol();
        }
    }

    protected override void patrol()
    {
        dusaAnimator.SetBool("Patrolling", true);
        if (facingLeft)
        {
            if (transform.position.x > patLeft)
            {
                if (transform.localScale.x != -1)
                {
                    dusaAnimator.SetBool("Patrolling", false);
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                dusaRB.velocity = new Vector2(-speed, transform.position.y);
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
                    dusaAnimator.SetBool("Patrolling", false);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                dusaRB.velocity = new Vector2(speed, transform.position.y);
            }
            else
            {
                facingLeft = true;
            }
        }
    }
}
