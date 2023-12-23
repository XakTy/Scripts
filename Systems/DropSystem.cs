using DG.Tweening;
using Leopotam.Ecs;
using UnityEngine;

namespace Zlodey
{
	public sealed class DropSystem : IEcsRunSystem
	{
		private readonly EcsFilter<Drop, TransformRef, DiedEvent> _dieds;
		public void Run()
		{
			foreach (var i in _dieds)
			{
				var dropData = _dieds.Get1(i).Data;
				var position = _dieds.Get2(i).value.position;


				var randomChance = Random.Range(0f, 1f);

				if (randomChance > 0.3f)
				{
					continue;
				}

				var instanceDrop = Object.Instantiate(dropData.View, position, Quaternion.identity);

				var random = Random.onUnitSphere * 1.2f;
				random.y = position.y + 0.8f;
				position += random;

				instanceDrop.transform.DOJump(position, 1f, 1, 0.3f);
			}
		}
	}
}