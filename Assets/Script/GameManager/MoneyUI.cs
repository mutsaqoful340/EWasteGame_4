using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public GameManager gameManager;
    public TMP_Text moneyText;

    void Update()
    {
        moneyText.text = "Money: $" + gameManager.money;
    }
}
