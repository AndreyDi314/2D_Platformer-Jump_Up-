using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс CameraController отвечает за передвижение камеры за персонажем в игре.
public class CameraController : MonoBehaviour
{
    // Компонент Transform цели (персонажа), за которой будет следовать камера.
    Transform target;

    // Вектор, отвечающий за сглаживание передвижения камеры.
    Vector3 velocity = Vector3.zero;

    // Параметр для управления сглаживанием передвижения камеры.
    [Range(0, 1)]
    public float smoothTime;

    // Отступ камеры от объекта.
    Vector3 positionOffset = new Vector3(0f, 5f, 10.19f);

    // Параметры для ограничения передвижения камеры по оси X и Y.
    [Header("Axis Limitation")]
    public Vector2 xLimit;
    public Vector2 yLimit;

    // Компонент GameObject игрока.
    private GameObject player;

    // Функция Awake вызывается в самом начале работы сценариев. Используется для инициализации переменных.
    private void Awake()
    {
        // Находим игрока по тегу и сохраняем его компонент Transform.
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;
    }

    // Функция FixedUpdate вызывается в определённый момент цикла обновления. Используется для работы с физикой и передвижением камеры.
    private void FixedUpdate()
    {
        // Если объект-цель существует, то передвигаем камеру.
        if (target != null)
        {
            // Вычисляем целевую позицию камеры.
            Vector3 targetPosition = ClampTargetPosition(target.position + positionOffset);

            // Сглаживаем передвижение камеры с помощью функции SmoothDamp.
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }

    // Функция ClampTargetPosition ограничивает передвижение камеры в пределах заданных границ по оси X и Y.
    private Vector3 ClampTargetPosition(Vector3 targetPosition)
    {
        // Возвращаем новую позицию камеры с ограничениями.
        return new Vector3(Mathf.Clamp(targetPosition.x, xLimit.x, xLimit.y), Mathf.Clamp(targetPosition.y, yLimit.x, yLimit.y), -10);
    }
}
