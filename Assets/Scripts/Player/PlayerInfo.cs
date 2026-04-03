using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private RectTransform display;

    private RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        UpdateInfo();
    }

    private void Update()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        
    }
}
