using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PowerUp {
	public List<Power> powers;
}

[System.Serializable]
public struct Power{
	public string effect;
	public float strength;
}