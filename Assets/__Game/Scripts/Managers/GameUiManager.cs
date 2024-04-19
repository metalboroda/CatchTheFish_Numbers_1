using Assets.__Game.Scripts.EventBus;
using TMPro;
using UnityEngine;

namespace Assets.__Game.Scripts.Managers
{
  public class GameUiManager : MonoBehaviour
  {
    [Header("Score")]
    [SerializeField] private TextMeshProUGUI _scoreCounterTxt;

    private int _currentScore;
    private int _overallScore;

    private EventBinding<EventStructs.FishSpawnerEvent> _fishSpawnerEvent;
    private EventBinding<EventStructs.FishReceivedEvent> _fishReceivedEvent;

    private void OnEnable()
    {
      _fishSpawnerEvent = new EventBinding<EventStructs.FishSpawnerEvent>(SetOverallScore);
      _fishReceivedEvent = new EventBinding<EventStructs.FishReceivedEvent>(DisplayScore);
    }

    private void OnDisable()
    {
      _fishSpawnerEvent.Remove(SetOverallScore);
    }

    private void SetOverallScore(EventStructs.FishSpawnerEvent fishSpawnerEvent)
    {
      _overallScore = fishSpawnerEvent.CorrectFishCount;
      _scoreCounterTxt.text = $"{_currentScore} / {_overallScore}";
    }

    private void DisplayScore(EventStructs.FishReceivedEvent fishReceivedEvent)
    {
      _currentScore += fishReceivedEvent.CorrectFishIncrement;
      _scoreCounterTxt.text = $"{_currentScore} / {_overallScore}";
    }
  }
}