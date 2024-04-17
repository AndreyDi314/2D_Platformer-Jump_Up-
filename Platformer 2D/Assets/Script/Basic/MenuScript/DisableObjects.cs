using UnityEngine;
using UnityEngine.SceneManagement;

// Скрипт отвечает за деактивацию объектов при загрузке определенной сцены.
public class DisableObjects : MonoBehaviour
{
    // Массив объектов, которые нужно деактивировать при загрузке сцены.
    public GameObject[] objectsToDisable;

    // Этот метод вызывается сразу после загрузки сцены.
    private void Start()
    {
        // Подписываемся на событие загрузки сцены.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Этот метод вызывается каждый раз, когда загружается новая сцена.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Проверяем, загружается ли сцена в добавочном режиме и является ли она третьей сценой в build-е.
        if (mode == LoadSceneMode.Additive && scene.buildIndex == 2)
        {
            // Проходимся по всем объектам в массиве objectsToDisable.
            for (int i = 0; i < objectsToDisable.Length; i++)
            {
                // Если объект не равен null, деактивируем его.
                if (objectsToDisable[i] != null)
                {
                    objectsToDisable[i].SetActive(false);
                }
            }
        }

        // Отписываемся от события загрузки сцены, чтобы метод OnSceneLoaded не вызывался повторно.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}