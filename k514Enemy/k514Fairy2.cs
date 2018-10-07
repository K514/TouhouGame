using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514Fairy2 : k514EnemyController {

    public int Pattren = 0;

    protected override IEnumerator GetAttckPatterns(){
        switch(Pattren){
            case 0 :
		    return AttackPattern();
            case 1 :
		    return AttackPattern1();
            default :
		    return AttackPattern2();
        }
	}

	protected override void Update(){
        base.Update();
        if(k514SystemManager.InteractMgr.GetPlayerVector().z - transform.position.z > 1f){
            DoInteract_Dead();
        }
	}

    // 5연발
    protected IEnumerator AttackPattern1(){
		yield return null;
		yield return new WaitForSeconds(firstAttackInterval);
        WaitForSeconds cooldown = new WaitForSeconds(7f), shotInterval = new WaitForSeconds(0.2f);
		while(true){
            for(int i = 0 ; i < 5 ; i++){
                k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BULLET.SHOT1,false);
				bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.SHADOW,k514SystemManager.InteractMgr.GetPlayerHitVectorFrom(transform.position),0.05f,7f);
				bullet.position = transform.position;
				bullet.gameObject.name = "Shadow";
				bullet.parent = k514SystemManager.BulletMgr.transform;
				k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
				yield return shotInterval;
            }
            yield return cooldown;
        }
	}

    // 난사
	protected IEnumerator AttackPattern2(){
        yield return null;
		yield return new WaitForSeconds(firstAttackInterval);
        WaitForSeconds cooldown = new WaitForSeconds(6f), shotInterval = new WaitForSeconds(0.1f);
		while(true){
            for(int i = 0 ; i < Random.Range(5,12) ; i++){
                    k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BULLET.SHOT1,false);
                for(int J = 0 ; J < Random.Range(5,12) ; J++){
                    if(Random.Range(0,10)>5){
                        bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.SMALL_CB,k514SystemManager.InteractMgr.GetPlayerHitVectorFrom(transform.position) + new Vector3(Random.Range(-3f,3f),Random.Range(-3f,3f),Random.Range(-1f,1f)),0.05f,Random.Range(5f,9f));
                    }else{
                        bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.SMALL_SP,k514SystemManager.InteractMgr.GetPlayerHitVectorFrom(transform.position) + new Vector3(Random.Range(-3f,3f),Random.Range(-3f,3f),Random.Range(-1f,1f)),0.05f,Random.Range(5f,9f));
                    }
                    bullet.position = transform.position;
                    bullet.gameObject.name = "Small";
                    bullet.parent = k514SystemManager.BulletMgr.transform;
                    k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                }
                yield return shotInterval;
            }
            yield return cooldown;
        }
	}



	public override void PopItem(){
        k514ItemController tmp = Random.Range(0,10) > 7 ? k514SystemManager.ItemMgr.GetPooledItem(ITEM_TYPE.SCORE) : k514SystemManager.ItemMgr.GetPooledItem(ITEM_TYPE.POWER);
        Transform tmp2 = null;

        if(tmp != null){
            tmp2 = tmp.transform;
            tmp2.position = transform.position;
        }

        int cnt = Random.Range(4,10);
        for(int i = 0 ; i < cnt ; i++){
            tmp = k514SystemManager.ItemMgr.GetPooledItem(ITEM_TYPE.SCORE);
            if(tmp != null){
                tmp2 = tmp.transform;
                tmp2.position = transform.position + new Vector3(Random.Range(-3f,3f),Random.Range(-3f,3f),Random.Range(-2f,2f));
            }
        }
    }

    public override void DoInteract_Dead(){
        base.DoInteract_Dead();
        k514SystemManager.EnemyMgr.KillEnemy(this,ENEMY_TYPE.FAIRY2);
    }
}
