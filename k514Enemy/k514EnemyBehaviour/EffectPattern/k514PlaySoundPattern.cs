using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514PlaySoundPattern : k514EnemyBehaviour {

	protected WaitForSeconds wait = null;
    protected SFX_TYPE_EFFECT e;
    protected SFX_TYPE_BULLET e2;
	private int type = 0;

	public void Init(IPatternable targetScript,SFX_TYPE_EFFECT e){
		base.Init(targetScript);
        this.e = e;
		type = 0;
	}
	
	public void Init(IPatternable targetScript,SFX_TYPE_BULLET e2){
		base.Init(targetScript);
        this.e2 = e2;
		type = 1;
	}

	public override bool Progress(){
		if(PatternCoroutine == null){
			PatternCoroutine = LifeSpan();
			StartCoroutine(PatternCoroutine);
		}
		return PatternLifeSpanTrig;
	}

	public override void EndProcess(){
		PatternCoroutine = null;
		PatternLifeSpanTrig = true;
	}

	IEnumerator LifeSpan(){
		yield return null;
		switch(type){
			case 0 :
				k514SystemManager.SoundMgr.PlayAudioClip(e,false);
			break;
			case 1 :
				k514SystemManager.SoundMgr.PlayAudioClip(e2,false);
			break;
		}
		PatternLifeSpanTrig = false;
	}
}
