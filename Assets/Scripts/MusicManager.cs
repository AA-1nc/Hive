using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioSource aSource;
    [SerializeField] private AudioSource bSource;
    [SerializeField] private float transitionSpeed = 5;

    private int track = 0;
    private float transitionPercent = 1;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (transitionPercent < 1)
        {
            if (track == 0)
            {
                aSource.volume = Mathf.Lerp(0, 1, transitionPercent);
                bSource.volume = Mathf.Lerp(1, 0, transitionPercent);
            }
            else
            {
                aSource.volume = Mathf.Lerp(1, 0, transitionPercent);
                bSource.volume = Mathf.Lerp(0, 1, transitionPercent);
            }

            transitionPercent += Time.deltaTime * transitionSpeed;
        }
    }

    public void ChangeTrack(int t)
    {
        track = t;
        transitionPercent = 0;
    }
}
