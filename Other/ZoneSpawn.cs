using Cysharp.Threading.Tasks;
using System;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Serialization;

namespace InterfaceWay
{
	public sealed class ZoneSpawn : MonoBehaviour
	{
		[SerializeField] private SpawnerUnit[] _spawnersUnits;
		[SerializeField] private float _tick;
		public void Spawn()
		{
			TickSpawn().Forget();
		}

		public async UniTaskVoid TickSpawn()
		{
			foreach (var spawnerUnit in _spawnersUnits)
			{
				await UniTask.Delay(TimeSpan.FromSeconds(_tick), cancellationToken: destroyCancellationToken);
				spawnerUnit.Spawn();
			}
		}
	}
}