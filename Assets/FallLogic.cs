using UnityEngine;
using System.Collections;

public class FallLogic : MonoBehaviour {

	Vector3 originalPosition;
	Quaternion originalRotation;
	public ArrowShoot mainShooter;

	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
		originalRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {

		// You knocked it off the platform, you win! Reset the target
		if (transform.position.y < 5) {
			GetComponent<Rigidbody> ().velocity = Vector3.zero;
			transform.position = originalPosition;
			transform.rotation = originalRotation;
			mainShooter.notifyWin ();
		}
	}
}
