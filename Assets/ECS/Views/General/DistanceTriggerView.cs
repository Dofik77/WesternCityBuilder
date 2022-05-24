using Leopotam.Ecs;
using UnityEngine;

namespace ECS.Views.General
{
    public class DistanceTriggerView : LinkableView
    {
        public ref GameObject[] GetUnlockable() => ref _unlockable;
        [SerializeField] private GameObject[] _unlockable;
        public ref GameObject[] GetLockable() => ref _lockable;
        [SerializeField] private GameObject[] _lockable;
        public ref LinkableView[] GetActivatedViews() => ref _activatedViews;
        [SerializeField] private LinkableView[] _activatedViews;
        public ref float GetTriggerDistance() => ref _triggerDistance;
        [SerializeField] private float _triggerDistance = 2.5f;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0.92f, 0.15f, 0.3f);
            Gizmos.DrawWireSphere(Transform.position, GetTriggerDistance());
        }
    }
    
    public struct DistanceTriggerComponent : IEcsIgnoreInFilter
    {
        
    }
}