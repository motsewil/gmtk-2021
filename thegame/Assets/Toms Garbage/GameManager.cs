using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

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

	[SerializeField]private TMP_Text scoreText;
	public int score {
		get {
			return _score;
		}
		set {
			_score = value;
			scoreText.text = _score.ToString();
		}
	}
	private int _score = 0;

	public bool started = false;

	public UnityEvent OnStarted = new UnityEvent();
	public UnityEvent OnGameOver = new UnityEvent();
	public UnityEvent OnRestart = new UnityEvent();

	[Button]
	public void StartGame(){
		started = true;
		OnStarted.Invoke();
	}

}
