using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514OptionManager : MonoBehaviour {

	private k514AudioSource targetBGM = null;
	public static k514OptionManager singleton = null;


	void Awake () {
		if(singleton == null){
			singleton = this;
		}else if(this != singleton){
			Destroy(gameObject);
		}
	}

	public void SetBGM(k514AudioSource a){
		this.targetBGM = a;
	}

	void Update(){
		if(targetBGM != null){
			if(targetBGM.gameObject.active){
				targetBGM.src.volume = (Time.timeScale==0) ? 0f : k514SoundManager.singleton.BGMVolume;
			}
		}
	}

	public void StopBGM(){
		targetBGM.StopClip();
	}
}
