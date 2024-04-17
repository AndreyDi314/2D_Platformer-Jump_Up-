using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс LevelSwitch отвечает за включение/выключение объекта в зависимости от того, входит ли в него игрок.
public class LevelSwitch : MonoBehaviour
{
    // Компонент GameObject активного объекта.
    public GameObject activeFrame;

    // Функция OnTriggerEnter2D вызывается, когда объект с коллайдером входит в триггер.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем, является ли объект, входивший в триггер, игроком.
        if (other.CompareTag("Player"))
        {
            // Включаем активный объект.
            activeFrame.SetActive(true);
        }
    }

    // Функция OnTriggerExit2D вызывается, когда объект с коллайдером покидает триггер.
    private void OnTriggerExit2D(Collider2D other)
    {
        // Проверяем, является ли объект, покинувший триггер, игроком.
        if (other.CompareTag("Player"))
        {
            // Выключаем активный объект.
            activeFrame.SetActive(false);
        }
    }
}