using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NameSpeaker", menuName = "Data/New Speaker")]
[System.Serializable]

public class Speaker : ScriptableObject
{
    public string speakerName;
    public Color textColor;
}
