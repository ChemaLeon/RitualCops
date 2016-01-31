using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScreenTime : MonoBehaviour {

	public Text textComponent;

	// Use this for initialization
	void Update () {
		TimerObject tObject = GameObject.FindObjectOfType<TimerObject>();
		if (tObject != null) {
			tObject.timerEnabled = false;
			textComponent.text = TimeToStringFormat(tObject.totalTime);
		}
	}

	string TimeToStringFormat(float time) {
		string result = Mathf.Floor ((time / 60f) % 60f).ToString ("0") + "."; // Minutes
		result += Mathf.Floor ((time % 60f)).ToString ("00") + "."; // Seconds
		result += ((time * 1000f) % 1000f).ToString ("000"); // Milliseconds
		return result;
	}
}
