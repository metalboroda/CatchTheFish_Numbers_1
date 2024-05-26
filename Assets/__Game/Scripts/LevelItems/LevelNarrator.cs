using Assets.__Game.Scripts.EventBus;
using Assets.__Game.Scripts.Game.States;
using Assets.__Game.Scripts.Tools;
using System.Collections;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.LevelItem
{
  [RequireComponent(typeof(AudioSource))]
  public class LevelNarrator : MonoBehaviour
  {
    [Header("Announcer")]
    [SerializeField] private AudioClip _questAudio;
    [SerializeField] private AudioClip[] _winAnnouncerClips;
    [SerializeField] private AudioClip[] _loseAnnouncerClips;
    [SerializeField] private AudioClip[] _stuporAnnouncerClips;

    private AudioSource _audioSource;

    private AudioTool _audioTool;

    private EventBinding<EventStructs.StateChanged> _stateEvent;
    private EventBinding<EventStructs.StuporEvent> _stuporEvent;
    private EventBinding<EventStructs.FishClickEvent> _fishClickEvent;

    private void Awake()
    {
      _audioSource = GetComponent<AudioSource>();

      _audioTool = new AudioTool(_audioSource);
    }

    private void OnEnable()
    {
      _stateEvent = new EventBinding<EventStructs.StateChanged>(PlayScreenSound);
      _stuporEvent = new EventBinding<EventStructs.StuporEvent>(PlayStuporSound);
      _fishClickEvent = new EventBinding<EventStructs.FishClickEvent>(PlayFishAudioCLip);
    }

    private void OnDisable()
    {
      _stateEvent.Remove(PlayScreenSound);
      _stuporEvent.Remove(PlayStuporSound);
      _fishClickEvent.Remove(PlayFishAudioCLip);
    }

    private void Start()
    {
      _audioSource.PlayOneShot(_questAudio);
    }

    private void PlayScreenSound(EventStructs.StateChanged state)
    {
      switch (state.State)
      {
        case GameWinState:
          StartCoroutine(DoPlayOneShotWithDelay(_audioTool.GetRandomCLip(_winAnnouncerClips), 2f));
          break;
        case GameLoseState:
          StartCoroutine(DoPlayOneShotWithDelay(_audioTool.GetRandomCLip(_loseAnnouncerClips), 2f));
          break;
      }
    }

    private IEnumerator DoPlayOneShotWithDelay(AudioClip audioClip, float delay = 0)
    {
      yield return new WaitForSeconds(delay);

      _audioSource.Stop();
      _audioSource.PlayOneShot(audioClip);
    }

    private void PlayStuporSound(EventStructs.StuporEvent stuporEvent)
    {
      _audioSource.Stop();
      _audioSource.PlayOneShot(_audioTool.GetRandomCLip(_stuporAnnouncerClips));
    }

    private void PlayFishAudioCLip(EventStructs.FishClickEvent fishClickEvent)
    {
      _audioSource.Stop();
      _audioSource.PlayOneShot(fishClickEvent.FishAudioCLip);
    }
  }
}