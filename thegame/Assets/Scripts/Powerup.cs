using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class PowerUpPickup : MonoBehaviour {
	public PowerUp powerup;
}

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