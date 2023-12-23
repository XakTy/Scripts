using Leopotam.Ecs;

namespace Zlodey
{
	public struct DamageEvent
	{
		public float value;
		public EcsEntity EntityA;
		public EcsEntity EntityB;
	}
}