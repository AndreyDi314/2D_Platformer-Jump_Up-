using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlatformRider : MonoBehaviour
{
    // ѕриватное булевое поле isRidingPlatform, которое указывает, ездит ли персонаж на платформе или нет.
    private bool isRidingPlatform = false;

    // ѕриватное поле platformPositionOffset, которое хранит смещение персонажа относительно позиции платформы.
    private Vector3 platformPositionOffset;

    // ѕриватное поле currentPlatformCollider, которое хранит текущую коллайдер платформы, на которой ездит персонаж.
    private Collider2D currentPlatformCollider;

    // ћетод OnTriggerEnter2D() вызываетс€, когда персонаж входит в триггерную зону другого коллайдера.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ≈сли коллайдер, с которым столкнулс€ персонаж, имеет тег "Platform", то устанавливаем значение isRidingPlatform в true.
        if (collision.CompareTag("Platform"))
        {
            isRidingPlatform = true;
            platformPositionOffset = transform.position - collision.transform.position;
            currentPlatformCollider = collision;
        }
    }

    // ћетод OnTriggerExit2D() вызываетс€, когда персонаж выходит из триггерной зоны другого коллайдера.
    private void OnTriggerExit2D(Collider2D collision)
    {
        // ≈сли коллайдер, с которым столкнулс€ персонаж, имеет тег "Platform", то устанавливаем значение isRidingPlatform в false.
        if (collision.CompareTag("Platform"))
        {
            isRidingPlatform = false;
            currentPlatformCollider = null;
        }
    }

    // ћетод FixedUpdate() вызываетс€ в фиксированном времени и используетс€ дл€ физики и других расчетов.
    private void FixedUpdate()
    {
        // ≈сли значение isRidingPlatform равно true и currentPlatformCollider не равно null, то перемещаем персонажа вместе с платформой.
        if (isRidingPlatform && currentPlatformCollider != null)
        {
            transform.position = currentPlatformCollider.transform.position + platformPositionOffset;
        }
    }
}