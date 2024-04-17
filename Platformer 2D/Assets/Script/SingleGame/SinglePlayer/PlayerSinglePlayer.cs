using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerSinglePlayer : MonoBehaviour
{
    // Компонент Rigidbody2D, необходимый для физического передвижения объекта.
    private Rigidbody2D rb;

    // Компонент BoxCollider2D, используется для проверки столкновений с другими объектами.
    private BoxCollider2D coll;

    // Флаг, указывающий, в каком направлении персонаж смотрит в данный момент.
    bool facingRight = true;

    // Настройки для проверки столкновений с площадками для прыжков.
    [Header("Collision Ground Settings")]
    [SerializeField] private LayerMask jumpableGround; // Маска слоёв для площадок для прыжков.
    [SerializeField] private float groundAcceleration = 10f; // Ускорение при передвижении по площадке для прыжков.
    [SerializeField] private float groundDeceleration = 5f; // Замедление при остановке на площадке для прыжков.
    [SerializeField] private float groundjumpForce = 35f; // Сила прыжка с площадки для прыжков.

    // Настройки для проверки столкновений с песком.
    [Header("Collision Sand Settings")]
    [SerializeField] private LayerMask sandGround; // Маска слоёв для песка.
    [SerializeField] private float sandAcceleration = 5f; // Ускорение при передвижении по песку.
    [SerializeField] private float sandDeceleration = 10f; // Замедление при остановке на песке.
    [SerializeField] private float sandjumpForce = 30f; // Сила прыжка с песка.

    // Настройки для других параметров передвижения.
    [Header("Other Settings")]
    [SerializeField] private float airAcceleration = 2f; // Ускорение при движении в воздухе.
    [SerializeField] private float maxSpeed = 10f; // Максимальная скорость передвижения.
    [SerializeField] private GroundChecker groundChecker; // Компонент для проверки столкновений с площадками.

    // Вспомогательные переменные для хранения направления и скорости передвижения.
    private Vector2 dirX;
    private bool isGrounded; // Флаг, указывающий, стоит ли персонаж на площадке для прыжков.
    private bool isSand; // Флаг, указывающий, стоит ли персонаж на песке.
    private float basicAcceleration; // Базовая скорость ускорения.
    private float basicDeceleration; // Базовое замедление.
    private float jumpForce; // Сила прыжка.
    private bool jumpInProgress = false; // Флаг, указывающий, происходит ли в данный момент прыжок.

    // Выполняется один раз при запуске сцены. Используется для инициализации компонентов.
    private void Awake()
    {
        // Получение компонента Rigidbody2D.
        rb = GetComponent<Rigidbody2D>();
        // Получение компонента GroundChecker.
        groundChecker = GetComponent<GroundChecker>();
    }

    // Обновляется каждый кадр.
    private void Update()
    {
            //Обновляет переменную dirX в зависимости от значения Input.GetAxisRaw("Horizontal")
            dirX = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

            //Определяет, находится ли игрок на земле или на песке
            isGrounded = groundChecker.IsGrounded();
            isSand = groundChecker.IsSand();

            if (isGrounded)
            {
                jumpInProgress = false;
                basicAcceleration = groundAcceleration;
                basicDeceleration = groundDeceleration;
                jumpForce = groundjumpForce;
            }

            if (isSand)
            {
                jumpInProgress = false;
                basicAcceleration = sandAcceleration;
                basicDeceleration = sandDeceleration;
                jumpForce = sandjumpForce;
            }

            //Вызывает методы HandleMovement и HandleJumping для обработки передвижения и прыжков
            HandleMovement();
            HandleJumping();

            //Обрабатывает переворачивание персонажа в зависимости от направления движения
            if (!facingRight && dirX.x > 0)
            {
                Flip();
            }
            else if (facingRight && dirX.x < 0)
            {
                Flip();
            }
    }

    //Метод управления движением персонажа
    private void HandleMovement()
    {
        // Определяем ускорение характерное для того, в каком режиме движения сейчас находится персонаж: на земле или в воздухе
        float acceleration = isGrounded ? basicAcceleration : airAcceleration;
        float deceleration = basicDeceleration;

        if (dirX.x != 0)
        {
            // Определяем целевую скорость движения вдоль оси x в зависимости от направления движения
            float targetSpeed = dirX.x * maxSpeed;

            // В зависимости от текущей и целевой скорости, вычисляем промежуточную скорость сглаженно на каждом кадре с помощью интерполяции
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, targetSpeed, Time.deltaTime * acceleration), rb.velocity.y);
        }
        else if (isGrounded)
        {
            // Если персонаж стоит на месте и стоит на земле, то притормаживаем его до нулевой скорости
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, Time.deltaTime * deceleration), rb.velocity.y);
        }
    }

    //Метод управления прыжком персонажа
    private void HandleJumping()
    {
        // Определяем, произошло ли нажатие кнопки "прыжок" и если да, еще и произошло ли это,
        // когда персонаж находится на земле или в песке
        if (Input.GetButtonDown("Jump") && (isGrounded || isSand) && !jumpInProgress)
        {
            // Применяем силу вверх, чтобы персонаж отпрыгнул
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // Отмечаем, что сейчас идет прыжок
            jumpInProgress = true;
        }
    }

    //Метод управления повором персонажа
    private void Flip()
    {
            // Меняем направление вращения персонажа, если он изменил направление движения
            facingRight = !facingRight;

            // Меняем масштаб персонажа, чтобы он вращался в нужную сторону
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;
    }
}