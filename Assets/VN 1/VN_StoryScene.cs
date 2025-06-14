using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
        public List<Action> actions;

        [System.Serializable]
        public struct Action
        {
            public VN_Speaker speaker;
            public int spriteIndex;
            public Type actionType;
            public Vector2 coords;
            public float moveSpeed;

            [System.Serializable]
            public enum Type
            {
                NONE, APPEAR, MOVE, DISAPPEAR
            }

        }
    }
}

