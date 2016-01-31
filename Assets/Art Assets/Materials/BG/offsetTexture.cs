using UnityEngine;
using System.Collections;

public class offsetTexture : MonoBehaviour {

	Material MAT;
	public float speed = 1f;

	// Use this for initialization
	void Start () {

		MAT = GetComponent<Renderer>().material;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		MAT.SetTextureOffset("_MainTex", new Vector2(Time.time*speed,0f));

	}
}
