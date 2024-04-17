using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using System;

public class Player : NetworkBehaviour
{
    // ������ �� ���������� Rigidbody2D � BoxCollider2D
    private Rigidbody2D rb;
    private BoxCollider2D coll;

    // ���������� ��� ������������ ����, � ����� ������� � players
    bool facingRight = true;

    // ������ �� ���������� ������
    public static Player localPlayer;

    // ��������� ���� ��� ����������� ����� ������
    public TextMesh NameDisplayText;

    // ���������������� ��� ������, ������� ����� ������������
    [SyncVar(hook = "DisplayPlayerName")]
    public string PlayerDisplayName;

    // ���������������� ID �����, � ������� ��������� �����
    [SyncVar]
    public string matchID;

    // ���������������� ������ �������� �����
    [SyncVar]
    public Match CurrentMatch;

    // UI ������� � �����
    public GameObject PlayerLobbyUI;

    // ���������� ������������� ������
    private Guid netIDGuid;

    // UI ������ � ������� �����
    private GameObject GameUI;

    // ������ �������� �����
    private NetworkMatch networkMatch;

    // ��������� ������ �� ������� �����������
    [Header("Collision Ground Settings")]
    [SerializeField]
    private LayerMask jumpableGround;
    [SerializeField]
    private float groundAcceleration = 10f;
    [SerializeField]
    private float groundDeceleration = 5f;
    [SerializeField]
    private float groundjumpForce = 35f;

    // ��������� ������ �� �����
    [Header("Collision Sand Settings")]
    [SerializeField]
    private LayerMask sandGround;
    [SerializeField]
    private float sandAcceleration = 5f;
    [SerializeField]
    private float sandDeceleration = 10f;
    [SerializeField]
    private float sandjumpForce = 30f;

    // ��������� ������������ � �������
    [Header("Other Settings")]
    [SerializeField]
    private float airAcceleration = 2f;
    [SerializeField]
    private float maxSpeed = 10f;
    [SerializeField]
    private GroundChecker groundChecker;

    // ��������������� ���������� ��� ������������
    private Vector2 dirX;
    private bool isGrounded;
    private bool isSand;
    private float basicAcceleration;
    private float basicDeceleration;
    private float jumpForce;
    private bool jumpInProgress = false;

    // �����, ���������� ��� ����������� �������
    private void Awake()
    {
        // �������� ������ �� ������ �������� �����
        networkMatch = GetComponent<NetworkMatch>();

        // ������� ������ UI � ����� "GameUI"
        GameUI = GameObject.FindGameObjectWithTag("GameUI");

        // �������������� ������ �� ����������
        rb = GetComponent<Rigidbody2D>();
        groundChecker = GetComponent<GroundChecker>();
    }

    // �����, ���������� ��� ������ �������
    void Start()
    {
        // �������� ������ �� ������ �������� �����
        networkMatch = GetComponent<NetworkMatch>();

        // ���� ����� �������� ���������
        if (isLocalPlayer)
        {
            // ��������� ������ �� ����
            localPlayer = this;

            // ���������� ��� ������ �� ������
            CmdSendName(MainMenuLobby.instance.DisplayName);
        }
        else
        {
            // ������� ���������������� ��������� ��� ������
            MainMenuLobby.instance.SpawnPlayerUIPrefab(this);
        }
    }

    // �����, ���������� ��� ������� �������
    public override void OnStartServer()
    {
        // ������� ���������� ������������� ������ �� ������ �������� ��������������
        netIDGuid = netId.ToString().ToGuid();

        // ������������� ���������� ������������� �����
        networkMatch.matchId = netIDGuid;
    }

    // �����, ���������� ��� ������� �������
    public override void OnStartClient()
    {
        // ���� ����� �������� ���������, ��������� ������ �� ����
        if (isLocalPlayer)
        {
            localPlayer = this;
        }
        else
        {
            // ����� ������� ���������������� ��������� ��� ������
            PlayerLobbyUI = MainMenuLobby.instance.SpawnPlayerUIPrefab(this);
        }
    }

