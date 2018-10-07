using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514EventChecker : MonoBehaviour {

	private k514PlayerController target = null;
	private int collide_count = 0;
	private bool StartTrig = false;


	public bool Progress(){
		AnimatorStateInfo stateInfo = target.animator.GetCurrentAnimatorStateInfo(0);
		if (collide_count != 0 && stateInfo.IsName("Base Layer.Idle")){
				StartTrig = true;
		}
		return this.StartTrig;
	}

	public void EndProcess(){
		collide_count = 0;
		StartTrig = false;
		gameObject.SetActive(false);
	}

	void Start (){
		target = GameObject.Find("Player").GetComponent<k514PlayerController>();
	}

	void OnTriggerEnter(Collider co){
		if(co.CompareTag("Player")){
			collide_count++;
			EventCheck();
		}
	}


	void OnTriggerExit(Collider co){
		if(co.CompareTag("Player")){
			collide_count--;
			EventCheck();
		}
	}

	void EventCheck(){
		switch(collide_count){
			case 0 :
				target.EventTrig = false;				
			break;
			case 1 :
				target.EventTrig = true;
			break;
		}
	}


}
