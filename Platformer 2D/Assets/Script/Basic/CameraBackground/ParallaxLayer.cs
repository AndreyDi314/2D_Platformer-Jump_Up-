using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Атрибут, необходимый для выполнения скрипта в редакторе в режиме реального времени.
[ExecuteInEditMode]

// Класс ParallaxLayer, реализующий параллаксный скроллинг.
public class ParallaxLayer : MonoBehaviour
{
    // Параметр для задания коэффициента параллаксного скроллинга.
    public float parallaxFactor;

    // Обновляет позицию спрайта в соответствии с параллаксным смещением.
    public void Move(float delta)
    {
        // Создаем новую позицию на основе текущей локальной позиции спрайта.
        Vector3 newPos = transform.localPosition;

        // Устанавливаем позицию спрайта в зависимости от значения параметра parallaxFactor. Чем больше значение параметра, тем больше смещение будет на экране.
        newPos.x -= delta * parallaxFactor;

        // Обновляем локальное смещение спрайта.
        transform.localPosition = newPos;
    }
}