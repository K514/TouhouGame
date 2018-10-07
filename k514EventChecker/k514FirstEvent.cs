using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514FirstEvent : MonoBehaviour {

	k514PlayerController target = null;

	void Start(){
		StartCoroutine(Act());
		target = GameObject.Find("Player").GetComponent<k514PlayerController>();
		
	}

	IEnumerator Act(){
		yield return null;
		target.EventTrig = true;		
		yield return new WaitForSeconds(2f);
		StartCoroutine(k514SystemManager.UI_SpecaMgr.UI_Slide_out(2f));
		StartCoroutine(k514SystemManager.UI_SpecaMgr.UI_Black_out(1f,2f));
        yield return new WaitForSeconds(2f);
        StartCoroutine(k514SystemManager.UI_SpecaMgr.UI_Black_out(3f,2f));
        yield return new WaitForSeconds(2.5f);
        k514SystemManager.SerifuMgr.Act2();
		while(!k514SystemManager.SerifuMgr.isEnd()){
            yield return null;
        }
        k514SystemManager.SerifuMgr.EndProcess();
        yield return new WaitForSeconds(1f);
        k514SystemManager.InteractMgr.GetPlayerTransform().GetComponent<k514PlayerController>().EventEnd();
        StartCoroutine(k514SystemManager.UI_SpecaMgr.UI_Slide_out(1f));
		
	}

}
