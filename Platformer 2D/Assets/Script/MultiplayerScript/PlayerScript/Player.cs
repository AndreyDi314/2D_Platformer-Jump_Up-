using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class Player : NetworkBehaviour
{
    // Ссылки на компоненты Rigidbody2D и BoxCollider2D
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    // Переменная для отслеживания того, в какую сторону у players
    bool facingRight = true;

    // Ссылка на локального игрока
    public static Player localPlayer;

    // Текстовое поле для отображения имени игрока
    public TextMesh NameDisplayText;

    // Синхронизируемое имя игрока, которое будет отображаться
    [SyncVar(hook = "DisplayPlayerName")]
    public string PlayerDisplayName;

    // Синхронизируемый ID матча, в котором находится игрок
    [SyncVar]
    public string matchID;

    // Синхронизируемый объект текущего матча
    [SyncVar]
    public Match CurrentMatch;

    // UI объекта в лобби
    public GameObject PlayerLobbyUI;

    // Уникальный идентификатор игрока
    private Guid netIDGuid;

    // UI объект в игровой сцене
    private GameObject GameUI;

    // Объект сетевого матча
    private NetworkMatch networkMatch;

    // Настройки прыжка на твердой поверхности
    [Header("Collision Ground Settings")]
    [SerializeField]
    private LayerMask jumpableGround;
    [SerializeField]
    private float groundAcceleration = 10f;
    [SerializeField]
    private float groundDeceleration = 5f;
    [SerializeField]
    private float groundjumpForce = 35f;

    // Настройки прыжка на песке
    [Header("Collision Sand Settings")]
    [SerializeField]
    private LayerMask sandGround;
    [SerializeField]
    private float sandAcceleration = 5f;
    [SerializeField]
    private float sandDeceleration = 10f;
    [SerializeField]
    private float sandjumpForce = 30f;

    // Настройки передвижения в воздухе
    [Header("Other Settings")]
    [SerializeField]
    private float airAcceleration = 2f;
    [SerializeField]
    private float maxSpeed = 10f;
    [SerializeField]
    private GroundChecker groundChecker;

    // Вспомогательные переменные для передвижения
    private Vector2 dirX;
    private bool isGrounded;
    private bool isSand;
    private float basicAcceleration;
    private float basicDeceleration;
    private float jumpForce;
    private bool jumpInProgress = false;

    // Метод, вызываемый при пробуждении объекта
    private void Awake()
    {
        // Получаем ссылку на объект сетевого матча
        networkMatch = GetComponent<NetworkMatch>();

        // Находим объект UI с тегом "GameUI"
        GameUI = GameObject.FindGameObjectWithTag("GameUI");

        // Инициализируем ссылки на компоненты
        rb = GetComponent<Rigidbody2D>();
        groundChecker = GetComponent<GroundChecker>();
    }

    // Метод, вызываемый при старте объекта
    void Start()
    {
        // Получаем ссылку на объект сетевого матча
        networkMatch = GetComponent<NetworkMatch>();

        // Если игрок является локальным
        if (isLocalPlayer)
        {
            // Сохраняем ссылку на него
            localPlayer = this;

            // Отправляем имя игрока на сервер
            CmdSendName(MainMenuLobby.instance.DisplayName);
        }
        else
        {
            // Создаем пользовательский интерфейс для игрока
            MainMenuLobby.instance.SpawnPlayerUIPrefab(this);
        }
    }

    // Метод, вызываемый при запуске сервера
    public override void OnStartServer()
    {
        // Создаем уникальный идентификатор игрока на основе сетевого идентификатора
        netIDGuid = netId.ToString().ToGuid();

        // Устанавливаем уникальный идентификатор матча
        networkMatch.matchId = netIDGuid;
    }

    // Метод, вызываемый при запуске клиента
    public override void OnStartClient()
    {
        // Если игрок является локальным, сохраняем ссылку на него
        if (isLocalPlayer)
        {
            localPlayer = this;
        }
        else
        {
            // Иначе создаем пользовательский интерфейс для игрока
            PlayerLobbyUI = MainMenuLobby.instance.SpawnPlayerUIPrefab(this);
        }
    }

    // Метод, вызываемый при остановке клиента
    public override void OnStopClient()
    {
        // Вызываем метод Disconnect() для игрока
        ClientDisconnect();
    }

    // Метод, вызываемый при остановке сервера
    public override void OnStopServer()
    {
        // Вызываем метод Disconnect() для игрока
        ServerDisconnect();
    }

    // Команда для отправки имени игрока на сервер
    [Command]
    public void CmdSendName(string name)
    {
        // Устанавливаем имя игрока
        PlayerDisplayName = name;
    }

    // Метод для отображения имени игрока
    public void DisplayPlayerName(string name, string playerName)
    {
        // Устанавливаем имя игрока
        name = PlayerDisplayName;

        // Выводим в консоль информацию об имени игрока
        Debug.Log("Имя " + name + " : " + playerName);

        // Отображаем имя игрока на пользовательском интерфейсе
        NameDisplayText.text = playerName;
    }

    // Метод для создания игровой сессии на локальном компьютере
    public void HostGame(bool publicMatch)
    {
        // Генерируем уникальный идентификатор для игровой сессии
        string ID = MainMenuLobby.GetRandomID();

        // Отправляем команду для создания игровой сессии на сервере
        CmdHostGame(ID, publicMatch);
    }

    // Команда для создания игровой сессии на сервере
    [Command]
    public void CmdHostGame(string ID, bool publicMatch)
    {
        // Устанавливаем уникальный идентификатор для игровой сессии
        matchID = ID;

        // Создаем игровое лобби и настраиваем его
        if (MainMenuLobby.instance.HostGame(ID, gameObject, publicMatch))
        {
            Debug.Log("Игровое лобби было создано успешно");

            // Устанавливаем уникальный идентификатор матча в объекте NetworkMatch
            networkMatch.matchId = ID.ToGuid();

            // Вызываем метод для отображения результата создания игрового лобби на клиенте
            TargetHostGame(true, ID);
        }
        else
        {
            Debug.Log("Ошибка в создании игрового лобби");

            // Вызываем метод для отображения результата создания игрового лобби на клиенте
            TargetHostGame(false, ID);
        }
    }

    // Делегат для отображения результата создания игрового лобби на клиенте
    [TargetRpc]
    void TargetHostGame(bool success, string ID)
    {
        // Устанавливаем уникальный идентификатор для игровой сессии
        matchID = ID;

        // Отображаем результат создания игрового лобби на клиенте
        MainMenuLobby.instance.HostSuccess(success, ID);
    }

    // Метод для подключения к уже существующей игровой сессии
    public void JoinGame(string inputID)
    {
        // Отправляем команду для подключения к существующей игровой сессии
        CmdJoinGame(inputID);
    }

    // Метод для присоединения игрока к игровому лобби по указанному ID
    // При успешном подключении устанавливает ID лобби для MatchMaker и вызывает TargetJoinGame с параметром успеха
    public void CmdJoinGame(string ID)
    {
        matchID = ID;
        if (MainMenuLobby.instance.JoinGame(ID, gameObject)) // Попытка присоединиться к лобби с указанным ID
        {
            Debug.Log("Успешное подключение к лобби");
            networkMatch.matchId = ID.ToGuid(); // Установка ID лобби для MatchMaker
            TargetJoinGame(true, ID); // Вызов метода для обновления пользовательского интерфейса
        }
        else
        {
            Debug.Log("Не удалось подключиться");
            TargetJoinGame(false, ID);
        }
    }

    // Метод для обновления пользовательского интерфейса в зависимости от результата попытки присоединиться к лобби
    // Выводит в консоль сообщение с ID лобби, к которому пытается присоединиться игрок
    [TargetRpc]
    void TargetJoinGame(bool success, string ID)
    {
        matchID = ID;
        Debug.Log($"ID {matchID} == {ID}");
        MainMenuLobby.instance.JoinSuccess(success, ID); // Обновление интерфейса в зависимости от успешности подключения
    }

    // Метод для отсоединения игрока от игрового лобби
    // Вызывает CmdDisconnectGame для обработки на сервере
    public void DisconnectGame()
    {
        CmdDisconnectGame();
    }

    // Команда для отсоединения игрока от игрового лобби
    // Вызывает ServerDisconnect для обработки на сервере
    // requiresAuthority = false, позволяет вызывать эту команду от любого клиента, не только хоста
    [Command(requiresAuthority = false)]
    void CmdDisconnectGame()
    {
        ServerDisconnect();
    }

    // Метод для обработки отсоединения от игрового лобби на сервере
    // Удаляет игрока из текущего лобби и вызывает RpcDisconnectGame для обработки на клиенте
    void ServerDisconnect()
    {
        MainMenuLobby.instance.PlayerDisconnected(gameObject, matchID);
        RpcDisconnectGame();
        networkMatch.matchId = netIDGuid;
    }

    // Метод для обработки отсоединения от игрового лобби на клиенте
    // Вызывает ClientDisconnect для обработки на клиенте
    [ClientRpc]
    void RpcDisconnectGame()
    {
        ClientDisconnect();
    }

    // Метод для обработки отсоединения от игрового лобби на клиенте
    // Удаляет экземпляр PlayerLobbyUI из сцены
    void ClientDisconnect()
    {
        if (PlayerLobbyUI != null)
        {
            if (!isServer)
            {
                Destroy(PlayerLobbyUI);
            }
            else
            {
                PlayerLobbyUI.SetActive(false);
            }
        }
    }

    // Метод для поиска игрового лобби
    // Вызывает CmdSearchGame для обработки на сервере
    public void SearchGame()
    {
        CmdSearchGame();
    }

    // Команда для поиска игрового лобби
    // Вызывает SearchGame на MainMenuLobby.instance для поиска доступных игровых лобби
    // Если лобби найдено, устанавливает его ID и вызывает TargetSearchGame с параметрами успеха и ID лобби
    // requiresAuthority = true, позволяет вызывать эту команду только хосту
    [Command]
    void CmdSearchGame()
    {
        if (MainMenuLobby.instance.SearchGame(gameObject, out matchID))
        {
            Debug.Log("Игра найдена успешно");
            networkMatch.matchId = matchID.ToGuid();
            TargetSearchGame(true, matchID);

            if (isServer && PlayerLobbyUI != null)
            {
                PlayerLobbyUI.SetActive(true);
            }
        }
        else
        {
            Debug.Log("Поиск игры не удался");
            TargetSearchGame(false, matchID);
        }
    }

    // Метод для обработки результата поиска игрового лобби на клиенте
    // Выводит в консоль сообщение о результате поиска
    [ClientRpc]
    void TargetSearchGame(bool success, string ID)
    {
        Debug.Log($"Поиск игры завершен: {success}");
    }

    // Метод для обработки результата поиска игрового лобби на клиенте
    // Устанавливает значение matchID и выводит в консоль сообщение о результате поиска
    // Вызывает MainMenuLobby.instance.SearchGameSuccess с параметрами успеха и ID лобби
    [TargetRpc]
    void TargetSearchGameResult(bool success, string ID)
    {
        matchID = ID;
        Debug.Log("ID: " + matchID + "==" + ID + " | " + success);
        MainMenuLobby.instance.SearchGameSuccess(success, ID);
    }

    // Метод для обновления количества игроков в лобби
    // Вызывает TargetPlayerCountUpdated для обновления пользовательского интерфейса
    [Server]
    public void PlayerCountUpdated(int playerCount)
    {
        TargetPlayerCountUpdated(playerCount);
    }

    // Метод для обновления количества игроков в лобби на клиенте
    // Если количество игроков больше 1, включает кнопку "Начать игру", иначе отключает ее
    [TargetRpc]
    void TargetPlayerCountUpdated(int playerCount)
    {
        if (playerCount > 1)
        {
            MainMenuLobby.instance.SetBeginButtonActive(true);
        }
        else
        {
            MainMenuLobby.instance.SetBeginButtonActive(false);
        }
    }

    // Метод для начала игры
    // Вызывает CmdBeginGame для обработки на сервере
    public void BeginGame()
    {
        CmdBeginGame();
    }

    // Команда для начала игры
    // Вызывает MainMenuLobby.instance.BeginGame с параметром matchID
    // requiresAuthority = true, позволяет вызывать эту команду только хосту
    [Command]
    public void CmdBeginGame()
    {
        MainMenuLobby.instance.BeginGame(matchID);
        Debug.Log("Игра начнется");
    }

    // Метод для начала игры на клиенте
    // Вызывает TargetBeginGame для обработки на клиенте
    public void StartGame()
    {
        TargetBeginGame();
    }

    // Метод для начала игры на клиенте
    [TargetRpc]
    void TargetBeginGame()
    {
        Debug.Log($"ID {matchID} | начало");

        //Сохраняет все экземпляры Player в памяти
        Player[] players = FindObjectsOfType<Player>();
        for (int i = 0; i < players.Length; i++)
        {
            DontDestroyOnLoad(players[i]);
        }

        //Устанавливает значение MainMenuLobby.instance.inGame в true
        MainMenuLobby.instance.inGame = true;
        
        //Выставляет значение facingRight в true
        facingRight = true;

        //Загружает следующую сцену
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }

    // Метод для установки времени игрока на клиенте
    // Выводит в консоль сообщение о значении playerTime
    // requiresAuthority = false, позволяет вызывать эту команду от любого клиента
    [ClientRpc]
    void RpcAssignPlayerTime(float playerTime)
    {
        Debug.Log("Player time assigned: " + playerTime);
    }

    public void DisconnectFromMatch()
    {
        if (isLocalPlayer)
        {
            CmdDisconnectFromMatch();
        }
    }

    [Command]
    private void CmdDisconnectFromMatch()
    {
        NetworkServer.Destroy(gameObject);
    }

    // Метод для обработки пользовательского ввода
    private void Update()
    {
        if (hasAuthority)
        {
            //Обновляет переменную dirX в зависимости от значения Input.GetAxisRaw("Horizontal")
            dirX = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

            //Определяет, находится ли игрок на земле или на песке
            isGrounded = groundChecker.IsGrounded();
            isSand = groundChecker.IsSand();

            if (isGrounded)
            {
                jumpInProgress = false;
                basicAcceleration = groundAcceleration;
                basicDeceleration = groundDeceleration;
                jumpForce = groundjumpForce;
            }

            if (isSand)
            {
                jumpInProgress = false;
                basicAcceleration = sandAcceleration;
                basicDeceleration = sandDeceleration;
                jumpForce = sandjumpForce;
            }

            //Вызывает методы HandleMovement и HandleJumping для обработки передвижения и прыжков
            HandleMovement();
            HandleJumping();

            //Обрабатывает переворачивание персонажа в зависимости от направления движения
            if (!facingRight && dirX.x > 0)
            {
                Flip();
            }
            else if (facingRight && dirX.x < 0)
            {
                Flip();
            }
        }
    }

    //Метод управления движением персонажа
    private void HandleMovement()
    {
        // Определяем ускорение характерное для того, в каком режиме движения сейчас находится персонаж:
        // - на земле или в воздухе
        float acceleration = isGrounded ? basicAcceleration : airAcceleration;
        float deceleration = basicDeceleration;

        if (dirX.x != 0)
        {
            // Определяем целевую скорость движения вдоль оси x в зависимости от направления движения
            float targetSpeed = dirX.x * maxSpeed;

            // В зависимости от текущей и целевой скорости, вычисляем промежуточную скорость сглаженно
            // на каждом кадре с помощью интерполяции
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, targetSpeed, Time.deltaTime * acceleration), rb.velocity.y);
        }
        else if (isGrounded)
        {
            // Если персонаж стоит на месте и стоит на земле, то притормаживаем его до нулевой скорости
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, Time.deltaTime * deceleration), rb.velocity.y);
        }
    }

    //Метод управления прыжком персонажа
    private void HandleJumping()
    {
        // Определяем, произошло ли нажатие кнопки "прыжок" и если да, еще и произошло ли это,
        // когда персонаж находится на земле или в песке
        if (Input.GetButtonDown("Jump") && (isGrounded || isSand) && !jumpInProgress)
        {
            // Применяем силу вверх, чтобы персонаж отпрыгнул
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // Отмечаем, что сейчас идет прыжок
            jumpInProgress = true;
        }
    }

    //Метод управления повором персонажа
    private void Flip()
    {
        // Проверяем, что текущий игрок является хозяином этой игровой сессии
        if (hasAuthority)
        {
            // Меняем направление вращения персонажа, если он изменил направление движения
            facingRight = !facingRight;

            // Меняем масштаб персонажа, чтобы он вращался в нужную сторону
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;

            // Если мы находимся в игровом режиме, то меняем направление вращения текста отображающего имя текущего игрока
            if (MainMenuLobby.instance.inGame)
            {
                Vector3 TextScale = NameDisplayText.transform.localScale;
                TextScale.x *= -1;
                NameDisplayText.transform.localScale = TextScale;
            }
        }
    }
}