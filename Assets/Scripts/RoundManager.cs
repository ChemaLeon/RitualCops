using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour {

	private List<EnemyControl> enemies;
	public GameObject portal;

	// Use this for initialization
	void Start () {
		enemies = new List<EnemyControl>();
		foreach (EnemyControl enemy in GameObject.FindObjectsOfType<EnemyControl>()) {
			enemies.Add(enemy);
		}
	}	

	public void EnemyDied(EnemyControl who) {
		enemies.Remove(who);
		checkEnemies();
	}

	void activatePortal() {
		portal.SetActive(true);
	}

	void checkEnemies() {
		if (enemies.Count <= 0) {
			activatePortal();
		}
	}
		
}
