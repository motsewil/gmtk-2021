using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

	[BoxGroup("CHEATS")][SerializeField][OnValueChanged("OnFlyToggled")]
		private bool flyCheat;

	[BoxGroup("Components")][SerializeField]
		private new Rigidbody2D rigidbody;

	[BoxGroup("Movement")][Min(0)][SerializeField]
		private float jumpForce = 200f;
	[BoxGroup("Movement")][Range(1, 10)][SerializeField]
		private float moveAcceleration = 2f;
	[BoxGroup("Movement")][Range(1, 10)][SerializeField]
		private float moveSpeed = 10f;
	private Vector2 moveVector;
	private bool grounded;

	[BoxGroup("Aiming")][SerializeField]
		private Transform aimReticle;
	private Vector2 aimPosition;

	private void Reset() {
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		CheckGrounded();
		UpdateMovement();
		UpdateAim();
	}

#region Aim
	private void UpdateAim() {
		Camera cam = CameraController.Main;
		Vector3 position = new Vector3(aimPosition.x, aimPosition.y, -cam.transform.position.z);
		aimReticle.position = cam.ScreenToWorldPoint(position);
	}

	private void OnAim(InputValue value) {
		aimPosition = value.Get<Vector2>();
	}
#endregion

#region Movement
	private void UpdateMovement() {
		rigidbody.AddForce(moveVector * moveSpeed);
		rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, moveSpeed);
	}

	private void OnMovement(InputValue value) {
		moveVector = value.Get<Vector2>();
		if (!flyCheat) {
			moveVector.y = 0;
		}
	}

	private void CheckGrounded() {
		// TODO
	}

	private void OnJump(InputValue value) {
		Vector2 jumpVector = new Vector2(0, jumpForce);
		rigidbody.AddForce(jumpVector);
	}
#endregion

#region CHEATS
	private void OnFlyToggled() {
		rigidbody.gravityScale = flyCheat ? 0 : 1;
	}
#endregion
}
