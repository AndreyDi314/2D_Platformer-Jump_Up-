using UnityEngine;
using UnityEngine.UI;

// ������ �������� �� ����� � ������� ������ � ����������� ����������.
public class ControlsPanelSetActive : MonoBehaviour
{
    // ������, ������� ����� ���������� � ��������.
    public GameObject panelObject;

    // ����������, ������� ������ ������� ��������� ������: ������� ��� ��� ���.
    private bool isActive = true;

    // ���� ����� ������ ��������� ������ �� ���������������.
    public void ControlsPanelActive()
    {
        // ������ �������� ���������� isActive �� ���������������.
        isActive = !isActive;

        // ������������� ��������� ������ � ����������� �� �������� isActive.
        panelObject.SetActive(isActive);
    }
}