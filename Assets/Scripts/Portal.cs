using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	void Start() {
		gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider other) {
		GameLogic logic = GameObject.FindObjectOfType<GameLogic>();
		logic.levelFinished = true;
		logic.PlayerControls[0].targetCamPosAnim.SetTrigger("Enabled");
	}
}
