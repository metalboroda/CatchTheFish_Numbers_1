using Assets.__Game.Scripts.Tools;
using UnityEngine;

namespace Assets.__Game.Scripts.Fish
{
  public class FishMovement : MonoBehaviour
  {
    [SerializeField] private float _minMovementDistance = 10;

    private RandomPointGenerator _randomPointGenerator;

    private void Awake()
    {
      _randomPointGenerator = new RandomPointGenerator(_minMovementDistance);
    }

    private void Start()
    {
      Debug.Log(_randomPointGenerator.GetRandomPointOnScreen());
    }
  }
}
