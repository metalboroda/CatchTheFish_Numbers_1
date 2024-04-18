using UnityEngine;

namespace Assets.__Game.Scripts.LevelItems
{
  public class BubbleSpawner : MonoBehaviour
  {
    [SerializeField] private GameObject[] _bubblesToSpawn;

    void Start()
    {
      SetScale();
    }

    private void SetScale()
    {
      float cameraWidth = Camera.main.orthographicSize * 2f * Camera.main.aspect;
      Vector3 objectSize = transform.localScale;
      float scaleFactor = cameraWidth / objectSize.x;

      transform.localScale = new Vector3(scaleFactor, objectSize.y, objectSize.z);
    }

    private GameObject GetRandomBubble()
    {
      return _bubblesToSpawn[Random.Range(0, _bubblesToSpawn.Length)];
    }
  }
}