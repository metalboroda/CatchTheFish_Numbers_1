using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.Fish;
using System.Collections.Generic;
using Assets.__Game.Scripts.SOs;
using UnityEngine;

namespace Assets.__Game.Scripts.Managers
{
  public class FishManager : MonoBehaviour
  {
    [SerializeField] private CorrectNumbersContainerSo _correctNumbersContainerSo;

    private List<FishHandler> _correctNumberFish = new();
    private List<FishHandler> _incorrectNumberFish = new();

    private EventBinding<EventStructs.FishSpawnerEvent> _fishSpawnerEvent;
    private EventBinding<EventStructs.FishClickEvent> _fishEvent;

    private void OnEnable()
    {
      _fishSpawnerEvent = new EventBinding<EventStructs.FishSpawnerEvent>(AddFishesToList);
      _fishEvent = new EventBinding<EventStructs.FishClickEvent>(ReceiveFish);
    }

    private void OnDisable()
    {
      _fishSpawnerEvent.Remove(AddFishesToList);
      _fishEvent.Remove(ReceiveFish);
    }

    private void AddFishesToList(EventStructs.FishSpawnerEvent fishSpawnerEvent)
    {
      _correctNumberFish.AddRange(fishSpawnerEvent.CorrectFishHandlers);
      _incorrectNumberFish.AddRange(fishSpawnerEvent.IncorrectFinishHandlers);
    }

    private void ReceiveFish(EventStructs.FishClickEvent fishClickEvent)
    {
      foreach (var number in _correctNumbersContainerSo.CorrectNumbers)
      {
        if (number == fishClickEvent.FishNumber)
        {
          fishClickEvent.FishHandler.DestroyFish(true);

          break;
        }

        if (number != fishClickEvent.FishNumber)
        {
          fishClickEvent.FishHandler.DestroyFish(false);

          break;
        }
      }
    }
  }
}