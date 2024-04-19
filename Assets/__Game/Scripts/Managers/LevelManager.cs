  using Assets.__Game.Scripts.Enums;
  using Assets.__Game.Scripts.EventBus;
  using UnityEngine;
  using UnityEngine.AddressableAssets;
  using UnityEngine.ResourceManagement.AsyncOperations;

  namespace Assets.__Game.Scripts.Managers
  {
    public class LevelManager : MonoBehaviour
    {
      [SerializeField] private Level[] _levels;

      private int _currentLevelIndex = 0;
      private GameObject _currentLevelPrefab;

      private GameSettings _gameSettings;

      private EventBinding<EventStructs.UiButtonEvent> _uiButtonEvent;

      private void Awake()
      {
        LoadSettings();
      }

      private void OnEnable()
      {
        _uiButtonEvent = new EventBinding<EventStructs.UiButtonEvent>(LoadNextLevel);
      }

      private void OnDisable()
      {
        _uiButtonEvent.Remove(LoadNextLevel);
      }

      private void Start()
      {
        SetSavedLevel();
        LoadLevel(_currentLevelIndex);
      }

      public async void LoadLevel(int levelIndex)
      {
        if (levelIndex >= _levels.Length)
        {
          int randomIndex = Random.Range(0, _levels.Length);

          levelIndex = randomIndex;
        }

        if (levelIndex < _levels.Length)
        {
          Level level = _levels[levelIndex];
          AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(level.LevelAddressableKey);

          await handle.Task;

          _currentLevelPrefab = handle.Result;

          Instantiate(_currentLevelPrefab);

          SettingsManager.SaveSettings(_gameSettings);
        }
      }

      private void LoadNextLevel(EventStructs.UiButtonEvent uiButtonEvent)
      {
        if (uiButtonEvent.UiEnums != UiEnums.WinNextLevelButton) return;

        _currentLevelIndex++;
        _gameSettings.LevelIndex = _currentLevelIndex;

        LoadLevel(_currentLevelIndex);
      }

      private void LoadSettings()
      {
        _gameSettings = SettingsManager.LoadSettings<GameSettings>();

        if (_gameSettings == null)
          _gameSettings = new GameSettings();
      }

      private void SetSavedLevel()
      {
        _currentLevelIndex = _gameSettings.LevelIndex;
      }
    }
  }