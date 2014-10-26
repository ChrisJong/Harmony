namespace MainMenu {

	using UnityEngine;
	using System.Collections;

	using GameInfo;

	public class CreditsController : MonoBehaviour {

		public string[] credText;
		public Vector3 startPos;
		public Vector3 endPos;
		public Vector3 fadeOutPos;
		public Vector3 fadeInPos;

		public int cycle;
		private Vector3 currentPos;
		private TextMesh creditsText;
		private Color credColor;

		void Start(){
			currentPos = transform.position;
			creditsText = this.transform.GetComponent<TextMesh>() as TextMesh;
			credColor = renderer.material.color;
		}

		void rollCredits (){
			currentPos = (currentPos + (new Vector3 (0, 0.01f, 0)));
			transform.position = currentPos;
			if ((currentPos.y > fadeOutPos.y) && (credColor.a != 0)) {
				credColor.a -= 0.04f;
			}

			if ((currentPos.y < fadeInPos.y) && (credColor.a != 1)) {
				credColor.a += 0.04f;
			}

			renderer.material.color = credColor;

			if (currentPos == endPos) {
				currentPos = startPos;
				cycle++;
				if (cycle == (credText.Length)) {
					cycle = 0;
				}
			creditsText.text = credText[cycle];
			}
		}

		void restartCredits(){
			cycle = 0;
			creditsText.text = credText[cycle];
			currentPos = startPos;
			credColor.a = 1.0f;
		}
		
		void Update () {
			if (MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.CREDITS) {
				rollCredits();
			} else {
				restartCredits();
			}
		}
	}
}