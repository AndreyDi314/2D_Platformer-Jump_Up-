using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс GroundChecker отвечает за проверку, стоит ли игрок на земле и на каком типе поверхности он стоит.
public class GroundChecker : MonoBehaviour
{
    // Коллайдер игрока.
    [SerializeField] private BoxCollider2D playerCollider;

    // Маска слоёв земли, на которых можно прыгать.
    [SerializeField] private LayerMask jumpableGround;

    // Маска слоёв песка.
    [SerializeField] private LayerMask sandGround;

    // Функция, которая проверяет, стоит ли игрок на земле.
    public bool IsGrounded()
    {
        // Добавляем небольшой запас высоты, чтобы увеличить точность проверки.
        float extraHeight = 0.1f;

        // Проверяем, есть ли в пределах коллайдер земли.
        RaycastHit2D raycastHit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeight, jumpableGround);

        // Если объект был обнаружен, значит игрок стоит на земле.
        return raycastHit.collider != null;
    }

    // Функция, которая проверяет, стоит ли игрок на песке.
    public bool IsSand()
    {
        // Добавляем небольшой запас высоты, чтобы увеличить точность проверки.
        float extraHeight = 0.1f;

        // Проверяем, есть ли в пределах коллайдер песка.
        RaycastHit2D raycastHit = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + extraHeight, sandGround);

        // Если объект был обнаружен, значит игрок стоит на песке.
        return raycastHit.collider != null;
    }

    // Функция OnCollisionEnter вызывается, когда коллайдер объекта начинает касаться другого коллайдера.
    private void OnCollisionEnter(Collision collision)
    {
        // Если объект, с которым столкнулся игрок, имеет тег "Платформа", то мы получаем его жесткий двумерный тело.
        if (collision.gameObject.CompareTag("Platform"))
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();

            // Если тело найдено, мы задаём ему тип "Кинематический".
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }
    }

    // Функция OnCollisionExit вызывается, когда коллайдер объекта перестаёт касаться другого коллайдера.
    private void OnCollisionExit(Collision collision)
    {
        // Если объект, с которым столкнулся игрок, имеет тег "Платформа", то мы получаем его жесткое двумерное тело.
        if (collision.gameObject.CompareTag("Platform"))
        {
            // Если тело найдено, мы возвращаем его тип на "Динамический".
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }
}
