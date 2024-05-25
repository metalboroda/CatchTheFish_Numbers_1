using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.Fish;
using Assets.__Game.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using Assets.__Game.Scripts.SOs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.__Game.Scripts.Managers
{
  public class FishManager : MonoBehaviour
  {
    [SerializeField] private CorrectValuesContainerSo _correctNumbersContainerSo;
    [Header("Stupor param's")]
    [SerializeField] private float _stuporTimeoutSeconds = 10f;

    private List<FishHandler> _correctNumberFish = new();
    private List<FishHandler> _incorrectNumberFish = new();
    private Coroutine _stuporTimeoutRoutine;

    private EventBinding<EventStructs.FishSpawnerEvent> _fishSpawnerEvent;
    private EventBinding<EventStructs.FishClickEvent> _fishClickEvent;
    private EventBinding<EventStructs.StateChanged> _stateChangedEvent;

    private GameBootstrapper _gameBootstrapper;

    private void Awake()
    {
      _gameBootstrapper = GameBootstrapper.Instance;
    }

    private void OnEnable()
    {
      _fishSpawnerEvent = new EventBinding<EventStructs.FishSpawnerEvent>(AddFishesToList);
      _fishClickEvent = new EventBinding<EventStructs.FishClickEvent>(ReceiveFish);
      _stateChangedEvent = new EventBinding<EventStructs.StateChanged>(StuporTimerDependsOnState);
    }

    private void OnDisable()
    {
      _fishSpawnerEvent.Remove(AddFishesToList);
      _fishClickEvent.Remove(ReceiveFish);
      _stateChangedEvent.Remove(StuporTimerDependsOnState);
    }

    private void Start()
    {
      EventBus<EventStructs.FishReceivedEvent>.Raise(new EventStructs.FishReceivedEvent
      {
        CorrectValues = _correctNumbersContainerSo.CorrectValues
      });
    }

    private void AddFishesToList(EventStructs.FishSpawnerEvent fishSpawnerEvent)
    {
      _correctNumberFish.AddRange(fishSpawnerEvent.CorrectFishHandlers);
      _incorrectNumberFish.AddRange(fishSpawnerEvent.IncorrectFinishHandlers);
    }

    private void ReceiveFish(EventStructs.FishClickEvent fishClickEvent)
    {
      ResetAndStartStuporTimer();

      foreach (var number in _correctNumbersContainerSo.CorrectValues)
      {
        if (_correctNumberFish.Contains(fishClickEvent.FishHandler))
        {
          _correctNumberFish.Remove(fishClickEvent.FishHandler);
          fishClickEvent.FishHandler.DestroyFish(true);

          EventBus<EventStructs.FishReceivedEvent>.Raise(new EventStructs.FishReceivedEvent
          {
            CorrectFish = true,
            CorrectFishIncrement = 1
          });

          break;
        }

        if (_incorrectNumberFish.Contains(fishClickEvent.FishHandler))
        {
          _incorrectNumberFish.Remove(fishClickEvent.FishHandler);
          fishClickEvent.FishHandler.DestroyFish(false);

          EventBus<EventStructs.FishReceivedEvent>.Raise(new EventStructs.FishReceivedEvent
          {
            CorrectFish = false,
            IncorrectFishIncrement = 1
          });

          break;
        }
      }

      CheckFishLists();
    }

    private void CheckFishLists()
    {
      if (_gameBootstrapper == null) return;

      if (_correctNumberFish.Count == 0)
        _gameBootstrapper.StateMachine.ChangeState(new GameWinState(_gameBootstrapper));

      if (_incorrectNumberFish.Count == 0)
        _gameBootstrapper.StateMachine.ChangeState(new GameLoseState(_gameBootstrapper));
    }

    private void StuporTimerDependsOnState(EventStructs.StateChanged stateChanged)
    {
      if (stateChanged.State is GameplayState)
        ResetAndStartStuporTimer();
      else
      {
        if (_stuporTimeoutRoutine != null)
          StopCoroutine(_stuporTimeoutRoutine);
      }
    }

    private void ResetAndStartStuporTimer()
    {
      if (_stuporTimeoutRoutine != null)
        StopCoroutine(_stuporTimeoutRoutine);

      _stuporTimeoutRoutine = StartCoroutine(DoStuporTimerCoroutine());
    }

    private IEnumerator DoStuporTimerCoroutine()
    {
      yield return new WaitForSeconds(_stuporTimeoutSeconds);

      EventBus<EventStructs.StuporEvent>.Raise(new EventStructs.StuporEvent());

      ResetAndStartStuporTimer();
    }
  }
}