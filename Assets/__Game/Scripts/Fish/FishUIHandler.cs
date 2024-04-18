using Assets.__Game.Scripts.EventBus;
using TMPro;
using UnityEngine;

namespace Assets.__Game.Scripts.Fish
{
  public class FishUiHandler : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI[] _numberTexts;

    private EventBinding<EventStructs.FishUiEvent> _fishEventBinding;

    private void OnEnable()
    {
      _fishEventBinding = new EventBinding<EventStructs.FishUiEvent>(OnFishNumberReceived);
    }

    private void OnDisable()
    {
      _fishEventBinding.Remove(OnFishNumberReceived);
    }

    private void OnFishNumberReceived(EventStructs.FishUiEvent fishUiEvent)
    {
      if (fishUiEvent.FishId != transform.GetInstanceID()) return;

      foreach (var text in _numberTexts)
      {
        text.SetText(fishUiEvent.FishNumber.ToString());
      }
    }
  }
}