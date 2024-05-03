using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.Fish;
using Assets.__Game.Scripts.Tools;
using Assets.__Game.Scripts.SOs;

namespace Assets.__Game.Scripts.LevelItems
{
  public class FishSpawner : MonoBehaviour
  {
    [SerializeField] private float _minFishMovementSpeed;
    [SerializeField] private float _maxFishMovementSpeed;

    [Space]
    [SerializeField] private CorrectNumbersContainerSo _correctNumbersContainerSo;

    [Space]
    [SerializeField] private FishSpawnInfo[] _fishToSpawn;

    private List<FishHandler> _correctNumbersFishHandlers = new List<FishHandler>();
    private List<FishHandler> _incorrectNumbersFishHandlers = new List<FishHandler>();

    private RandomPointInCamera _randomPointInCamera;

    private void Awake()
    {
      _randomPointInCamera = new RandomPointInCamera(Camera.main);
    }

    private void Start()
    {
      StartCoroutine(DoSpawnFishRoutine());
    }

    private IEnumerator DoSpawnFishRoutine()
    {
      float randSpeed = Random.Range(_minFishMovementSpeed, _maxFishMovementSpeed);

      foreach (var fishInfo in _fishToSpawn)
      {
        for (int i = 0; i < fishInfo.Amount; i++)
        {
          Vector3 point = _randomPointInCamera.GetRandomPointInCamera();
          Vector3 spawnPosition = new Vector3(point.x, point.y, 0);
          Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0, 2) == 0 ? 90f : -90f, 0f);

          GameObject fishPrefab = fishInfo.FishContainerSo.GetRandomFish();
          GameObject spawnedFish = Instantiate(fishPrefab, spawnPosition, randomRotation);
          FishHandler fishHandler = spawnedFish.GetComponent<FishHandler>();
          FishMovement fishMovement = spawnedFish.GetComponent<FishMovement>();

          fishHandler.SetFishNumber(fishInfo.FishNumber);
          fishMovement.SetParameters(randSpeed);

          if (ArrayContains(_correctNumbersContainerSo.CorrectNumbers, fishInfo.FishNumber))
            _correctNumbersFishHandlers.Add(fishHandler);
          else
            _incorrectNumbersFishHandlers.Add(fishHandler);

          yield return null;
        }
      }

      EventBus<EventStructs.FishSpawnerEvent>.Raise(new EventStructs.FishSpawnerEvent
      {
        CorrectFishHandlers = _correctNumbersFishHandlers,
        CorrectFishCount = _correctNumbersFishHandlers.Count,
        IncorrectFinishHandlers = _incorrectNumbersFishHandlers,
        IncorrectFinishCount = _incorrectNumbersFishHandlers.Count
      });
    }

    private bool ArrayContains(int[] array, int number)
    {
      foreach (int num in array)
      {
        if (num == number) return true;
      }

      return false;
    }
  }
}