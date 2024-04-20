using Assets.__Game.Scripts.Misc;
using UnityEngine;

namespace Assets.__Game.Scripts.LevelItems
{
  public class Reward
  {
    private string[] _fishList;

    public Reward()
    {
      _fishList = new[]
      {
        Hashes.PennantCoralfish,
        Hashes.JackDempsey,
        Hashes.AcanthurusLeucosternon,
        Hashes.Pterophyllum,
        Hashes.FlameAngelfish,
        Hashes.RoyalGramma,
        Hashes.CopperbandButterflyfish,
      };
    }

    public void OpenRandomWikipediaFishLink()
    {
      string randomFish = _fishList[Random.Range(0, _fishList.Length)];
      string wikipediaLink = "https://en.wikipedia.org/wiki/" + randomFish;

      Application.OpenURL(wikipediaLink);
    }
  }
}