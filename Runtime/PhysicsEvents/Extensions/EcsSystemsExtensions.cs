using Leopotam.Ecs;


namespace Library
{
    public static class EcsSystemsExtensions
    {
        public static EcsSystems OneFramePhysicsEvents(this EcsSystems ecsSystems)
        {
            ecsSystems.OneFrame<TriggerEnterEvent>();
            ecsSystems.OneFrame<TriggerStayEvent>();
            ecsSystems.OneFrame<TriggerExitEvent>();
            ecsSystems.OneFrame<CollisionEnterEvent>();
            ecsSystems.OneFrame<CollisionStayEvent>();
            ecsSystems.OneFrame<CollisionExitEvent>();

            return ecsSystems;
        }
    }
}