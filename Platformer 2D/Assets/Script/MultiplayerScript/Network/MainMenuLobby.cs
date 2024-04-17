using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using System;
using System.Security.Cryptography;
using System.Text;

// ����� Match
[System.Serializable]
public class Match
{
    // ���������� ������������� �����
    public string ID;
    // ���� � ���, �������� �� ���� ���������
    public bool PublicMatch;
    // ���� � ���, ��������� �� ����� � �����
    public bool InMatch;
    // ���� � ���, �������� �� ����
    public bool MatchFull;
    // ������ �������, ����������� � �����
    public List<GameObject> players = new List<GameObject>();

    // ����������� ����� � ��������� ���������������, ������� � ������ � ���, �������� �� ���� ���������
    public Match(string ID, GameObject player, bool publicMatch)
    {
        MatchFull = false;
        InMatch = false;
        this.ID = ID;
        PublicMatch = publicMatch;
        players.Add(player);
    }

    // ����������� ��� ����������
    public Match()
    {
        players = new List<GameObject>();
    }
}

// ����� MainMenuLobby, ����������� ����� �������� ����
// �������� ������ ������, ������������ ���������� �������, ������ �� �������� ���� � ��� UI-�����������
public class MainMenuLobby : NetworkBehaviour
{
    // ��������� ������ MainMenuLobby, ������������ ��� ������� � ��� ��������� � ������� �� ������ ��������
    public static MainMenuLobby instance;
    // ���������������� ������ ������
    public readonly SyncList<Match> matches = new SyncList<Match>();
    // ���������������� ������ ���������� ��������������� ������
    public readonly SyncList<string> matchIDs = new SyncList<string>();
    // ������������ ���������� ������� � �����
    public int MaxPlayers;
    // ������ �� �������� ����
    private NetworkManager networkManager;

    // UI-����������
    [Header("MainMenuLobby")]
    // ���� ����� ��� ����� ����� ����� ��� ������
    public InputField JoinInput;
    // ������ ��� ������ ������
    public Button[] Buttons;
    // ������ �����
    public Canvas LobbyCanvas;
    // ������ ������ ������
    public Canvas SearchCanvas;
    // ���� � ���, ���������� �� ����� ������
    private bool searching;

    // UI-���������� ��� ������ � ������ ������
    [Header("Name")]
    // ������ ��� ��������� �����
    public GameObject ChangeNamePanel;
    // ������ �������� ������ ��������� �����
    public Button CloseButton;
    // ������ ��������� �����
    public Button SetNameButton;
    // ���� ����� �����
    public InputField NameInput;
    // ������ ��� �� ������� ������
    public int firstTime = 1;
    // ������������ ��� ������
    [SyncVar] public string DisplayName;

    // ������ ��� ����� �������� ����
    [Header("Lobby")]
    public Transform UIPLayerParent;
    // �������������� ��������� ������ UI-����������� ������
    public GameObject UIPlayerPrefab;
    // ��������� ���� ��� ����������� �������������� �����
    public Text IDText;
    // ������ ������ ����
    public Button BeginGameButton;
    // UI-��������� ������ �������� ������
    public GameObject localPlayerLobbyUI;
    // ���� � ���, ������ �� ����
    public bool inGame;
    // ���� � ���, �������� �� ������� ����� ������ �����
    public bool host;

    // ������ ��� ����������� ������
    [Header("Error")]
    public GameObject ErrorPanel;
    // ��������� ���� ��� ����������� ������ ������
    public Text ErrorText;

    // �����, ���������� ��� ������ �������
    private void Start()
    {
        // ���������� ���������� ������ MainMenuLobby ��� ������� � ��� ��������� � ������� �� ������ ��������
        instance = this;

        // ��������� ��������� ����
        networkManager = FindObjectOfType<NetworkManager>();

        // ��������� ������� ���� ������� ������� �� PlayerPrefs
        firstTime = PlayerPrefs.GetInt("firstTime", 1);

        // ���� �� ������ ��� ������ � PlayerPrefs
        if (!PlayerPrefs.HasKey("Name"))
        {
            // ������� �� ������
            return;
        }

        // ��������� ����� ������ �� PlayerPrefs
        string defaultName = PlayerPrefs.GetString("Name");
        // ��������� ������ � ���� ����� �����
        NameInput.text = defaultName;
        // ��������� ������������� ����� ������
        DisplayName = defaultName;
        // ����� ������ ��������� ����� ������
        SetName(defaultName);
    }

