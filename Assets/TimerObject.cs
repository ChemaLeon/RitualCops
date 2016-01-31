using UnityEngine;
using System.Collections;

public class TimerObject : MonoBehaviour {

	private CanvasManager canvas;
	public float totalTime = 0f;
	public bool timerEnabled = true;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (timerEnabled) {
			totalTime+=Time.deltaTime;
		}
		if (canvas == null) {
			canvas = GameObject.FindObjectOfType<CanvasManager>();
		} else {
			canvas.timeText.text = TimeToStringFormat(totalTime);
		}
	}

	string TimeToStringFormat(float time) {
		string result = Mathf.Floor ((time / 60f) % 60f).ToString ("0") + "."; // Minutes
		result += Mathf.Floor ((time % 60f)).ToString ("00") + "."; // Seconds
		result += ((time * 1000f) % 1000f).ToString ("000"); // Milliseconds
		return result;
	}
}
