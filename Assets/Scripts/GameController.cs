using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
  public static GameController instance;

  public GameObject gameOver;

  private void Start()
  {
    instance = this;
  }

  public void ShowGameOver()
  {
    gameOver.SetActive(true);
  }

  public void RestartLevel()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
