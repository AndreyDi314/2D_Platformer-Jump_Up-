using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// Класс Connection отвечает за подключение клиента к серверу.
public class Connection : MonoBehaviour
{
    // Объект NetworkManager, который управляет сетевыми соединениями.
    public NetworkManager networkManager;

    // Метод Start() вызывается один раз в начале игры.
    private void Start()
    {
        // Проверяем, запускается ли игра в пакетном режиме (без графического интерфейса).
        if (!Application.isBatchMode)
        {
            // Если игра не запускается в пакетном режиме, подключаемся к серверу.
            networkManager.StartClient();
        }
    }

    // Метод JoinClient() вызывается при нажатии на кнопку в интерфейсе пользователя.
    public void JoinClient()
    {
        // Устанавливаем адрес сервера (в данном случае, сервер запущен на локальном компьютере).
        networkManager.networkAddress = "localhost";
        // Подключаемся к серверу.
        networkManager.StartClient();
    }
}