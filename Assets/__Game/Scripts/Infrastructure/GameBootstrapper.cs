using Assets.__Game.Scripts.Enums;
using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.Game.States;
using UnityEngine;

namespace Assets.__Game.Scripts.Infrastructure
{
  public class GameBootstrapper : MonoBehaviour
  {
    public static GameBootstrapper Instance { get; private set; }

    public StateMachine StateMachine;
    public SceneLoader SceneLoader;

    public GameBootstrapper()
    {
      StateMachine = new StateMachine();
      SceneLoader = new SceneLoader();
    }

    private EventBinding<EventStructs.UiButtonEvent> _uiButtonEvent;

    private void Awake()
    {
      InitSingleton();
    }

    private void OnEnable()
    {
      _uiButtonEvent = new EventBinding<EventStructs.UiButtonEvent>(ChangeState);
    }

    private void OnDisable()
    {
      _uiButtonEvent.Remove(ChangeState);
    }

    private void Start()
    {
      StateMachine.Init(new GameQuestState(this));

      EventBus<EventStructs.SendComponentEvent<GameBootstrapper>>.Raise(
        new EventStructs.SendComponentEvent<GameBootstrapper> { Data = this });
    }

    private void InitSingleton()
    {
      if (Instance != null && Instance != this)
      {
        Destroy(gameObject);
      }
      else
      {
        Instance = this;

        DontDestroyOnLoad(gameObject);
      }
    }

    private void ChangeState(EventStructs.UiButtonEvent uiButtonEvent)
    {
      switch (uiButtonEvent.UiEnums)
      {
        case UiEnums.QuestPlayButton:
          StateMachine.ChangeState(new GameplayState(this));
          break;
      }
    }
  }
}