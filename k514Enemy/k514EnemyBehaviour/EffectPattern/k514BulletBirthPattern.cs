using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514BulletBirthPattern : k514Move {

	protected WaitForSeconds wait = null;

    // // inherit
    // protected IPatternable masterScript = null;
	// protected Transform master;
	// protected Rigidbody masterRigid;

	// protected float moveTime = 1f, Speed = 1f , inversedMoveTime;
	// protected int LifeSpanCnt = 100;
    // protected Vector3 localDestination;

	public override void Init(IPatternable targetScript){
		base.Init(targetScript);
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
		float Elapsed = 0f;
		int type = 0,step = 0;
		while(true){
			step = Random.Range(5,23);
			for(int j = 0 ; j < step ; j++){
				type = Random.Range(0,3);
				k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.GRAZE,false);
				
				switch(type){
					case 0 :
					bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.RED,new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f)),0.05f,1f);
					bullet.gameObject.name = "Red";
					k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
					break;
					case 1 :
					bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.TEST,new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f)),0.05f,0.6f);
					bullet.gameObject.name = "Test";
					k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
					break;
					case 2 :
					bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.SHADOW,new Vector3(Random.Range(-1f,1f),Random.Range(-1f,1f),Random.Range(-1f,1f)),0.05f,0.3f);
					bullet.gameObject.name = "Shadow";
					k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
					break;
				}

				bullet.localScale = Vector3.one * Random.Range(1f,2f);
				bullet.position = master.position;
				bullet.parent = k514SystemManager.BulletMgr.transform;
			}
			while(Elapsed < 1.5f){
				Elapsed += Time.deltaTime;
				masterRigid.MovePosition(Vector3.MoveTowards(master.position, master.position + Vector3.back*10f, 5f*Time.deltaTime));
				yield return new WaitForSeconds(Time.deltaTime);
			}
			Elapsed = 0f;
		}
		yield return null;
		PatternLifeSpanTrig = false;

	}
}
