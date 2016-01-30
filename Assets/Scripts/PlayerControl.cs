using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public int PlayerNumber = 1;

	private Rigidbody _rigidbody;

	void Awake() {
		_rigidbody = GetComponent<Rigidbody>();
	}

	void Start () {
	
	}

	void Update () {
		_rigidbody.velocity = new Vector3(Input.GetAxis("Horizontal_1"), 0f, Input.GetAxis("Vertical_1"));
	}
}
