using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuCanvas : MonoBehaviour {

	public Animator fadeAnimator;

	public void LoadScene(string NextScene) {
		if (NextScene != null) {
			StartCoroutine(C_LoadScene(NextScene));
		}
	}

	IEnumerator C_LoadScene(string NextScene) {
		yield return new WaitForSeconds(1f);
		SceneManager.LoadSceneAsync(NextScene, LoadSceneMode.Single);
	}

	public void SetFade(bool value) {
		fadeAnimator.SetBool("Enabled", value);
	}
}
