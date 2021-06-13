using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PowerUpPickup : LineSegment {

	[SerializeField] private Animator animator;
	[SerializeField] private AudioSource audioSource;
	public bool isBomb = true;
	[ShowIf("isBomb")][SerializeField] private float bombRadius = 0.5f;
	public int score = 1;

	private void Awake() {
		collider.isTrigger = false;
	}

	// TODO sfx & vfx
	public void Explode() {
		audioSource.Play();
		// overlapsphere and find the poweruppickups and destroy them
		// remove score
		animator.SetTrigger("explode");
		LayerMask mask = LayerMask.GetMask("Pickup");
		Collider2D[] pickups = Physics2D.OverlapCircleAll(transform.position, bombRadius, mask, -1f, 1f);
		foreach (Collider2D p in pickups)
		{
			if (p.gameObject != gameObject) {
				if (p.GetComponent<PowerUpPickup>() != null) {
					if (p.transform.parent != null) {
						p.transform.parent.GetComponent<LineSegment>()?.ClearSlot();
					}
					Destroy(p.gameObject);
				}
			}
		}
		Destroy(gameObject, 2f);
	}

	public void EnableGathering() {
		audioSource.Play();
		collider.isTrigger = true;
	}

}
