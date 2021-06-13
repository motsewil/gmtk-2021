using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class HookSegment : LineSegment {

	public System.Action OnReelComplete;

	private bool Lock {
		get {
			return Lock;
		}
		set {
			if (value) {
				rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
			} else {
				rigidbody.constraints = RigidbodyConstraints2D.None;
			}
		}
	}

	public void AddForce(Vector2 force) {
		Lock = false;
		rigidbody.velocity = Vector2.zero;
		rigidbody.AddForce(force);
	}

	public void Reel(Vector2 position, float speed) {
		Lock = true;
		rigidbody.velocity = Vector2.zero;
		StartCoroutine(ReelCoroutine(position, speed));
	}

	private IEnumerator ReelCoroutine(Vector3 pos, float speed) {
		Vector2 dir = pos - transform.position;
		while(dir.sqrMagnitude > 1f) {
			dir = pos - transform.position;
			transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
			yield return new WaitForFixedUpdate();
		}
		OnReelComplete.Invoke();
	}
}
