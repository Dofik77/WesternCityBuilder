using UnityEngine;
using Random = System.Random;

namespace PdUtils.RandomProvider.Impl
{
	public class SystemRandomProvider : IRandomProvider
	{
		private readonly Random _random = new Random();

		public float Value => (float) _random.NextDouble();
		public int Range(int min, int max) => _random.Next(min, max);

		public float Range(float min, float max)
			=> (float) _random.NextDouble() * (max - min) + min;

		public Vector2 GetNormalizedVector2() => new Vector2(Range(-1, 1), Range(-1, 1)).normalized;
	}
}