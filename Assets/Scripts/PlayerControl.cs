using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour {

	public int PlayerNumber = 1;
	public float movementSpeed = 3f;
	public float fireCooldown = 1f;
	public int magazineSize = 8;
	public GameObject bulletObject;

	private Rigidbody Rigidbody;
	private string HorizontalAxisName;
	private string VerticalAxisName;
	private string FireAxisName;
	private float currentCooldown = 0f;
	private int currentMagazineSize;

	void SetupPlatformDependentInput() {
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) {
			HorizontalAxisName = "MacLookHorizontal_"+PlayerNumber.ToString();
			VerticalAxisName = "MacLookVertical_"+PlayerNumber.ToString();
			FireAxisName = "MacFire_"+PlayerNumber.ToString();
		} else {
			HorizontalAxisName = "LookHorizontal_"+PlayerNumber.ToString();
			VerticalAxisName = "LookVertical_"+PlayerNumber.ToString();
			FireAxisName = "Fire_"+PlayerNumber.ToString();
		}
	}

	void Awake() {
		SetupPlatformDependentInput();
		Rigidbody = GetComponent<Rigidbody>();
	}

	void Start () {
		currentMagazineSize = magazineSize;
	}

	void Update () {
		MovePlayer();
		RotatePlayer();
		if (Input.GetAxis(FireAxisName) == 1f && currentCooldown <= 0f) {
			Fire();
			currentCooldown = fireCooldown;
		}
		currentCooldown -= Time.deltaTime;
		if (currentCooldown <= 0f) currentCooldown = 0f;
	}

	void Fire() {
		if (currentMagazineSize > 0) {
			currentMagazineSize--;
			Instantiate(bulletObject, transform.position, transform.rotation);
		}
	}

	void MovePlayer() {
		Rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal_1"), 0f, Input.GetAxis("Vertical_1")).normalized*movementSpeed;
	}

	void RotatePlayer() {
		float RightStickHorizontal = Input.GetAxis(HorizontalAxisName);
		float RightStickVertical = Input.GetAxis(VerticalAxisName);
		float TargetRotationAngle = Vector2.Angle(new Vector2(0f,1f), new Vector2(RightStickHorizontal, RightStickVertical).normalized);
		if (RightStickHorizontal < 0f) TargetRotationAngle = -TargetRotationAngle;
		if (Mathf.Abs(RightStickHorizontal) > 0.1f || Mathf.Abs(RightStickVertical) > 0.1f) {
			transform.rotation = Quaternion.Euler(new Vector3(0f, TargetRotationAngle, 0f));
		}
	}
}
