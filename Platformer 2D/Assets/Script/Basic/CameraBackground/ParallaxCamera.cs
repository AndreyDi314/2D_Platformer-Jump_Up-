using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ёти атрибуты нужны дл€ выполнени€ кода функции Update()  каждый кадр, даже в редакторе.
[ExecuteInEditMode]

// ƒелегат событи€, которое будет срабатывать каждый раз, когда камера сдвинетс€.
public delegate void ParallaxCameraDelegate(float deltaMovement);

//  ласс ParallaxCamera, реализующий параллаксный скроллинг.
public class ParallaxCamera : MonoBehaviour
{
    // ѕубличное поле дл€ событи€ CameraTranslate, которое может быть перехвачено в другом классе.
    public ParallaxCameraDelegate onCameraTranslate;

    // ѕеременна€, в которой сохран€етс€ предыдуща€ позици€ камеры.
    private float oldPosition;

    void Start()
    {
        // ¬ начале устанавливаем значение oldPosition равным текущей позиции камеры по горизонтали.
        oldPosition = transform.position.x;
    }

    void Update()
    {
        // ≈сли позици€ камеры по горизонтали не равна предыдущей позиции,
        if (transform.position.x != oldPosition)
        {
            // то провер€ем, не равно ли null событие onCameraTranslate.
            if (onCameraTranslate != null)
            {
                // ≈сли событие не равно null, то вызываем его и передаем аргументом величину перемещени€ камеры.
                float delta = oldPosition - transform.position.x;
                onCameraTranslate(delta);
            }

            // ѕосле вызова обновл€ем oldPosition дл€ следующего кадра.
            oldPosition = transform.position.x;
        }
    }
}