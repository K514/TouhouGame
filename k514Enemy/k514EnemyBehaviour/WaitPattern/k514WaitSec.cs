using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514WaitSec : k514EnemyBehaviour {

	protected WaitForSeconds wait = null;

	public void Init(IPatternable targetScript,float time){
		base.Init(targetScript);
		wait = new WaitForSeconds(time);
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
		yield return wait;
		PatternLifeSpanTrig = false;
	}
}
