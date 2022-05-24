using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Views.General
{
	public abstract class LinkableView : MonoBehaviour, ILinkable
	{
		public EcsEntity Entity;
		public int Hash => transform.GetHashCode();
		public Transform Transform => transform;
		public int UnityInstanceId => gameObject.GetInstanceID();

		public virtual void Link(EcsEntity entity)
		{
			Entity = entity;
		}

		public virtual void DestroyObject()
		{
#if UNITY_EDITOR
			if (UnityEditor.EditorApplication.isPlaying)
				Destroy(gameObject);
			else
				DestroyImmediate(gameObject);
#else
			Destroy(gameObject);
#endif
		}
	}
}