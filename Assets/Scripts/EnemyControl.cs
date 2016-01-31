using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

	public float movementSpeed = 5f;
	public GameObject meatyParticleObject;

	private GameLogic GameLogic;
	private PlayerControl target;
	private Rigidbody Rigidbody;
	bool makeRegdoll = true;

	void Awake() {
		GameLogic = GameObject.FindObjectOfType<GameLogic>();
		Rigidbody = GetComponent<Rigidbody>();
	}

	void Start () {
		target = GameLogic.PlayerControls[0];
	}

	void Update () {
		transform.LookAt(target.gameObject.transform);
		Rigidbody.velocity = transform.forward*movementSpeed;
	}

	void OnCollisionEnter(Collision other) {
		if (other.collider.tag == "Bullet") {
			BulletObject obj = other.collider.GetComponent<BulletObject>();
			if (obj != null) Destroy(obj.gameObject);
			Destroy(gameObject);
			if(makeRegdoll){
				Instantiate(meatyParticleObject, transform.position, meatyParticleObject.transform.rotation);
				makeRegdoll = false;
			}
		}
	}
}
