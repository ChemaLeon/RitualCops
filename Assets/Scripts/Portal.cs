using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	void Start() {
		gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			StartCoroutine(LoadNextScene());
		}
	}

	IEnumerator LoadNextScene() {
		GameLogic logic = GameObject.FindObjectOfType<GameLogic>();
		logic.levelFinished = true;
		logic.PlayerControls[0].targetCamPosAnim.SetTrigger("Enabled");
		logic.audioServices.PlaySFX("Huh2", 1f);
		logic.audioServices.PlayBGM("CelebrationLoop");
		yield return new WaitForSeconds(6.5f);
		logic.CanvasManager.SetFade(true);
		if (logic.CanvasManager.currentLevel < 7) {
			logic.CanvasManager.LoadScene("Level_0"+(logic.CanvasManager.currentLevel+1));
		} else {
			logic.CanvasManager.LoadScene("Start");
		}
	}
}
