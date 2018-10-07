using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class k514EnemyBehaviour : MonoBehaviour {
	
	protected IEnumerator PatternCoroutine = null;
	protected bool PatternLifeSpanTrig = true;
	
	protected IPatternable masterScript = null;
	protected Transform master;
	protected Rigidbody masterRigid;

	protected float moveTime = 1f, Speed = 1f , inversedMoveTime;
	protected int LifeSpanCnt = 100;

	public virtual void Init(IPatternable masterScript){
		this.masterScript = masterScript;
		this.master = masterScript.transform;
		this.masterRigid = master.GetComponent<Rigidbody>();
		this.inversedMoveTime = 1f/moveTime;
	}


	public virtual void SetSpeed(float Speed){
		this.Speed = Speed;
	}

	public virtual void SetMoveTime(float moveTime){
		this.moveTime = moveTime;
		this.inversedMoveTime = 1f/moveTime;
	}

	public virtual void SetLifeSpanCnt(int LifeSpanCnt){
		this.LifeSpanCnt = LifeSpanCnt;
	}

	public abstract bool Progress();
	public abstract void EndProcess();
}
