using UnityEngine;

namespace Assets.__Game.Scripts.Tools
{
  public class RandomPointInCamera
  {
    private Camera _mainCamera;

    public RandomPointInCamera(Camera camera)
    {
      _mainCamera = camera;
    }

    public Vector3 GetRandomPointInCamera()
    {
      Vector3 randomPoint = GetRandomPoint();

      return randomPoint;
    }

    private Vector3 GetRandomPoint()
    {
      float randomX = Random.Range(0f, 1f);
      float randomY = Random.Range(0f, 1f);
      Vector3 viewportPoint = new Vector3(randomX, randomY, 0f);

      return _mainCamera.ViewportToWorldPoint(viewportPoint);
    }
  }
}