    // �����, ���������� ��� ��������� �������
    public override void OnStopClient()
    {
        // �������� ����� Disconnect() ��� ������
        ClientDisconnect();
    }

    // �����, ���������� ��� ��������� �������
    public override void OnStopServer()
    {
        // �������� ����� Disconnect() ��� ������
        ServerDisconnect();
    }

    // ������� ��� �������� ����� ������ �� ������
    [Command]
    public void CmdSendName(string name)
    {
        // ������������� ��� ������
        PlayerDisplayName = name;
    }

    // ����� ��� ����������� ����� ������
    public void DisplayPlayerName(string name, string playerName)
    {
        // ������������� ��� ������
        name = PlayerDisplayName;

        // ������� � ������� ���������� �� ����� ������
        Debug.Log("��� " + name + " : " + playerName);

        // ���������� ��� ������ �� ���������������� ����������
        NameDisplayText.text = playerName;
    }

    // ����� ��� �������� ������� ������ �� ��������� ����������
    public void HostGame(bool publicMatch)
    {
        // ���������� ���������� ������������� ��� ������� ������
        string ID = MainMenuLobby.GetRandomID();

        // ���������� ������� ��� �������� ������� ������ �� �������
        CmdHostGame(ID, publicMatch);
    }

    // ������� ��� �������� ������� ������ �� �������
    [Command]
    public void CmdHostGame(string ID, bool publicMatch)
    {
        // ������������� ���������� ������������� ��� ������� ������
        matchID = ID;

        // ������� ������� ����� � ����������� ���
        if (MainMenuLobby.instance.HostGame(ID, gameObject, publicMatch))
        {
            Debug.Log("������� ����� ���� ������� �������");

            // ������������� ���������� ������������� ����� � ������� NetworkMatch
            networkMatch.matchId = ID.ToGuid();

            // �������� ����� ��� ����������� ���������� �������� �������� ����� �� �������
            TargetHostGame(true, ID);
        }
        else
        {
            Debug.Log("������ � �������� �������� �����");

            // �������� ����� ��� ����������� ���������� �������� �������� ����� �� �������
            TargetHostGame(false, ID);
        }
    }

    // ������� ��� ����������� ���������� �������� �������� ����� �� �������
    [TargetRpc]
    void TargetHostGame(bool success, string ID)
    {
        // ������������� ���������� ������������� ��� ������� ������
        matchID = ID;

        // ���������� ��������� �������� �������� ����� �� �������
        MainMenuLobby.instance.HostSuccess(success, ID);
    }

    // ����� ��� ����������� � ��� ������������ ������� ������
    public void JoinGame(string inputID)
    {
        // ���������� ������� ��� ����������� � ������������ ������� ������
        CmdJoinGame(inputID);
    }

    // ����� ��� ������������� ������ � �������� ����� �� ���������� ID
    // ��� �������� ����������� ������������� ID ����� ��� MatchMaker � �������� TargetJoinGame � ���������� ������
    public void CmdJoinGame(string ID)
    {
        matchID = ID;
        if (MainMenuLobby.instance.JoinGame(ID, gameObject)) // ������� �������������� � ����� � ��������� ID
        {
            Debug.Log("�������� ����������� � �����");
            networkMatch.matchId = ID.ToGuid(); // ��������� ID ����� ��� MatchMaker
            TargetJoinGame(true, ID); // ����� ������ ��� ���������� ����������������� ����������
        }
        else
        {
            Debug.Log("�� ������� ������������");
            TargetJoinGame(false, ID);
        }
    }

    // ����� ��� ���������� ����������������� ���������� � ����������� �� ���������� ������� �������������� � �����
    // ������� � ������� ��������� � ID �����, � �������� �������� �������������� �����
    [TargetRpc]
    void TargetJoinGame(bool success, string ID)
    {
        matchID = ID;
        Debug.Log($"ID {matchID} == {ID}");
        MainMenuLobby.instance.JoinSuccess(success, ID); // ���������� ���������� � ����������� �� ���������� �����������
    }

