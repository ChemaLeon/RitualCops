using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	void Start() {
		gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Player") {
			GameLogic logic = GameObject.FindObjectOfType<GameLogic>();
			logic.levelFinished = true;
			logic.PlayerControls[0].targetCamPosAnim.SetTrigger("Enabled");
			logic.audioServices.PlaySFX("Huh2", 1f);
			logic.audioServices.PlayBGM("CelebrationLoop");
		}
	}
}
