using UnityEngine;
using UnityEngine.SceneManagement;

// ������ �������� �� ����������� �������� ��� �������� ������������ �����.
public class DisableObjects : MonoBehaviour
{
    // ������ ��������, ������� ����� �������������� ��� �������� �����.
    public GameObject[] objectsToDisable;

    // ���� ����� ���������� ����� ����� �������� �����.
    private void Start()
    {
        // ������������� �� ������� �������� �����.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ���� ����� ���������� ������ ���, ����� ����������� ����� �����.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ���������, ����������� �� ����� � ���������� ������ � �������� �� ��� ������� ������ � build-�.
        if (mode == LoadSceneMode.Additive && scene.buildIndex == 2)
        {
            // ���������� �� ���� �������� � ������� objectsToDisable.
            for (int i = 0; i < objectsToDisable.Length; i++)
            {
                // ���� ������ �� ����� null, ������������ ���.
                if (objectsToDisable[i] != null)
                {
                    objectsToDisable[i].SetActive(false);
                }
            }
        }

        // ������������ �� ������� �������� �����, ����� ����� OnSceneLoaded �� ��������� ��������.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}