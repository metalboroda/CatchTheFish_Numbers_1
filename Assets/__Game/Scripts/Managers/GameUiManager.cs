using System.Collections;
using System.Collections.Generic;
using Assets.__Game.Scripts.Enums;
using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.Game.States;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.__Game.Scripts.Managers
{
  public class GameUiManager : MonoBehaviour
  {
    [Header("Quest Canvas")]
    [SerializeField] private GameObject _questCanvas;
    [SerializeField] private TextMeshProUGUI _questCorrectNumbersTxt;
    [SerializeField] private Button _questPlayButton;

    [Header("Game Canvas")]
    [SerializeField] private GameObject _gameCanvas;
    [SerializeField] private TextMeshProUGUI _gameScoreCounterTxt;

    private List<GameObject> _canvases = new();
    private int _currentScore;
    private int _overallScore;

    private EventBinding<EventStructs.StateChanged> _stateChanged;
    private EventBinding<EventStructs.FishSpawnerEvent> _fishSpawnerEvent;
    private EventBinding<EventStructs.FishReceivedEvent> _fishReceivedEvent;

    private void OnEnable()
    {
      _stateChanged = new EventBinding<EventStructs.StateChanged>(SwitchCanvasesDependsOnState);
      _fishSpawnerEvent = new EventBinding<EventStructs.FishSpawnerEvent>(SetOverallScore);
      _fishReceivedEvent = new EventBinding<EventStructs.FishReceivedEvent>(DisplayScore);
      _fishReceivedEvent = new EventBinding<EventStructs.FishReceivedEvent>(DisplayCorrectNumbersArray);
    }

    private void OnDisable()
    {
      _stateChanged.Remove(SwitchCanvasesDependsOnState);
      _fishSpawnerEvent.Remove(SetOverallScore);
      _fishReceivedEvent.Remove(DisplayScore);
      _fishReceivedEvent.Remove(DisplayCorrectNumbersArray);
    }

    private void Start()
    {
      SubscribeButtons();
      AddCanvasesToList();
    }

    private void SubscribeButtons()
    {
      _questPlayButton.onClick.AddListener(() =>
      {
        EventBus<EventStructs.UiButtonEvent>.Raise(new EventStructs.UiButtonEvent
        {
          UiEnums = UiEnums.QuestPlayButton
        });
      });
    }

    private void AddCanvasesToList()
    {
      _canvases.Add(_questCanvas);
      _canvases.Add(_gameCanvas);
    }

    private void SetOverallScore(EventStructs.FishSpawnerEvent fishSpawnerEvent)
    {
      _overallScore = fishSpawnerEvent.CorrectFishCount;
      _gameScoreCounterTxt.text = $"{_currentScore} / {_overallScore}";
    }

    private void DisplayScore(EventStructs.FishReceivedEvent fishReceivedEvent)
    {
      _currentScore += fishReceivedEvent.CorrectFishIncrement;
      _gameScoreCounterTxt.text = $"{_currentScore} / {_overallScore}";
    }

    private void DisplayCorrectNumbersArray(EventStructs.FishReceivedEvent fishReceivedEvent)
    {
      if (fishReceivedEvent.CorrectNumbers == null) return;

      string arrayString = "";

      for (int i = 0; i < fishReceivedEvent.CorrectNumbers.Length; i++)
      {
        arrayString += fishReceivedEvent.CorrectNumbers[i].ToString();

        if (i < fishReceivedEvent.CorrectNumbers.Length - 1)
          arrayString += " ";
      }

      _questCorrectNumbersTxt.text = arrayString;
    }

    private void SwitchCanvasesDependsOnState(EventStructs.StateChanged state)
    {
      switch (state.State)
      {
        case GameQuestState:
          SwitchCanvas(_questCanvas);
          break;
        case GameplayState:
          SwitchCanvas(_gameCanvas);
          break;
      }
    }

    private void SwitchCanvas(GameObject canvas, float delay = 0)
    {
      StartCoroutine(DoSwitchCanvas(canvas, delay));
    }

    private IEnumerator DoSwitchCanvas(GameObject canvas, float delay)
    {
      yield return new WaitForSeconds(delay);

      foreach (var canvasItem in _canvases)
      {
        if (canvasItem == canvas)
          canvas.SetActive(true);
        else
          canvasItem.SetActive(false);
      }
    }
  }
}