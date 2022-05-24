using UnityEngine;

namespace PdUtils.RandomProvider.Impl
{
	public class UnityRandomProvider : IRandomProvider
	{
		public float Value => Random.value;
		public int Range(int min, int max) => Random.Range(min, max);
		public float Range(float min, float max) => Random.Range(min, max);

		public Vector2 GetNormalizedVector2() => Random.insideUnitCircle;
	}
}