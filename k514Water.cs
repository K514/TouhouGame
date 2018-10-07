using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514Water : MonoBehaviour {

	void OnTriggerEnter (Collider co) {
		if(co.CompareTag("Player")){
			k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.DIVE,false);
		}
	}

	void OnTriggerExit (Collider co) {
		if(co.CompareTag("Player")){
			k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.DIVEOUT,false);
			k514SystemManager.UI_SpecaMgr.OutWater();
		}
	}

	void OnTriggerStay(Collider co){
		if(co.CompareTag("Player")){
			if(transform.position.y - co.transform.position.y > 2f){
				k514SystemManager.UI_SpecaMgr.InWater();
			}else{
				k514SystemManager.UI_SpecaMgr.OutWater();
			}
		}
	}
}
