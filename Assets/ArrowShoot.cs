using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ArrowShoot : MonoBehaviour {

	public GameObject arrow;
	public GameObject lastMissile;
	public GameObject head;
	public GameObject reticle;
	public GameObject lastGhost;
	public GameObject scoreCounter;
	public int numAttempts;

	Vector3 start;
	bool isFollowing;
	bool isPressing;
	Vector3 thrust;
	Vector3 originalScale;
	float lastTime;

	// Use this for initialization
	void Start () {
		start = transform.position;
		isFollowing = false;
		originalScale = reticle.transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		
		// Record time when button wasfirst pressed
		if (Input.GetMouseButtonDown (0)) {
			lastTime = Time.fixedTime;
			isPressing = true;
		}

		// Scale the reticle while you are holding the button down
		if (isPressing & !isFollowing) {
			Vector3 scale = reticle.transform.localScale;
			scale.x += 0.1f;
			reticle.transform.localScale = scale;
		}

		// Two actions: Either you are shooting, or resetting
		if (Input.GetMouseButtonUp (0)) {
			
			isPressing = false;
			float delta = Time.fixedTime - lastTime;
			 
			Ray forward = head.GetComponent<CardboardHead> ().Gaze;
			thrust = forward.direction * delta * 1000;

			// Reset
			if (isFollowing) {
				transform.position = start;
				isFollowing = false;
			} else {

				// Shoot
				Vector3 start = transform.position + forward.direction.normalized * 2;
				GameObject missile = (GameObject)Instantiate (arrow, start, Quaternion.identity);

				missile.GetComponent<Rigidbody> ().AddForce (thrust);
				lastMissile = missile;
				isFollowing = true;

				if (lastGhost != null) {
					Destroy (lastGhost);
				}

				// Leave a ghost of your last reticle, so you can improve your aim
				GameObject ghost = (GameObject) Instantiate (reticle, reticle.transform.position, reticle.transform.rotation);
				ghost.GetComponent<MeshRenderer> ().material.color = new Color (1.0f, 0, 0, 0.2f);
				lastGhost = ghost;
				reticle.transform.localScale = originalScale;

				// Keep track of score
				numAttempts += 1;
				scoreCounter.GetComponent<Text> ().text = "Attempts:" + numAttempts;
			}
				
		}

		// While you are following, make the camera track the ball
		if (isFollowing) {
			Vector3 follow = lastMissile.transform.position;
			Vector3 v = lastMissile.GetComponent<Rigidbody> ().velocity.normalized;

			follow.y += 1;
			follow -= v * 2;
			transform.position = follow;
		}


	}

	public void notifyWin() {
		scoreCounter.GetComponent<Text> ().text = "You win! N: " + numAttempts;
		numAttempts = 0;

		if (lastGhost != null) {
			Destroy (lastGhost);
		}
	}
}
