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
	[BoxGroup("Movement")][SerializeField]
		private Vector2 rayOrigin = Vector2.zero;
	[BoxGroup("Movement")][Range(0, 2)][SerializeField]
		private float rayDist = 1f;
	
	private Vector2 moveVector;
	private bool isGrounded;

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
		rigidbody.AddForce(moveVector * moveAcceleration);
		rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, moveSpeed);
	}

	private void OnMovement(InputValue value) {
		moveVector = value.Get<Vector2>();
		if (!flyCheat) {
			moveVector.y = 0;
		}
	}

	private void CheckGrounded() {
		if (rigidbody.velocity.y < 0) { // TODO ignore when already grounded? need to fix walking off platform
			Vector2 rayPos = ((Vector2)transform.position) + rayOrigin;
			int groundedMask = LayerMask.GetMask("Groundable");
			Debug.DrawRay(rayPos, Vector2.down * rayDist, Color.yellow, 0.1f);
			List<RaycastHit2D> results = new List<RaycastHit2D>(1);
			ContactFilter2D cf = new ContactFilter2D();
			cf.SetLayerMask(groundedMask);
			if (Physics2D.Raycast(rayPos, Vector2.down, cf, results, rayDist) > 0) {
				transform.position = results[0].point;
				SetGrounded(true);

			}
		}
	}

	private void SetGrounded(bool grounded) {
		isGrounded = grounded;
		if (grounded) {
			rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
		}
	}

	private void OnJump(InputValue value) {
		if (isGrounded) {
			Vector2 jumpVector = new Vector2(0, jumpForce);
			rigidbody.AddForce(jumpVector);
			SetGrounded(false);
		}
	}

	private void ToggleGravity() {
		rigidbody.gravityScale = rigidbody.gravityScale > Mathf.Epsilon ? 0 : 1f;
	}

	private void ToggleGravity(bool on) {
		rigidbody.gravityScale = on ? 1f : 0;
	}
#endregion

#region CHEATS
	private void OnFlyToggled() {
		ToggleGravity(!flyCheat);
		SetGrounded(false);
	}
#endregion
}
