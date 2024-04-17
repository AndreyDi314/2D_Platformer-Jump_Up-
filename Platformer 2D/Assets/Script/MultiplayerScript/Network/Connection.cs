using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// ����� Connection �������� �� ����������� ������� � �������.
public class Connection : MonoBehaviour
{
    // ������ NetworkManager, ������� ��������� �������� ������������.
    public NetworkManager networkManager;

    // ����� Start() ���������� ���� ��� � ������ ����.
    private void Start()
    {
        // ���������, ����������� �� ���� � �������� ������ (��� ������������ ����������).
        if (!Application.isBatchMode)
        {
            // ���� ���� �� ����������� � �������� ������, ������������ � �������.
            networkManager.StartClient();
        }
    }

    // ����� JoinClient() ���������� ��� ������� �� ������ � ���������� ������������.
    public void JoinClient()
    {
        // ������������� ����� ������� (� ������ ������, ������ ������� �� ��������� ����������).
        networkManager.networkAddress = "localhost";
        // ������������ � �������.
        networkManager.StartClient();
    }
}