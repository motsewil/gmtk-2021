using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class LevelTile : MonoBehaviour {

	public List<LevelTileTags> tags;
	public LevelTileType type;

	[SerializeField] private float yoinkSpeed = 5f;
	[SerializeField] private new Rigidbody2D rigidbody;

	[SerializeField]
	private List<AdjacentSprites> adjacentSprites;
	[SerializeField]
	private Sprite defaultSprite;
	
	private List<LevelTile> neighbors = new List<LevelTile>();

	private void Reset() {
		if (tags.Contains(LevelTileTags.Grabbable)) {
			if (rigidbody == null) {
				
				TryGetComponent<Rigidbody2D>(out rigidbody);
			}
		}
	}

	private void Awake(){
		group = new List<LevelTile>();
		group.Add(this);
		ResetNeighbours();
	}

	private void ResetNeighbours(){
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 1f, transform.up);
		neighbors = new List<LevelTile>();
		for(int i = 0; i < hits.Length; i++){
			RaycastHit2D hit = hits[i];
			LevelTile tile = hit.transform.GetComponent<LevelTile>();
			if(tile){
				neighbors.Add(tile);
			}
		}
	}

	public Vector2 yoinkOrigin = Vector2.negativeInfinity;
	public void Yoink(Vector2 origin) {

		foreach(LevelTile tile in group){
			Vector3 offset = tile.transform.position - transform.position;
			if (tile.rigidbody) {
				tile.yoinkOrigin = origin + (Vector2)offset;
				tile.rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
			}
		}
		// TODO add juice event!
	}


	private void Update(){
		if(!float.IsInfinity(yoinkOrigin.magnitude) && rigidbody){
			rigidbody.velocity = (yoinkOrigin - (Vector2)transform.position).normalized * yoinkSpeed;
		}
		if(rigidbody && rigidbody.velocity.magnitude < Mathf.Epsilon && !float.IsInfinity(gridPos.magnitude)){
			transform.localPosition = Vector3.Lerp(transform.localPosition, gridPos, Time.deltaTime * gridSmooth);
		}

	}

	public Vector3 gridPos = Vector3.negativeInfinity;
	[SerializeField]
	private float gridSmooth = 10f;
	private List<LevelTile> group;
	private void OnCollisionEnter2D(Collision2D other) {
		LevelTile tile = other.gameObject.GetComponent<LevelTile>();
		if(tile && !neighbors.Contains(tile) && !group.Contains(tile)){
			if(!float.IsInfinity(yoinkOrigin.magnitude)){
				foreach(LevelTile gTile in group){
					// Vector3 offset = (transform.position - gTile.transform.position); 
					gTile.gridPos = new Vector3(
						Mathf.Floor(gTile.transform.localPosition.x) + 0.5f,
						Mathf.Floor(gTile.transform.localPosition.y) + 0.5f,
						gTile.transform.localPosition.z
					);
					// gTile.gridPos += offset;
				}
			}
			if(tile.tags.Contains(LevelTileTags.Grabbable) && tile.rigidbody){
				AddToGroup(tile);
			}

			foreach(LevelTile gTile in group){
				if(gTile.rigidbody){
					gTile.yoinkOrigin = Vector2.negativeInfinity;
					gTile.rigidbody.velocity = Vector2.zero;
					gTile.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
					gTile.ResetNeighbours();
				}
			}
		}
	}

	public void AddToGroup(LevelTile tile){
		if(group == null){
			group = new List<LevelTile>();
			group.Add(this);
		}
		group.Add(tile);
		foreach(LevelTile t in group){
			if(t != this){
				t.group.Add(this);
				t.GetComponent<SpriteRenderer>().color = Color.magenta;
			}
		}
	}

	[Button]
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