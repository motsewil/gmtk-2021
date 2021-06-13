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

	private void Reset() {
		collider = GetComponent<CircleCollider2D>();
	}

	public void Init(Vector2 pos, float size) {
		posOld = pos;
		posNow = pos;
		collider.radius = size;
	}

	public void ClearSlot() {
		this.slot = null;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		// if other is valid collectable add it to this
		PowerUpPickup item = other.GetComponent<PowerUpPickup>();
		if (item != null) {
			if (item.playOneShot) {
				Destroy(item.gameObject);
				GameManager.Instance.ResolvePowerUp(item.powerup);
			} else {
				slot = item;
			}
		}
		// Fire onCatch event to the other, ie. if bomb it'll blow up and remove some segments
		// if line it'll add segments
	}
}
