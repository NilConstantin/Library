using Leopotam.Ecs;
using System;
using UnityEngine;


namespace Library
{
    public class EcsComponentProvider<T> : BaseEcsComponentProvider where T : struct
    {
        [SerializeField] public T Component;


        public override void Prepare(EcsEntityGameObject entityGameObject)
        {
        }


        public override void AddComponentToEntity(EcsEntityGameObject entityGameObject)
        {
            entityGameObject.Entity.Replace(Component);
        }


#if UNITY_EDITOR
        private void OnValidate()
        {
            var hasSerializableAttribute = Attribute.IsDefined(typeof(T), typeof(SerializableAttribute));
            if (!hasSerializableAttribute)
            {
                throw new UnityException($"Component {typeof(T)} must have serializable attribute");
            }
        }
#endif
    }
}