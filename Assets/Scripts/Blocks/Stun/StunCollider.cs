namespace Blocks {

    using System.Collections;

    using UnityEngine;

    using AI;
    using Player;
    using GameInfo;

    public class StunCollider : MonoBehaviour {
        public StunBlock parentNode;

        void OnTriggerEnter(Collider obj) {
            if(obj.tag == "Player") {
                Debug.Log(obj.tag);
                obj.GetComponent<PlayerController>().isStunned = true;
            } else if(obj.tag == "AI") {
                Debug.Log(obj.tag);
                obj.GetComponent<AIController>().isStunned = true;
            }
        }

        void Awake() {
            if(this.parentNode != null)
                return;

            var temp = this.transform.parent.gameObject.GetComponent<StunBlock>() as StunBlock;
            if(temp == null)
                return;

            this.parentNode = temp;
        }
    }
}