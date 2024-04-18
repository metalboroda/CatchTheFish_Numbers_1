using Assets.__Game.Scripts.EventBus;
using TMPro;
using UnityEngine;
using static Assets.__Game.Scripts.EventBus.EventStructs;

namespace Assets.__Game.Scripts.Fish
{
  public class FishUiHandler : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI[] _numberTexts;

    private EventBinding<FishEvent> _fishEventBinding;

    private void OnEnable()
    {
      _fishEventBinding = new EventBinding<FishEvent>(OnFishNumberReceived);
    }

    private void OnDisable()
    {
      _fishEventBinding.Remove(OnFishNumberReceived);
    }

    private void OnFishNumberReceived(FishEvent fishEvent)
    {
      if (fishEvent.FishId != transform.GetInstanceID()) return;

      foreach (var text in _numberTexts)
      {
        text.SetText(fishEvent.FishNumber.ToString());
      }
    }
  }
}