using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

public class Hook : MonoBehaviour {
	[SerializeField] private new Rigidbody2D rigidbody;
	[SerializeField] private new Collider2D collider;
	[SerializeField] private Grabber grabber;

	public Rigidbody2D RB {get {return rigidbody;}}
	public System.Action<LevelTile> OnHook;

	[HideInInspector] public LevelTile hookedTile;

	[HideInInspector] public bool hooked = false;
	[HideInInspector] public bool attachedToGun = true;

	private void Awake() {
		attachedToGun = true;
		hooked = false;
	}

	private void Start() {
		collider.enabled = false;
	}

	private void Update() {
		if (attachedToGun) {
			transform.position = grabber.point.position;
		} else if (hookedTile) {
			transform.position = hookedTile.transform.position;
		}
		// } else if (Vector2.Distance(grabber.transform.position, transform.position) > grabber.GrabRange) {
		// 	// TODO limit range somehow
		// }
	}

	public void AttachToGun() {
		Debug.Log("attach");
		attachedToGun = true;
		collider.enabled = false;
		rigidbody.gravityScale = 0f;
		rigidbody.velocity = Vector2.zero;

	}

	public void DetachFromGun() {
		if (attachedToGun) {
			Debug.Log("Detach");
			attachedToGun = false;
			hooked = false;
			collider.enabled = true;
			rigidbody.gravityScale = 1f;
		}
	}

	public void Yoink() {
		Debug.Log("returning to gun");
		rigidbody.gravityScale = 0f;
		rigidbody.velocity = Vector2.zero;
		collider.enabled = false;
		if (hookedTile) {
			Debug.Log("yoikning");
			hookedTile.Yoink(grabber.point.position);
			hookedTile = null;
		}
		StartCoroutine(ReturnToGun());
	}

	private IEnumerator ReturnToGun() {
		Vector2 dir = grabber.point.position - transform.position;
		while(dir.sqrMagnitude > 1f) {
			dir = grabber.point.position - transform.position;
			transform.Translate(dir.normalized * grabber.ReturnSpeed, Space.World);
			yield return new WaitForFixedUpdate();
		}
		AttachToGun();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (hooked) {
			return;
		}

		rigidbody.velocity = Vector2.zero;
		rigidbody.angularVelocity = 0f;

		LevelTile tile;
		if (other.transform.TryGetComponent<LevelTile>(out tile)) {
			if (tile.tags.Contains(LevelTileTags.Grabbable)) {
				Debug.Log("hit a tile");
				hooked = true;
				rigidbody.gravityScale = 0f;
				collider.enabled = false;
				this.hookedTile = tile;
			}
		}
	}
}
