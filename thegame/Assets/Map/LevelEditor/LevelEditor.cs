using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelEditor : MonoBehaviour {

	[SerializeField]
	private GameObject[] tilePrefabs;
	private int index = 0;

	public GameObject currentPrefab;
	public GameObject potentialObject;

	private Vector2 mousePos;
	[SerializeField]
	private GameObject root;

	private void Update() {
		currentPrefab = tilePrefabs[0];

		if(potentialObject != null){
			Vector3 position = new Vector3(mousePos.x, mousePos.y, -CameraController.Main.transform.position.z);
			position = CameraController.Main.ScreenToWorldPoint(position);
			potentialObject.transform.position = new Vector3(Mathf.Floor(position.x) + 0.5f,Mathf.Floor(position.y) + 0.5f,position.z);
		}
		if(placing){
			AttemptPlace();
		}
	}

	private void AttemptPlace(){
		Transform parent = root.transform;
		for(int i = parent.childCount-1; i >= 0; i--){
			Transform child = parent.GetChild(i);
			if(Vector3.Distance(child.transform.position, potentialObject.transform.position) == 0){
				Destroy(child.gameObject);
			}
		}
		potentialObject.transform.SetParent(parent);
		potentialObject = null;
		UpdateCurrentPrefab();
	}

	private void UpdateCurrentPrefab(){
		if(potentialObject != null){
			Destroy(potentialObject);
		}
		Vector3 position = new Vector3(mousePos.x, mousePos.y, -CameraController.Main.transform.position.z);
		potentialObject = Instantiate(tilePrefabs[index], CameraController.Main.ScreenToWorldPoint(position), Quaternion.identity);
	}

	private void OnChangeSelected(InputValue value){
		index++;
		index = index >= tilePrefabs.Length ? index - tilePrefabs.Length : index;
		UpdateCurrentPrefab();
	}

	private void OnAim(InputValue value) {
		mousePos = value.Get<Vector2>();
	}

	private bool placing = false;
	private void OnPlaceItem(InputValue value) {
		if(potentialObject == null){
			return;
		}
		placing = value.Get<float>() == 1f;
	}
}
