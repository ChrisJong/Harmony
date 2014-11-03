namespace Animation {
    using System.Collections;

    using UnityEngine;

    using Grid;

    public class EndAnimation : MonoBehaviour {
        void Update() {
            if(!GridController.instance.EndMenuActive) {
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y + 1.0f, this.transform.position.z), Time.deltaTime * 2.0f);
                this.transform.Rotate(0.0f, 50.0f * Time.deltaTime, 0.0f); 
            } else
                Destroy(this.gameObject);
        }
    }
}