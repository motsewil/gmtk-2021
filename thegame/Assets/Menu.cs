using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class Menu : MonoBehaviour {

	[SerializeField]private List<GameObject> titleElements;
	[SerializeField]private List<GameObject> gameOverElements;

	private void Awake() {
		GameManager.Instance.OnStarted.AddListener(OnStartGame);
		GameManager.Instance.OnGameOver.AddListener(OnGameOver);
		GameManager.Instance.OnRestart.AddListener(OnRestart);
	}

	private void OnStartGame(){
		foreach(GameObject obj in titleElements){
			obj.SetActive(false);
		}
	}
	
	private void OnGameOver(){
		foreach(GameObject obj in gameOverElements){
			obj.SetActive(true);
		}
	}

	private void OnRestart(){
		foreach(GameObject obj in gameOverElements){
			obj.SetActive(false);
		}
	}

}
