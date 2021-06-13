using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class LineSegment : MonoBehaviour {

	[SerializeField] private new CircleCollider2D collider;
	[SerializeField] private new Rigidbody2D rigidbody;

	public Vector2 posOld;
	private Vector2 _posNow;
	public Vector2 posNow {
		get { return _posNow; }
		set {
			_posNow = value;
			transform.position = value;
		}
	}

	private GameObject slot;
	public GameObject Slot { get {return slot; } }

	private void Reset() {
		collider = GetComponent<CircleCollider2D>();
	}

	public void Init(Vector2 pos, float size) {
		posOld = pos;
		posNow = pos;
		collider.radius = size;
	} 

	public void AddForce(Vector2 force) {
		rigidbody.velocity = Vector2.zero;
		rigidbody.AddForce(force);
	}

	public void ClearSlot() {
		this.slot = null;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// if other is valid collectable add it to this
		slot = other.gameObject; // TODO make the real class not gambojbe
		// Fire onCatch event to the other, ie. if bomb it'll blow up and remove some segments
		// if line it'll add segments
	}
}
