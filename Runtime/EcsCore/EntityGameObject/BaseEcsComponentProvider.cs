using UnityEngine;


namespace Library
{
    [RequireComponent(typeof(EcsEntityGameObject))]
    public abstract class BaseEcsComponentProvider : MonoBehaviour
    {
        public abstract void Prepare(EcsEntityGameObject entityGameObject);
        
        public abstract void AddComponentToEntity(EcsEntityGameObject entityGameObject);
    }
}