using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class score : MonoBehaviour
{
    private TextMeshProUGUI m_TextMeshPro;

    private void Start()
    {
        m_TextMeshPro = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (m_TextMeshPro != null)
        {
            m_TextMeshPro.text = PlayerPrefs.GetInt("score").ToString();
        }
    }
}
