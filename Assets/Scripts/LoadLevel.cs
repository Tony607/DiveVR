using UnityEngine;
using System.Collections;

public class LoadLevel : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(LoadNewLevel());
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    IEnumerator LoadNewLevel() {
        yield return new WaitForSeconds(18);
        Application.LoadLevel(1);
        DontDestroyOnLoad(transform.gameObject);
    }
}
