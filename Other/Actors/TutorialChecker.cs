using Leopotam.Ecs;
using UnityEngine;

namespace Zlodey.Actors
{
	public sealed class TutorialChecker : MonoBehaviour
	{
		public EntityActor Entity;

		private bool _islive = false;
		private void Update()
		{
			if (_islive)
			{
				if (!Entity.Entity.IsAlive())
				{
					Object.Destroy(gameObject);
				}

			}


			if (Entity.Entity.IsAlive())
			{
				_islive = true;
			}
		}
	}
}