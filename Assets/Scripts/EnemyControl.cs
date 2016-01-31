using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

	public float movementSpeed = 5f;
	public GameObject meatyParticleObject;
	public GameObject bulletObject;
	public float fireCooldown = 1f;

	private bool makeRegdoll = true;
	private float dodgingFrequency = 600f;
	private float shootingFrequency = 400f;
	private float dodgeScale = 0.05f;

	private GameLogic GameLogic;
	private PlayerControl target;
	private Rigidbody Rigidbody;
	private EnemyState currentEnemyState;
	private float currentCooldown;

	public enum EnemyState {
		CHASING,
		SHOOTING,
		DODGING,
		NULL
	}

	void Awake() {
		GameLogic = GameObject.FindObjectOfType<GameLogic>();
		Rigidbody = GetComponent<Rigidbody>();
		currentEnemyState = EnemyState.CHASING;
	}

	void Start () {
		target = GameLogic.PlayerControls[0];
	}

	void Update () {
		Vector3 dodgeDirection = transform.right;
		if (target != null) {
			switch(currentEnemyState) {
			case(EnemyState.CHASING): {
					transform.LookAt(target.gameObject.transform);
					Rigidbody.velocity = transform.forward*movementSpeed;
					if (Random.Range(0f,1000f-shootingFrequency) < 1f) {
						currentEnemyState = EnemyState.SHOOTING;
					} else if (Random.Range(0f,1000f-dodgingFrequency) < 1f) {
						currentCooldown = dodgeScale;
						if (Random.Range(0,2) == 1) dodgeDirection = -transform.right;
						currentEnemyState = EnemyState.DODGING;
					}
				}
				break;
			case(EnemyState.SHOOTING): {
					transform.LookAt(target.gameObject.transform);
					if (currentCooldown <= 0f) {
						GameObject enemyBullet = Instantiate(bulletObject, transform.position+(transform.forward*2.5f), transform.rotation) as GameObject;
						enemyBullet.tag = "EnemyBullet";
						currentCooldown = fireCooldown;
						GameLogic.CameraShake.Shake(0.1f, 0.15f);
					}
					currentCooldown -= Time.deltaTime;
					if (currentCooldown <= 0f) currentCooldown = 0f;
					if (Random.Range(0f,200f) < 1f) {
						currentEnemyState = EnemyState.CHASING;
					} else if (Random.Range(0f,1000f-dodgingFrequency) < 1f) {
						currentCooldown = dodgeScale;
						if (Random.Range(0,2) == 1) dodgeDirection = -transform.right;
						currentEnemyState = EnemyState.DODGING;
					}
				}
				break;
			case(EnemyState.DODGING): {
					transform.LookAt(target.gameObject.transform);
					Rigidbody.velocity = movementSpeed*10f*dodgeDirection;
					currentCooldown -= Time.deltaTime;
					if (currentCooldown <= 0f) {
						currentCooldown = 0f;
						currentEnemyState = EnemyState.CHASING;
					}
				}
				break;
			}
		}

	}

	void OnCollisionEnter(Collision other) {
		if (other.collider.tag == "Bullet") {
			BulletObject obj = other.collider.GetComponent<BulletObject>();
			if (obj != null) Destroy(obj.gameObject);
			Destroy(gameObject);
			if (makeRegdoll){
                Instantiate(meatyParticleObject, transform.position, meatyParticleObject.transform.rotation);
                makeRegdoll = false;
            }
		}
	}
}
