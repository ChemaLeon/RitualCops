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
	public AudioServices audioServices;
	public bool levelFinished = false;

	private float lastVoiceCountdown = 5f;
	private float voiceCountdown = 0f;

	void Start() {
		audioServices = GameObject.FindObjectOfType<AudioServices>();
		audioServices.PlayBGM("BGM");
	}

	void Update() {
		voiceCountdown -= Time.deltaTime;
		if (voiceCountdown <= 0f) voiceCountdown = 0f;
	}

	public void RandomKillSFX() {
		if (voiceCountdown <= 0f && Random.Range(0,100) <= 90) {
			audioServices.PlaySFX("Kill"+Random.Range(1,13), 0.95f, 1f);
			voiceCountdown = lastVoiceCountdown;
		}
	}

	public void RandomDamageSFX() {
		if (voiceCountdown <= 0f && Random.Range(0,100) <= 30) {
			audioServices.PlaySFX("Damage"+Random.Range(1,6));
			voiceCountdown = lastVoiceCountdown;
		}
	}

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
