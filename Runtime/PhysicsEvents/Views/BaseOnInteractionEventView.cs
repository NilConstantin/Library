using UnityEngine;


namespace Library
{
    public class BaseOnInteractionEventView : MonoBehaviour
    {
        [SerializeField] private bool isNestedCollider;

        protected CollisionsService CollisionsService;
        protected EcsEntityGameObject EntityGameObject;


        protected virtual void Awake()
        {
            CollisionsService = Service<CollisionsService>.Get();

            if (isNestedCollider)
            {
                EntityGameObject = GetComponentInParent<EcsEntityGameObject>();
            }
            else
            {
                EntityGameObject = GetComponent<EcsEntityGameObject>();
            }

            if (EntityGameObject == null)
            {
                throw new UnityException($"Collision event view must contains {nameof(EcsEntityGameObject)}");
            }
        }
    }
}