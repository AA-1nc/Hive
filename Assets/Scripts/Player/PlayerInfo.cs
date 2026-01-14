using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private RectTransform rt;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    private void Update()
    {
        UpdateInfo();
    }

    private void UpdateInfo()
    {
        rt.position = Camera.main.WorldToScreenPoint(player.transform.position);
        rt.position += new Vector3(rt.sizeDelta.x / 2, rt.sizeDelta.y / 2);
    }
}
