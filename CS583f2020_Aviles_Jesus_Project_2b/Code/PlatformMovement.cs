using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public float leftPath;
    public float rightPath;
    public Rigidbody2D platformRB;
    public bool facingLeft = false;
    public float platformSpeed;
    // Start is called before the first frame update
    public void Start()
    {
        platformRB = GetComponent<Rigidbody2D>();
        platformSpeed = 2f;
    }

    // Update is called once per frame
    public void Update()
    {

        patrol();

    }
    public void patrol()
    {
        if (facingLeft)
        {
            if (transform.position.x > leftPath)
            {
                platformRB.velocity = new Vector2(-platformSpeed, transform.position.y);
            }
            else
            {
                facingLeft = false;
            }
        }
        else
        {
            if (transform.position.x < rightPath)
            {
                platformRB.velocity = new Vector2(platformSpeed, transform.position.y);
            }
            else
            {
                facingLeft = true;
            }
        }
    }
}
