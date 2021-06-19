using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lizard : Enemy
{
    public float patLeft;
    public float patRight;
    public bool facingLeft = true;
    public float speed;
    public LayerMask Ground;
    public Collider2D lizColl;
    public Rigidbody2D lizRB;
    public Animator lizAnimator;

    // Start is called before the first frame update
    protected override void Start()
    {
        lizColl = GetComponent<Collider2D>();
        lizRB = GetComponent<Rigidbody2D>();
        lizAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (lizAnimator.GetBool("Patrolling"))
        {
            patrol();
        }
    }

    protected override void patrol()
    {
        lizAnimator.SetBool("Patrolling", true);
        if (facingLeft)
        {
            if (transform.position.x > patLeft)
            {
                if (transform.localScale.x != -1)
                {
                    lizAnimator.SetBool("Patrolling", false);
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                lizRB.velocity = new Vector2(-speed, transform.position.y);
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
                    lizAnimator.SetBool("Patrolling", false);
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                lizRB.velocity = new Vector2(speed, transform.position.y);
            }
            else
            {
                facingLeft = true;
            }
        }
    }
}
