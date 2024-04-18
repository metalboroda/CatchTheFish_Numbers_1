using Assets.__Game.Scripts.EventBus;
using UnityEngine;

namespace Assets.__Game.Scripts.Fish
{
  public class FishHandler : MonoBehaviour
  {
    public void SetFishNumber(int fishNumber)
    {
      EventBus<EventStructs.FishEvent>.Raise(new EventStructs.FishEvent
      {
        FishId = transform.GetInstanceID(),
        FishNumber = fishNumber
      });
    }
  }
}