using UnityEngine;

namespace Assets.__Game.Scripts.Managers
{
  public class AudioManager : MonoBehaviour
  {
    public static AudioManager Instance { get; private set; }

    private void Awake()
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
  }
}