using UnityEngine;
using System.Collections;

public class RagdollPhysics : MonoBehaviour {

	public Rigidbody spineRigidbody;

	void Start () {
		spineRigidbody.AddForce((-transform.forward+transform.up)*Random.Range(300f, 600f));
	}

}
