/**************************
 * File: Tools
 * Author: Flynn Duniho
 * Description: General useful methods, just a place to put them all
**************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Object;

namespace Assets.Scripts
{
    static class Tools
    {

        /// <summary>
        /// Copy all values from one component to another
        /// </summary>
        /// <typeparam name="T">Type of component</typeparam>
        /// <param name="comp">Component to copy to</param>
        /// <param name="other">Component to copy from</param>
        /// <returns>New Component</returns>
        public static T GetCopyOf<T>(this Component comp, T other) where T : Component
        {
            Type type = comp.GetType();
            if (type != other.GetType()) return null; // type mis-match
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = type.GetProperties(flags);
            //Copy all values
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                    }
                    catch { } // In case of NotImplementedException being thrown.
                }
            }

            FieldInfo[] finfos = type.GetFields(flags);
            //Copy all values
            foreach (var finfo in finfos)
            {
                finfo.SetValue(comp, finfo.GetValue(other));
            }
            return comp as T;
        }

        /// <summary>
        /// Add a copy of a component from one GameObject to another
        /// </summary>
        /// <typeparam name="T">Type of component</typeparam>
        /// <param name="go">GameObject to add the new component to</param>
        /// <param name="toAdd">Component to copy from</param>
        /// <returns>New component</returns>
        public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
        {
            return go.AddComponent<T>().GetCopyOf(toAdd);
        }//Example usage  Health myHealth = gameObject.AddComponent<Health>(enemy.health);

        /// <summary>
        /// Make a copy of the gameobject at offsets
        /// </summary>
        /// <param name="go">GameObject to copy</param>
        /// <param name="b">The bounds of the field</param>
        /// <returns>All new objects</returns>
        public static GameObject[] MakeGrid(this GameObject go, BoxCollider2D b)
        {
            List<GameObject> obj = new List<GameObject>();
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    if (y != 0 || x != 0)
                    {
                        //Add new GameObject
                        GameObject h = new GameObject($"{go.name}_Tile({x}, {y})");
                        //Add a sprite renderer and a tile controller
                        h.AddComponent(go.GetComponent<SpriteRenderer>());
                        TileController tc = h.AddComponent<TileController>();
                        tc.Parent = go;
                        tc.Offset = new Vector3(x * b.bounds.size.x, y * b.bounds.size.y, 0);
                        obj.Add(h);
                    }
            //Return all new objects
            return obj.ToArray();
        }

        /// <summary>
        /// Make a grid of prefabs
        /// </summary>
        /// <param name="go">Prefab to Instantiate</param>
        /// <param name="b">The bounds fot he field</param>
        /// <returns>All new objects</returns>
        public static GameObject[] MakePrefabGrid(this GameObject go, BoxCollider2D b)
        {
            List<GameObject> obj = new List<GameObject>();
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    if (y != 0 || x != 0)
                    {
                        //Add prefab
                        GameObject h = Instantiate(go);
                        h.AddComponent(go.GetComponent<SpriteRenderer>());

                        //Add a tile controller if needed
                        TileController tc = h.GetComponent<TileController>();
                        if (tc == null)
                        {
                            tc = h.AddComponent<TileController>();
                        }

                        tc.Parent = go;
                        tc.Offset = new Vector3(x * b.bounds.size.x, y * b.bounds.size.y, 0);
                        obj.Add(h);
                    }
            //return all new objects
            return obj.ToArray();
        }

        /// <summary>
        /// Multiplies each element together
        /// </summary>
        /// <param name="a">First Vector</param>
        /// <param name="b">Second Vector</param>
        /// <returns>Multiplied Vector</returns>
        public static Vector3 Mult(this Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        /// <summary>
        /// Play a sound effect clip
        /// </summary>
        /// <param name="audio">AudioClip to play</param>
        /// <param name="volume">Volume between 0 and 1</param>
        public static void PlayClip (AudioClip audio, float volume)
        {
            if (audio == null)
                return;
            GameObject o = new GameObject("Sound Effect");
            o.AddComponent<DelayedDestroy>().Delay = audio.length;
            AudioSource a = o.AddComponent<AudioSource>();
            a.clip = audio;
            a.volume = volume;
            a.Play();
        }

        /// <summary>
        /// Get the active GameController component
        /// </summary>
        /// <returns>Active GameController component</returns>
        public static GameManager GetGame()
        {
            return GameManager.Manager;
        }

        /// <summary>
        /// Find the closest version of the target, so it can pass over repeat bounaries
        /// </summary>
        /// <param name="position">Position to compare to</param>
        /// <param name="target">Point to get closest version of</param>
        /// <param name="bounds">The bounds of the field</param>
        /// <returns></returns>
        public static Vector3 GetClosestPoint(Vector3 position, Vector3 target, Bounds bounds)
        {
            Vector3 tar = Vector3.zero;
            float d = float.MaxValue;

            //Find the closest version of the target, so it can pass over repeat bounaries
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector3 temp = target + new Vector3(x * bounds.size.x, y * bounds.size.y);
                    if ((temp - position).magnitude < d)
                    {
                        tar = temp;
                        d = (temp - position).magnitude;
                    }
                }
            }

            return tar;
        }
    }
}
