using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXObject", menuName = "Function/SFXObject")]
public class SFXObject : ScriptableObject
{
    public AudioClip[] clips;
}
