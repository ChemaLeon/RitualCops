using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	// How long the object should shake for.
	private float shake = 0f;

	// Amplitude of the shake. A larger value shakes the camera harder.
	private float shakeAmount = 0.7f;
	private float decreaseFactor = 1.0f;

	Vector3 originalPos;

	public void Shake(float seconds, float intensity) {
		shake = seconds;
		shakeAmount = intensity;
	}

	void OnEnable() {
		originalPos = transform.localPosition;
	}

	void Update() {
		if (shake > 0) {
			transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
			shake -= Time.deltaTime * decreaseFactor;
		}
		else {
			shake = 0f;
			transform.localPosition = originalPos;
		}
	}
}