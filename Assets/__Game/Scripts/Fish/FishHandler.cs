using Assets.__Game.Scripts.EventBus;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.__Game.Scripts.Fish
{
  public class FishHandler : MonoBehaviour, IPointerClickHandler
  {
    private int _fishNumber;

    public int FishNumber
    {
      get => _fishNumber;
      private set => _fishNumber = value;
    }

    public void SetFishNumber(int fishNumber, bool correct, bool tutorial = false)
    {
      _fishNumber = fishNumber;

      EventBus<EventStructs.FishUiEvent>.Raise(new EventStructs.FishUiEvent
      {
        FishId = transform.GetInstanceID(),
        FishNumber = _fishNumber,
        Correct = correct,
        Tutorial = tutorial
      });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
      EventBus<EventStructs.FishClickEvent>.Raise(new EventStructs.FishClickEvent
      {
        FishHandler = this,
        FishNumber = _fishNumber
      });
    }

    public void DestroyFish(bool correct)
    {
      EventBus<EventStructs.FishDestroyEvent>.Raise(new EventStructs.FishDestroyEvent
      {
        FishId = transform.GetInstanceID(),
        Correct = correct
      });

      Destroy(gameObject);
    }
  }
}