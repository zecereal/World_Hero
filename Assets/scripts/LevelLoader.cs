﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour {

	public GameObject loadingScreen;
	public Slider slider;
	public TMP_Text progressText;

	public static bool isPlayed;

	void Start()
	{
		isPlayed = false;
	}

	public void LoadLevel(int sceneIndex){
		if(sceneIndex == 0){
			isPlayed = true;
		}
		StartCoroutine(LoadAsynchronously(sceneIndex));
	}

	IEnumerator LoadAsynchronously(int sceneIndex){
		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
		loadingScreen.SetActive(true);
		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / 0.9f);
			slider.value = progress;
			progressText.text = progress * 100f + "%";
			yield return null;
		}
	}
}