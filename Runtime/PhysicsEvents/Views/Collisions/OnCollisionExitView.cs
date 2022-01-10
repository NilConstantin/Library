using UnityEngine;


namespace Library
{
    public class OnCollisionExitView : BaseOnInteractionEventView
    {
        private void OnCollisionExit(Collision collision)
        {
            CollisionsService.TryToSendCollisionExitEvent(EntityGameObject.Entity, collision.gameObject);
        }
    }
}