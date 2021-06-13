using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PowerUp {
	public List<Power> powers;
	public Sprite sprite;
}

[System.Serializable]
public struct Power{
	public string effect;
	public float strength;
}