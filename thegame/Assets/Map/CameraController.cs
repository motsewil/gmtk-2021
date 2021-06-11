using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CameraController : MonoBehaviour {
	
	[SerializeField]
	private Transform target;

	private CameraPoint currentPoint;
	[SerializeField]
	private float transitionSpeed = 10f;

	private void Start() {
		SetPoint(CameraPoint.GetClosestToWorldPos(target.position));
	}

	private void Update() {
		CameraPoint closestPoint = CameraPoint.GetClosestToWorldPos(target.position);
		if(currentPoint != closestPoint && closestPoint.data.bounds.Contains(target.position)){
			SetPoint(closestPoint);
		} else if( !currentPoint.data.bounds.Contains(target.position)){
			SetPoint(CameraPoint.GetClosestToWorldPos(target.position, currentPoint));
		}
		UpdateView();
	}

	private void UpdateView(){
		
		Vector3 newPos = Vector3.Lerp(transform.position, currentPoint.transform.position, Time.deltaTime * transitionSpeed);
		newPos.z = Main.transform.position.z;
		Main.transform.position = newPos;
		Main.orthographicSize = Mathf.Lerp(Main.orthographicSize, currentPoint.data.orthographicSize, Time.deltaTime * transitionSpeed);
	}

	private void SetPoint(CameraPoint newPoint){
		currentPoint = newPoint;

		Vector2 spawnPoint = newPoint.data.ClosestSpawnPoint(target.position);
		if(spawnPoint.magnitude > Vector2.negativeInfinity.magnitude){
			target.transform.position = spawnPoint;
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
