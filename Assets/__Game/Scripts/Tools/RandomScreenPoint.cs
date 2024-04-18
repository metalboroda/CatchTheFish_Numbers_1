using UnityEngine;

namespace Assets.__Game.Scripts.Tools
{
  public class RandomPointGenerator
  {
    private readonly float _minMovementDistance;

    private Vector2 _lastPoint;

    public RandomPointGenerator(float minMovementDistance)
    {
      _minMovementDistance = minMovementDistance;
      _lastPoint = GetRandomPointOnScreen();
    }

    public Vector2 GetRandomPointOnScreen()
    {
      Vector2 randomPoint;

      do
      {
        var randomX = Random.Range(0f, 1f);
        var randomY = Random.Range(0f, 1f);

        randomPoint = new Vector2(randomX, randomY);

        randomPoint.x *= Screen.width;
        randomPoint.y *= Screen.height;

      } while (Vector2.Distance(randomPoint, _lastPoint) < _minMovementDistance);

      _lastPoint = randomPoint;

      return randomPoint;
    }
  }
}