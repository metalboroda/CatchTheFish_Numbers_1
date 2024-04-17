using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.__Game.Scripts.Infrastructure
{
  public class SceneLoader
  {
    public void LoadSceneAsync(string sceneName, Action callback)
    {
      SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single).completed += (AsyncOperation asyncOp) =>
      {
        callback?.Invoke();
      };
    }

    public void LoadScene(string sceneName)
    {
      SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void RestartScene()
    {
      SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
  }
}