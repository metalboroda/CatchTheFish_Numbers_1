using System.Collections;
using System.Collections.Generic;
using Assets.__Game.Scripts.Enums;
using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using Assets.__Game.Scripts.LevelItems;
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
    [SerializeField] private GameObject _gameStarIcon;

    [Space]
    [SerializeField] private TextMeshProUGUI _gameLoseCounterTxt;
    [SerializeField] private GameObject _gameAngryFaceIcon;

    [Space]
    [SerializeField] private float _gameIconScaleIn = 1.3f;
    [SerializeField] private float _gameIconAnimDuration = 0.15f;

    [Header("Win Canvas")]
    [SerializeField] private GameObject _winCanvas;
    [SerializeField] private Button _winNextLevelBtn;
    [SerializeField] private Button _winRewardButton;

    [Header("Lose Canvas")]
    [SerializeField] private GameObject _loseCanvas;
    [SerializeField] private Button _loseRestartBtn;

    private List<GameObject> _canvases = new();
    private int _currentScore;
    private int _overallScore;
    private int _currentLoses;

    private GameBootstrapper _gameBootstrapper;
    private Reward _reward;

    private EventBinding<EventStructs.SendComponentEvent<GameBootstrapper>> _componentEvent;
    private EventBinding<EventStructs.StateChanged> _stateChanged;
    private EventBinding<EventStructs.FishSpawnerEvent> _fishSpawnerEvent;
    private EventBinding<EventStructs.FishReceivedEvent> _fishReceivedEvent;

    private void Awake()
    {
      _reward = new Reward();
    }

    private void OnEnable()
    {
      _componentEvent = new EventBinding<EventStructs.SendComponentEvent<GameBootstrapper>>(SetBootstrapper);
      _stateChanged = new EventBinding<EventStructs.StateChanged>(SwitchCanvasesDependsOnState);
      _fishSpawnerEvent = new EventBinding<EventStructs.FishSpawnerEvent>(SetOverallScore);
      _fishReceivedEvent = new EventBinding<EventStructs.FishReceivedEvent>(DisplayScore);
      _fishReceivedEvent = new EventBinding<EventStructs.FishReceivedEvent>(DisplayCorrectNumbersArray);
      _fishReceivedEvent = new EventBinding<EventStructs.FishReceivedEvent>(IconScaleAnimation);
    }

    private void OnDisable()
    {
      _componentEvent.Remove(SetBootstrapper);
      _stateChanged.Remove(SwitchCanvasesDependsOnState);
      _fishSpawnerEvent.Remove(SetOverallScore);
      _fishReceivedEvent.Remove(DisplayScore);
      _fishReceivedEvent.Remove(DisplayCorrectNumbersArray);
      _fishReceivedEvent.Remove(IconScaleAnimation);
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
      _winRewardButton.onClick.AddListener(() => { _reward.OpenRandomWikipediaFishLink(); });

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
      if (fishReceivedEvent.CorrectFish == true)
      {
        _currentScore += fishReceivedEvent.CorrectFishIncrement;
        _gameScoreCounterTxt.text = $"{_currentScore} / {_overallScore}";
      }
      else
      {
        _currentLoses += fishReceivedEvent.IncorrectFishIncrement;
        _gameLoseCounterTxt.text = $"{_currentLoses}";
      }
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

    private void IconScaleAnimation(EventStructs.FishReceivedEvent fishReceivedEvent)
    {
      Sequence seq = DOTween.Sequence();
      Transform icon = null;

      if (fishReceivedEvent.CorrectFish == true)
        icon = _gameStarIcon.transform;
      else
        icon = _gameAngryFaceIcon.transform;

      seq.Append(icon.DOScale(_gameIconScaleIn, _gameIconAnimDuration));
      seq.Append(icon.DOScale(1f, _gameIconAnimDuration));
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
          TryToEnableReward();
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

    private void TryToEnableReward()
    {
      if (_currentLoses > 0) return;

      _winRewardButton.gameObject.SetActive(true);
    }
  }
}