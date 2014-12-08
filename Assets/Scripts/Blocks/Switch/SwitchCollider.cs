namespace Blocks {

    using System.Collections;

    using UnityEngine;

    using GameInfo;

    public class SwitchCollider : MonoBehaviour {

        public SwitchBlock parentNode;

        void OnTriggerEnter(Collider obj) {
            if(obj.tag == "Player" || obj.tag == "AI") {
                if(this.parentNode.isUp) {
                    this.parentNode.IsFlipped = true;
                    this.parentNode.MoveDown();
                } else {
                    this.parentNode.IsFlipped = true;
                    this.parentNode.MoveUp();
                }
            }
        }

        void Awake() {
            if(this.parentNode != null)
                return;

            var temp = this.transform.parent.gameObject.GetComponent<SwitchBlock>() as SwitchBlock;
            if(temp == null)
                return;

            this.parentNode = temp;
        }
    }
}