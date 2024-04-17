using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс CanvasOn отвечает за активацию канваса в начале игры.
public class CanvasOn : MonoBehaviour
{
    // Объект канваса, который нужно активировать.
    public GameObject Canvas;

    // Метод Start() вызывается один раз в начале игры.
    private void Start()
    {
        // Активируем канвас.
        Canvas.SetActive(true);
    }
}