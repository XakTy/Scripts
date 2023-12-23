using System;
using UnityEngine;
using Zlodey.Actors;

namespace Zlodey
{
	[Serializable]
	public struct AttackRangeDataRef
	{
		public Transform StartPoint;
		public AttackRangeData value;
	}
}