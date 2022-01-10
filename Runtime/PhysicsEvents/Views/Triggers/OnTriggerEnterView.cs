using UnityEngine;


namespace Library
{
    public class OnTriggerEnterView : BaseOnInteractionEventView
    {
        private void OnTriggerEnter(Collider collider)
        {
            CollisionsService.TryToSendTriggerEnterEvent(EntityGameObject.Entity, collider.gameObject);
        }
    }
}