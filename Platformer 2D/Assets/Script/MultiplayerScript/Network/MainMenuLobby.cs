using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;
using System.Security.Cryptography;
using System.Text;

// Класс Match
[System.Serializable]
public class Match
{
    // Уникальный идентификатор матча
    public string ID;
    // Флаг о том, является ли матч публичным
    public bool PublicMatch;
    // Флаг о том, находится ли игрок в матче
    public bool InMatch;
    // Флаг о том, заполнен ли матч
    public bool MatchFull;
    // Список игроков, участвующих в матче
    public List<GameObject> players = new List<GameObject>();

    // Конструктор матча с указанным идентификатором, игроком и флагом о том, является ли матч публичным
    public Match(string ID, GameObject player, bool publicMatch)
    {
        MatchFull = false;
        InMatch = false;
        this.ID = ID;
        PublicMatch = publicMatch;
        players.Add(player);
    }

    // Конструктор без параметров
    public Match()
    {
        players = new List<GameObject>();
    }
}

// Класс MainMenuLobby, управляющий лобби игрового меню
// Содержит список матчей, максимальное количество игроков, ссылку на менеджер сети и ряд UI-компонентов
public class MainMenuLobby : NetworkBehaviour
{
    // Экземпляр класса MainMenuLobby, используемый для доступа к его свойствам и методам из других скриптов
    public static MainMenuLobby instance;
    // Синхронизируемый список матчей
    public readonly SyncList<Match> matches = new SyncList<Match>();
    // Синхронизируемый список уникальных идентификаторов матчей
    public readonly SyncList<string> matchIDs = new SyncList<string>();
    // Максимальное количество игроков в матче
    public int MaxPlayers;
    // Ссылка на менеджер сети
    private NetworkManager networkManager;

    // UI-компоненты
    [Header("MainMenuLobby")]
    // Поле ввода для ввода имени матча при поиске
    public InputField JoinInput;
    // Кнопки для поиска матчей
    public Button[] Buttons;
    // Канвас лобби
    public Canvas LobbyCanvas;
    // Канвас поиска матчей
    public Canvas SearchCanvas;
    // Флаг о том, происходит ли поиск матчей
    private bool searching;

    // UI-компоненты для работы с именем игрока
    [Header("Name")]
    // Панель для изменения имени
    public GameObject ChangeNamePanel;
    // Кнопка закрытия панели изменения имени
    public Button CloseButton;
    // Кнопка установки имени
    public Button SetNameButton;
    // Поле ввода имени
    public InputField NameInput;
    // Первый раз ли запущен скрипт
    public int firstTime = 1;
    // Отображаемое имя игрока
    [SyncVar] public string DisplayName;

    // Панель для лобби игрового меню
    [Header("Lobby")]
    public Transform UIPLayerParent;
    // Предварительно созданный объект UI-компонентов игрока
    public GameObject UIPlayerPrefab;
    // Текстовое поле для отображения идентификатора матча
    public Text IDText;
    // Кнопка начала игры
    public Button BeginGameButton;
    // UI-компонент игрока текущего игрока
    public GameObject localPlayerLobbyUI;
    // Флаг о том, начата ли игра
    public bool inGame;
    // Флаг о том, является ли текущий игрок хостом матча
    public bool host;

    // Панель для отображения ошибок
    [Header("Error")]
    public GameObject ErrorPanel;
    // Текстовое поле для отображения текста ошибки
    public Text ErrorText;

    // Метод, вызываемый при старте скрипта
    private void Start()
    {
        // Назначение экземпляра класса MainMenuLobby для доступа к его свойствам и методам из других скриптов
        instance = this;

        // Получение менеджера сети
        networkManager = FindObjectOfType<NetworkManager>();

        // Получение первого раза запуска скрипта из PlayerPrefs
        firstTime = PlayerPrefs.GetInt("firstTime", 1);

        // Если не задано имя игрока в PlayerPrefs
        if (!PlayerPrefs.HasKey("Name"))
        {
            // Возврат из метода
            return;
        }

        // Получение имени игрока из PlayerPrefs
        string defaultName = PlayerPrefs.GetString("Name");
        // Установка текста в поле ввода имени
        NameInput.text = defaultName;
        // Установка отображаемого имени игрока
        DisplayName = defaultName;
        // Вызов метода установки имени игрока
        SetName(defaultName);
    }

