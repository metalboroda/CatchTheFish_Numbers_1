using UnityEngine;

namespace Assets.__Game.Scripts.Infrastructure
{
  public class GameBootstrapper : MonoBehaviour
  {
    public static GameBootstrapper Instance { get; private set; }

    public StateMachine GameStateMachine;
    public SceneLoader SceneLoader;

    public GameBootstrapper()
    {
      GameStateMachine = new StateMachine();
      SceneLoader = new SceneLoader();
    }
  }
}