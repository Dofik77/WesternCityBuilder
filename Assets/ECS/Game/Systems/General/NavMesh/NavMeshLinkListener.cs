using ECS.Core.Utils.SystemInterfaces;
using ECS.Game.Components.Flags;
using ECS.Game.Components.General;
using ECS.Utils.Extensions;
using ECS.Views.General;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.AI;

namespace ECS.Game.Systems.General.NavMesh
{
    public class NavMeshLinkListener : IEcsUpdateSystem
    {
        private readonly EcsFilter<PlayerComponent, LinkComponent>.Exclude<EventOnNavMeshLinkComponent> _player;
        public void Run()
        {
            foreach (var i in _player)
            {
                if (_player.Get2(i).Get<PlayerView>().GetAgent().isOnOffMeshLink)
                {
                    var view = _player.Get2(i).Get<PlayerView>();
                    var data = view.GetAgent().currentOffMeshLinkData;
                    ref var linkEvent = ref _player.GetEntity(i).AddTimer<EventOnNavMeshLinkComponent>();
                    linkEvent.LinkView = (view.GetAgent().navMeshOwner as NavMeshLink).GetComponent<NavMeshLinkView>();
                    linkEvent.From = view.Transform.position;
                    if (Vector3.Distance(view.Transform.position, data.startPos)
                        <= Vector3.Distance(view.Transform.position, data.endPos))
                        linkEvent.To = data.endPos;
                }
            }
        }
    }
}