    // Метод, вызываемый каждый кадр
    private void Update()
    {
        // Если игра не начата
        if (!inGame)
        {
            // Получение массива игроков
            Player[] players = FindObjectsOfType<Player>();

            // Если первый раз запускается скрипт
            if (firstTime == 1)
            {
                // Активация панели изменения имени
                ChangeNamePanel.SetActive(true);
                // Деактивация кнопки закрытия панели изменения имени
                CloseButton.gameObject.SetActive(false);
            }
            else
            {
                // Активация кнопки закрытия панели изменения имени
                CloseButton.gameObject.SetActive(true);
            }

            // Сохранение текущего значения первого раза запуска скрипта в PlayerPrefs
            PlayerPrefs.SetInt("firstTime", firstTime);
        }
    }

    // Метод установки имени
    public void SetName(string name)
    {
        // Если введенное имя соответствует текущему отображаемому имени или введено пустое имя, кнопка установки имени becomes неактивна
        if (name == DisplayName || string.IsNullOrEmpty(name))
        {
            SetNameButton.interactable = false;
        }
        else
        {
            SetNameButton.interactable = true; // В противном случае кнопка установки имени becomes активна
        }
    }

    // Метод сохранения имени
    public void SaveName()
    {
        ChangeNamePanel.SetActive(false); // Скрыть панель изменения имени
        JoinInput.interactable = false; // Кнопка входа becomes неактивна
                                        // Деактивируйте все кнопки
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }
        firstTime = 0; // Сбросьте первый флаг времени
        DisplayName = NameInput.text; // Обновите отображаемое имя
        PlayerPrefs.SetString("Name", DisplayName); // Сохраните имя в настройках игрока
        Invoke(nameof(Disconnect), 1f); // Отключите игрока после 1 секунды
    }

    // Метод отключения
    public void Disconnect()
    {
        if (networkManager.mode == NetworkManagerMode.Host)
        {
            networkManager.StopHost(); // Остановите хост, если он существует
        }
        else if (networkManager.mode == NetworkManagerMode.ClientOnly)
        {
            networkManager.StopClient(); // Остановите клиент, если он существует
        }
    }

    // Метод установки активности кнопки начала игры
    public void SetBeginButtonActive(bool active)
    {
        BeginGameButton.interactable = active; // Установите активность кнопки начала игры в зависимости от переданного булевого значения
    }

    // Метод хоста игры
    public void Host(bool publicHost)
    {
        JoinInput.interactable = false; // Кнопка входа becomes неактивна
                                        // Деактивируйте все кнопки
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }

        Player.localPlayer.HostGame(publicHost); // Хост игры
    }

    // Метод обратного вызова успешного хоста
    public void HostSuccess(bool success, string matchID)
    {
        if (success)
        {
            LobbyCanvas.enabled = true; // Включите канву лобби

            if (localPlayerLobbyUI != null)
            {
                Destroy(localPlayerLobbyUI); // Удалите существующий объект пользовательского интерфейса лобби игрока
            }

            host = true; // Установите флаг хоста
            localPlayerLobbyUI = SpawnPlayerUIPrefab(Player.localPlayer); // Порождьте и сохраните пользовательский интерфейс лобби игрока
            IDText.text = matchID; // Обновите идентификатор матча
            BeginGameButton.interactable = true; // Активируйте кнопку начала игры
        }
        else
        {
            ErrorPanel.SetActive(true); // Отобразите панель ошибки
            ErrorText.text = "Не удалось создать лобби"; // Отобразите сообщение об ошибке
        }
    }

    // Метод присоединения к игре
    public void Join()
    {
        JoinInput.interactable = false; // Кнопка ввода becomes неактивна
                                        // Деактивируйте все кнопки
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }

        Player.localPlayer.JoinGame(JoinInput.text.ToUpper()); // Присоединитесь к игре с указанным идентификатором
    }

    // Метод обратного вызова успешного присоединения
    public void JoinSuccess(bool success, string matchID)
    {
        if (success)
        {
            LobbyCanvas.enabled = true; // Включите канву лобби

            if (localPlayerLobbyUI != null)
            {
                Destroy(localPlayerLobbyUI); // Удалите существующий объект пользовательского интерфейса лобби игрока
            }

            host = false; // Установите флаг хоста на false
            localPlayerLobbyUI = SpawnPlayerUIPrefab(Player.localPlayer); // Порождьте и сохраните пользовательский интерфейс лобби игрока
            IDText.text = matchID; // Обновите идентификатор матча
            BeginGameButton.interactable = false; // Деактивируйте кнопку начала игры
        }
        else
        {
            ErrorPanel.SetActive(true); // Отобразите панель ошибки
            ErrorText.text = "ID не найден"; // Отобразите сообщение об ошибке
        }
    }

    // Метод включения
    public void Enable()
    {
        ErrorPanel.SetActive(false); // Скрыть панель ошибки
                                     // Активируйте все кнопки
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true;
        }
        JoinInput.interactable = true; // Активируйте кнопку ввода
    }

    // Метод отключения от игры
    public void DisconnectGame()
    {
        if (localPlayerLobbyUI != null)
        {
            Destroy(localPlayerLobbyUI); // Удалите объект пользовательского интерфейса лобби игрока
        }

        Player.localPlayer.DisconnectGame(); // Отключитесь от игры
        LobbyCanvas.enabled = false; // Отключите канву лобби
        JoinInput.interactable = true; // Активируйте кнопку ввода
                                       // Активируйте все кнопки
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true;
        }
    }

    // Метод создания игры
    public bool HostGame(string matchID, GameObject player, bool publicMatch)
    {
        if (!matchIDs.Contains(matchID)) // Если указанный идентификатор матча не существует
        {
            matchIDs.Add(matchID); // Добавьте идентификатор матча в список
            Match match = new Match(matchID, player, publicMatch); // Создайте новый матч с указанным идентификатором, игроком и флагом публичного матча
            matches.Add(match); // Добавьте матч в список матчей
            player.GetComponent<Player>().CurrentMatch = match; // Назначьте игроку текущий матч
            return true; // Возвратите true, если матч был успешно создан
        }
        else
        {
            return false; // В противном случае возвратите false
        }
    }

    // Метод присоединения к игре
    public bool JoinGame(string matchID, GameObject player)
    {
        if (matchIDs.Contains(matchID)) // Если указанный идентификатор матча существует
        {
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].ID == matchID) // Найдите матч с указанным идентификатором
                {
                    if (!matches[i].InMatch && !matches[i].MatchFull) // Если матч еще не начался и не заполнен
                    {
                        matches[i].players.Add(player); // Добавьте игрока в список игроков матча
                        player.GetComponent<Player>().CurrentMatch = matches[i]; // Назначьте игроку текущий матч
                        matches[i].players[0].GetComponent<Player>().PlayerCountUpdated(matches[i].players.Count); // Обновите количество игроков в матче
                        if (matches[i].players.Count == MaxPlayers) // Если матч полный
                        {
                            matches[i].MatchFull = true; // Установите флаг матча на полный
                        }
                        break;
                    }
                    else
                    {
                        return false; // В противном случае верните false, если матч уже начался или заполнен
                    }
                }
            }

            return true; // В противном случае верните true, если игрок успешно присоединился к матчу
        }
        else
        {
            return false; // В противном случае верните false, если указанный идентификатор матча не существует
        }
    }

    // Метод поиска игры
    public bool SearchGame(GameObject player, out string ID)
    {
        ID = ""; // Обнулите идентификатор

        for (int i = 0; i < matches.Count; i++)
        {
            Debug.Log("Проверка ID " + matches[i].ID + " | в игре " + matches[i].InMatch + " | лобби полное " + matches[i].MatchFull + " | публичное лобби " + matches[i].PublicMatch);
            if (!matches[i].InMatch && !matches[i].MatchFull && matches[i].PublicMatch)
            {
                if (JoinGame(matches[i].ID, player)) // Присоединитесь к игре с указанным идентификатором
                {
                    ID = matches[i].ID; // Сохраните идентификатор матча
                    return true; // Верните true, если найдена доступная игра
                }
            }
        }

        return false; // В противном случае верните false, если не найдена доступная игра
    }

    // Метод создания случайного идентификатора
    public static string GetRandomID()
    {
        string ID = string.Empty;
        for (int i = 0; i < 5; i++)
        {
            int rand = UnityEngine.Random.Range(0, 36);
            if (rand < 26)
            {
                ID += (char)(rand + 65); // Создайте случайный идентификатор, используя латинские буквы и цифры
            }
            else
            {
                ID += (rand - 26).ToString();
            }
        }
        return ID;
    }

    // Метод создания объекта пользовательского интерфейса игрока
    public GameObject SpawnPlayerUIPrefab(Player player)
    {
        GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPLayerParent); // Порождьте новый объект пользовательского интерфейса игрока
        newUIPlayer.GetComponent<PlayerUI>().SetPlayer(player.PlayerDisplayName); // Назначьте имя игрока для пользовательского интерфейса игрока

        return newUIPlayer;
    }

    // Метод начала игры
    public void StartGame()
    {
        Player.localPlayer.BeginGame(); // Начните игру
    }

    // Метод поиска игры
    public void SearchGame()
    {
        StartCoroutine(Searching()); // Запустите корутину поиска игры
    }

    // Метод отмены поиска игры
    public void CancelSearchGame()
    {
        JoinInput.interactable = true; // Активируйте кнопку ввода
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true; // Активируйте все кнопки
        }

        searching = false; // Отмените поиск игры
    }

    // Метод успешного поиска игры
    public void SearchGameSuccess(bool success, string ID)
    {
        if (success)
        {
            SearchCanvas.enabled = false; // Отключите канву поиска игры
            searching = false; // Отмените поиск игры
            JoinSuccess(success, ID); // Вызовите метод успешного присоединения к игре
        }
    }

    // Метод начала игры
