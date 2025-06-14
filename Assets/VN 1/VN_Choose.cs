using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChooseScene", menuName = "Data/New Choose")]
[System.Serializable]

public class VN_Choose : MonoBehaviour
{
    public List<ChooseLabel1> labels;

    [System.Serializable]
    public struct ChooseLabel1
    {
        public string text;
        public VN_Choose nextSceme;
    }
}
