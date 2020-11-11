/**************************
 * File: StarController
 * Author: Flynn Duniho
 * Description: Add this to the stars for a prallax effect
**************************/
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : MonoBehaviour
{
    [Tooltip("The bounds of the field")]
    public BoxCollider2D Bounds;

    [Tooltip("The amount the stars follow the camera. 0 for still, 1 for stays with camera")]
    public float Speed = 0;



    private Transform cam;


    void Start()
    {
        //Scale for seamless tiling
        transform.localScale = new Vector3(1-Speed, 1-Speed, 0);
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        //Move with camera
        Vector2 c = cam.position;
        transform.position = new Vector3(c.x * Speed, c.y * Speed, 0);
    }
}
