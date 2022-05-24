using System;
using PdUtils.RandomProvider;

namespace PdUtils
{
	[Serializable]
	public struct Range
	{
		public float min;
		public float max;

		public Range(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		public float Random(IRandomProvider randomProvider)
			=> randomProvider.Range(min, max);
	}
}