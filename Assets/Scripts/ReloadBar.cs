using UnityEngine;
using System.Collections;

public class ReloadBar : MonoBehaviour {

	private GameLogic GameLogic;
	public int playerNumber;

	void Awake() {
		GameLogic = GameObject.FindObjectOfType<GameLogic>();
	}

	public void FinishReloading() {
		//TODO: Per Player Reloading?
		GameLogic.PlayerControls[playerNumber].FinishReload();
	}
}
