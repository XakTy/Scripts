using Leopotam.Ecs;
using LeopotamGroup.Globals;
using System;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

namespace Zlodey.Actors
{
	public abstract class EntityActor : MonoBehaviour
	{
		private EcsEntity _entity;
		public EcsEntity Entity => _entity;

		[SerializeField] private bool _isDestroy;

		[SerializeField] private ConverterMonoComponent[] _converterMono;

		public void Init(EcsWorld world)
		{
			_entity = world.NewEntity();

			foreach (var converterMonoComponent in _converterMono)
			{
				converterMonoComponent.Convert(_entity);
			}

			InitComponents();
			DestroyActor();
		}
		public void Init()
		{
			var world = Service<EcsWorld>.Get();

			if (world != null)
			{
				_entity = world.NewEntity();
			}


			foreach (var converterMonoComponent in _converterMono)
			{
				converterMonoComponent.Convert(_entity);
			}

			InitComponents();
			DestroyActor();
		}

		private void DestroyActor()
		{
			if (_isDestroy)
			{
				Object.Destroy(this);
			}
		}
		protected abstract void InitComponents();
	}
}