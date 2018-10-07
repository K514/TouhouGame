using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514TargetGuideShot : k514BulletBridge {


	public override void Init (Vector3 dir, float moveTime, float speed, int lifeSpan, float damage = 5f, bool whos_shot = true){
		base.Init(dir,moveTime,speed,lifeSpan,damage,whos_shot);
	}

	public override void Progress(){
		if(TargetPlace!=null){
			Rigid.MovePosition(Vector3.MoveTowards(transform.position, TargetPlace.position, inversedMoveTime*Time.deltaTime));
		}else{
			Rigid.MovePosition(Vector3.MoveTowards(transform.position, transform.position + speed*Direction, inversedMoveTime*Time.deltaTime));
		}
		lifeSpan--;
		if(lifeSpan < 0) Interactive_Destroy();
		if((k514SystemManager.InteractMgr.GetPlayerVector().z - transform.position.z) > 2f) Interactive_Destroy();
		
	}

	public override void Interactive_Destroy(){
		base.Interactive_Destroy();
	}
}
