using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rod : MonoBehaviour {

	/**
	SHoot ALL the hooks in some direction
	Recall ALL the hooks back to the rod
	*/

	[SerializeField] private Transform reticle;
	[SerializeField] private Line baseline;
	[SerializeField] private float yeetForce = 100f;
	private Vector2 mousePosition;

	private void Update() {
		Camera cam = CameraController.Main;
		Vector3 pos = new Vector3(mousePosition.x, mousePosition.y, -cam.transform.position.z);
		reticle.position = cam.ScreenToWorldPoint(pos);
	}

	public void OnFire() {
		Vector2 dir = (reticle.position - transform.position).normalized;
		baseline.Hook.AddForce(dir * yeetForce);
	}

	private void OnAim(InputValue value) {
		mousePosition = value.Get<Vector2>();
	}
}
