using UnityEngine;
using System.Collections;

public class ReloadBar : MonoBehaviour {

	private GameLogic GameLogic;

	void Awake() {
		GameLogic = GameLogic.Instance;
	}

	public void FinishReloading() {
		//TODO: Per Player Reloading?
		GameLogic.PlayerControls[0].FinishReload();
	}
}
