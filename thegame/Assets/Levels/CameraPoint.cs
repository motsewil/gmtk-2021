using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class CameraPoint : MonoBehaviour {
	
	public LevelData data;

	[SerializeField]List<GameObject> potentialPowerups;
	[MinMaxSlider(0, 100)][SerializeField] private Vector2 spawnQty = new Vector2(20, 60);

	private void Start() {
		if (potentialPowerups != null && potentialPowerups.Count > 0) {
			SpawnStuff();
		}
	}

	private void SpawnStuff(){
		// Powerups
		for(int i = 0; i < Mathf.FloorToInt(Random.Range(spawnQty.x, spawnQty.y)); i++){
			Vector3 position = data.RandomPointInBounds();
			int index = Mathf.FloorToInt(Random.Range(0f, potentialPowerups.Count - 0.1f));
			if (potentialPowerups[index] != null) {
				GameObject pickup = Instantiate(potentialPowerups[index], position, Quaternion.identity);
			}
		}
	}

	private void Update() {
		data.position = transform.position;
	}

	public static List<CameraPoint> AllPoints {
		get {
			_allPoints = new List<CameraPoint>();
			_allPoints.AddRange(GameObject.FindObjectsOfType<CameraPoint>());
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
				if(closest == null){
					closest = point;
				} else {
					float distToPoint   = Vector2.Distance(worldPos, point.transform.position);
					float distToClosest = Vector2.Distance(worldPos, closest.transform.position);
					if(distToPoint < distToClosest ){
						closest = point;
					} else if( distToPoint == distToClosest ){
						if(point.data.orthographicSize < closest.data.orthographicSize){
							closest = point;
						}
					}
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
		
		bottomLeft = data.bounds.center - (data.bounds.size * 0.5f);
		topLeft = data.bounds.center - new Vector3(data.bounds.size.x, -data.bounds.size.y, 0f) * 0.5f;
		topRight = data.bounds.center + (data.bounds.size * 0.5f);
		bottomRight = data.bounds.center + new Vector3(data.bounds.size.x, -data.bounds.size.y, 0f) * 0.5f;
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(bottomLeft, topLeft);
		Gizmos.DrawLine(bottomLeft, bottomRight);
		Gizmos.DrawLine(bottomRight, topRight);
		Gizmos.DrawLine(topLeft, topRight);
	}
}

[System.Serializable]
public struct LevelData {
	[Min(1)]
	public float orthographicSize;

	public List<Vector2> spawnPoints;

	public Vector3 position;

	public Bounds bounds {
		get {
			return new Bounds(position, new Vector3(orthographicSize * 2.5f * CameraController.Main.aspect, orthographicSize * 2.5f, float.MaxValue));
		}
	}

	public Vector2 ClosestSpawnPoint(Vector3 worldPos){
		if(spawnPoints == null || spawnPoints.Count ==0){
			return Vector2.negativeInfinity;
		}
		Vector2 closest = spawnPoints[0];
		foreach(Vector2 vec in spawnPoints){
			if(Vector2.Distance(vec, (Vector2)worldPos) < Vector2.Distance(closest, (Vector2)worldPos)){
				closest = vec;
			}
		}
		return closest;
	}

	public Vector3 RandomPointInBounds(){
		Vector2 pos = bounds.center - bounds.extents;
		pos.x += Random.Range(1f, bounds.size.x - 1f);
		pos.y += Random.Range(1f, bounds.size.y - 1f);
		return (Vector3)pos;
	}
}