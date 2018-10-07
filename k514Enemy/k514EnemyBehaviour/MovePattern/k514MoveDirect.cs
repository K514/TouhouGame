using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514MoveDirect : k514Move {
	public override bool Progress(){
		if(PatternCoroutine == null){
			switch(MoveType){
				case 0 :
					PatternCoroutine = LifeSpanType0();
				break;
				case 1 :
					PatternCoroutine = LifeSpanType1();
				break;
				case 2 :
					PatternCoroutine = LifeSpanType2();
				break;
			}
			StartCoroutine(PatternCoroutine);
		}
		return PatternLifeSpanTrig;
	}

	public override void EndProcess(){
		PatternCoroutine = null;
		PatternLifeSpanTrig = true;
	}

	IEnumerator LifeSpanType0(){
		yield return null;

		while((master.position - Destination.position).sqrMagnitude > 0.0001f){
			masterRigid.MovePosition(Vector3.MoveTowards(master.position, Destination.position, Speed*inversedMoveTime*Time.deltaTime));
			yield return null;
		}
		PatternLifeSpanTrig = false;
		yield return null;
	}

	IEnumerator LifeSpanType1(){
		yield return null;
		while((master.position - localDestination).sqrMagnitude > 0.005f){
			masterRigid.MovePosition(Vector3.MoveTowards(master.position, localDestination, Speed*inversedMoveTime*Time.deltaTime));
			yield return null;
		}
		PatternLifeSpanTrig = false;
		yield return null;
	}

	IEnumerator LifeSpanType2(){
		yield return null;
		localDestination = k514SystemManager.InteractMgr.GetPlayerHitVector();
		while((master.position - localDestination).sqrMagnitude > 0.005f){
			masterRigid.MovePosition(Vector3.MoveTowards(master.position, localDestination, Speed*inversedMoveTime*Time.deltaTime));
			yield return null;
		}
		PatternLifeSpanTrig = false;
		yield return null;
	}
}
