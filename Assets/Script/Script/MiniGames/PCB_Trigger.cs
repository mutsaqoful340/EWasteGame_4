using UnityEngine;

public class PCBTrigger : MonoBehaviour
{
    public MiniGameManager miniGameManager;

    private void OnMouseDown()
    {
        miniGameManager.StartMiniGame();
    }
}
