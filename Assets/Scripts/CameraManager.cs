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
		GameLogic = GameLogic.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameLogic.levelFinished) {
			Vector3 averagePlayerPosition = new Vector3();
			foreach(Transform pTransform in playerTransforms) {
				if (pTransform != null)
				averagePlayerPosition += pTransform.position;
			}

			averagePlayerPosition *= 1f/playerTransforms.Length;
			transform.position = mainPath.GetPointAt(averagePlayerPosition.x/maxLength);
			transform.LookAt(averagePlayerPosition);
		} else {
			transform.position = Vector3.Lerp(transform.position, GameLogic.PlayerControls[0].targetCamPos.position, 2f*Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, GameLogic.PlayerControls[0].targetCamPos.rotation, 2f*Time.deltaTime);
		}
	}
}
