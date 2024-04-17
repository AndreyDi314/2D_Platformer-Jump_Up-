using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс ParallaxCamera отвечает за создание эффекта параллакса у камеры. Выполняется в редакторской среде (ExecuteInEditMode).
[ExecuteInEditMode]

public class ParalaxCamera : MonoBehaviour
{
    // Компонент ParallaxCamera, к которому прикреплен данный скрипт.
    public ParallaxCamera parallaxCamera;

    // Список слоёв ParallaxLayer.
    List<ParallaxLayer> parallaxLayers = new List<ParallaxLayer>();

    // Функция Start вызывается в самом начале работы скрипта.
    void Start()
    {
        // Если скрипт ParallaxCamera не прикреплён к камере, то прикрепляем его к главной камере.
        if (parallaxCamera == null)
            parallaxCamera = Camera.main.GetComponent<ParallaxCamera>();

        // Если скрипт ParallaxCamera найден, то подписываемся на событие CameraTranslate.
        if (parallaxCamera != null)
            parallaxCamera.onCameraTranslate += Move;

        // Инициализируем слои ParallaxLayer.
        SetLayers();
    }

    // Функция SetLayers инициализирует слои ParallaxLayer.
    void SetLayers()
    {
        // Очищаем список слоёв.
        parallaxLayers.Clear();

        // Проходимся по всем дочерним объектам текущего объекта.
        for (int i = 0; i < transform.childCount; i++)
        {
            // Получаем компонент ParallaxLayer у текущего дочернего объекта.
            ParallaxLayer layer = transform.GetChild(i).GetComponent<ParallaxLayer>();

            // Если компонент ParallaxLayer существует, то добавляем его в список слоёв.
            if (layer != null)
            {
                layer.name = "Layer-" + i;
                parallaxLayers.Add(layer);
            }
        }
    }

    // Функция Move перемещает слои ParallaxLayer.
    void Move(float delta)
    {
        // Перемещаем все слои из списка слоёв.
        foreach (ParallaxLayer layer in parallaxLayers)
        {
            layer.Move(delta);
        }
    }
}