namespace Animation {
    using System.Collections;

    using UnityEngine;

    using Grid;

    public class EndAnimation : MonoBehaviour {

        /*private bool _moveCamera;
        private GameObject _mainCamera;

        void Awake() {
            this._mainCamera = Camera.main.gameObject;
        }*/

        void Update() {
            if(!GridController.instance.EndMenuActive) {
                this.transform.position = Vector3.Lerp(this.transform.position, new Vector3(this.transform.position.x, this.transform.position.y + 1.0f, this.transform.position.z), Time.deltaTime * 2.0f);
                this.transform.Rotate(0.0f, 50.0f * Time.deltaTime, 0.0f);

                /*if(this._moveCamera)
                    this._mainCamera.transform.position = Vector3.Lerp(this._mainCamera.transform.position, new Vector3(this._mainCamera.transform.position.x, this._mainCamera.transform.position.y + this.transform.position.y, this._mainCamera.transform.position.z), Time.deltaTime * 0.5f);

                if(this.transform.position.y > (Camera.main.transform.position.y - 3.0f))
                    this._moveCamera = true;*/
            } else
                Destroy(this.gameObject);
        }
    }
}