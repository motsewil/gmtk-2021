using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor;

public class LevelEditor : MonoBehaviour {

	[SerializeField]
	private GameObject[] tilePrefabs;
	private int index = 0;

	public GameObject currentPrefab;
	public GameObject potentialObject;

	private Vector2 mousePos;
	[SerializeField]
	private GameObject root;
	private bool placing = false;
	private bool removing = false;

	private void Update () {
		// Darren if you see this I'm sorry
		root = CameraPoint.GetClosestToWorldPos(transform.position).transform.parent.GetChild(1).gameObject;
		currentPrefab = tilePrefabs[0];

		Vector3 targetPos = new Vector3 (mousePos.x, mousePos.y, -CameraController.Main.transform.position.z);
		targetPos = CameraController.Main.ScreenToWorldPoint (targetPos);
		targetPos = new Vector3 (Mathf.Floor (targetPos.x) + 0.5f, Mathf.Floor (targetPos.y) + 0.5f, targetPos.z);
		if (potentialObject != null) {
			potentialObject.transform.position = targetPos;
		}
		if (placing) {
			AttemptPlace ();
		} else if (removing) {
			GameObject toDestroy = null;
			for (int i = root.transform.childCount - 1; i >= 0; i--) {
				Transform child = root.transform.GetChild (i);
				if (Vector3.Distance (child.transform.position, targetPos) == 0) {
					toDestroy = child.gameObject;
				}
			}
			if (toDestroy != null) {
				Destroy (toDestroy);
			}
		}
	}

	private void AttemptPlace () {
		Transform parent = root.transform;
		for (int i = parent.childCount - 1; i >= 0; i--) {
			Transform child = parent.GetChild (i);
			if (Vector3.Distance (child.transform.position, potentialObject.transform.position) == 0) {
				Destroy (child.gameObject);
			} else {
				child.GetComponent<LevelTile> ().PopulateAdjacencies ();
			}
		}
		potentialObject.GetComponent<LevelTile> ().PopulateAdjacencies ();
		potentialObject.transform.SetParent (parent);
		potentialObject = null;
		UpdateCurrentPrefab ();
	}

	private void UpdateCurrentPrefab () {
		if (potentialObject != null) {
			Destroy (potentialObject);
		}
		Vector3 position = new Vector3 (mousePos.x, mousePos.y, -CameraController.Main.transform.position.z);
		potentialObject = PrefabUtility.InstantiatePrefab(tilePrefabs[index]) as GameObject;
	}

	private void OnChangeSelected (InputValue value) {
		index++;
		index = index >= tilePrefabs.Length ? index - tilePrefabs.Length : index;
		UpdateCurrentPrefab ();
	}

	private void OnAim (InputValue value) {
		mousePos = value.Get<Vector2> ();
	}

	private void OnRemoveItem (InputValue value) {
		removing = value.Get<float> () == 1f;
	}

	private void OnPlaceItem (InputValue value) {
		if (potentialObject == null) {
			return;
		}
		placing = value.Get<float> () == 1f;
	}
}