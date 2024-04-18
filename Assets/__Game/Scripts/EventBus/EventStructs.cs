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

    #region Player
    public struct FishEvent : IEvent
    {
      public int FishId;
      public int FishNumber;
    }
    #endregion
  }
}