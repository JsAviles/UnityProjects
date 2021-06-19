using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStone : MonoBehaviour
{
    public float highPoint;
    public float lowPoint;
    public Rigidbody2D stoneRB;
    public bool Down;
    public float riseSpeed;
    public float dropSpeed;
    // Start is called before the first frame update
    public void Start()
    {
        stoneRB = GetComponent<Rigidbody2D>();
        Down = false;
    }

    // Update is called once per frame
    public void Update()
    {

        patrol();

    }
    public void patrol()
    {
        if (Down)
        {
            if (transform.position.y > lowPoint)
            {
                stoneRB.velocity = new Vector2(transform.position.x, -dropSpeed);
            }
            else
            {

                Down = false;
            }
        }
        else
        {
            if (transform.position.y < highPoint)
            {
                stoneRB.velocity = new Vector2(transform.position.x, riseSpeed);
            }
            else
            {
                Down = true;
            }
        }
    }
}