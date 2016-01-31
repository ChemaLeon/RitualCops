using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	GameLogic logic;

	void Start() {
		gameObject.SetActive(false);
		logic = GameObject.FindObjectOfType<GameLogic>();
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			logic.audioServices.PlaySFX("Huh2", 1f);
			logic.audioServices.PlayBGM("CelebrationLoop");
			StartCoroutine(LoadNextScene());
		}
	}

	IEnumerator LoadNextScene() {
		logic.levelFinished = true;
		logic.PlayerControls[0].targetCamPosAnim.SetTrigger("Enabled");
		yield return new WaitForSeconds(7f);
		logic.CanvasManager.SetFade(true);
		if (logic.CanvasManager.currentLevel < 6) {
			logic.CanvasManager.LoadScene("Level_0"+(logic.CanvasManager.currentLevel+1));
		} else {
			logic.CanvasManager.LoadScene("Score");
		}
	}
}
