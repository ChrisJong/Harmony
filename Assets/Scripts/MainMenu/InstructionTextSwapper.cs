namespace MainMenu {

    using System.Collections;

    using UnityEngine;

    public class InstructionTextSwapper : MonoBehaviour {

        private TextMesh _instructions;

        void Start() {
            this._instructions = this.GetComponent<Renderer>().GetComponent<TextMesh>() as TextMesh;
#if UNITY_IPHONE || UNITY_ANDROID
		this._instructions.text = "Move Yin by swiping the screen";
#else
            this._instructions.text = "Move Yin with your direction arrows";
#endif
        }
    }
}