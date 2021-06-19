using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected virtual void Start()
    {
    }

    protected virtual void patrol()
    {
        //override in specific enemy script
    }
    
    //universal function for all enemies
    public void die()
    {
        Destroy(this.gameObject);
    }
}
