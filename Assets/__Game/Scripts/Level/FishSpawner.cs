using Assets.__Game.Scripts.SOs;
using Assets.__Game.Scripts.Tools;
using UnityEngine;

namespace Assets.__Game.Scripts.Level
{
  public class FishSpawner : MonoBehaviour
  {
    [SerializeField] private FishContainerSO _fishContainer;

    [Header("Spawn Param's")]
    [SerializeField] private int _spawnAmount;

    private RandomPointInCamera _randomPointInCamera;

    private void Awake()
    {
      _randomPointInCamera = new RandomPointInCamera(Camera.main);
    }
  }
}