    // ����� ��� ������������ ������ �� �������� �����
    // �������� CmdDisconnectGame ��� ��������� �� �������
    public void DisconnectGame()
    {
        CmdDisconnectGame();
    }

    // ������� ��� ������������ ������ �� �������� �����
    // �������� ServerDisconnect ��� ��������� �� �������
    // requiresAuthority = false, ��������� �������� ��� ������� �� ������ �������, �� ������ �����
    [Command(requiresAuthority = false)]
    void CmdDisconnectGame()
    {
        ServerDisconnect();
    }

    // ����� ��� ��������� ������������ �� �������� ����� �� �������
    // ������� ������ �� �������� ����� � �������� RpcDisconnectGame ��� ��������� �� �������
    void ServerDisconnect()
    {
        MainMenuLobby.instance.PlayerDisconnected(gameObject, matchID);
        RpcDisconnectGame();
        networkMatch.matchId = netIDGuid;
    }

    // ����� ��� ��������� ������������ �� �������� ����� �� �������
    // �������� ClientDisconnect ��� ��������� �� �������
    [ClientRpc]
    void RpcDisconnectGame()
    {
        ClientDisconnect();
    }

    // ����� ��� ��������� ������������ �� �������� ����� �� �������
    // ������� ��������� PlayerLobbyUI �� �����
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

    // ����� ��� ������ �������� �����
    // �������� CmdSearchGame ��� ��������� �� �������
    public void SearchGame()
    {
        CmdSearchGame();
    }

    // ������� ��� ������ �������� �����
    // �������� SearchGame �� MainMenuLobby.instance ��� ������ ��������� ������� �����
    // ���� ����� �������, ������������� ��� ID � �������� TargetSearchGame � ����������� ������ � ID �����
    // requiresAuthority = true, ��������� �������� ��� ������� ������ �����
    [Command]
    void CmdSearchGame()
    {
        if (MainMenuLobby.instance.SearchGame(gameObject, out matchID))
        {
            Debug.Log("���� ������� �������");
            networkMatch.matchId = matchID.ToGuid();
            TargetSearchGame(true, matchID);

            if (isServer && PlayerLobbyUI != null)
            {
                PlayerLobbyUI.SetActive(true);
            }
        }
        else
        {
            Debug.Log("����� ���� �� ������");
            TargetSearchGame(false, matchID);
        }
    }

    // ����� ��� ��������� ���������� ������ �������� ����� �� �������
    // ������� � ������� ��������� � ���������� ������
    [ClientRpc]
    void TargetSearchGame(bool success, string ID)
    {
        Debug.Log($"����� ���� ��������: {success}");
    }

    // ����� ��� ��������� ���������� ������ �������� ����� �� �������
    // ������������� �������� matchID � ������� � ������� ��������� � ���������� ������
    // �������� MainMenuLobby.instance.SearchGameSuccess � ����������� ������ � ID �����
    [TargetRpc]
    void TargetSearchGameResult(bool success, string ID)
    {
        matchID = ID;
        Debug.Log("ID: " + matchID + "==" + ID + " | " + success);
        MainMenuLobby.instance.SearchGameSuccess(success, ID);
    }

    // ����� ��� ���������� ���������� ������� � �����
    // �������� TargetPlayerCountUpdated ��� ���������� ����������������� ����������
    [Server]
    public void PlayerCountUpdated(int playerCount)
    {
        TargetPlayerCountUpdated(playerCount);
    }

    // ����� ��� ���������� ���������� ������� � ����� �� �������
    // ���� ���������� ������� ������ 1, �������� ������ "������ ����", ����� ��������� ��
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

    // ����� ��� ������ ����
    // �������� CmdBeginGame ��� ��������� �� �������
    public void BeginGame()
    {
        CmdBeginGame();
    }

