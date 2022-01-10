using UnityEngine;


namespace Library
{
    public class OnTriggerExitView : BaseOnInteractionEventView
    {
        private void OnTriggerExit(Collider collider)
        {
            CollisionsService.TryToSendTriggerExitEvent(EntityGameObject.Entity, collider.gameObject);
        }
    }
}