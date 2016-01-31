using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreScreenTime : MonoBehaviour {

	private Text textComponent;

	// Use this for initialization
	void Start () {
		textComponent.GetComponent<Text>();
		TimerObject tObject = GameObject.FindObjectOfType<TimerObject>();
		if (tObject != null) {
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
