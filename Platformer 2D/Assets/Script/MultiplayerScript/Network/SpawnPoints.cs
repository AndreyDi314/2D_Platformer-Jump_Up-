using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    // Приватное поле spawnPoints, которое будет хранить все экземпляры SpawnPoints в сцене.
    private SpawnPoints[] spawnPoints;

    private void Awake()
    {
        // Ищем все экземпляры SpawnPoints в сцене.
        spawnPoints = FindObjectsOfType<SpawnPoints>();

        // Если количество экземпляров SpawnPoints больше 1, то удаляем текущий экземпляр.
        if (spawnPoints.Length > 1)
        {
            Destroy(gameObject);
        }

        // Сохраняем текущий экземпляр в памяти между сменами сцен.
        DontDestroyOnLoad(gameObject);
    }
}