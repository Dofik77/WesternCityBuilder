using ECS.Game.Systems.General.NavMesh;
using UnityEngine.AI;

namespace ECS.Views.General
{
    public class PlayerView : LinkableView,
        IHasNavMeshAgent
    {
        public ref NavMeshAgent GetAgent()
        {
            throw new System.NotImplementedException();
        }
    }
}