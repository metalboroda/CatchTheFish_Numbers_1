using UnityEngine;

namespace Assets.__Game.Scripts.Tools
{
  public class RandomPointInCamera
  {
    private float _minDistance;
    private Vector3 _previousPoint;
    private Camera _mainCamera;

    public RandomPointInCamera(float minDistance, Camera camera)
    {
      _minDistance = minDistance;
      _previousPoint = Vector3.zero;
      _mainCamera = camera;
    }

    public Vector3 GetRandomPointInCamera()
    {
      Vector3 randomPoint = GetRandomPoint();

      while (!IsFarEnoughFromPrevious(randomPoint))
      {
        randomPoint = GetRandomPoint();
      }

      _previousPoint = randomPoint;

      return randomPoint;
    }

    private Vector3 GetRandomPoint()
    {
      float randomX = Random.Range(0f, 1f);
      float randomY = Random.Range(0f, 1f);
      Vector3 viewportPoint = new(randomX, randomY, 0f);

      return _mainCamera.ViewportToWorldPoint(viewportPoint);
    }

    private bool IsFarEnoughFromPrevious(Vector3 point)
    {
      if (_previousPoint == Vector3.zero) return true;

      return Vector3.Distance(point, _previousPoint) >= _minDistance;
    }
  }
}