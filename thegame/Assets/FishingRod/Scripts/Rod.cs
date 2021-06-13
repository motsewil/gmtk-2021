using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rod : MonoBehaviour {

	[SerializeField] private Transform reticle;
	[SerializeField] private Line line;
	
	private Vector2 mousePosition;

	private void Update() {
		Camera cam = CameraController.Main;
		Vector3 pos = new Vector3(mousePosition.x, mousePosition.y, -cam.transform.position.z);
		reticle.position = cam.ScreenToWorldPoint(pos);
	}

	public void OnFire() {
		if (line.IsReeled) {
			Vector2 dir = (reticle.position - transform.position).normalized;
			line.Cast(dir);
		}
	}

	public void OnYoink() {
		line.Reel();
	}

	private void OnAim(InputValue value) {
		mousePosition = value.Get<Vector2>();
	}
}
