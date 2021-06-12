using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class LevelTile : MonoBehaviour {

	public List<LevelTileTags> tags;
	public LevelTileType type;

	[SerializeField]
	private Transform levelRoot;

	[SerializeField]
	private List<AdjacentSprites> adjacentSprites;
	[SerializeField]
	private Sprite defaultSprite;

	[Button]
	private void PopulateAdjacencies () {
		LevelTileAdjacency adjacency;
		adjacency = LevelTileAdjacency.None;
		if (levelRoot == null) {
			return;
		}

		Vector2 origin = transform.position;
		Vector2 up = origin + Vector2.up;
		Vector2 down = origin + Vector2.down;
		Vector2 left = origin + Vector2.left;
		Vector2 right = origin + Vector2.right;

		for (int i = 0; i < levelRoot.childCount; i++) { 
			Transform potentialAdj = levelRoot.GetChild(i);
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