using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class k514GamePlayManager : MonoBehaviour {

	public static k514GamePlayManager singleton = null;
	public bool Activate = false;
	public Transform pauseUI;
	// Use this for initialization
	void Awake () {
		if(singleton == null){
			singleton = this;
		}
		else if(singleton != this) Destroy(gameObject);
	}


	void Update(){
		CheckPause();
		Time.timeScale = Activate ? 0f : 1f ;
	}

	void CheckPause(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			Activate = ! Activate;
			pauseUI.gameObject.SetActive(Activate);
		}
	}
	
	public void GoEnd(){
		StartCoroutine(End());
	}

	IEnumerator End(){
		yield return new WaitForSeconds(4f);
		Time.timeScale = 1f;
		k514SystemManager.SoundMgr.ClearSfx();
		k514SystemManager.InteractMgr.GetPlayerTransform().GetComponent<k514PlayerController>().EventTrig = true;
		yield return new WaitForSeconds(3f);
		StartCoroutine(k514SystemManager.UI_SpecaMgr.UI_Slide_out(1f));
		// StartCoroutine(k514SystemManager.UI_SpecaMgr.UI_Black_out(1f,2f));
		yield return new WaitForSeconds(1f);
		k514SystemManager.SerifuMgr.Act3(false);
		while(!k514SystemManager.SerifuMgr.isEnd()){
            yield return null;
        }
        k514SystemManager.SerifuMgr.EndProcess();
		yield return new WaitForSeconds(2f);		
		k514Loader.sceneName = "k514End";
		SceneManager.LoadScene("k514LoadingScene");
	}

}
