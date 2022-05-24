using UnityEngine;

namespace Zenject
{
    public class GizmoRenderer : MonoBehaviour
    {
        GizmoRenderableManager _renderableManager;

        [Inject]
        void Construct(GizmoRenderableManager renderableManager)
        {
            _renderableManager = renderableManager;
        }

        private void OnDrawGizmos() {
            _renderableManager.OnDrawGizmos();
        }
    }
}
