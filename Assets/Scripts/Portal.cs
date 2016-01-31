﻿using UnityEngine;
using System.Collections;

public class Portal : MonoBehaviour {

	void Start() {
		gameObject.SetActive(false);
	}

	void OnTriggerEnter(Collider other) {
		GameObject.FindObjectOfType<GameLogic>().levelFinished = true;
	}
}
