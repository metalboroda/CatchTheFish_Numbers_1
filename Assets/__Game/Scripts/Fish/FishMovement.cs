using Assets.__Game.Scripts.Tools;
using DG.Tweening;
using UnityEngine;

namespace Assets.__Game.Scripts.Fish
{
  public class FishMovement : MonoBehaviour
  {
    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _minMovementDelay = 1f;
    [SerializeField] private float _maxMovementDelay = 2f;
    [SerializeField] private float _rotationDuration = 1f;

    private Tweener _rotationTween;

    private RandomPointInCamera _randomPointInCamera;

    private void Awake()
    {
      _randomPointInCamera = new RandomPointInCamera(Camera.main);
    }

    private void Start()
    {
      MoveToPoint();
    }

    private void MoveToPoint()
    {
      Vector3 point = _randomPointInCamera.GetRandomPointInCamera();
      float randDelay = Random.Range(_minMovementDelay, _maxMovementDelay);

      point.z = 0f;

      transform.DOLookAt(point, _rotationDuration);
      transform.DOMove(point, _movementSpeed)
        .SetEase(Ease.InOutQuad)
        .SetSpeedBased(true)
        .OnComplete(() =>
        {
          MoveToPoint();
          //AlignRotation();
        });
    }

    private void AlignRotation()
    {
      DOTween.Kill(_rotationTween);
      Vector3 rotation = new(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

      _rotationTween = transform.DORotate(rotation, _rotationDuration * 2)
        .SetEase(Ease.OutQuad);
    }
  }
}