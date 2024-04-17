using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Класс, реализующий функциональность главного меню.
public class MainMenu : MonoBehaviour
{
    // Метод, запускающий игру и удаляющий объект NetworkManager.
    public void PlayGame()
    {
        // Получаем объект NetworkManager.
        GameObject networkManagerObject = GameObject.Find("NetworkManager");

        if (networkManagerObject != null)
        {
            // Удаляем объект NetworkManager.
            Destroy(networkManagerObject);
        }

        // Загружаем сцену с идентификатором 3.
        SceneManager.LoadScene(3);
    }

    // Метод вызывается при нажатии на кнопку, на которую вешается этот скрипт.
    public void QuitGame()
    {
        // Завершаем игру.
        Application.Quit();
    }
}