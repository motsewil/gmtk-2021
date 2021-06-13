using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;

public class GameManager : MonoBehaviour {

	public static GameManager Instance {
		get {
			if(_instance == null){
				_instance = GameObject.FindObjectOfType<GameManager>();
			}
			return _instance;
		}
	}
	private static GameManager _instance;

	[SerializeField]private TMP_Text scoreText1;
	[SerializeField]private TMP_Text scoreText2;
	public int score {
		get {
			return _score;
		}
		set {
			_score = value;
			scoreText1.text = _score.ToString();
			scoreText2.text = _score.ToString();
		}
	}
	private int _score = 0;

	public bool started = false;
	public bool ended = false;

	public UnityEvent OnStarted = new UnityEvent();
	public UnityEvent OnGameOver = new UnityEvent();
	public UnityEvent OnRestart = new UnityEvent();

	[Button]
	public void StartGame(){
		started = true;
		ended = false;
		OnStarted.Invoke();
	}

	public void EndGame(){
		ended = true;
		OnGameOver.Invoke();
	}

	public void Restart(){
		UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
	}

	public void ResolvePowerUp(Power power) {
		switch(power.effect) {
			case "coin":
				score += (int)power.strength;
				break;
			// TODO bombs here too?
		}
	}
}
