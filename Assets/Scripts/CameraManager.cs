using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public Transform[] playerTransforms;
	public BezierCurve mainPath;
	public float maxLength = 50f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 averagePlayerPosition = new Vector3();
		foreach(Transform pTransform in playerTransforms) {
			averagePlayerPosition += pTransform.position;
		}
		averagePlayerPosition *= 1f/playerTransforms.Length;
		transform.position = mainPath.GetPointAt(averagePlayerPosition.x/maxLength);
		transform.LookAt(averagePlayerPosition);

	}
}
