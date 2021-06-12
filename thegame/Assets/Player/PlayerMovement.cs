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
	[BoxGroup("Components")][SerializeField]
		private LineRenderer lineRenderer;

	[BoxGroup("Movement")][Min(0)][SerializeField]
		private float jumpForce = 200f;
	[BoxGroup("Movement")][Range(1, 10)][SerializeField]
		private float moveAcceleration = 2f;
	[BoxGroup("Movement")][Range(1, 10)][SerializeField]
		private float moveSpeed = 10f;
	[BoxGroup("Movement")][Range(1, 100)][SerializeField]
		private float jumpSpeed = 20f;
	[BoxGroup("Movement")][SerializeField]
		private Vector2 groundRayOrigin = Vector2.zero;
	[BoxGroup("Movement")][Range(0, 2)][SerializeField]
		private float groundRayDist = 1f;
	[BoxGroup("Movement")][Range(0, 1)][SerializeField]
		private float groundedCooldown = 0.1f;
	
	private Vector2 moveVector;
	private float jumpTime;
	private bool isGrounded;

	[BoxGroup("Grabber")][SerializeField][Tooltip("The grabber will collide with ANY of these layers")]
		private LayerMask[] grabberLayerMasks;
	[BoxGroup("Grabber")][SerializeField]
		private Transform aimReticle;
	[BoxGroup("Grabber")][Range(0, 100)][SerializeField]
		private float grabberRange = 10f;
	[BoxGroup("Grabber")][SerializeField]
		private Transform grabber;
	
	private Vector2 aimPosition;

	private void Reset() {
		rigidbody = GetComponent<Rigidbody2D>();
		lineRenderer = GetComponent<LineRenderer>();
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
		lineRenderer.SetPosition(lineRenderer.positionCount - 1, aimReticle.position);

		Vector3 dir = aimReticle.position - grabber.position;
		float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		grabber.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

	}

	private void OnAim(InputValue value) {
		aimPosition = value.Get<Vector2>();
	}

	private void OnFire(InputValue value) {

		ContactFilter2D filter = new ContactFilter2D();
		int layerMask = 0;
		foreach (int m in grabberLayerMasks)
		{
			layerMask |= m;
		}
		filter.SetLayerMask(layerMask);

		List<RaycastHit2D> results = new List<RaycastHit2D>(1);
		Vector2 direction = aimReticle.position - grabber.position;
		if (Physics2D.Raycast(grabber.position, direction, filter, results, grabberRange) > 0) {
			LevelTile tile;
			if (results[0].collider.TryGetComponent<LevelTile>(out tile)) {
				if (tile.tags.Contains(LevelTileTags.Grabbable)) {
					tile.GetComponent<SpriteRenderer>().material.color = Color.red;
					tile.Yoink(grabber.position);
				}
			}
		}
	}
#endregion

#region Movement
	private void UpdateMovement() {
		lineRenderer.SetPosition(0, grabber.position);
		rigidbody.AddForce(moveVector * moveAcceleration);
		// rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, moveSpeed);
		float xVel = Mathf.Clamp(rigidbody.velocity.x, -moveSpeed, moveSpeed);
		float yVel = rigidbody.velocity.y;
		if(moveVector.magnitude < Mathf.Epsilon){
			xVel = Mathf.Lerp(rigidbody.velocity.x, 0f, Time.deltaTime * moveAcceleration);
		}
		if(!isGrounded){
			yVel -= Time.deltaTime * jumpSpeed;
		} else {
			yVel = 0f;
		}
		rigidbody.velocity = new Vector2(xVel, yVel);
	}

	private void OnMovement(InputValue value) {
		moveVector = value.Get<Vector2>();
		if (!flyCheat) {
			moveVector.y = 0;
		}
	}

	private void CheckGrounded() {
		if (rigidbody.velocity.y < 0) {
			Vector2 rayPos = ((Vector2)transform.position) + groundRayOrigin;
			int groundedMask = LayerMask.GetMask("Groundable");
			Debug.DrawRay(rayPos, Vector2.down * groundRayDist, Color.yellow, 0.1f);
			List<RaycastHit2D> results = new List<RaycastHit2D>(1);
			ContactFilter2D cf = new ContactFilter2D();
			cf.SetLayerMask(groundedMask);
			if (Physics2D.Raycast(rayPos, Vector2.down, cf, results, groundRayDist) > 0) {
				transform.position = results[0].point;
				SetGrounded(true);
			}
		}
	}

	private void SetGrounded(bool grounded) {
		if(grounded && (Time.time - jumpTime) < groundedCooldown){
			return;
		}
		isGrounded = grounded;
		jumpTime = Time.time;
	}

	private void OnJump(InputValue value) {
		if (isGrounded) {
			Vector2 jumpVector = new Vector2(0, jumpForce);
			SetGrounded(false);
			rigidbody.AddForce(jumpVector);
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
