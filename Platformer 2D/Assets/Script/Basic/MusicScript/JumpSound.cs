using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс, воспроизводящий звук прыжка при нажатии на пробел.
public class JumpSound : MonoBehaviour
{
    // Аудиосорс для воспроизведения звука.
    private AudioSource audioSource;

    // Метод, выполняемый при начале программы.
    private void Start()
    {
        // Находим встроенный аудиокомпонент объекта.
        audioSource = GetComponent<AudioSource>();
    }

    // Метод, выполняемый каждый кадр.
    private void Update()
    {
        // Проверяем, была ли нажата пробел на клавиатуре.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Вызываем метод Play() у аудиокомпонента и воспроизводим звук прыжка.
            audioSource.Play();
        }
    }
}