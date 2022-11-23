using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFollowerController : MonoBehaviour
{
  [SerializeField] private GameObject[] waypoints;
  private int currentWaypointIndex = 0;

  [SerializeField] private float speed = 2f;

  private void Update()
  {
    Move();
  }

  public void Move()
  {
    if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < .1f)
    {
      currentWaypointIndex++;
      if (currentWaypointIndex >= waypoints.Length)
      {
        currentWaypointIndex = 0;
      }
    }
    transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, Time.deltaTime * speed);
  }

  private void OnTriggerEnter2D(Collider2D collider)
  {
    if (collider.gameObject.CompareTag("Player"))
    {
      collider.gameObject.transform.SetParent(transform);
    }
  }

  private void OnTriggerExit2D(Collider2D collider)
  {
    if (collider.gameObject.CompareTag("Player"))
    {
      collider.gameObject.transform.SetParent(null);
    }
  }
}
