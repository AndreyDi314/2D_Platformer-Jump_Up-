using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс FallingPlatform отвечает за поведение платформы, которая падает после контакта с игроком
public class FallingPlatform : MonoBehaviour
{
    // Ссылка на Rigidbody2D для управления физикой платформы
    private Rigidbody2D rb;

    // Переменные для хранения начальных позиции и вращения платформы
    private Vector2 initialPosition;
    private Quaternion initialRotation;

    // Переменные для управления перемещением платформы в обратную сторону
    private bool platformMovingBack;
    private Quaternion targetRotation;
    private float rotationSpeed = 90f;

    // Начальная инициализация
    private void Start()
    {
        // Получаем ссылку на Rigidbody2D
        rb = GetComponent<Rigidbody2D>();

        // Сохраняем начальные позицию и вращение платформы
        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Устанавливаем целевое вращение на 90 градусов вправо относительно начального
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, 90));
    }

    // Обновление вращения и позиции платформы в Update
    private void Update()
    {
        if (platformMovingBack)
        {
            MovePlatformBack();
        }
    }

    // Метод для перемещения платформы в обратную сторону
    private void MovePlatformBack()
    {
        // Перемещаем платформу в начальную позицию
        transform.position = Vector2.MoveTowards(transform.position, initialPosition, 20f * Time.deltaTime);

        // Вращаем платформу обратно в начальную позицию
        transform.rotation = Quaternion.RotateTowards(transform.rotation, initialRotation, rotationSpeed * Time.deltaTime);

        // Проверяем, пришли ли координаты платформы в начальную позицию
        if (Mathf.Approximately(transform.position.y, initialPosition.y))
        {
            // Обнуляем флаг, перемещаем платформу в начальную позицию и вращение
            platformMovingBack = false;
            transform.position = initialPosition;
            transform.rotation = initialRotation;
        }
    }

    // Метод для обработки столкновений с коллайдером
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Если платформа сталкивается с игроком и еще не переворачивалась
        if (collision.gameObject.CompareTag("Player") && !platformMovingBack)
        {
            // Запускаем метод DropPlatform через 0.5 секунд
            Invoke("DropPlatform", 0.5f);
        }
    }

    // Фиксированное обновление для управления физикой платформы
    private void FixedUpdate()
    {
        // Если платформа еще не перевернулась и не перемещается в обратную сторону
        if (!rb.isKinematic && !platformMovingBack)
        {
            // Вращаем платформу в целевое вращение
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    // Метод DropPlatform отвечает за падение платформы после столкновения с игроком
    private void DropPlatform()
    {
        // Отключаем isKinematic у Rigidbody2D, чтобы платформа стала управляемой физикой
        rb.isKinematic = false;

        // Запускаем метод GetPlatformBack через 1 секунду
        Invoke("GetPlatformBack", 1f);
    }

    // Метод GetPlatformBack возвращает платформу в начальную позицию и вращение
    private void GetPlatformBack()
    {
        // Обнуляем скорость платформы
        rb.velocity = Vector2.zero;

        // Включаем isKinematic у Rigidbody2D, чтобы платформа перестала управляться физикой
        rb.isKinematic = true;

        // Устанавливаем флаг platformMovingBack в true, чтобы начать перемещение платформы в обратную сторону
        platformMovingBack = true;

        // Устанавливаем целевое вращение платформы на 180 градусов относительно начального
        targetRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, 0, 180));
    }
}