using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.Misc;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.__Game.Scripts.Managers
{
  public class AudioManager : MonoBehaviour
  {
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioMixer _mixer;

    private GameSettings _gameSettings;

    private EventBinding<EventStructs.AudioSwitchedEvent> _audioSwitchedEvent;

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

    private void OnEnable()
    {
      _audioSwitchedEvent = new EventBinding<EventStructs.AudioSwitchedEvent>(SwitchMasterVolume);
    }

    private void OnDisable()
    {
      _audioSwitchedEvent?.Remove(SwitchMasterVolume);
    }

    private void Start()
    {
      LoadSettings();
      LoadVolumes();
    }

    private void LoadSettings()
    {
      if (_gameSettings == null)
        _gameSettings = new GameSettings();

      _gameSettings = SettingsManager.LoadSettings<GameSettings>();
    }

    private void LoadVolumes()
    {
      if (_gameSettings.IsMusicOn == true)
        _mixer.SetFloat(Hashes.MasterVolume, 0);
      else
        _mixer.SetFloat(Hashes.MasterVolume, -80f);
    }

    public void SwitchMasterVolume()
    {
      _mixer.GetFloat(Hashes.MasterVolume, out float currentVolume);

      if (currentVolume == 0)
      {
        _mixer.SetFloat(Hashes.MasterVolume, -80f);
        _gameSettings.IsMusicOn = false;
      }
      else
      {
        _mixer.SetFloat(Hashes.MasterVolume, 0);
        _gameSettings.IsMusicOn = true;
      }

      SettingsManager.SaveSettings(_gameSettings);
    }
  }
}