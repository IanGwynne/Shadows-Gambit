using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class VentTilemapBehaviour : MonoBehaviour
{
    private Tilemap tilemap;
    private Color enterColor = new Color(1, 1, 1, 0); // Fully transparent
    private Color exitColor = new Color(1, 1, 1, 1);  // Fully opaque

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tilemap.color = enterColor;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tilemap.color = enterColor;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tilemap.color = exitColor;
        }
    }
}