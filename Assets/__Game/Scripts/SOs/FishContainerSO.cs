using System.Collections.Generic;
using UnityEngine;

namespace Assets.__Game.Scripts.SOs
{
  [CreateAssetMenu(fileName = "FishContainer", menuName = "Fish/FishContainer")]
  public class FishContainerSO : ScriptableObject
  {
    [SerializeField] private List<GameObject> fishList = new();

    public List<GameObject> FishList
    {
      get { return fishList; }
      private set { fishList = value; }
    }
  }
}