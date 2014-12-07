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
                obj.GetComponent<PlayerController>().isStunned = true;
                this.parentNode.isEnabled = true;
                this.parentNode.blockMaterials[1] = this.parentNode.stunUpMaterial;
                this.parentNode.blockRenderer.materials = this.parentNode.blockMaterials;
            } else if(obj.tag == "AI") {
                obj.GetComponent<AIController>().isStunned = true;
                this.parentNode.isEnabled = true;
                this.parentNode.blockMaterials[1] = this.parentNode.stunUpMaterial;
                this.parentNode.blockRenderer.materials = this.parentNode.blockMaterials;
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