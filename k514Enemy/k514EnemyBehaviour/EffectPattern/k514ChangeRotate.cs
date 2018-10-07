using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514ChangeRotate : k514Move {

	protected WaitForSeconds wait = null;

    // // inherit
    // protected IPatternable masterScript = null;
	// protected Transform master;
	// protected Rigidbody masterRigid;

	// protected float moveTime = 1f, Speed = 1f , inversedMoveTime;
	// protected int LifeSpanCnt = 100;
    // protected Vector3 localDestination;

	private float rotateSpeed,deadline = 0f,speed = 0f;

	public void Init(IPatternable targetScript,Vector3 targetScale, float f, float d = 0f, float s = 0f){
		base.Init(targetScript,targetScale);
		rotateSpeed = f;
		deadline = d;
		speed = s;
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
		float cnt = 0f;
		while(k514SystemManager.InteractMgr.GetPlayerVector().z - master.position.z < 2f  && (deadline == 0f || deadline > cnt)){
            master.Rotate(localDestination,rotateSpeed*Time.deltaTime);
			masterRigid.MovePosition(Vector3.MoveTowards(master.position, master.position + Vector3.forward, speed*Time.deltaTime));
			if(deadline != 0f) cnt += rotateSpeed*Time.deltaTime;
            yield return null;
		}
		yield return null;
		PatternLifeSpanTrig = false;
	}
}
