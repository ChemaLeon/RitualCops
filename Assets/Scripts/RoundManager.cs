using UnityEngine;
using System.Collections;

public class RoundManager : MonoBehaviour {

	public GameObject[] enemies;
	bool everybodyDied = false;
	public GameObject portal;

	// Use this for initialization
	void Start () {

		portal.SetActive(false);
	
	}
	
	// Update is called once per frame
	void Update () {
		checkEnemies();
		if(everybodyDied)
		{
			activatePortal();
		}
	
	}

	void activatePortal()
	{
		portal.SetActive(true);
	}

	void checkEnemies()
	{
		foreach(var enemy in enemies)
		{
			if(enemy != null)
			{
				return;
			}

		}
		everybodyDied = true;
	}
		
}
