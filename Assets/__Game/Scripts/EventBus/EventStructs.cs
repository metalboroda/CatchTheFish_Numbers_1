using System.Collections.Generic;
using Assets.__Game.Scripts.Enums;
using Assets.__Game.Scripts.Fish;
using Assets.__Game.Scripts.StateMachine;

namespace Assets.__Game.Scripts.EventBus
{
  public class EventStructs
  {
    #region Infrastructure
    public struct StateChanged : IEvent
    {
      public State State;
    }
    #endregion

    #region Fish Spawner
    public struct FishSpawnerEvent : IEvent
    {
      public List<FishHandler> CorrectFishHandlers;
      public int CorrectFishCount;
      public List<FishHandler> IncorrectFinishHandlers;
      public int IncorrectFinishCount;
    }
    #endregion

    #region Fish Manager
    public struct FishReceivedEvent : IEvent
    {
      public int[] CorrectNumbers;
      public int CorrectFishIncrement;
      public int IncorrectFishIncrement;
    }
    #endregion

    #region Fish
    public struct FishUiEvent : IEvent
    {
      public int FishId;
      public int FishNumber;
    }

    public struct FishClickEvent : IEvent
    {
      public int FishNumber;
      public FishHandler FishHandler;
    }

    public struct FishDestroyEvent : IEvent
    {
      public int FishId;
      public bool Correct;
    }
    #endregion

    #region UI

    public struct UiButtonEvent : IEvent
    {
      public UiEnums UiEnums;
    }
    #endregion
  }
}