    // �����, ���������� ������ ����
    private void Update()
    {
        // ���� ���� �� ������
        if (!inGame)
        {
            // ��������� ������� �������
            Player[] players = FindObjectsOfType<Player>();

            // ���� ������ ��� ����������� ������
            if (firstTime == 1)
            {
                // ��������� ������ ��������� �����
                ChangeNamePanel.SetActive(true);
                // ����������� ������ �������� ������ ��������� �����
                CloseButton.gameObject.SetActive(false);
            }
            else
            {
                // ��������� ������ �������� ������ ��������� �����
                CloseButton.gameObject.SetActive(true);
            }

            // ���������� �������� �������� ������� ���� ������� ������� � PlayerPrefs
            PlayerPrefs.SetInt("firstTime", firstTime);
        }
    }

    // ����� ��������� �����
    public void SetName(string name)
    {
        // ���� ��������� ��� ������������� �������� ������������� ����� ��� ������� ������ ���, ������ ��������� ����� becomes ���������
        if (name == DisplayName || string.IsNullOrEmpty(name))
        {
            SetNameButton.interactable = false;
        }
        else
        {
            SetNameButton.interactable = true; // � ��������� ������ ������ ��������� ����� becomes �������
        }
    }

    // ����� ���������� �����
    public void SaveName()
    {
        ChangeNamePanel.SetActive(false); // ������ ������ ��������� �����
        JoinInput.interactable = false; // ������ ����� becomes ���������
                                        // ������������� ��� ������
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }
        firstTime = 0; // �������� ������ ���� �������
        DisplayName = NameInput.text; // �������� ������������ ���
        PlayerPrefs.SetString("Name", DisplayName); // ��������� ��� � ���������� ������
        Invoke(nameof(Disconnect), 1f); // ��������� ������ ����� 1 �������
    }

    // ����� ����������
    public void Disconnect()
    {
        if (networkManager.mode == NetworkManagerMode.Host)
        {
            networkManager.StopHost(); // ���������� ����, ���� �� ����������
        }
        else if (networkManager.mode == NetworkManagerMode.ClientOnly)
        {
            networkManager.StopClient(); // ���������� ������, ���� �� ����������
        }
    }

    // ����� ��������� ���������� ������ ������ ����
    public void SetBeginButtonActive(bool active)
    {
        BeginGameButton.interactable = active; // ���������� ���������� ������ ������ ���� � ����������� �� ����������� �������� ��������
    }

    // ����� ����� ����
    public void Host(bool publicHost)
    {
        JoinInput.interactable = false; // ������ ����� becomes ���������
                                        // ������������� ��� ������
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }

        Player.localPlayer.HostGame(publicHost); // ���� ����
    }

    // ����� ��������� ������ ��������� �����
    public void HostSuccess(bool success, string matchID)
    {
        if (success)
        {
            LobbyCanvas.enabled = true; // �������� ����� �����

            if (localPlayerLobbyUI != null)
            {
                Destroy(localPlayerLobbyUI); // ������� ������������ ������ ����������������� ���������� ����� ������
            }

            host = true; // ���������� ���� �����
            localPlayerLobbyUI = SpawnPlayerUIPrefab(Player.localPlayer); // ��������� � ��������� ���������������� ��������� ����� ������
            IDText.text = matchID; // �������� ������������� �����
            BeginGameButton.interactable = true; // ����������� ������ ������ ����
        }
        else
        {
            ErrorPanel.SetActive(true); // ���������� ������ ������
            ErrorText.text = "�� ������� ������� �����"; // ���������� ��������� �� ������
        }
    }

    // ����� ������������� � ����
    public void Join()
    {
        JoinInput.interactable = false; // ������ ����� becomes ���������
                                        // ������������� ��� ������
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false;
        }

        Player.localPlayer.JoinGame(JoinInput.text.ToUpper()); // �������������� � ���� � ��������� ���������������
    }

    // ����� ��������� ������ ��������� �������������
    public void JoinSuccess(bool success, string matchID)
    {
        if (success)
        {
            LobbyCanvas.enabled = true; // �������� ����� �����

            if (localPlayerLobbyUI != null)
            {
                Destroy(localPlayerLobbyUI); // ������� ������������ ������ ����������������� ���������� ����� ������
            }

            host = false; // ���������� ���� ����� �� false
            localPlayerLobbyUI = SpawnPlayerUIPrefab(Player.localPlayer); // ��������� � ��������� ���������������� ��������� ����� ������
            IDText.text = matchID; // �������� ������������� �����
            BeginGameButton.interactable = false; // ������������� ������ ������ ����
        }
        else
        {
            ErrorPanel.SetActive(true); // ���������� ������ ������
            ErrorText.text = "ID �� ������"; // ���������� ��������� �� ������
        }
    }

    // ����� ���������
    public void Enable()
    {
        ErrorPanel.SetActive(false); // ������ ������ ������
                                     // ����������� ��� ������
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true;
        }
        JoinInput.interactable = true; // ����������� ������ �����
    }

    // ����� ���������� �� ����
    public void DisconnectGame()
    {
        if (localPlayerLobbyUI != null)
        {
            Destroy(localPlayerLobbyUI); // ������� ������ ����������������� ���������� ����� ������
        }

        Player.localPlayer.DisconnectGame(); // ����������� �� ����
        LobbyCanvas.enabled = false; // ��������� ����� �����
        JoinInput.interactable = true; // ����������� ������ �����
                                       // ����������� ��� ������
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true;
        }
    }

    // ����� �������� ����
    public bool HostGame(string matchID, GameObject player, bool publicMatch)
    {
        if (!matchIDs.Contains(matchID)) // ���� ��������� ������������� ����� �� ����������
        {
            matchIDs.Add(matchID); // �������� ������������� ����� � ������
            Match match = new Match(matchID, player, publicMatch); // �������� ����� ���� � ��������� ���������������, ������� � ������ ���������� �����
            matches.Add(match); // �������� ���� � ������ ������
            player.GetComponent<Player>().CurrentMatch = match; // ��������� ������ ������� ����
            return true; // ���������� true, ���� ���� ��� ������� ������
        }
        else
        {
            return false; // � ��������� ������ ���������� false
        }
    }

    // ����� ������������� � ����
    public bool JoinGame(string matchID, GameObject player)
    {
        if (matchIDs.Contains(matchID)) // ���� ��������� ������������� ����� ����������
        {
            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].ID == matchID) // ������� ���� � ��������� ���������������
                {
                    if (!matches[i].InMatch && !matches[i].MatchFull) // ���� ���� ��� �� ������� � �� ��������
                    {
                        matches[i].players.Add(player); // �������� ������ � ������ ������� �����
                        player.GetComponent<Player>().CurrentMatch = matches[i]; // ��������� ������ ������� ����
                        matches[i].players[0].GetComponent<Player>().PlayerCountUpdated(matches[i].players.Count); // �������� ���������� ������� � �����
                        if (matches[i].players.Count == MaxPlayers) // ���� ���� ������
                        {
                            matches[i].MatchFull = true; // ���������� ���� ����� �� ������
                        }
                        break;
                    }
                    else
                    {
                        return false; // � ��������� ������ ������� false, ���� ���� ��� ������� ��� ��������
                    }
                }
            }

            return true; // � ��������� ������ ������� true, ���� ����� ������� ������������� � �����
        }
        else
        {
            return false; // � ��������� ������ ������� false, ���� ��������� ������������� ����� �� ����������
        }
    }

    // ����� ������ ����
    public bool SearchGame(GameObject player, out string ID)
    {
        ID = ""; // �������� �������������

        for (int i = 0; i < matches.Count; i++)
        {
            Debug.Log("�������� ID " + matches[i].ID + " | � ���� " + matches[i].InMatch + " | ����� ������ " + matches[i].MatchFull + " | ��������� ����� " + matches[i].PublicMatch);
            if (!matches[i].InMatch && !matches[i].MatchFull && matches[i].PublicMatch)
            {
                if (JoinGame(matches[i].ID, player)) // �������������� � ���� � ��������� ���������������
                {
                    ID = matches[i].ID; // ��������� ������������� �����
                    return true; // ������� true, ���� ������� ��������� ����
                }
            }
        }

        return false; // � ��������� ������ ������� false, ���� �� ������� ��������� ����
    }

    // ����� �������� ���������� ��������������
    public static string GetRandomID()
    {
        string ID = string.Empty;
        for (int i = 0; i < 5; i++)
        {
            int rand = UnityEngine.Random.Range(0, 36);
            if (rand < 26)
            {
                ID += (char)(rand + 65); // �������� ��������� �������������, ��������� ��������� ����� � �����
            }
            else
            {
                ID += (rand - 26).ToString();
            }
        }
        return ID;
    }

    // ����� �������� ������� ����������������� ���������� ������
    public GameObject SpawnPlayerUIPrefab(Player player)
    {
        GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPLayerParent); // ��������� ����� ������ ����������������� ���������� ������
        newUIPlayer.GetComponent<PlayerUI>().SetPlayer(player.PlayerDisplayName); // ��������� ��� ������ ��� ����������������� ���������� ������

        return newUIPlayer;
    }

    // ����� ������ ����
    public void StartGame()
    {
        Player.localPlayer.BeginGame(); // ������� ����
    }

    // ����� ������ ����
    public void SearchGame()
    {
        StartCoroutine(Searching()); // ��������� �������� ������ ����
    }

    // ����� ������ ������ ����
    public void CancelSearchGame()
    {
        JoinInput.interactable = true; // ����������� ������ �����
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = true; // ����������� ��� ������
        }

        searching = false; // �������� ����� ����
    }

    // ����� ��������� ������ ����
    public void SearchGameSuccess(bool success, string ID)
    {
        if (success)
        {
            SearchCanvas.enabled = false; // ��������� ����� ������ ����
            searching = false; // �������� ����� ����
            JoinSuccess(success, ID); // �������� ����� ��������� ������������� � ����
        }
    }

    // ����� ������ ����
