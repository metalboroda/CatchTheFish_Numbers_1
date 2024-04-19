using System.Collections;
using System.Collections.Generic;
using Assets.__Game.Scripts.Enums;
using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using DG.Tweening;
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
    [SerializeField] public GameObject _gameStarIcon;
    [SerializeField] public float _gameStarScaleIn = 1.15f;
    [SerializeField] public float _gameStarAnimDuration = 0.25f;

    [Header("Win Canvas")]
    [SerializeField] private GameObject _winCanvas;
    [SerializeField] private Button _winNextLevelBtn;

    [Header("Lose Canvas")]
    [SerializeField] private GameObject _loseCanvas;
    [SerializeField] private Button _loseRestartBtn;

    private List<GameObject> _canvases = new();
    private int _currentScore;
    private int _overallScore;

    private GameBootstrapper _gameBootstrapper;

    private EventBinding<EventStructs.SendComponentEvent<GameBootstrapper>> _componentEvent;
    private EventBinding<EventStructs.StateChanged> _stateChanged;
    private EventBinding<EventStructs.FishSpawnerEvent> _fishSpawnerEvent;
    private EventBinding<EventStructs.FishReceivedEvent> _fishReceivedEvent;

    private void OnEnable()
    {
      _componentEvent = new EventBinding<EventStructs.SendComponentEvent<GameBootstrapper>>(SetBootstrapper);
      _stateChanged = new EventBinding<EventStructs.StateChanged>(SwitchCanvasesDependsOnState);
      _fishSpawnerEvent = new EventBinding<EventStructs.FishSpawnerEvent>(SetOverallScore);
      _fishReceivedEvent = new EventBinding<EventStructs.FishReceivedEvent>(DisplayScore);
      _fishReceivedEvent = new EventBinding<EventStructs.FishReceivedEvent>(DisplayCorrectNumbersArray);
      _fishReceivedEvent = new EventBinding<EventStructs.FishReceivedEvent>(StarIconScaleAnimation);
    }

    private void OnDisable()
    {
      _componentEvent.Remove(SetBootstrapper);
      _stateChanged.Remove(SwitchCanvasesDependsOnState);
      _fishSpawnerEvent.Remove(SetOverallScore);
      _fishReceivedEvent.Remove(DisplayScore);
      _fishReceivedEvent.Remove(DisplayCorrectNumbersArray);
      _fishReceivedEvent.Remove(StarIconScaleAnimation);
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

      _winNextLevelBtn.onClick.AddListener(() =>
      {
        EventBus<EventStructs.UiButtonEvent>.Raise(new EventStructs.UiButtonEvent
        {
          UiEnums = UiEnums.WinNextLevelButton
        });

        _gameBootstrapper.RestartLevel();
      });

      _loseRestartBtn.onClick.AddListener(() => { _gameBootstrapper.RestartLevel(); });
    }

    private void AddCanvasesToList()
    {
      _canvases.Add(_questCanvas);
      _canvases.Add(_gameCanvas);
      _canvases.Add(_winCanvas);
      _canvases.Add(_loseCanvas);
    }

    private void SetBootstrapper(EventStructs.SendComponentEvent<GameBootstrapper> componentEvent)
    {
      _gameBootstrapper = componentEvent.Data;
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

    private void StarIconScaleAnimation(EventStructs.FishReceivedEvent fishReceivedEvent)
    {
      if (fishReceivedEvent.CorrectFish == false) return;

      Sequence seq = DOTween.Sequence();

      seq.Append(_gameStarIcon.transform.DOScale(_gameStarScaleIn, _gameStarAnimDuration));
      seq.Append(_gameStarIcon.transform.DOScale(1f, _gameStarAnimDuration));
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
        case GameWinState:
          SwitchCanvas(_winCanvas);
          break;
        case GameLoseState:
          SwitchCanvas(_loseCanvas);
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