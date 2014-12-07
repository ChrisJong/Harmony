namespace Blocks {

    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;

    using Grid;
    using AI;
    using Player;
    using GameInfo;

    public class WarpCollider : MonoBehaviour {

        public WarpBlock parentNode;
        public GameObject warpNode;

        public BoxCollider warpNodeCollider;

        void OnTriggerEnter(Collider obj) {
            if(obj.tag == "Player") {
                if(PlayerController.instance.CurrentDirection == parentNode.WarpDirection) {
                    this.warpNodeCollider.enabled = false;
                    this.parentNode.blockMaterials[1] = this.parentNode.warpUpMaterials[this.parentNode.firstDirectionValue - 1];
                    this.parentNode.blockRenderer.materials = this.parentNode.blockMaterials;
                    if(this.warpNode.GetComponent<WarpBlock>().isUp) {
                        obj.transform.position = new Vector3(this.warpNode.transform.position.x, this.warpNode.transform.position.y + this.transform.position.y + 2.5f, this.warpNode.transform.position.z + 0.5f);
                    } else {
                        obj.transform.position = new Vector3(this.warpNode.transform.position.x, this.warpNode.transform.position.y + this.transform.position.y + 1.0f, this.warpNode.transform.position.z + 0.5f);
                    }
                }
            } else if(obj.tag == "AI") {
                if(AIController.instance.CurrentDirection == parentNode.WarpDirection) {
                    this.warpNodeCollider.enabled = false;
                    this.parentNode.blockMaterials[1] = this.parentNode.warpUpMaterials[this.parentNode.firstDirectionValue - 1];
                    this.parentNode.blockRenderer.materials = this.parentNode.blockMaterials;
                    if(this.warpNode.GetComponent<WarpBlock>().isUp) {
                        obj.transform.position = new Vector3(this.warpNode.transform.position.x, this.warpNode.transform.position.y + this.transform.position.y + 2.5f, this.warpNode.transform.position.z + 0.5f);
                    } else {
                        obj.transform.position = new Vector3(this.warpNode.transform.position.x, this.warpNode.transform.position.y + this.transform.position.y + 1.0f, this.warpNode.transform.position.z + 0.5f);
                    }
                }
            }
        }

        void Awake() {
            if(this.parentNode != null)
                return;

            var temp = this.transform.parent.gameObject.GetComponent<WarpBlock>() as WarpBlock;
            if(temp == null)
                return;

            this.parentNode = temp;
            this.warpNode = parentNode.warpNode;
            this.warpNodeCollider = this.warpNode.transform.GetChild(0).GetComponent<BoxCollider>();
        }
    }
}