using Leopotam.Ecs;


namespace Library
{
    public struct CollisionEnterEvent
    {
        public EcsEntity SourceEntity;
        public EcsEntity IncomingEntity;
    }
}