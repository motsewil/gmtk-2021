using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LineSegment : MonoBehaviour {

	/**
	Has a trigger collider which will catch objects and addd them to this line
	*/

	private GameObject slot;
	public GameObject Slot { get {return slot; } }

	private void OnTriggerEnter2D(Collider2D other) {
		// if other is valid collectable add it to this
		slot = other.gameObject; // TODO make the real class not gambojbe
		// Fire onCatch event to the other, ie. if bomb it'll blow up and remove some segments
		// if line it'll add segments
	}
}
