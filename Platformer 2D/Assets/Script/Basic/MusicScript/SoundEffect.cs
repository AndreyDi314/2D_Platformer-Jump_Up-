using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс, реализующий воспроизведение звука пилы при видимом положении пилы.
public class SoundEffect : MonoBehaviour
{
    // Аудиокомпонент для воспроизведения звука.
    private AudioSource audioSource;

    // Флаг активности звука.
    private bool isPlaying = false;

    // Метод, выполняемый при начале программы.
    private void Start()
    {
        // Инициализируем аудиокомпонент.
        audioSource = GetComponent<AudioSource>();

        // Останавливаем воспроизведение звука.
        audioSource.Stop();
    }

    // Метод, выполняемый каждый кадр.
    private void Update()
    {
        // Находим все пилы на сцене по тегу "Saw"
        foreach (GameObject sawObject in GameObject.FindGameObjectsWithTag("Saw"))
        {
            // Если пила видна и звук не играет, то запускаем звук.
            if (IsVisible(sawObject) && !isPlaying)
            {
                audioSource.Play();
                isPlaying = true;

                // Выходим из цикла foreach, чтобы не запускать звук несколько раз подряд.
                return;
            }
        }

        // Если звук активен, а ни одна пила не видна, останавливаем звук.
        if (isPlaying && !IsAnySawVisible())
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }

    // Метод, проверяющий, видна ли пила в поле зрения камеры.
    bool IsVisible(GameObject obj)
    {
        // Метод WorldToViewportPoint() преобразует позицию объекта из мировых координат в координаты экрана.

        // Если координаты в видимом пространстве экрана, возвращаем true.
        Vector3 viewPos = Camera.main.WorldToViewportPoint(obj.transform.position);
        return viewPos.z > 0 && viewPos.x > 0 && viewPos.x < 1 && viewPos.y > 0 && viewPos.y < 1;
    }

    // Метод, проверяющий, видна ли любая пила на сцене.
    bool IsAnySawVisible()
    {
        // Находим все пилы на сцене по тегу "Saw"
        foreach (GameObject sawObject in GameObject.FindGameObjectsWithTag("Saw"))
        {
            // Если пила видна, возвращаем true.
            if (IsVisible(sawObject))
            {
                return true;
            }
        }

        // Если ни одна пила не видна, возвращаем false.
        return false;
    }
}