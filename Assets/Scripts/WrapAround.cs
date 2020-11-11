/**************************
 * File: WrapAround
 * Author: Flynn Duniho
 * Description: Given a BoxCollider, if out of bounds, 
 * wrap around to the other side of the box
**************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrapAround : MonoBehaviour
{
    [Tooltip("The bounds of the field")]
    public BoxCollider2D Bounds;


    void Start()
    {
        if (Bounds == null)
        {
            Tile t = GetComponent<Tile>();
            if (t != null)
            {
                Bounds = t.Bounds;
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 pos;
        //Wrap, keeping distance away from edge the same
        if (transform.position.x > Bounds.bounds.max.x)
        {
            pos = new Vector2(Bounds.bounds.min.x + (transform.position.x - Bounds.bounds.max.x), 0);
        }
        else if (transform.position.x < Bounds.bounds.min.x)
        {
            pos = new Vector2(Bounds.bounds.max.x - (Bounds.bounds.min.x - transform.position.x), 0);
        }
        else
        {
            pos = new Vector2(transform.position.x, 0);
        }

        if (transform.position.y > Bounds.bounds.max.y)
        {
            pos = new Vector2(pos.x, Bounds.bounds.min.y + (transform.position.y - Bounds.bounds.max.y));
        }
        else if (transform.position.y < Bounds.bounds.min.y)
        {
            pos = new Vector3(pos.x, Bounds.bounds.max.y - (Bounds.bounds.min.y - transform.position.y));
        }
        else
        {
            pos = new Vector2(pos.x, transform.position.y);
        }

        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }
}
