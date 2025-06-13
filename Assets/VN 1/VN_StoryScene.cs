using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
[System.Serializable]

public class VN_StoryScene : ScriptableObject
{
    public List<Sentence> sentences;
    public Sprite background;
    public VN_StoryScene nextScene;

    [System.Serializable]
    public struct Sentence
    {
        public string text;
        public VN_Speaker speaker;
    }
}
