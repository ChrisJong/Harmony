namespace Blocks {

    using System.Collections;

    using UnityEngine;

    using AI;
    using Player;
    using GameInfo;

    public class GlassCollider : MonoBehaviour {

        public GlassBlock parentNode;

        void OnTriggerEnter(Collider obj) {
            if(obj.tag == "Player" || obj.tag == "AI") {
                this.parentNode.MoveDown();
            }
        }

        void Awake() {
            if(this.parentNode != null)
                return;

            var temp = this.transform.parent.gameObject.GetComponent<GlassBlock>() as GlassBlock;
            if(temp == null)
                return;

            this.parentNode = temp;
            this.parentNode.glassCollider = this.gameObject;
        }
    }
}