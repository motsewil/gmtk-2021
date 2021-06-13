using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class LevelTile : MonoBehaviour {

	public List<LevelTileTags> tags;
	public LevelTileType type;
	private new Rigidbody2D rigidbody {
		get {
			if(_rigidbody == null){ TryGetComponent<Rigidbody2D>(out _rigidbody); }
			return _rigidbody;
		}
	}
	private Rigidbody2D _rigidbody;
	private new BoxCollider2D collider {
		get {
			if(_collider == null){ TryGetComponent<BoxCollider2D>(out _collider); }
			return _collider;
		}
	}
	private BoxCollider2D _collider;
	[SerializeField]private List<AdjacentSprites> adjacentSprites;
	[SerializeField]private Sprite defaultSprite;
	[SerializeField]private float yoinkSpeed;
	[SerializeField]private float smoothSpeed = 5f;

	private Vector2 velocity;
	private Vector2 hitOffset;
	private Vector2 hitPoint { 
		get { 
			return (Vector2)transform.position + hitOffset;
		}
	}
	private Vector2 targetPosWhenStill = Vector2.negativeInfinity;

	public void Yoink(Vector2 origin, Vector2 hitPoint) {
		hitOffset = hitPoint - (Vector2)transform.position;
		Vector2 pullDir = (origin - hitPoint).normalized;
		rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
		velocity = pullDir * yoinkSpeed;
		// TODO add juice event!
	}

	private void Update() {
		if(!rigidbody){
			return;
		}

		if(rigidbody.velocity.magnitude > Mathf.Epsilon){
			RaycastHit2D hit = Physics2D.Raycast(hitPoint + velocity.normalized * 0.01f, velocity.normalized, 0.05f);
			if(hit){
				LevelTile tile = hit.transform.GetComponent<LevelTile>();
				if(tile){
					velocity = Vector2.zero;
					rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
					targetPosWhenStill = tile.transform.position + Vector3.up;
				}
			}
		}
		if(velocity.magnitude < Mathf.Epsilon && !float.IsInfinity(targetPosWhenStill.magnitude)){
			transform.position = Vector3.Lerp(transform.position, targetPosWhenStill, Time.deltaTime * smoothSpeed);
		}
		rigidbody.velocity = velocity;
	}


	private void OnDrawGizmos() {
		if(rigidbody){
			// Gizmos.DrawLine(transform.position, (Vector2)transform.position + rigidbody.velocity.normalized);
		}
	}

	public void PopulateAdjacencies () {
		LevelTileAdjacency adjacency;
		adjacency = LevelTileAdjacency.None;
		if (transform.parent == null) {
			return;
		}

		Vector2 origin = transform.position;
		Vector2 up = origin + Vector2.up;
		Vector2 down = origin + Vector2.down;
		Vector2 left = origin + Vector2.left;
		Vector2 right = origin + Vector2.right;

		for (int i = 0; i < transform.parent.childCount; i++) { 
			Transform potentialAdj = transform.parent.GetChild(i);
			if(Vector2.Distance(up, potentialAdj.position) < 0.1f){
				adjacency |= LevelTileAdjacency.Up;
			}
			if(Vector2.Distance(down, potentialAdj.position) < 0.1f){
				adjacency |= LevelTileAdjacency.Down;
			}
			if(Vector2.Distance(left, potentialAdj.position) < 0.1f){
				adjacency |= LevelTileAdjacency.Left;
			}
			if(Vector2.Distance(right, potentialAdj.position) < 0.1f){
				adjacency |= LevelTileAdjacency.Right;
			}
		}

		for(int i = 0; i < adjacentSprites.Count; i++){
			if(adjacentSprites[i].adjacency == adjacency){
				GetComponent<SpriteRenderer>().sprite = adjacentSprites[i].sprite;
				return;
			}
		}
		GetComponent<SpriteRenderer>().sprite = defaultSprite;
	}

}
public enum LevelTileTags {
	Grabbable,
	Flaming,
	Sticky,
	Icy
}

public enum LevelTileType {
	Ground,
	Bedrock
}

[Flags] public enum LevelTileAdjacency {
	None = 0,
	Right = 1,
	Left = 2,
	Up = 4,
	Down = 8,
}

[System.Serializable]
public struct AdjacentSprites {
	public Sprite sprite;
	public LevelTileAdjacency adjacency;
}