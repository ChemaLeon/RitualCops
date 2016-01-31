using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour {
	
	public Camera mainCam;
	public CameraShake CameraShake;
	public List<PlayerControl> PlayerControls;
	public List<EnemyControl> EnemyControls;
	public CanvasManager CanvasManager;
	public bool levelFinished = false;

	public void InitializePlayerList() {
		PlayerControls = new List<PlayerControl>();
	}

	public void InitializeEnemyList() {
		EnemyControls = new List<EnemyControl>();
	}

	public void RespawnLevel() {
		StartCoroutine(Respawn());
	}

	IEnumerator Respawn() {
		yield return new WaitForSeconds(1f);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		PlayerControls = new List<PlayerControl>();
		EnemyControls = new List<EnemyControl>();
	}
}
