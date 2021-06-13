using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Grabber : MonoBehaviour {
	[SerializeField] private Rope rope;
	[SerializeField] private Transform reticle; // the mouse pos object
	[SerializeField] private Hook hook; // the thing that gets shot out that the rope is attached to
	public Transform point;

	[SerializeField] private float grabRange = 5f;
	public float GrabRange { get {return grabRange; } }
	[Min(0f)][SerializeField] private float launchForce = 2f;
	[Min(0f)][SerializeField] private float returnSpeed = 2f;
	public float ReturnSpeed { get { return returnSpeed; } }

	private void Start() {
		// TODO init rope length based on the grabrange field
	}

	public void Launch() {
		/**
		 * Launches the grapple in the direction of the target
		 * Stops when the grapple hits something OR the grapple range is reached
		 */
		hook.DetachFromGun();
		Vector2 direction = (reticle.position - hook.transform.position).normalized; // this is probably the same as transform.forward
		hook.RB.AddForce(direction * launchForce);
	}

	public void OnYoink() {
		hook.Yoink();
	}
}