public void BeginGame(string matchID)
{
    for (int i = 0; i < matches.Count; i++)
    {
        if (matches[i].ID == matchID)
        {
            matches[i].InMatch = true; // ���������� ���� ����� �� true, ����� ������ ����
            foreach (var player in matches[i].players)
            {
                player.GetComponent<Player>().StartGame(); // ������� ���� ��� ������� ������ � �����
            }
            break;
        }
    }
}

// ����� ��������� ���������� ������
public void PlayerDisconnected(GameObject player, string ID)
{
    for (int i = 0; i < matches.Count; i++)
    {
        if (matches[i].ID == ID)
        {
            int playerIndex = matches[i].players.IndexOf(player);
            if (matches[i].players.Count > playerIndex)
            {
                matches[i].players.RemoveAt(playerIndex); // ������� ������������ ������ �� ������ �������
            }

            if (matches[i].players.Count == 0)
            {
                matches.RemoveAt(i); // ������� ���� ����, ���� �� �������� �������
                matchIDs.Remove(ID);
            }
            else
            {
                matches[i].players[0].GetComponent<Player>().PlayerCountUpdated(matches[i].players.Count); // �������� ���������� ������� � �����
            }
            break;
        }
    }
}

    // ����� ������ �� ����
    public void Exit()
    {
        Disconnect(); // ����������� �� �������
        Application.Quit(); // �������� ����������
    }

    // �������� ������ ����
    IEnumerator Searching()
    {
        JoinInput.interactable = false; // ������������� ������ �����
        for (int i = 0; i < Buttons.Length; i++)
        {
            Buttons[i].interactable = false; // ������������� ��� ������
        }
        SearchCanvas.enabled = true; // �������� ����� ������ ����
        searching = true; // ���������� ���� ������ ���� �� true

        float searchInterval = 1; // �������� ������ ���� (� ��������)
        float currentTime = 1; // ������� �����

        while (searching)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime; // ��������� ������� ����� ����� ��������� �� ���� ������� ���������� � ������� ���������� �����
            }
            else
            {
                currentTime = searchInterval; // �������� ������� �����
                Player.localPlayer.SearchGame(); // ��������� ����� ����
            }
            yield return null; // ����� ���������� �����
        }

        SearchCanvas.enabled = false; // ��������� ����� ������ ����
    }
}

// ����������� ����� ��� ���������� ������ String
public static class MatchExtension
{
    // ����� ���������� ToGuid, ������� ������������ ������ � Guid ��� ������ ����������� � ������� ��������� MD5
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hasBytes = provider.ComputeHash(inputBytes);

        return new Guid(hasBytes);
    }
}