namespace Zlodey
{
	public struct Health
	{
		public float CurrentHealth;
		public float MaxValue;

		public Health SetValue(float health)
		{
			CurrentHealth = health;
			MaxValue = health;

			return this;
		}
	}
}