using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Класс, реализующий функциональность паузы в игре.
public class PauseMenu : MonoBehaviour
{
    // Канва с паузой.
    public GameObject pauseGameMenu;

    // Метод, проверяющий, нажата ли клавиша Esc.
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Включает или выключает паузу в зависимости от текущего состояния.
            pauseGameMenu.SetActive(!pauseGameMenu.activeSelf);
        }
    }

    // Метод, вызываемый после загрузки уровня.
    private void OnEnable()
    {
        // Добавляем делегат, который будет вызываться после загрузки уровня
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Метод, вызываемый при отключении объекта
    private void OnDisable()
    {
        // Удаляем делегат, чтобы он больше не вызывался
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Метод вызывается каждый раз, когда загружается новая сцена.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded called");
        // Проверяем, является ли загружаемая сцена главным меню.
        if (scene.buildIndex == 0)
        {
            // Находим все объекты со значением тэга "Destroy".
            GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag("Destroy");
            // Проходимся по всем найденным объектам.
            foreach (GameObject obj in objectsToDestroy)
            {
                Debug.Log($"Destroying object: {obj.name}");
                // Уничтожаем каждый объект.
                Destroy(obj);
            }

            // Запускаем корутину, которая загружает главное меню с задержкой в 1 секунду.
            StartCoroutine(LoadMenuAfterDelay(1.0f));
        }
    }

    // Корутина, загружающая главное меню после указанной задержки
    IEnumerator LoadMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadMenu();
    }

    // Метод, загружающий главное меню.
    public void LoadMenu()
    {
        // Загружает сцену Menu.
        SceneManager.LoadScene(0);
    }
}