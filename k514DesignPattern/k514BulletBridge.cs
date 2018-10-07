using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class k514BulletBridge : IPatternable {

	protected Vector3 Direction;
	protected Rigidbody Rigid;
	protected float moveTime, inversedMoveTime, speed;
	protected int lifeSpan,id;
	protected bool DestroyTrig = false;
	protected Transform TargetPlace;
	protected float damage = 5f;
	protected bool whos_shot = true; // true : enemy
	[System.NonSerialized]public bool onceTrig = false;

	public abstract void Progress();
	public virtual void Interactive_Destroy(){
		if(!this.whos_shot){
			k514PlayerController.CURRENT_BULLET--;
		}
		transform.localScale = Vector3.one;
		transform.rotation = Quaternion.identity;
		TargetPlace = null;
		this.DestroyTrig = true;
	}

	public virtual void Init (Vector3 dir, float moveTime, float speed, int lifeSpan, float damage = 5f, bool whos_shot = true){
		this.Rigid = GetComponent<Rigidbody>();
		this.Direction = dir;
		this.moveTime = moveTime;
		this.speed = speed;
		this.lifeSpan = lifeSpan;
		this.damage = damage;
		this.whos_shot = whos_shot;
		this.inversedMoveTime = 1.0f/moveTime;
		this.DestroyTrig = false;
	}

	public void DoInteract(){
		onceTrig = true;
		Interactive_Destroy();
	}

	public void SetTargetTransform(Transform TargetPlace){
		this.TargetPlace = TargetPlace;
	}

	public void SetID(int id){
		this.id = id;
	}

	public int GetID(){
		return this.id;
	}

	public float GetDamage(){
		return this.damage;
	}

	public bool isDead(){
		return DestroyTrig;
	}

	public bool isEnemyShot(){
		return whos_shot;
	}

	public bool GetOnceTrig(){
		return onceTrig;
	}

	public void SetInfo(k514EnemyBehaviour target){
		target.SetSpeed(this.speed);
		target.SetMoveTime(this.moveTime);
		target.SetLifeSpanCnt(this.lifeSpan);
	}
}
