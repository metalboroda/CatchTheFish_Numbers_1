using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.__Game.Scripts.Managers
{
  public class LevelManager : MonoBehaviour
  {
    [SerializeField] public List<Level> _levels;

    private int _currentLevelIndex = 0;

    void Start()
    {
      LoadLevel(_currentLevelIndex);
    }

    async void LoadLevel(int levelIndex)
    {
      if (levelIndex < _levels.Count)
      {
        Level level = _levels[levelIndex];
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(level.LevelAddressableKey);

        await handle.Task;

        GameObject levelPrefab = handle.Result;

        Instantiate(levelPrefab);
      }
      else
      {
        Debug.LogWarning("No more _levels to load.");
      }
    }

    public void LoadNextLevel()
    {
      _currentLevelIndex++;

      LoadLevel(_currentLevelIndex);
    }
  }
}