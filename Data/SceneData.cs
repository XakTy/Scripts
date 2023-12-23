using System;
using NaughtyAttributes;
using UnityEngine;
using Zlodey.Actors;
using Object = UnityEngine.Object;

namespace Zlodey
{
    [Serializable]
    public class SceneData
    {
        public Player Player;

        public EntityActor[] Enemies;

        public CameraPlayer[] CameraPool;

        public AudioSource Sounder;
    }
}