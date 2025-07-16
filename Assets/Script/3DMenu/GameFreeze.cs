using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFreeze : MonoBehaviour
{
    private void Start()
    {
        // Freeze the game at the start
        Time.timeScale = 0f;
    }
}
