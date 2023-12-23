using System;
using UnityEngine;
using Zlodey.Actors;

namespace Zlodey
{
    [Serializable]
    public class RuntimeData
    {
        public int Level;
        public GameState GameState;
        public float LevelStartedTime;

        public float deltaTime;

        public CameraPlayer CurrentCamera;

        public Collider[] CollidersPool = new Collider[100];
    }
}