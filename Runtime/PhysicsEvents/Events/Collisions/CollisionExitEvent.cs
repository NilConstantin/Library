using Leopotam.Ecs;


namespace Library
{
    public struct CollisionExitEvent
    {
        public EcsEntity SourceEntity;
        public EcsEntity IncomingEntity;
    }
}