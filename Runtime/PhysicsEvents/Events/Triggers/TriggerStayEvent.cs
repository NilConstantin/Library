using Leopotam.Ecs;


namespace Library
{
    public struct TriggerStayEvent
    {
        public EcsEntity SourceEntity;
        public EcsEntity IncomingEntity;
    }
}