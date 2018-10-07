using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514ChangeScale : k514Move {

	protected WaitForSeconds wait = null;

    // // inherit
    // protected IPatternable masterScript = null;
	// protected Transform master;
	// protected Rigidbody masterRigid;

	// protected float moveTime = 1f, Speed = 1f , inversedMoveTime;
	// protected int LifeSpanCnt = 100;
    // protected Vector3 localDestination;

	public override void Init(IPatternable targetScript,Vector3 targetScale){
		base.Init(targetScript,targetScale);
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
        WaitForSeconds interval = new WaitForSeconds(0.1f);
        Transform bullet = null;
		while((master.localScale - localDestination).sqrMagnitude > 0.0001f){
            master.localScale = Vector3.Lerp(master.localScale,localDestination,10f*Time.deltaTime);
            k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.GRAZE,false);
            yield return interval;
		}
		yield return null;
		PatternLifeSpanTrig = false;
	}
}
