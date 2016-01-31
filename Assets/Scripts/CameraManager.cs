using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public Camera mainCamera;
	public Transform[] playerTransforms;
	public BezierCurve mainPath;
	public float maxLength = 50f;

	private GameLogic GameLogic;

	// Use this for initialization
	void Start () {
		GameLogic = GameObject.FindObjectOfType<GameLogic>();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 averagePlayerPosition = new Vector3();
		foreach(Transform pTransform in playerTransforms) {
			if (pTransform != null)
			averagePlayerPosition += pTransform.position;
		}

		averagePlayerPosition *= 1f/playerTransforms.Length;
		transform.position = mainPath.GetPointAt(averagePlayerPosition.x/maxLength);
		transform.LookAt(averagePlayerPosition);

	}
}
