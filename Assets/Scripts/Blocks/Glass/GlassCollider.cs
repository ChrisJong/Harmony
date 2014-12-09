namespace Blocks {

    using System.Collections;

    using UnityEngine;

    using AI;
    using Player;
    using GameInfo;

    public class GlassCollider : MonoBehaviour {

        public MeshRenderer blockRenderer;
        public BoxCollider blockCollider;

        //public List<Material> glassUpMaterials;
        //public List<Material> glassDownMaterials;

        public GlassBlock parentNode;

        void Awake() {
            if(this.parentNode != null)
                return;

            var temp = this.transform.parent.gameObject.GetComponent<GlassBlock>() as GlassBlock;
            if(temp == null)
                return;

            this.parentNode = temp;
            this.parentNode.glassBlock = this.gameObject;
            this.blockCollider = this.transform.GetComponent<BoxCollider>() as BoxCollider;
        }

        public void BreakApart() {
            this.blockCollider.enabled = false;
            this.blockRenderer.enabled = false;
        }

        public void UndoBreakApart() {
            this.blockRenderer.enabled = true;
            this.blockCollider.enabled = true;
        }

        public void Undo() {

        }

        public void MoveDown() {
            this.parentNode.previousBreakCount = this.parentNode.breakCount;
            this.parentNode.breakCount--;

            if(this.parentNode.breakCount == 0)
                this.BreakApart();
            else {
                Debug.Log(this.parentNode.breakCount);
            }
        }
    }
}