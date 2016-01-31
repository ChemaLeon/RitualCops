using UnityEngine;
using System.Collections;

public class BGCamera : MonoBehaviour {

	Transform statPosition;

	// Use this for initialization
	void Start () {

		statPosition = transform;
	
	}
	
	// Update is called once per frame
	void Update () {

		transform.position = statPosition.position + new Vector3(0f,Mathf.Sin(Time.time*2f)*0.05f,Mathf.Sin(Time.time)*0.1f);
		transform.eulerAngles = statPosition.eulerAngles + new Vector3(0f,0f,(Mathf.Sin(Time.time*1.2f))*0.5f);
	
	}
}
