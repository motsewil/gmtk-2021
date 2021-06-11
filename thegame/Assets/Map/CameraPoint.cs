using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CameraPoint : MonoBehaviour {
	
	public CameraData data;

	public static List<CameraPoint> AllPoints {
		get {
			if(_allPoints == null){
				_allPoints = new List<CameraPoint>();
				_allPoints.AddRange(GameObject.FindObjectsOfType<CameraPoint>());
			}
			return _allPoints;
		}
	}
	private static List<CameraPoint> _allPoints;

	public static CameraPoint GetClosestToWorldPos(Vector2 worldPos, List<CameraPoint> exclusions = null){
		if(AllPoints.Count == 0 || (exclusions != null && AllPoints.Count <= exclusions.Count)){
			Debug.LogError("There are no valid camera points");
			return null;
		}
		CameraPoint closest = null;
		foreach(CameraPoint point in AllPoints){
			if(exclusions == null || !exclusions.Contains(point)){
				if(closest == null || 
					Vector2.Distance(worldPos, point.transform.position) < Vector2.Distance(worldPos, closest.transform.position)){
					closest = point;
				}
			}
		}
		return closest;
	}

	public static CameraPoint GetClosestToWorldPos(Vector2 worldPos, CameraPoint exclusion){
		List<CameraPoint> list = new List<CameraPoint>();
		list.Add(exclusion);
		return GetClosestToWorldPos(worldPos, list);
	}
	public static CameraPoint GetClosestToWorldPos(Vector3 worldPos, List<CameraPoint> exclusions = null){ return GetClosestToWorldPos((Vector2)worldPos, exclusions); }
	public static CameraPoint GetClosestToWorldPos(Vector3 worldPos, CameraPoint exclusion){ return GetClosestToWorldPos((Vector2)worldPos, exclusion); }

	private void OnDrawGizmos() {
		Camera cam = CameraController.Main;

		Vector2 bottomLeft = (Vector2)transform.position - new Vector2(cam.aspect * data.orthographicSize, data.orthographicSize);
		Vector2 topLeft = (Vector2)transform.position - new Vector2(cam.aspect * data.orthographicSize, -data.orthographicSize);
		Vector2 topRight = (Vector2)transform.position + new Vector2(cam.aspect * data.orthographicSize, data.orthographicSize);
		Vector2 bottomRight = (Vector2)transform.position + new Vector2(cam.aspect * data.orthographicSize, -data.orthographicSize);

		Gizmos.DrawLine(bottomLeft, topLeft);
		Gizmos.DrawLine(bottomLeft, bottomRight);
		Gizmos.DrawLine(bottomRight, topRight);
		Gizmos.DrawLine(topLeft, topRight);

		foreach(Vector2 vec in data.spawnPoints){
			Gizmos.DrawSphere(vec, 0.25f);
		}
	}
}

[System.Serializable]
public struct CameraData {
	[Min(1)]
	public float orthographicSize;

	public List<Vector2> spawnPoints;

	public Vector2 ClosestSpawnPoint(Vector3 worldPos){
		if(spawnPoints == null || spawnPoints.Count ==0){
			Debug.LogError("No spawn points");
			return Vector2.zero;
		}
		Vector2 closest = spawnPoints[0];
		foreach(Vector2 vec in spawnPoints){
			if(Vector2.Distance(vec, (Vector2)worldPos) < Vector2.Distance(closest, (Vector2)worldPos)){
				closest = vec;
			}
		}
		return closest;
	}
}