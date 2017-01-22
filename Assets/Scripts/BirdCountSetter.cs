using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdCountSetter : MonoBehaviour {

    public int countToSet;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += SetPlayerCount;
	}
    
    void SetPlayerCount(Scene scene, LoadSceneMode mode)
    {
        FieldManager fieldManager = FindObjectOfType<FieldManager>();
        fieldManager.playerCount = countToSet;
        SceneManager.sceneLoaded -= SetPlayerCount;
        Destroy(gameObject);
    }
    
}
