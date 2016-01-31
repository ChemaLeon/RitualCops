﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControl : MonoBehaviour {

	public int PlayerNumber = 1;
	public float movementSpeed = 3f;
	public float fireCooldown = 1f;
	public int magazineSize = 8;
	public float knockbackFactor = 20f;
	public GameObject bulletObject;
	public GameObject meatyParticleObject;

	private Rigidbody Rigidbody;
	private string HorizontalAxisName;
	private string VerticalAxisName;
	private string FireButtonName;
	private string ReloadButtonName;
	private float currentCooldown = 0f;
	private int currentMagazineSize;
	private GameLogic GameLogic;
	private PlayerState currentState;

	public enum PlayerState {
		IDLE,
		RELOADING,
		NULL
	}

	void SetupPlatformDependentInput() {
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) {
			HorizontalAxisName = "MacLookHorizontal_"+PlayerNumber.ToString();
			VerticalAxisName = "MacLookVertical_"+PlayerNumber.ToString();
			FireButtonName = "MacFire_"+PlayerNumber.ToString();
			ReloadButtonName = "MacReload_"+PlayerNumber.ToString();
		} else {
			HorizontalAxisName = "LookHorizontal_"+PlayerNumber.ToString();
			VerticalAxisName = "LookVertical_"+PlayerNumber.ToString();
			FireButtonName = "Fire_"+PlayerNumber.ToString();
			ReloadButtonName = "Reload_"+PlayerNumber.ToString();
		}
	}

	void Awake() {
		SetupPlatformDependentInput();
		Rigidbody = GetComponent<Rigidbody>();
		GameLogic = GameObject.FindObjectOfType<GameLogic>();
	}

	void Start () {
		currentMagazineSize = magazineSize;
	}

	void Update () {
		MovePlayer();
		RotatePlayer();
		if (Input.GetButtonDown(ReloadButtonName)) {
			Reload();
		}
		if (Input.GetButton(FireButtonName) && currentCooldown <= 0f && currentState == PlayerState.IDLE) {
			Fire();
			currentCooldown = fireCooldown;
		}
		if (Input.GetButtonUp(FireButtonName)) {
			currentCooldown = 0f;
		}
		currentCooldown -= Time.deltaTime;
		if (currentCooldown <= 0f) currentCooldown = 0f;
	}

	void Fire() {
		if (currentMagazineSize > 0) {
			currentMagazineSize--;
			if (currentMagazineSize <= 0) {
				GameLogic.CanvasManager.ReloadAnimator.SetBool("Enabled", true);
			}
			GameLogic.CanvasManager.BulletBar.SetBulletIconStatus(currentMagazineSize,false);
			Instantiate(bulletObject, transform.position+(transform.forward*2.5f), transform.rotation);
			GameLogic.CameraShake.Shake(0.1f, 0.15f);
			Rigidbody.AddForce(-transform.forward*knockbackFactor);
		} else {
			Reload();
		}
	}

	void Reload() {
		if (currentState != PlayerState.RELOADING) {
			currentState = PlayerState.RELOADING;
			GameLogic.CanvasManager.ReloadBarAnimator.SetTrigger("Enabled");
		}
	}

	public void FinishReload() {
		currentState = PlayerState.IDLE;
		GameLogic.CanvasManager.BulletBar.ResetBulletIcons();
		currentMagazineSize = magazineSize;
		GameLogic.CanvasManager.ReloadAnimator.SetBool("Enabled", false);
	}

	void MovePlayer() {
		Vector3 cameraPosition = new Vector3(GameLogic.MainCamera.transform.position.x, 0f, GameLogic.MainCamera.transform.position.z);
		Vector3 playerPosition = new Vector3(transform.position.x, 0f, transform.position.z);
		Vector3 relationToCamera = (playerPosition-cameraPosition).normalized;
		Rigidbody.velocity = (GameLogic.MainCamera.transform.right*Input.GetAxis("Horizontal_1")+relationToCamera*Input.GetAxis("Vertical_1"))*movementSpeed;
	}

	void RotatePlayer() {
		float RightStickHorizontal = Input.GetAxis(HorizontalAxisName);
		float RightStickVertical = Input.GetAxis(VerticalAxisName);
		float TargetRotationAngle = Vector2.Angle(new Vector2(0f,1f), new Vector2(RightStickHorizontal, RightStickVertical).normalized);
		if (RightStickHorizontal < 0f) TargetRotationAngle = -TargetRotationAngle;
		if (Mathf.Abs(RightStickHorizontal) > 0.1f || Mathf.Abs(RightStickVertical) > 0.1f) {
			Quaternion cameraRotation = Quaternion.Euler(0f, GameLogic.MainCamera.transform.rotation.eulerAngles.y, 0f);
			transform.rotation = cameraRotation*Quaternion.Euler(new Vector3(0f, TargetRotationAngle, 0f));
		}
	}

	void OnCollisionEnter(Collision other) {
		if (other.collider.tag == "EnemyBullet") {
			BulletObject obj = other.collider.GetComponent<BulletObject>();
			if (obj != null) Destroy(obj.gameObject);
			Instantiate(meatyParticleObject, transform.position, meatyParticleObject.transform.rotation);
			GameLogic.RespawnLevel();
			gameObject.SetActive(false);
		}
	}
}
