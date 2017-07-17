namespace MainMenu {

	using UnityEngine;
	using System.Collections;

	using GameInfo;

	public class CreditsController : MonoBehaviour {

        public static CreditsController instance;

        public GameObject emptyBlockContainer;

		public GameObject teamLogo;
		public GameObject credRole;
		public GameObject credName;
		public string[] roleText;
		public string[] nameText;

		public Vector3 roleStartPos;
		public Vector3 roleEndPos;
		public Vector3 roleFadeOutPos;
		public Vector3 roleFadeInPos;
		private Vector3 rolePos;
		public Vector3 nameStartPos;
		private Vector3 namePos;

		private int cycle;
		private TextMesh roleTextMesh;
		private TextMesh nameTextMesh;
		private Color credColor;
		private Color teamLogoColor;
		private float timer;

        void Awake() {
            instance = this;

            rolePos = credRole.transform.position;
            namePos = credName.transform.position;
            roleTextMesh = credRole.transform.GetComponent<TextMesh>() as TextMesh;
            nameTextMesh = credName.transform.GetComponent<TextMesh>() as TextMesh;
            credColor = credRole.GetComponent<Renderer>().material.color;
            teamLogoColor = teamLogo.GetComponent<Renderer>().material.color;

            this.restartCredits();
        }

        void Update() {
            if(MainMenuController.instance.currentMenuScene == MainMenuInfo.MenuTypes.CREDITS) {
                timer += Time.deltaTime;
                logoShow();
                if(timer >= 5) {
                    rollCredits();
                }
            } else {
                restartCredits();
            }
        }

		private void logoShow(){
			if ((timer > 0.25f) && timer < 2.75f) {
				teamLogoColor.a += 0.01f;
				teamLogo.GetComponent<Renderer>().material.color = teamLogoColor;
			} else {
				teamLogoColor.a -= 0.01f;
				teamLogo.GetComponent<Renderer>().material.color = teamLogoColor;
			}
		}

        private void rollCredits() {
			rolePos = (rolePos + (new Vector3 (0, 0.01f, 0)));
			namePos = (namePos + (new Vector3 (0, 0.01f, 0)));
			credRole.transform.position = rolePos;
			credName.transform.position = namePos;
			if ((rolePos.y > roleFadeOutPos.y) && (credColor.a >= 0)) {
				credColor.a -= 0.02f;
			}

			if ((rolePos.y < roleFadeInPos.y) && (credColor.a <= 1)) {
				credColor.a += 0.02f;
			}

			credRole.GetComponent<Renderer>().material.color = credColor;
			credName.GetComponent<Renderer>().material.color = credColor;

			if (rolePos == roleEndPos) {
				rolePos = roleStartPos;
				namePos = nameStartPos;
				cycle++;
				if (cycle == (roleText.Length)) {
					restartCredits();
				}
				roleTextMesh.text = roleText[cycle];
				nameTextMesh.text = nameText[cycle];
			}
		}

        private void restartCredits() {
			cycle = 0;
			roleTextMesh.text = roleText [cycle];
			nameTextMesh.text = nameText [cycle];
			rolePos = roleStartPos;
			credRole.transform.position = rolePos;
			namePos = nameStartPos;
			credName.transform.position = namePos;
			credColor.a = 0.0f;
			credRole.GetComponent<Renderer>().material.color = credColor;
			credName.GetComponent<Renderer>().material.color = credColor;
			teamLogoColor.a = 0.0f;
			timer = 0;
		}
	}
}