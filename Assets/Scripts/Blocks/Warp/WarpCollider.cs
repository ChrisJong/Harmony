namespace Blocks {

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    using GameInfo;

    public class WarpCollider : MonoBehaviour {

        public List<Material> warpUpMaterials;
        public List<Material> warpDownMaterials;

        public WarpBlock parentNode;

        void OnTriggerEnter(Collider obj) {
            if(obj.tag == "Player") {

            } else if(obj.tag == "AI") {

            }
        }

        void Awake() {
            if(this.parentNode != null)
                return;

            var temp = this.transform.parent.gameObject.GetComponent<WarpBlock>() as WarpBlock;
            if(temp == null)
                return;

            this.parentNode = temp;
        }
    }
}