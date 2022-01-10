using Leopotam.Ecs;


namespace Library
{
    public struct TriggerExitEvent
    {
        public EcsEntity SourceEntity;
        public EcsEntity IncomingEntity;
    }
}