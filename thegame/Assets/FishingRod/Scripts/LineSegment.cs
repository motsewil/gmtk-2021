using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class LineSegment : MonoBehaviour {

	[SerializeField] protected new CircleCollider2D collider;
	[SerializeField] protected new Rigidbody2D rigidbody;

	public Vector2 posOld;
	protected Vector2 _posNow;
	public Vector2 posNow {
		get { return _posNow; }
		set {
			_posNow = value;
			transform.position = value;
		}
	}

	protected PowerUpPickup slot;
	public PowerUpPickup Slot { get {return slot; } }
	protected Vector2 slotPos;

	private void Reset() {
		collider = GetComponent<CircleCollider2D>();
	}

	public void Init(Vector2 pos, float size) {
		posOld = pos;
		posNow = pos;
		collider.radius = size;
	}

	protected void Update() {
		if (slot) {
			slot.transform.localPosition = slotPos;
		}
	}

	public void ClearSlot() {
		if (slot) {
			GameManager.Instance.score -= slot.score;
			slot.transform.parent = null;
		}
		slot = null;
	}

	protected void OnTriggerEnter2D(Collider2D other) {
		if (!GameManager.Instance.started) {
			return;
		}

		// if other is valid collectable add it to this
		PowerUpPickup item = other.GetComponent<PowerUpPickup>();
		if (item != null) {
			if (item.isBomb) {
				item.Explode();
				ClearSlot();
			} else if (slot == null && item.transform.parent == null) { // item is not attached already
				GameManager.Instance.score += item.score;
				slot = item;
				item.transform.parent = transform;
				slotPos = slot.transform.localPosition;
				item.EnableGathering();
			}
		}
		// Fire onCatch event to the other, ie. if bomb it'll blow up and remove some segments
		// if line it'll add segments
	}
}
