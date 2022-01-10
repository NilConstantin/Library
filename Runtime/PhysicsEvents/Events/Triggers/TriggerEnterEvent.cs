using Leopotam.Ecs;


namespace Library
{
    public struct TriggerEnterEvent
    {
        public EcsEntity SourceEntity;
        public EcsEntity IncomingEntity;
    }
}