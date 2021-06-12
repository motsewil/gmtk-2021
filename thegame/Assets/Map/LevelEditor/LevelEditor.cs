using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class LevelEditor : MonoBehaviour {

	[SerializeField]
	private GameObject[] tilePrefabs;
	private int index = 0;

	public GameObject currentPrefab;
	public GameObject potentialObject;

	private void Update() {
		currentPrefab = tilePrefabs[0];

		if(Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonDown(1)){
			index++;
			index = index >= tilePrefabs.Length ? index - tilePrefabs.Length : index;
			UpdateCurrentPrefab();
		}
		if(potentialObject != null){
			potentialObject.transform.position = Utility.MousePos;
		}
	}

	private void UpdateCurrentPrefab(){
		if(potentialObject != null){
			Destroy(potentialObject);
		}
		potentialObject = Instantiate(tilePrefabs[index], Utility.MousePos, Quaternion.identity);
	}
}

public class Utility {

	public static Vector2 MousePos {
		get {
			return (Vector2)CameraController.Main.ScreenToWorldPoint(Input.mousePosition);
		}
	} 
}
