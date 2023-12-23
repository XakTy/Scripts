using Leopotam.Ecs;
using Pathfinding;
using UnityEngine;
using Zlodey;
using Zlodey.Actors;

namespace InterfaceWay
{
	public class SpawnerUnit : MonoBehaviour
	{
		public Enemy PrefabEntity;
		public Transform SpawnPoint;
		public Transform MovePoint;
		public SpawnView View;
		public bool MovingPlayer;
		public void Spawn()
		{
			View?.Play();
			var instanceEnemy = Object.Instantiate(PrefabEntity, SpawnPoint.position, SpawnPoint.rotation);
			instanceEnemy.Init();

			if (MovingPlayer)
			{
				instanceEnemy.Entity.Get<MovePlayer>();
			}
			if (MovePoint)
			{
				instanceEnemy.Entity.Get<AI>().value.destination = MovePoint.position;
			}
		}

	}
}