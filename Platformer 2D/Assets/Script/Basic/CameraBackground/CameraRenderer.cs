using UnityEngine;
using Cinemachine; // Подключаем пространство имен Cinemachine.

// Класс CameraRenderer отвечает за настройку ортографической камеры Cinemachine.
public class CameraRenderer : MonoBehaviour
{
    // Компонент CinemachineVirtualCamera.
    public CinemachineVirtualCamera virtualCamera;

    // Функция Start вызывается в самом начале работы сценария.
    void Start()
    {
        // Получаем компонент CinemachineVirtualCamera из объекта, на котором висит скрипт.
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        // Вычисляем размер ортографической камеры в зависимости от соотношения сторон экрана.
        float orthoSize = 23f / ((float)Screen.width / (float)Screen.height);

        // Задаём размер ортографической камеры.
        virtualCamera.m_Lens.OrthographicSize = orthoSize;
    }
}