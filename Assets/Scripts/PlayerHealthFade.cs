using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthFade : MonoBehaviour
{
    private HealthController health;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        health = transform.parent.gameObject.GetComponent<HealthController>();
        health.PropertyChanged += Health_PropertyChanged;
    }

    private void Health_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1 - health.Health / (float)(health.StartHealth));
    }
}
