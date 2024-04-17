using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Класс WaypointFollower отвечает за передвижение объекта между waypoint-ами.
public class WaypointFollower : MonoBehaviour
{
    // Массив waypoint-ов, по которым объект будет перемещаться.
    [SerializeField] private GameObject[] waypoints;
    // Индекс текущего waypoint-а.
    private int currentWaypointIndex = 0;

    // Скорость передвижения объекта.
    [SerializeField] private float speed = 2f;

    // Метод Update() вызывается каждый кадр.
    private void Update()
    {
        // Проверка на то, находится ли объект вблизи текущего waypoint-а.
        if (IsWithinDistance(waypoints[currentWaypointIndex].transform.position, transform.position, 0.1f))
        {
            // Если объект вблизи текущего waypoint-а, переходим к следующему waypoint-у.
            currentWaypointIndex++;
            // Если индекс текущего waypoint-а превысил максимальный индекс, переходим к первому waypoint-у.
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        // Передвигаем объект к текущему waypoint-у со скоростью, заданной в поле "Скорость".
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
    }

    // Метод IsWithinDistance() проверяет, находится ли объект на заданном расстоянии от цели.
    private bool IsWithinDistance(Vector2 a, Vector2 b, float distance)
    {
        // Вычисляем квадрат расстояния между объектом и waypoint-ом.
        float distanceSqr = distance * distance;
        // Если квадрат расстояния между объектом и waypoint-ом меньше заданного квадрата расстояния, значит, объект находится вблизи waypoint-а.
        return Vector2.SqrMagnitude(a - b) < distanceSqr;
    }
}