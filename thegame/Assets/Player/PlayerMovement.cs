using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {

	[BoxGroup("Components")][SerializeField]
		private new Rigidbody2D rigidbody;

	[BoxGroup("Movement")][Min(0)][SerializeField]
		private float jumpForce = 200f;
	[BoxGroup("Movement")][Range(1, 10)][SerializeField]
		private float moveAcceleration = 2f;
	[BoxGroup("Movement")][Range(1, 10)][SerializeField]
		private float moveSpeed = 10f;
	private Vector2 movementVector;

	[BoxGroup("Aiming")][SerializeField]
		private Transform aimReticle;
	private Vector2 aimPosition;

	private void Reset() {
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void Update() {
		UpdateMovement();
		UpdateAim();
	}

#region Aim
	private void UpdateAim() {
		// TODO screen to camera
		// set aim ret to position
		Debug.Log(aimPosition);
	}

	private void OnAim(InputValue value) {
		aimPosition = value.Get<Vector2>();
	}
#endregion

#region Movement
	private void UpdateMovement() {
		rigidbody.AddForce(movementVector * moveAcceleration);
		rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, moveSpeed);
	}

	private void OnMovement(InputValue value) {
		movementVector = value.Get<Vector2>();
	}

	private void OnJump(InputValue value) {
		Vector2 jumpVector = new Vector2(0, jumpForce);
		rigidbody.AddForce(jumpVector);
	}
#endregion
}