public void BeginGame(string matchID)
{
    for (int i = 0; i < matches.Count; i++)
    {
        if (matches[i].ID == matchID)
        {
            matches[i].InMatch = true; // Установите флаг матча на true, чтобы начать игру
            foreach (var player in matches[i].players)
            {
                player.GetComponent<Player>().StartGame(); // Начните игру для каждого игрока в матче
            }
            break;
        }
    }
}

// Метод обработки отключения игрока
public void PlayerDisconnected(GameObject player, string ID)
{
    for (int i = 0; i < matches.Count; i++)
    {
        if (matches[i].ID == ID)
        {
            int playerIndex = matches[i].players.IndexOf(player);
            if (matches[i].players.Count > playerIndex)
            {
                matches[i].players.RemoveAt(playerIndex); // Удалите отключенного игрока из списка игроков
            }

            if (matches[i].players.Count == 0)
            {
                matches.RemoveAt(i); // Удалите весь матч, если не осталось игроков
                matchIDs.Remove(ID);
            }
            else
            {
                matches[i].players[0].GetComponent<Player>().PlayerCountUpdated(matches[i].players.Count); // Обновите количество игроков в матче
            }
            break;
        }
    }
}

    // Метод выхода из игры
    public void Exit()
    {
        Disconnect(); // Отключитесь от сервера
        Application.Quit(); // Закройте приложение
    }

    // Корутина поиска игры
    IEnumerator Searching()
    {
        JoinInput.interactable = false; // Деактивируйте кнопку ввода
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false; // Деактивируйте все кнопки
        }
        SearchCanvas.enabled = true; // Включите канву поиска игры
        searching = true; // Установите флаг поиска игры на true

        float searchInterval = 1; // Интервал поиска игры (в секундах)
        float currentTime = 1; // Текущее время

        while (searching)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime; // Уменьшите текущее время путем вычитания из него времени прошедшего с момента последнего кадра
            }
            else
            {
                currentTime = searchInterval; // Обновите текущее время
                Player.localPlayer.SearchGame(); // Запустите поиск игры
            }
            yield return null; // Ждите следующего кадра
        }

        SearchCanvas.enabled = false; // Отключите канву поиска игры
    }
}

// Статический класс для расширения класса String
public static class MatchExtension
{
    // Метод расширения ToGuid, который конвертирует строку в Guid при помощи хэширования с помощью алгоритма MD5
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hasBytes = provider.ComputeHash(inputBytes);

        return new Guid(hasBytes);
    }
}