using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514ReimuAbillity : k514EigenAbillity {

	// @ inherited
	// target : Transform
	// anim : Anomator
	// ActivateTrig : bool
	// targetScript : k514PlayerController
	//

	// public
	public Material front,back;
	public GameObject Effect1,Effect2,Effect3,Effect4;

	// private
	float firstDelay = 0.1f;
	int step = 50;
	float moveTime = 0.005f;

	IEnumerator A_LifeSPAN;

	Vector3 targetPlace = Vector3.up*0.5f;
	Vector3 initPlace = Vector3.up*-2.5f;

	public override void Activate(){
		// 1 . 모션을 바꾼다.
		// 2 . 비활성화 상태인 부적을 활성화 시키고 땅에서 솟아나게 만든다.
		if(!ActivateTrig){
			anim.SetBool("Cast",true);
			targetScript.MovableTrig = false;
			targetScript.ShotableTrig = false;			
			ActivateTrig = true;
			A_LifeSPAN = LifeSpan();
			StartCoroutine(A_LifeSPAN);
			k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.MAGIC1,false);
		}else{
			// do nothing
		}
	}
	public override void Deactivate(){
		// 1 . 부적을 불태우고, 비활성화 시킨다.
		// 2 . 모션을 되돌린다.
		if(!DeactivateTrig && ActivateTrig){
			DeactivateTrig = true;
			StartCoroutine(DelifeSpan());
		}else{
			// do nothing
		}
	}

	public override void EndProcess(){
		ActivateTrig = false;
		DeactivateTrig = false;
		transform.localPosition = initPlace;
		front.SetFloat("_Progress",1f);
		back.SetFloat("_Progress",1f);
		transform.gameObject.SetActive(false);
		targetScript.MovableTrig = true;	
		targetScript.ShotableTrig = true;					
	}

	IEnumerator LifeSpan(){
		yield return null;
		yield return new WaitForSeconds(firstDelay);
		WaitForSeconds moveDelay = new WaitForSeconds(moveTime/step);

		// move amullet
		for(int i = 0 ; i < step ; i++){
			transform.position = Vector3.Lerp(transform.position,targetPlace+target.position,(float)i/(step-1)); 
			yield return moveDelay;
		}

		SetEffect(true);

	}

	IEnumerator DelifeSpan(){
		yield return null;
		SetEffect(false);			
		yield return new WaitForSeconds(firstDelay);
		StopCoroutine(A_LifeSPAN);
		k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.FIRE1,false);					
		
		WaitForSeconds moveDelay = new WaitForSeconds(moveTime/step);
		anim.SetBool("Cast",false);

		// move amullet
		for(int i = 0 ; i < step ; i++){
			float tmp = Mathf.Lerp(1f,0f,(float)i/(step-1)); 
			front.SetFloat("_Progress",tmp);
			back.SetFloat("_Progress",tmp);
			yield return moveDelay;
		}
		EndProcess();
	}

	void SetEffect(bool trig){
		Effect1.SetActive(trig);
		Effect2.SetActive(trig);
		Effect3.SetActive(trig);
		Effect4.SetActive(trig);
	}

}
