using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AudioServices : Singleton<AudioServices>
{
	public Dictionary<string, AudioClip> dict_SoundEffects;
	public Dictionary<string, AudioClip> dict_BackgroundMusic;

	private GameObject go_sfxPrefab;
	private GameObject go_lsfxPrefab;
	private GameObject go_bgmPrefab;

	private GameObject go_introSource;
	private GameObject go_bgmSource;
	private string s_bgmPlaying;
	private Dictionary<string, GameObject> dict_go_activeLoopingSFX;

	private bool b_sfxIntroPlaying = false;
	private bool b_sfxIntroInterrupted = false;
	private bool b_bgmIntroPlaying = false;
	private bool b_bgmIntroInterrupted = false;
	private bool b_playingSFXWhileFading = false;
	private string s_pendingOutro = "";
	private Coroutine PlayThenLoopC;

	void Awake() {
		DontDestroyOnLoad(gameObject);

		go_sfxPrefab = Resources.Load ("Audio/Prefabs/SFXPrefab") as GameObject;
		go_lsfxPrefab = Resources.Load ("Audio/Prefabs/LSFXPrefab") as GameObject;
		go_bgmPrefab = Resources.Load ("Audio/Prefabs/BGMPrefab") as GameObject;

		// Populate Resources/Audio/SFX into a Dictionary
		dict_SoundEffects = new Dictionary<string, AudioClip> ();
		UnityEngine.Object[] objectsInSFXFolder = Resources.LoadAll ("Audio/SFX");
		foreach (UnityEngine.Object obj in objectsInSFXFolder) {
			AudioClip clip = obj as AudioClip;
			dict_SoundEffects.Add(obj.name, clip);
		}

		// Populate Resources/Audio/BGM into a Dictionary
		dict_BackgroundMusic = new Dictionary<string, AudioClip> ();
		UnityEngine.Object[] objectsInBGMFolder = Resources.LoadAll ("Audio/BGM");
		foreach (UnityEngine.Object obj in objectsInBGMFolder) {
			AudioClip clip = obj as AudioClip;
			dict_BackgroundMusic.Add(obj.name, clip);
		}

		dict_go_activeLoopingSFX = new Dictionary<string, GameObject> ();
	}

	public AudioSource GetBGMSource() {
		return go_bgmSource.GetComponent<AudioSource> ();
	}

	public void PlaySFXWhileFadingActiveBGM(string s_sfxName, float f_delay = 0f) {
		StartCoroutine (C_PlaySFXWhileFadingActiveBGM (s_sfxName, f_delay));
	}

	IEnumerator C_PlaySFXWhileFadingActiveBGM(string s_sfxName, float delay) {
		yield return new WaitForSeconds (delay);
		AudioSource bgmSource = GetBGMSource ();
		if (!b_playingSFXWhileFading) {
			b_playingSFXWhileFading = true;
			float initialVolume = bgmSource.volume;
			if (bgmSource != null) {
				for (float v = initialVolume; v > 0.1f; v -= Time.deltaTime/0.3f) {
					bgmSource.volume = v;
					yield return null;
				}
			}
			PlaySFX (s_sfxName);
			if (dict_SoundEffects.ContainsKey (s_sfxName)) {
				yield return new WaitForSeconds (dict_SoundEffects [s_sfxName].length * 1.1f);
			}
			if (bgmSource != null) {
				for (float v = 0.1f; v < initialVolume; v += Time.deltaTime/0.3f) {
					bgmSource.volume = v;
					yield return null;
				}
				bgmSource.volume = initialVolume;
			}
			b_playingSFXWhileFading = false;
		}
	}

	public void PlaySFX(AudioClip ac_clip) {
		GameObject newSFX = Instantiate (go_sfxPrefab, Camera.main.transform.position, Quaternion.identity) as GameObject;
		newSFX.transform.parent = gameObject.transform;
		newSFX.GetComponent<AudioSource> ().clip = ac_clip;
		newSFX.GetComponent<AudioSource>().Play();
	}

	public void PlayIntro(AudioClip ac_clip)
	{
		GameObject newIntro = Instantiate(go_sfxPrefab, Camera.main.transform.position, Quaternion.identity) as GameObject;
		newIntro.transform.parent = gameObject.transform;
		newIntro.GetComponent<AudioSource>().clip = ac_clip;
		newIntro.GetComponent<AudioSource>().Play();
		go_introSource = newIntro;
	}

	public void PlaySFX(string s_sfxName, float f_pitch = 1f, float f_volume = 1f) {
		if (dict_SoundEffects.ContainsKey(s_sfxName)) {
			AudioClip clip = dict_SoundEffects [s_sfxName];
			GameObject newSFX = Instantiate (go_sfxPrefab, Camera.main.transform.position, Quaternion.identity) as GameObject;
			newSFX.transform.parent = gameObject.transform;
			newSFX.GetComponent<AudioSource> ().clip = clip;
			newSFX.GetComponent<AudioSource> ().pitch = f_pitch;
			newSFX.GetComponent<AudioSource>().volume = f_volume;
			newSFX.GetComponent<AudioSource>().Play();
		} else {
			Debug.LogWarning("AudioClip with name: '"+s_sfxName+"' doesn't exist.");
		}
	}

	public void PlayRandomSFX (string s_sfxName, int i_maxNumber,  float f_pitch = 1f, float f_volume = 1f) {
		string randomSFXName = s_sfxName + UnityEngine.Random.Range (1, i_maxNumber+1).ToString ("00");
		PlaySFX (randomSFXName, f_pitch, f_volume);
	}

	public IEnumerator PlaySFX(string s_sfxName, Action<string> callback, string s_nextSfxName) {
		if (dict_SoundEffects.ContainsKey(s_sfxName)) {
			AudioClip clip = dict_SoundEffects [s_sfxName];
			GameObject newSFX = Instantiate (go_sfxPrefab, Camera.main.transform.position, Quaternion.identity) as GameObject;
			newSFX.transform.parent = gameObject.transform;
			newSFX.GetComponent<AudioSource> ().clip = clip;
			newSFX.GetComponent<AudioSource>().Play();
			yield return new WaitForSeconds(clip.length-0.1f);
			b_sfxIntroPlaying = false;
			if (!b_sfxIntroInterrupted) {
				callback(s_nextSfxName);
			} else {
				b_sfxIntroInterrupted = false;
				if (s_pendingOutro != "") {
					PlaySFX(s_pendingOutro);
					s_pendingOutro = "";
				}
			}
		} else {
			Debug.LogWarning("AudioClip with name: '"+s_sfxName+"' doesn't exist.");
		}
	}

	public void StartSFXLoop(string s_sfxNameLoop) {
		if (dict_SoundEffects.ContainsKey(s_sfxNameLoop)) {
			AudioClip clip = dict_SoundEffects [s_sfxNameLoop];
			GameObject newSFX = Instantiate (go_lsfxPrefab, Camera.main.transform.position, Quaternion.identity) as GameObject;
			newSFX.transform.parent = gameObject.transform;
			newSFX.GetComponent<AudioSource> ().clip = clip;
			newSFX.GetComponent<AudioSource>().Play();
			dict_go_activeLoopingSFX.Add(s_sfxNameLoop, newSFX);
		} else {
			//Debug.LogWarning("AudioClip with name: '"+s_sfxNameLoop+"' doesn't exist.");
		}
	}

	public void StartSFXLoopWithIntro(string s_sfxNameLoop, string s_sfxNameIntro) {
		b_sfxIntroPlaying = true;
		StartCoroutine(PlaySFX (s_sfxNameIntro, StartSFXLoop, s_sfxNameLoop));
	}

	public void EndSFXLoop(string s_sfxNameLoop) {
		if (dict_go_activeLoopingSFX.ContainsKey(s_sfxNameLoop)) {
			GameObject sfxObject = dict_go_activeLoopingSFX[s_sfxNameLoop];
			AudioSource source = sfxObject.GetComponent<AudioSource>();
			dict_go_activeLoopingSFX.Remove(s_sfxNameLoop);
			StartCoroutine(FadeOutAndDestroy(source, 1f));
		} else {
			if (b_sfxIntroPlaying) {
				b_sfxIntroInterrupted = true;
			}
			//Debug.LogWarning("AudioClip with name: '"+s_sfxNameLoop+"' doesn't exist.");
		}
	}

	public void EndSFXLoopWithOutro(string s_sfxNameLoop, string s_sfxNameOutro) {
		if (dict_go_activeLoopingSFX.ContainsKey(s_sfxNameLoop)) {
			GameObject sfxObject = dict_go_activeLoopingSFX[s_sfxNameLoop];
			AudioSource source = sfxObject.GetComponent<AudioSource>();
			dict_go_activeLoopingSFX.Remove(s_sfxNameLoop);
			StartCoroutine(FadeOutAndDestroy(source, 0.2f, s_sfxNameOutro));
		} else {
			if (b_sfxIntroPlaying) {
				b_sfxIntroInterrupted = true;
				s_pendingOutro = s_sfxNameOutro;
			}
			//Debug.LogWarning("AudioClip with name: '"+s_sfxNameLoop+"' doesn't exist.");
		}
	}

	public void PlayBGM(string s_bgmName, float f_volume = 1f) {
		if (dict_BackgroundMusic.ContainsKey(s_bgmName) && s_bgmPlaying != s_bgmName) {
			AudioClip clip = dict_BackgroundMusic [s_bgmName];
			s_bgmPlaying = s_bgmName;
			if (go_bgmSource == null) {
				GameObject newBGM = Instantiate(go_bgmPrefab, Camera.main.transform.position, Quaternion.identity) as GameObject;
				newBGM.transform.parent = gameObject.transform;
				newBGM.GetComponent<AudioSource>().clip = clip;
				newBGM.GetComponent<AudioSource>().Play();
				newBGM.GetComponent<AudioSource>().volume = f_volume;
				go_bgmSource = newBGM;
			} else {
				AudioSource source = go_bgmSource.GetComponent<AudioSource>();
				if (source.isPlaying) {
					StartCoroutine(FadeOutAndPlay(source, clip, 1f));
				}
			}
		}
	}

	public void PlayBGMWithIntro(string s_bgmName, string s_introName) {
		if (dict_BackgroundMusic.ContainsKey (s_bgmName) && dict_BackgroundMusic.ContainsKey (s_introName) && s_bgmPlaying != s_bgmName) {
			DestroyIntroIfPlaying();
			AudioClip clip = dict_BackgroundMusic [s_bgmName];
			AudioClip intro = dict_BackgroundMusic [s_introName];
			s_bgmPlaying = s_bgmName;
			if (go_bgmSource == null) {
				GameObject newBGM = Instantiate (go_bgmPrefab, Camera.main.transform.position, Quaternion.identity) as GameObject;
				newBGM.transform.parent = gameObject.transform;
				AudioSource source = newBGM.GetComponent<AudioSource> ();
				if (PlayThenLoopC != null) StopCoroutine(PlayThenLoopC);
				PlayThenLoopC = StartCoroutine(PlayIntroThenLoop (source, intro, clip));
				go_bgmSource = newBGM;
			} else {
				AudioSource source = go_bgmSource.GetComponent<AudioSource> ();
				StartCoroutine (FadeOutAndPlayWithIntro (source, intro, clip, 1f));
			}
		} else {
			if (!dict_BackgroundMusic.ContainsKey (s_bgmName)) {
				Debug.LogWarning("AudioClip with name: '"+s_bgmName+"' doesn't exist.");
			}
			if (!dict_BackgroundMusic.ContainsKey (s_introName)) {
				Debug.LogWarning("AudioClip with name: '"+s_introName+"' doesn't exist.");
			}
		}
	}

	public void PlayBGMIntroOnly(string s_introName) {
		if (dict_BackgroundMusic.ContainsKey (s_introName)) {
			AudioClip intro = dict_BackgroundMusic [s_introName];
			s_bgmPlaying = null;
			if (go_bgmSource == null) {
				GameObject newBGM = Instantiate (go_bgmPrefab, Camera.main.transform.position, Quaternion.identity) as GameObject;
				newBGM.transform.parent = gameObject.transform;
				AudioSource source = newBGM.GetComponent<AudioSource> ();
				StartCoroutine (PlayIntro (source, intro));
				go_bgmSource = newBGM;
			} else {
				AudioSource source = go_bgmSource.GetComponent<AudioSource> ();
				if (source.isPlaying) {
					StartCoroutine (FadeOutAndPlayIntro (source, intro, 1f));
				}
			}
		}
	}

	IEnumerator PlayIntroThenLoop(AudioSource source, AudioClip intro, AudioClip newClip) {
		b_bgmIntroInterrupted = false;
		b_bgmIntroPlaying = true;
		PlayIntro(intro);
		yield return new WaitForSeconds(intro.length - 0.1f);

		b_bgmIntroPlaying = false;
		go_introSource = null;
		if (!b_bgmIntroInterrupted)
		{
			source.clip = newClip;
			source.Play();
		}
		b_bgmIntroInterrupted = false;
	}

	IEnumerator PlayIntro(AudioSource source, AudioClip intro) {
		b_bgmIntroPlaying = true;
		PlayIntro (intro);
		yield return new WaitForSeconds (intro.length-0.1f);
		b_bgmIntroPlaying = false;
		go_introSource = null;
	}

	IEnumerator FadeOutAndPlayWithIntro(AudioSource source, AudioClip intro, AudioClip newClip, float fadeLength) {
		for (float f = 1f; f >= 0f; f -= Time.deltaTime/fadeLength) {
			source.volume = f;
			yield return null;
		}
		source.Stop ();
		source.volume = 1f;
		if (PlayThenLoopC != null) StopCoroutine(PlayThenLoopC);
		PlayThenLoopC = StartCoroutine(PlayIntroThenLoop(source, intro, newClip));
		yield return PlayThenLoopC;
	}

	IEnumerator FadeOutAndPlayIntro(AudioSource source, AudioClip intro, float fadeLength) {
		for (float f = 1f; f >= 0f; f -= Time.deltaTime/fadeLength) {
			source.volume = f;
			yield return null;
		}
		source.Stop ();
		source.volume = 1f;
		yield return StartCoroutine(PlayIntro (source, intro));
	}

	IEnumerator FadeOutAndPlay(AudioSource source, AudioClip newClip, float fadeLength) {
		for (float f = 1; f > 0f; f -= Time.deltaTime/fadeLength) {
			source.volume = f;
			yield return null;
		}
		source.Stop ();
		source.clip = newClip;
		source.volume = 1f;
		yield return new WaitForSeconds (0.1f);
		source.Play ();
	}

	IEnumerator FadeOutAndDestroy(AudioSource source, float fadeLength) {
		for (float f = 1; f > 0f; f -= Time.deltaTime/fadeLength) {
			source.volume = f;
			yield return null;
		}
		source.Stop ();
		Destroy (source.gameObject);
	}

	IEnumerator FadeOutAndDestroy(AudioSource source, float fadeLength, string s_outtro) {
		PlaySFX (s_outtro);
		for (float f = 1; f > 0f; f -= Time.deltaTime/fadeLength) {
			source.volume = f;
			yield return null;
		}
		source.Stop ();
		Destroy (source.gameObject);
	}

	public void StopBGM()
	{
		DestroyIntroIfPlaying();
		if (go_bgmSource != null)
		{
			AudioSource source = go_bgmSource.GetComponent<AudioSource>();
			if (source.isPlaying)
			{
				StartCoroutine(FadeOutAndDestroy(source, 1f));
			}
			go_bgmSource = null;
		}
	}

	public void StopAllSFXLoops()
	{
		if (b_sfxIntroPlaying)
		{
			b_sfxIntroInterrupted = true;
		}
		DestroyIntroIfPlaying();
		for (int i = 0; i < dict_go_activeLoopingSFX.Count; i++)
		{
			string[] temp = new string[dict_go_activeLoopingSFX.Count];
			dict_go_activeLoopingSFX.Keys.CopyTo(temp, 0);
			Debug.Log("Destroying SFX: " + dict_go_activeLoopingSFX[temp[dict_go_activeLoopingSFX.Count - 1]].name);
			Destroy(dict_go_activeLoopingSFX[temp[dict_go_activeLoopingSFX.Count - 1]]);
			dict_go_activeLoopingSFX.Remove(temp[dict_go_activeLoopingSFX.Count - 1]);
		}
	}

	private void DestroyIntroIfPlaying()
	{
		if (b_bgmIntroPlaying)
		{
			b_bgmIntroInterrupted = true;
			AudioSource source = go_introSource.GetComponent<AudioSource>();
			if (source.isPlaying)
			{
				StartCoroutine(FadeOutAndDestroy(source, 1f));
			}
			b_bgmIntroPlaying = false;
			go_introSource = null;
		}
	}
}