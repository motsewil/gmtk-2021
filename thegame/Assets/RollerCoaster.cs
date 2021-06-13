using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class RollerCoaster : MonoBehaviour {

	public static RollerCoaster Instance {
		get {
			return GameObject.FindObjectOfType<RollerCoaster> ();
		}
	}

	public List<Transform> points;
	public GameObject target;

	Transform curPoint;
	Transform nextPoint;

	int index = 1;
	[SerializeField] float moveSpeed = 10f;

	public LineRenderer line;

	private void Awake () {
		index = 1;
		for (int i = 0; i < transform.childCount; i++) {
			points.Add (transform.GetChild (i));
		}
		curPoint = points[0];
		nextPoint = points[1];
		target.transform.position = curPoint.transform.position;
	}

	void Update () {
		float increment = Time.deltaTime * moveSpeed;
		Vector2 direction = nextPoint.transform.position - curPoint.transform.position;
		Vector2 velocity = direction.normalized * increment;
		target.transform.position += (Vector3) velocity;
		if (Vector3.Distance (target.transform.position, nextPoint.transform.position) < 0.05f) {
			curPoint = nextPoint;
			index++;
			if (index < points.Count) {
				nextPoint = points[index];
			}
		}
		target.transform.up =  Util.RoundToDegrees(velocity, 1f);
		if (line == null) {
			line = GetComponent<LineRenderer> ();
		}
		line.positionCount = points.Count;
		for (int i = 0; i < line.positionCount; i++) {
			line.SetPosition (i, points[i].transform.position);
		}
	}

	private void OnDrawGizmos () {
		for (int i = 0; i < points.Count - 1; i++) {
			Gizmos.DrawLine (points[i].transform.position, points[i + 1].transform.position);
		}
	}

}