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

	protected Dictionary<PowerUpPickup, Vector2> slot = new Dictionary<PowerUpPickup, Vector2>();
	public Dictionary<PowerUpPickup, Vector2> Slot { get {return slot; } }

	private void Reset() {
		collider = GetComponent<CircleCollider2D>();
	}

	public void Init(Vector2 pos, float size) {
		posOld = pos;
		posNow = pos;
		collider.radius = size;
	}

	protected void Update() {
		foreach (var s in slot.Keys)
		{
			if (s) {
				s.transform.localPosition = slot[s];
			}
		}
	}

	public void ClearSlot() {
		if (slot.Count > 0) {
			foreach (var s in slot.Keys)
			{
				if (s) {
					GameManager.Instance.score -= s.score;
					Destroy(s.gameObject);
				}
			}
		}
		slot.Clear();
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
			} else if (item.transform.parent == null) { // item is not attached already
				GameManager.Instance.score += item.score;
				item.transform.parent = transform;
				slot.Add(item, item.transform.localPosition);
				item.EnableGathering();
			}
		}
		// Fire onCatch event to the other, ie. if bomb it'll blow up and remove some segments
		// if line it'll add segments
	}
}
