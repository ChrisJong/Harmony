namespace Blocks {

    using System.Collections;

    using UnityEngine;

    using GameInfo;

    public class SwitchCollider : MonoBehaviour {

        public SwitchBlock _parentNode;

        void OnTriggerEnter(Collider obj) {
            if(obj.tag == "Player" || obj.tag == "AI") {
                if(this._parentNode.isUp) {
                    this._parentNode.BlockState = BlockInfo.BlockState.DOWN;
                } else {
                    this._parentNode.BlockState = BlockInfo.BlockState.UP;
                }
            }
        }

        void Awake() {
            if(this._parentNode != null)
                return;

            var temp = this.transform.parent.gameObject.GetComponent<SwitchBlock>() as SwitchBlock;
            if(temp == null)
                return;

            this._parentNode = temp;
        }
    }
}