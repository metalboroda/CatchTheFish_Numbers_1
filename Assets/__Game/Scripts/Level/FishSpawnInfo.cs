using Assets.__Game.Scripts.SOs;
using UnityEngine;

namespace Assets.__Game.Scripts.Level
{
  [System.Serializable]
  public class FishSpawnInfo
  {
    public FishContainerSo FishContainerSo;

    [Space]
    public int FishNumber;
    public int Amount;
  }
}