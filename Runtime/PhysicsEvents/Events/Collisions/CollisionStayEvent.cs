using Leopotam.Ecs;


namespace Library
{
    public struct CollisionStayEvent
    {
        public EcsEntity SourceEntity;
        public EcsEntity IncomingEntity;
    }
}