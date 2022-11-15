using UnityEngine;


namespace Library
{
    public class TriggerEnterMediator : MonoBehaviour
    {
        public EcsEntityGameObject EntityGameObject;
        
        
        protected virtual void Awake()
        {
            EntityGameObject = GetComponentInParent<EcsEntityGameObject>();
        }
    }
}