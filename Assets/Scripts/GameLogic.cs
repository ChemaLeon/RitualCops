using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour {
	
	public Camera MainCamera;
	public CameraShake CameraShake;
	public PlayerControl[] PlayerControls;
	public CanvasManager CanvasManager;

	public void RespawnLevel() {
		StartCoroutine(Respawn());
	}

	IEnumerator Respawn() {
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene("Level_01");
	}
}
