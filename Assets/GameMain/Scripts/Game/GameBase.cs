using System;
using GameFramework.Event;
using GameFramework.Fsm;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace StarForce
{
    public abstract class GameBase
    {
        public abstract GameMode GameMode { get; }

        public bool GameOver { get; private set; }

        public virtual void Initialize()
        {
            GameOver = false;
        }

        public virtual void Shutdown()
        {
            GameOver = true;
        }

        public void OnGameOver()
        {
            GameOver = true;
        }
        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {
        }
    }
}