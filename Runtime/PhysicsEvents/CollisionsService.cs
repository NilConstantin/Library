using Library;
using Leopotam.Ecs;
using UnityEngine;


namespace Library
{
    public class CollisionsService
    {
        private EcsWorld world;
        
        
        public void Init(EcsWorld world)
        {
            this.world = world;
        }


        public void Shutdown()
        {
            this.world = null;
        }
        
        
        public void TryToSendTriggerEnterEvent(EcsEntity sourceEntity, GameObject incomingGameObject)
        {
            var isEntityGameObject = incomingGameObject.TryGetComponent(out EcsEntityGameObject incomingEntityGameObject);
            if (isEntityGameObject)
            {
                var eventEntity = world.NewEntity();
                ref var eventComponent = ref eventEntity.Get<TriggerEnterEvent>();
                eventComponent.SourceEntity = sourceEntity;
                eventComponent.IncomingEntity = incomingEntityGameObject.Entity;
            }
        }
        
        
        public void TryToSendTriggerStayEvent(EcsEntity sourceEntity, GameObject incomingGameObject)
        {
            var isEntityGameObject = incomingGameObject.TryGetComponent(out EcsEntityGameObject incomingEntityGameObject);
            if (isEntityGameObject)
            {
                var eventEntity = world.NewEntity();
                ref var eventComponent = ref eventEntity.Get<TriggerStayEvent>();
                eventComponent.SourceEntity = sourceEntity;
                eventComponent.IncomingEntity = incomingEntityGameObject.Entity;
            }
        }
        

        public void TryToSendTriggerExitEvent(EcsEntity sourceEntity, GameObject incomingGameObject)
        {
            var isEntityGameObject = incomingGameObject.TryGetComponent(out EcsEntityGameObject incomingEntityGameObject);
            if (isEntityGameObject)
            {
                var eventEntity = world.NewEntity();
                ref var eventComponent = ref eventEntity.Get<TriggerExitEvent>();
                eventComponent.SourceEntity = sourceEntity;
                eventComponent.IncomingEntity = incomingEntityGameObject.Entity;
            }
        }
        
        
        public void TryToSendCollisionEnterEvent(EcsEntity sourceEntity, GameObject incomingGameObject)
        {
            var isEntityGameObject = incomingGameObject.TryGetComponent(out EcsEntityGameObject incomingEntityGameObject);
            if (isEntityGameObject)
            {
                var eventEntity = world.NewEntity();
                ref var eventComponent = ref eventEntity.Get<CollisionEnterEvent>();
                eventComponent.SourceEntity = sourceEntity;
                eventComponent.IncomingEntity = incomingEntityGameObject.Entity;
            }
        }

        
        public void TryToSendCollisionStayEvent(EcsEntity sourceEntity, GameObject incomingGameObject)
        {
            var isEntityGameObject = incomingGameObject.TryGetComponent(out EcsEntityGameObject incomingEntityGameObject);
            if (isEntityGameObject)
            {
                var eventEntity = world.NewEntity();
                ref var eventComponent = ref eventEntity.Get<CollisionStayEvent>();
                eventComponent.SourceEntity = sourceEntity;
                eventComponent.IncomingEntity = incomingEntityGameObject.Entity;
            }
        }

        
        public void TryToSendCollisionExitEvent(EcsEntity sourceEntity, GameObject incomingGameObject)
        {
            var isEntityGameObject = incomingGameObject.TryGetComponent(out EcsEntityGameObject incomingEntityGameObject);
            if (isEntityGameObject)
            {
                var eventEntity = world.NewEntity();
                ref var eventComponent = ref eventEntity.Get<CollisionExitEvent>();
                eventComponent.SourceEntity = sourceEntity;
                eventComponent.IncomingEntity = incomingEntityGameObject.Entity;
            }
        }
    }
}