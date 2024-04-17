using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт нужен для того, чтобы объект, на котором он висит, не уничтожался при загрузке новой сцены.
public class DontDestroy : MonoBehaviour
{
    // Этот метод вызывается сразу после создания объекта.
    void Awake()
    {
        // Находим все объекты со скриптом DontDestroy в сцене.
        DontDestroy[] dontDestroyObjects = FindObjectsOfType<DontDestroy>();

        // Если таких объектов больше одного, значит, этот скрипт уже висел на другом объекте, и мы можем его удалить, чтобы избежать дублирования.
        if (dontDestroyObjects.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            // Если же таких объектов нет, значит, это первый объект с таким скриптом, и мы должны сохранить его при загрузке новых сцен.
            DontDestroyOnLoad(this.gameObject);
        }
    }
}