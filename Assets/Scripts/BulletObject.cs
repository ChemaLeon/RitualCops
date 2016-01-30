using UnityEngine;
using System.Collections;

public class BulletObject : MonoBehaviour {

	public float bulletSpeed = 20f;
	public float lifetime = 3f;

	void Update () {
		transform.position += transform.forward*bulletSpeed*Time.deltaTime;
		lifetime -= Time.deltaTime;
		if (lifetime <= 0f) Destroy(gameObject);
	}

	public void Destroy() {
		Destroy(gameObject);
	}
}
