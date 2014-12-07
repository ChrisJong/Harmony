namespace Blocks {

    using System.Collections;

    using UnityEngine;

    using GameInfo;

    public class SwitchCollider : MonoBehaviour {

        public SwitchBlock parentNode;

        void OnTriggerEnter(Collider obj) {
            if(obj.tag == "Player" || obj.tag == "AI") {
                if(this.parentNode.isUp) {
                    this.parentNode.BlockState = BlockInfo.BlockState.DOWN;
                    this.parentNode.PreviousState = BlockInfo.BlockState.UP;
                    this.parentNode.IsFlipped = true;
                } else {
                    this.parentNode.BlockState = BlockInfo.BlockState.UP;
                    this.parentNode.PreviousState = BlockInfo.BlockState.DOWN;
                    this.parentNode.IsFlipped = true;
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