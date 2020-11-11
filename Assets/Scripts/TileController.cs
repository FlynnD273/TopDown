/**************************
 * File: TileController
 * Author: Flynn Duniho
 * Description: Copies the rotation of the parent object, 
 * given a position offset in world coordinate space.
 * Don't add this component to any gameobject in the editor
**************************/
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour
{
    [Tooltip("The GameObject to copy position and rotation from")]
    public GameObject Parent;

    [Tooltip("position offset from parent object")]
    public Vector3 Offset;


    void LateUpdate()
    {
        //Copy transform and rotation
        transform.localScale = Parent.transform.localScale;
        transform.position = Parent.transform.position + (Offset.Mult(Parent.transform.localScale));
        transform.rotation = Parent.transform.rotation;
    }
}
