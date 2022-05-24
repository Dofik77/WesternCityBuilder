using UnityEditor;
using UnityEngine;

namespace PdUtils
{
	public static class GizmosUtil
	{
		public static void DrawText(Vector3 position, string text)
		{
#if UNITY_EDITOR
			Handles.Label(position, text);
#endif
		}
	}
}