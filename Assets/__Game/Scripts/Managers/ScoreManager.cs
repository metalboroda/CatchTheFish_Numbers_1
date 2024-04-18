using Assets.__Game.Scripts.EventBus;
using UnityEngine;

namespace Assets.__Game.Scripts.Managers
{
  public class ScoreManager : MonoBehaviour
  {
    [SerializeField] public int[] _correctNumbers;

    private EventBinding<EventStructs.FishClickEvent> _fishEvent;

    private void OnEnable()
    {
      _fishEvent = new EventBinding<EventStructs.FishClickEvent>(ReceiveFish);
    }

    private void OnDisable()
    {
      _fishEvent.Remove(ReceiveFish);
    }

    private void ReceiveFish(EventStructs.FishClickEvent fishClickEvent)
    {
      foreach (var number in _correctNumbers)
      {
        if (number == fishClickEvent.FishNumber)
        {
          fishClickEvent.FishHandler.DestroyFish();

          break;
        }
      }
    }
  }
}