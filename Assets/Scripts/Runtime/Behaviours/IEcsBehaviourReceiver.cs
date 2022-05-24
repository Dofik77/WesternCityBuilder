using Leopotam.Ecs;

namespace Runtime.Behaviours
{
    public interface IEcsBehaviourReceiver
    {
        void SetEntity(EcsEntity entity);
    }
}