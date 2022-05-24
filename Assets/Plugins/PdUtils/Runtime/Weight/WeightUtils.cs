using System.Collections.Generic;
using PdUtils.RandomProvider;

namespace PdUtils.Weight
{
	public static class WeightUtils
	{
		public static IWeight GetByWeight(IWeight[] weights, IRandomProvider randomProvider)
		{
			var sum = CalcWeightSum(weights);
			var random = randomProvider.Range(0, sum);

			var currentSum = 0f;
			foreach (var weight in weights)
			{
				currentSum += weight.Weight;
				if (random < currentSum)
					return weight;
			}
			
			throw new System.Exception($"[ObjectService] Can't choose object");
		}

		private static float CalcWeightSum(IEnumerable<IWeight> weights)
		{
			var sum = 0f;
			foreach (var weight in weights)
			{
				sum += weight.Weight;
			}

			return sum;
		}
	}
}