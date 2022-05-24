using UnityEngine;

namespace PdUtils.RandomProvider
{
    public interface IRandomProvider
    {
        float Value { get; }
        int Range(int min, int max);
        float Range(float min, float max);
        Vector2 GetNormalizedVector2();
    }
}