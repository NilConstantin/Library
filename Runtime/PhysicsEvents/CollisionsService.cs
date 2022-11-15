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
                world.NewEntity().Get<TriggerEnterEvent>() = new TriggerEnterEvent
                {
                    SourceEntity = sourceEntity,
                    IncomingEntity = incomingEntityGameObject.Entity
                };
            }
            else 
            {
                var isTriggerEnterMediator = incomingGameObject.TryGetComponent(out TriggerEnterMediator triggerEnterMediator);
                if (isTriggerEnterMediator)
                {
                    world.NewEntity().Get<TriggerEnterEvent>() = new TriggerEnterEvent
                    {
                        SourceEntity = sourceEntity,
                        IncomingEntity = triggerEnterMediator.EntityGameObject.Entity
                    };
                }
            }
        }
        
        
        public void TryToSendTriggerStayEvent(EcsEntity sourceEntity, GameObject incomingGameObject)
        {
            var isEntityGameObject = incomingGameObject.TryGetComponent(out EcsEntityGameObject incomingEntityGameObject);
            if (isEntityGameObject)
            {
                world.NewEntity().Get<TriggerStayEvent>() = new TriggerStayEvent
                {
                    SourceEntity = sourceEntity,
                    IncomingEntity = incomingEntityGameObject.Entity
                };
            }
        }
        

        public void TryToSendTriggerExitEvent(EcsEntity sourceEntity, GameObject incomingGameObject)
        {
            var isEntityGameObject = incomingGameObject.TryGetComponent(out EcsEntityGameObject incomingEntityGameObject);
            if (isEntityGameObject)
            {
                world.NewEntity().Get<TriggerExitEvent>() = new TriggerExitEvent
                {
                    SourceEntity = sourceEntity,
                    IncomingEntity = incomingEntityGameObject.Entity
                };
            }
        }
        
        
        public void TryToSendCollisionEnterEvent(EcsEntity sourceEntity, GameObject incomingGameObject)
        {
            var isEntityGameObject = incomingGameObject.TryGetComponent(out EcsEntityGameObject incomingEntityGameObject);
            if (isEntityGameObject)
            {
                world.NewEntity().Get<CollisionEnterEvent>() = new CollisionEnterEvent
                {
                    SourceEntity = sourceEntity,
                    IncomingEntity = incomingEntityGameObject.Entity
                };
            }
        }

        
        public void TryToSendCollisionStayEvent(EcsEntity sourceEntity, GameObject incomingGameObject)
        {
            var isEntityGameObject = incomingGameObject.TryGetComponent(out EcsEntityGameObject incomingEntityGameObject);
            if (isEntityGameObject)
            {
                world.NewEntity().Get<CollisionStayEvent>() = new CollisionStayEvent
                {
                    SourceEntity = sourceEntity,
                    IncomingEntity = incomingEntityGameObject.Entity
                };
            }
        }

        
        public void TryToSendCollisionExitEvent(EcsEntity sourceEntity, GameObject incomingGameObject)
        {
            var isEntityGameObject = incomingGameObject.TryGetComponent(out EcsEntityGameObject incomingEntityGameObject);
            if (isEntityGameObject)
            {
                world.NewEntity().Get<CollisionExitEvent>() = new CollisionExitEvent
                {
                    SourceEntity = sourceEntity,
                    IncomingEntity = incomingEntityGameObject.Entity
                };
            }
        }
    }
}