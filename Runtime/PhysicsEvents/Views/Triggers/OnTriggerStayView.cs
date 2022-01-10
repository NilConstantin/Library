using UnityEngine;


namespace Library
{
    public class OnTriggerStayView : BaseOnInteractionEventView
    {
        private void OnTriggerStay(Collider collider)
        {
            CollisionsService.TryToSendTriggerStayEvent(EntityGameObject.Entity, collider.gameObject);
        }
    }
}