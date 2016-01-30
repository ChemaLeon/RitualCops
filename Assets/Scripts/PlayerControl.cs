using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public int PlayerNumber = 1;
	public float movementSpeed = 3f;

	private Rigidbody _rigidbody;

	void Awake() {
		_rigidbody = GetComponent<Rigidbody>();
	}

	void Start () {
	
	}

	void Update () {
		_rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).normalized*movementSpeed;
	}
}
