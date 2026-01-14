using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHexPlacer : MonoBehaviour
{
    [SerializeField] private GameObject hex;
    [SerializeField] private float minPlaceRadius = 6;
    [SerializeField] private float maxPlaceRadius = 7;
    [SerializeField] private int rows;
    [SerializeField] private int columns;

    private void Start()
    {
        float startX = -(columns - 1) * 0.375f;
        float startY = -(rows - 1) / 2;

        for (int col = 0; col < columns; col++)
        {
            float yOffset = col % 2 == 0 ? 0.5f : 0;

            for (int row = 0; row < rows; row++)
            {
                Vector3 pos = new Vector3(startX + col * 0.75f, startY + row + yOffset, 0);
                float dist = Vector3.Distance(Vector3.zero, pos);
                if (dist >= minPlaceRadius && dist <= maxPlaceRadius)
                    Instantiate(hex, pos, Quaternion.identity);
            }
        }
    }
}
