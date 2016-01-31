using UnityEngine;
using System.Collections;

public class EnemyControl : MonoBehaviour {

	public float movementSpeed = 5f;
	public GameObject meatyParticleObject;
	public GameObject bulletObject;
	public float fireCooldown = 1.5f;
	public Transform shootFrom;
	public ParticleSystem shootParticle;

	private bool makeRegdoll = true;
	private float dodgingFrequency = 800f;
	private float shootingFrequency = 400f;
	private float dodgeScale = 0.2f;
	private float minDistanceToShoot = 12f;
	private float minDistanceToStop = 8f;
	private float minDistanceToAwaken = 20f;

	private GameLogic logic;
	private PlayerControl target;
	private Rigidbody Rigidbody;
	private EnemyState currentEnemyState;
	private float currentCooldown;
	private bool chargingShot = false;

	public enum EnemyState {
		CHASING,
		SHOOTING,
		DODGING,
		IDLE,
		NULL
	}

	void Awake() {
		logic = GameObject.FindObjectOfType<GameLogic>();
		Rigidbody = GetComponent<Rigidbody>();
		currentEnemyState = EnemyState.IDLE;
		logic.EnemyControls.Add(this);
	}

	public void SetTarget(PlayerControl newTarget) {
		target = newTarget;
	}

	void FixedUpdate () {
		Vector3 dodgeDirection = transform.right;
		if (target != null) {
			switch(currentEnemyState) {
			case(EnemyState.IDLE): {
					Rigidbody.velocity = Vector3.zero;
					if (Vector3.Distance(transform.position, target.transform.position) < minDistanceToAwaken) {
						currentEnemyState = EnemyState.CHASING;
					}
				}
				break;
			case(EnemyState.CHASING): {
					transform.LookAt(target.gameObject.transform);
					Rigidbody.velocity = transform.forward*movementSpeed;
					if (Random.Range(0f,1000f-shootingFrequency) < 1f || Vector3.Distance(transform.position, target.transform.position) < 5f) {
						currentEnemyState = EnemyState.SHOOTING;
					} else if (Random.Range(0f,1000f-dodgingFrequency) < 1f) {
						currentCooldown = dodgeScale;
						if (Random.Range(0,2) == 1) dodgeDirection = -transform.right;
						currentEnemyState = EnemyState.DODGING;
					} else if (Vector3.Distance(transform.position, target.transform.position) > minDistanceToAwaken) {
						currentEnemyState = EnemyState.IDLE;
					}
				}
				break;
			case(EnemyState.SHOOTING): {
					transform.LookAt(target.gameObject.transform);
					Rigidbody.velocity = transform.forward*movementSpeed*0.1f;
					if (Vector3.Distance(transform.position, target.transform.position) < minDistanceToShoot) {
						if (currentCooldown <= 0f) {
							currentCooldown = fireCooldown;
							StartCoroutine(Fire());
						}
					}

					if (!chargingShot) {
						if (Random.Range(0f,200f) < 1f) {
							currentEnemyState = EnemyState.CHASING;
						} else if (Random.Range(0f,1000f-dodgingFrequency) < 1f) {
							currentCooldown = dodgeScale;
							if (Random.Range(0,2) == 1) dodgeDirection = -transform.right;
							currentEnemyState = EnemyState.DODGING;
						}
					}

					currentCooldown -= Time.deltaTime;
					if (currentCooldown <= 0f) currentCooldown = 0f;
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

	IEnumerator Fire() {
		shootParticle.Play(true);
		chargingShot = true;
		yield return new WaitForSeconds(1f);
		GameObject enemyBullet = Instantiate(bulletObject, shootFrom.position, transform.rotation) as GameObject;
		enemyBullet.tag = "EnemyBullet";
		logic.CameraShake.Shake(0.1f, 0.15f);
		chargingShot = false;
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
			RoundManager round = GameObject.FindObjectOfType<RoundManager>();
			if (round != null) {
				round.EnemyDied(this);
				logic.EnemyControls.Remove(this);
			}
		}
	}
}