    // ������� ��� ������ ����
    // �������� MainMenuLobby.instance.BeginGame � ���������� matchID
    // requiresAuthority = true, ��������� �������� ��� ������� ������ �����
    [Command]
    public void CmdBeginGame()
    {
        MainMenuLobby.instance.BeginGame(matchID);
        Debug.Log("���� ��������");
    }

    // ����� ��� ������ ���� �� �������
    // �������� TargetBeginGame ��� ��������� �� �������
    public void StartGame()
    {
        TargetBeginGame();
    }

    // ����� ��� ������ ���� �� �������
    [TargetRpc]
    void TargetBeginGame()
    {
        Debug.Log($"ID {matchID} | ������");

        //��������� ��� ���������� Player � ������
        Player[] players = FindObjectsOfType<Player>();
        for (int i = 0; i < players.Length; i++)
        {
            DontDestroyOnLoad(players[i]);
        }

        //������������� �������� MainMenuLobby.instance.inGame � true
        MainMenuLobby.instance.inGame = true;
        
        //���������� �������� facingRight � true
        facingRight = true;

        //��������� ��������� �����
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }

    // ����� ��� ��������� ������� ������ �� �������
    // ������� � ������� ��������� � �������� playerTime
    // requiresAuthority = false, ��������� �������� ��� ������� �� ������ �������
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

    // ����� ��� ��������� ����������������� �����
    private void Update()
    {
        if (hasAuthority)
        {
            //��������� ���������� dirX � ����������� �� �������� Input.GetAxisRaw("Horizontal")
            dirX = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

            //����������, ��������� �� ����� �� ����� ��� �� �����
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

            //�������� ������ HandleMovement � HandleJumping ��� ��������� ������������ � �������
            HandleMovement();
            HandleJumping();

            //������������ ��������������� ��������� � ����������� �� ����������� ��������
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

    //����� ���������� ��������� ���������
    private void HandleMovement()
    {
        // ���������� ��������� ����������� ��� ����, � ����� ������ �������� ������ ��������� ��������:
        // - �� ����� ��� � �������
        float acceleration = isGrounded ? basicAcceleration : airAcceleration;
        float deceleration = basicDeceleration;

        if (dirX.x != 0)
        {
            // ���������� ������� �������� �������� ����� ��� x � ����������� �� ����������� ��������
            float targetSpeed = dirX.x * maxSpeed;

            // � ����������� �� ������� � ������� ��������, ��������� ������������� �������� ���������
            // �� ������ ����� � ������� ������������
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, targetSpeed, Time.deltaTime * acceleration), rb.velocity.y);
        }
        else if (isGrounded)
        {
            // ���� �������� ����� �� ����� � ����� �� �����, �� �������������� ��� �� ������� ��������
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, Time.deltaTime * deceleration), rb.velocity.y);
        }
    }

    //����� ���������� ������� ���������
    private void HandleJumping()
    {
        // ����������, ��������� �� ������� ������ "������" � ���� ��, ��� � ��������� �� ���,
        // ����� �������� ��������� �� ����� ��� � �����
        if (Input.GetButtonDown("Jump") && (isGrounded || isSand) && !jumpInProgress)
        {
            // ��������� ���� �����, ����� �������� ���������
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

            // ��������, ��� ������ ���� ������
            jumpInProgress = true;
        }
    }

    //����� ���������� ������� ���������
    private void Flip()
    {
        // ���������, ��� ������� ����� �������� �������� ���� ������� ������
        if (hasAuthority)
        {
            // ������ ����������� �������� ���������, ���� �� ������� ����������� ��������
            facingRight = !facingRight;

            // ������ ������� ���������, ����� �� �������� � ������ �������
            Vector3 Scale = transform.localScale;
            Scale.x *= -1;
            transform.localScale = Scale;

            // ���� �� ��������� � ������� ������, �� ������ ����������� �������� ������ ������������� ��� �������� ������
            if (MainMenuLobby.instance.inGame)
            {
                Vector3 TextScale = NameDisplayText.transform.localScale;
                TextScale.x *= -1;
                NameDisplayText.transform.localScale = TextScale;
            }
        }
    }
}