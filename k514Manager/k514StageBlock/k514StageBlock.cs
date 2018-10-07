using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514StageBlock : MonoBehaviour {

	private bool trig = false;

	public bool GetState(){
		return trig;
	}

	public void SetState(bool trig){
		this.trig = trig;
	}

	void OnTriggerEnter(Collider co){
		if(co.CompareTag("Player"))
			trig = true;
	}


}
