using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514PatternedShot : k514BulletBridge {


	// Enemy Behaviour
	[System.NonSerialized]public List<k514EnemyBehaviour> patterns;
	protected k514EnemyBehaviour currentPatterns = null;
	protected int patternsIndex = 0;

	public override void Init (Vector3 dir, float moveTime, float speed, int lifeSpan, float damage = 5f, bool whos_shot = true){
		base.Init(dir,moveTime,speed,lifeSpan,damage,whos_shot);
		patterns = new List<k514EnemyBehaviour>();
	}

	public override void Progress(){
		// if(TargetPlace!=null){
		// 	Rigid.MovePosition(Vector3.MoveTowards(transform.position, TargetPlace.position, inversedMoveTime*Time.deltaTime));
		// }else{
		// 	Rigid.MovePosition(Vector3.MoveTowards(transform.position, transform.position + speed*Direction, inversedMoveTime*Time.deltaTime));
		// }
		// lifeSpan--;
		// if(lifeSpan < 0) Interactive_Destroy();

		if((k514SystemManager.InteractMgr.GetPlayerVector().z - transform.position.z) > 2f) Interactive_Destroy();

        if(currentPatterns == null && patterns.Count > 0){
			currentPatterns = patterns[patternsIndex];
			patternsIndex++;
			if(LoopTrig) patternsIndex = patternsIndex % patterns.Count;
		}
		if(currentPatterns != null && currentPatterns.Progress()){
			// Progress 진행됨.
		}else{
			// Progress 종료됨.
			if(currentPatterns != null) currentPatterns.EndProcess();
			currentPatterns = null;
		}
		
		if(currentPatterns == null && patternsIndex == patterns.Count){
			Interactive_Destroy();
		};
		
	}

	public override void Interactive_Destroy(){
		patternsIndex = 0;
		SetLoop(true);
		base.Interactive_Destroy();
	}
}
