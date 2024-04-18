using Assets.__Game.Scripts.EventBus;
using UnityEngine;

namespace Assets.__Game.Scripts.Fish
{
  public class FishHandler : MonoBehaviour
  {
    [SerializeField] private int _number = 0;

    private void Start()
    {
      SetFishNumber(_number);
    }

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