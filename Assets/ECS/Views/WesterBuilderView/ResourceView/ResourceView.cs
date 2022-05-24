using ECS.Game.Components.WesternBuilder_Component;
using ECS.Views.General;
using UnityEngine;

namespace ECS.Views.WesterBuilderView
{
    public class ResourceView : LinkableView
    {
        [SerializeField] public RequiredResourceType ResourceType;
    }
}