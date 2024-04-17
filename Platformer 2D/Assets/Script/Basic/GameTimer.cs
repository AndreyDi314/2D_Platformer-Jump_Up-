using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Класс GameTimer отвечает за отсчет времени в игре.
public class GameTimer : MonoBehaviour
{
    // Время начала отсчета.
    private float startTime;
    // Флаг, указывающий, запущен ли отсчет времени.
    private bool isRunning = false;

    // Объект текстового элемента, в котором отображается время.
    public Text timerText;

    // Метод Start() вызывается один раз в начале игры.
    void Start()
    {
        // Запускаем таймер.
        startTime = Time.time;
        isRunning = true;
    }

    // Метод Update() вызывается каждый кадр.
    void Update()
    {
        // Если таймер запущен, вычисляем прошедшее время и отображаем его на экране.
        if (isRunning)
        {
            float elapsedTime = Time.time - startTime;

            // Форматируем время в виде "минуты:секунда".
            string formattedTime = string.Format("{0:00}:{1:00}", Mathf.Floor(elapsedTime / 60), Mathf.RoundToInt(elapsedTime % 60));

            // Отображаем форматированное время на экране.
            timerText.text = formattedTime;
        }
    }
}