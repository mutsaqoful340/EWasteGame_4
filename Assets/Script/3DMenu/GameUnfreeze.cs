using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUnfreeze : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f; // Unfreeze the game by setting time scale to 1
    }
}
