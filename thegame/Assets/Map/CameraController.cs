using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CameraController : MonoBehaviour {
	
	[SerializeField]
	private Transform target;

	private CameraPoint currentPoint;

	private void Start() {
		SetPoint(CameraPoint.GetClosestToWorldPos(target.position));
	}

	private void Update() {
		if(!Bounds.Contains(target.position)){
			SetPoint(CameraPoint.GetClosestToWorldPos(target.position, currentPoint));
		}
	}

	private void SetPoint(CameraPoint newPoint){
		Vector3 newPos = newPoint.transform.position;
		newPos.z = Main.transform.position.z;
		Main.transform.position = newPos;
		Main.orthographicSize = newPoint.data.orthographicSize;
		currentPoint = newPoint;
		target.transform.position = newPoint.data.ClosestSpawnPoint(target.position);
		if(!Bounds.Contains(target.position)){
			Debug.LogError ("Spawn point is off camera");
		}
	}

	private void OnDrawGizmos() {
		if(currentPoint != null && target != null){
			Gizmos.DrawLine(target.position, currentPoint.transform.position);
		}
	}

	public static Bounds Bounds {
		get {
			return new Bounds(Main.transform.position, new Vector3(Main.orthographicSize * 2 * Main.aspect, Main.orthographicSize * 2, float.MaxValue));
		}
	}
	
	public static Camera Main {
		get {
			if(_cam == null){
				_cam = Camera.main;
			}
			return _cam;
		}
	}
	private static Camera _cam;
}
