using UnityEngine;
using System.Collections;

public class ImpactEffect : MonoBehaviour {

	public GameObject meatyParticleObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision other) {
		if (other.collider.tag == "Bullet") {
			BulletObject obj = other.collider.GetComponent<BulletObject>();
			if (obj != null) Destroy(obj.gameObject);
			//Destroy(gameObject);
			if(meatyParticleObject != null)
			Instantiate(meatyParticleObject, transform.position, meatyParticleObject.transform.rotation);

		}
	}
}
