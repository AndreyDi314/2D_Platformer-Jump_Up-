using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAnimation : NetworkBehaviour
{
    // Объявление переменных для Animator, SpriteRenderer, Player и Rigidbody2D компонентов
    private Animator anim;
    private SpriteRenderer sprite;
    private Player player;
    private Rigidbody2D rb;

    // Определение перечисления для состояний передвижения
    private enum MovementState { idle, running, jumping, falling }

    // Переменная для хранения текущего состояния передвижения
    private MovementState currentState;

    // Переменная для направления движения вдоль оси X
    public Vector2 dirX;

    // Метод, вызываемый перед первой отрисовкой кадра
    private void Start()
    {
        // Получение компонентов Animator, SpriteRenderer, Player и Rigidbody2D
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Метод, вызываемый каждый кадр
    private void Update()
    {
        // Проверка, имеет ли текущий объект владение (authority)
        if (hasAuthority)
        {
            // Обновление направления движения вдоль оси X на основе ввода горизонтальной оси
            dirX = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

            // Определение текущего состояния передвижения
            if (dirX.x > 0 || dirX.x < 0)
            {
                ChangeState(MovementState.running);
            }
            else
            {
                ChangeState(MovementState.idle);
            }

            // Определение текущего состояния прыжка или падения
            if (rb.velocity.y > 0.1f)
            {
                ChangeState(MovementState.jumping);
            }
            else if (rb.velocity.y < -0.1f)
            {
                ChangeState(MovementState.falling);
            }
        }
    }

    // Метод для изменения текущего состояния передвижения
    private void ChangeState(MovementState newState)
    {
        // Проверка, изменилось ли состояние передвижения
        if (currentState != newState)
        {
            // Обновление текущего состояния передвижения
            currentState = newState;

            // Установка текущего состояния передвижения в Animator через целочисленное значение
            anim.SetInteger("state", (int)currentState);
        }
    }
}
