using UnityEngine;


namespace Library
{
    public class OnCollisionEnterView : BaseOnInteractionEventView
    {
        private void OnCollisionEnter(Collision collision)
        {
            CollisionsService.TryToSendCollisionEnterEvent(EntityGameObject.Entity, collision.gameObject);
        }
    }
}