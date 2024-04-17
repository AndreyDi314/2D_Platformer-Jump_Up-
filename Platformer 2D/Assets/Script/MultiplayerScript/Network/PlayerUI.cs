using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    // —сылка на текстовое поле в пользовательском интерфейсе
    public Text PlayerText;

    // ћетод устанавливает им€ игрока в текстовое поле
    public void SetPlayer(string name)
    {
        PlayerText.text = name;
    }
}