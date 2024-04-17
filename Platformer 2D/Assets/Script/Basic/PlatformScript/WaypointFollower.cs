using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����� WaypointFollower �������� �� ������������ ������� ����� waypoint-���.
public class WaypointFollower : MonoBehaviour
{
    // ������ waypoint-��, �� ������� ������ ����� ������������.
    [SerializeField] private GameObject[] waypoints;
    // ������ �������� waypoint-�.
    private int currentWaypointIndex = 0;

    // �������� ������������ �������.
    [SerializeField] private float speed = 2f;

    // ����� Update() ���������� ������ ����.
    private void Update()
    {
        // �������� �� ��, ��������� �� ������ ������ �������� waypoint-�.
        if (IsWithinDistance(waypoints[currentWaypointIndex].transform.position, transform.position, 0.1f))
        {
            // ���� ������ ������ �������� waypoint-�, ��������� � ���������� waypoint-�.
            currentWaypointIndex++;
            // ���� ������ �������� waypoint-� �������� ������������ ������, ��������� � ������� waypoint-�.
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        // ����������� ������ � �������� waypoint-� �� ���������, �������� � ���� "��������".
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
    }

    // ����� IsWithinDistance() ���������, ��������� �� ������ �� �������� ���������� �� ����.
    private bool IsWithinDistance(Vector2 a, Vector2 b, float distance)
    {
        // ��������� ������� ���������� ����� �������� � waypoint-��.
        float distanceSqr = distance * distance;
        // ���� ������� ���������� ����� �������� � waypoint-�� ������ ��������� �������� ����������, ������, ������ ��������� ������ waypoint-�.
        return Vector2.SqrMagnitude(a - b) < distanceSqr;
    }
}