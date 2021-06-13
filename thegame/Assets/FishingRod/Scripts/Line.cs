using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Line : MonoBehaviour {

	[SerializeField] private Transform segmentParent;
	private List<LineSegment> segments;

	private void Awake() {
		segments = new List<LineSegment>();
	}
	/**
	Has a collection of LineSegments
	Collect the objects from the segments somehow to get points
	*/
}
