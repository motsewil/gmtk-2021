using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PowerUpPickup : LineSegment {

	public bool isBomb = true;
	public int score = 1;

	private void Awake() {
		collider.isTrigger = false;
	}

	// TODO sfx & vfx
	public void Explode() {
		// overlapsphere and find the poweruppickups and destroy them
		// remove score
	}

	public void EnableGathering() {
		collider.isTrigger = true;
	}

}
