using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "NewStoryScene", menuName = "Data/New Story Scene")]
[System.Serializable]

public class VN_StoryScene : GameScene
{
    public List<Sentence> sentences;
    public Sprite background;
    public GameScene nextScene;

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

public class GameScene : ScriptableObject { }
