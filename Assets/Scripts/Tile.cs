/**************************
 * File: Tile
 * Author: Flynn Duniho
 * Description: Create 8 copies of the gameobject, around the parent gameobject
**************************/
using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [Tooltip("The bounds of the field")]
    public BoxCollider2D Bounds;

    [Tooltip("Prefab to base new objects off of. Can be null")]
    public GameObject Prefab;

    [Tooltip("Array of all tiled GameObjects")]
    public GameObject[] Children;


    void Start()
    {
        if (Prefab == null)
        {
            Children = gameObject.MakeGrid(Bounds);
        }
        else
        {
            Children = Prefab.MakePrefabGrid(Bounds);
        }
    }

    /// <summary>
    /// Remove all instances of the tiled object, ad the object itself
    /// </summary>
    public void Remove()
    {
        foreach (GameObject o in Children)
        {
            Destroy(o);
        }

        Destroy(gameObject);
    }
}
