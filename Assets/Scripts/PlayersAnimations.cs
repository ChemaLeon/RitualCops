using UnityEngine;
using System.Collections;

public class PlayersAnimations : MonoBehaviour {

	public Animator anim;
	float walkSpeed = 0f;
	float walkDir = 0f;
	public GameObject player;
	Vector3 dir;
	Rigidbody rb;

	// Use this for initialization
	void Start () {

		rb = player.GetComponent<Rigidbody>();
		dir = player.transform.forward;
	}
	
	// Update is called once per frame
	void Update () {

		if( rb.velocity.magnitude > 0.1f)
		{
			if(Vector3.Dot(rb.velocity.normalized,player.transform.forward) >= -0.1f)
			{
				dir = rb.velocity.normalized;
				walkDir = 1f;
			}
			else
			{
				dir = -rb.velocity.normalized;
				walkDir = -1f;
			}
		}

		transform.forward = Vector3.Lerp(transform.forward,dir,Time.deltaTime*5f);
		walkSpeed = Mathf.Lerp(walkSpeed, rb.velocity.magnitude,Time.deltaTime*10f)*walkDir;
		anim.SetFloat("walk",walkSpeed);

	}
}
