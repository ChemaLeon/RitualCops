using UnityEngine;
using System.Collections;

public class DestroyAfterFinishedAudio : MonoBehaviour 
{
	private AudioSource audioSource;
	void Start() {
		audioSource = GetComponent<AudioSource> ();
		if (audioSource.clip != null) {
			StartCoroutine (C_DestroyAfterFinished ());
		} else {
			Debug.LogWarning("There is no AudioClip in AudioSource Component. Destroying");
			Destroy(gameObject);
		}
	}
	IEnumerator C_DestroyAfterFinished() {
		yield return new WaitForSeconds(audioSource.clip.length+1f);
		Destroy(gameObject);
	}
}
