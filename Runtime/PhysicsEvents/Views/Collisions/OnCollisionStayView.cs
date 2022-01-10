using UnityEngine;


namespace Library
{
    public class OnCollisionStayView : BaseOnInteractionEventView
    {
        private void OnCollisionStay(Collision collision)
        {
            CollisionsService.TryToSendCollisionStayEvent(EntityGameObject.Entity, collision.gameObject);
        }
    }
}