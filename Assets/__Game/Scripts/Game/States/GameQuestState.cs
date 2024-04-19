﻿using Assets.__Game.Scripts.Infrastructure;
using UnityEngine;

namespace Assets.__Game.Scripts.Game.States
{
  public class GameQuestState : GameBaseState
  {
    public GameQuestState(GameBootstrapper gameBootstrapper) : base(gameBootstrapper)
    {
    }

    public override void Enter()
    {
      Time.timeScale = 0;
    }

    public override void Exit()
    {
      Time.timeScale = 1;
    }
  }
}