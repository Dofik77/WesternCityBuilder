using System.Collections.Generic;
using DataBase.Timer;
using ECS.Core.Utils.ReactiveSystem.Components;
using ECS.Game.Components.General;
using Leopotam.Ecs;
using PdUtils;
using UnityEngine;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace ECS.DataSave
{
    public class GameState
    {
        public List<SaveState> SaveState;
    }

    public class SaveState
    {
        public bool IsCamera { get; set; }
        public string Prefab;
        public Timer? Timer { get; set; }
        public Quaternion? Rotation { get; set; }
        public Uid? Uid { get; set; }
        public Vector3? Position { get; set; }

        public void WriteState(EcsEntity entity)
        {
            if (entity.Has<TimerComponent>())
                Timer = entity.Get<TimerComponent>().Value;

            if (entity.Has<PositionComponent>())
                Position = entity.Get<PositionComponent>().Value;
            
            if (entity.Has<PrefabComponent>())
                Prefab = entity.Get<PrefabComponent>().Value;
            
            if (entity.Has<RotationComponent>())
                Rotation = entity.Get<RotationComponent>().Value;
            
            if (entity.Has<UIdComponent>())
                Uid = entity.Get<UIdComponent>().Value;
        }

        public void ReadState(EcsEntity entity)
        {
            if (Timer.HasValue) 
                entity.Get<TimerComponent>().Value = Timer.Value;

            if (Position.HasValue)
                entity.Get<PositionComponent>().Value = Position.Value;
            
            if (Rotation.HasValue)
                entity.Get<RotationComponent>().Value = Rotation.Value;
            
            if (Uid.HasValue)
                entity.Get<UIdComponent>().Value = Uid.Value;
            
            if (Prefab != null)
            {
                entity.Get<PrefabComponent>().Value = Prefab;
                entity.Get<EventAddComponent<PrefabComponent>>();
            }
        }
    }
}
