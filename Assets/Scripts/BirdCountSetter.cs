using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdCountSetter : MonoBehaviour {

    public int countToSet;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += SetPlayerCount;
	}
    
    void SetPlayerCount(Scene scene, LoadSceneMode mode)
    {
        FieldManager fieldManager = FindObjectOfType<FieldManager>();
        fieldManager.playerCount = countToSet;
        SceneManager.sceneLoaded -= SetPlayerCount;
        Destroy(gameObject);
    }

    public void StartGame(int playerCount)
    {
        audioSource.Play();
        Random.InitState(System.DateTime.Now.Millisecond);
        int StageID = Random.Range(1, 6);
        countToSet = playerCount;
        SceneManager.LoadScene(string.Format("Stage_{0}", StageID));
    }

}
