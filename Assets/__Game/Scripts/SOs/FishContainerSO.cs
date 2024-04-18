using System.Collections.Generic;
using UnityEngine;

namespace Assets.__Game.Scripts.SOs
{
  [CreateAssetMenu(fileName = "FishContainer", menuName = "Fish/FishContainer")]
  public class FishContainerSo : ScriptableObject
  {
    [SerializeField] private List<GameObject> fishList = new();

    public List<GameObject> FishList
    {
      get => fishList;
      private set => fishList = value;
    }

    public GameObject GetRandomFish()
    {
      if (fishList == null || fishList.Count == 0) return null;

      int randomIndex = Random.Range(0, fishList.Count);

      return fishList[randomIndex];
    }
  }
}