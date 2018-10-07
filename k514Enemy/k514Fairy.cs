using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514Fairy : k514EnemyController {

    public int Pattren = 0;

    protected override IEnumerator GetAttckPatterns(){
        switch(Pattren){
            case 0 :
		    return AttackPattern();
            default :
		    return AttackPattern1();
        }
	}

	protected override void Update(){
        base.Update();
        if(k514SystemManager.InteractMgr.GetPlayerVector().z - transform.position.z > 1f){
            DoInteract_Dead();
        }
	}

	protected IEnumerator AttackPattern1(){
		yield return null;
		yield return new WaitForSeconds(firstAttackInterval);

        List<k514EnemyBehaviour> tmp;
        k514MoveBezier tmp2 = null;
        k514PatternedShot tmp3 = null;
        k514WaitSec tmp4 = null;
        k514MoveDirect tmp5 = null;
        k514PlaySoundPattern tmp6 = null;

        // 반지름 5로 나선형으로 탄막이 6개 펼쳐지는 탄
        int emit_danmak = 4;
        float radius = 20f, theta0 , theta1 , sin0 , cos0 , sin1 , cos1 , inversed_emit = 360f/emit_danmak;
        Vector3 here;
		 while(true){
                for(int i = 0 ; i < 5 ; i++){
                    k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BULLET.SHOT1,false);
                    for(int j = 0 ; j < emit_danmak ; j++){

                        // bullet generate
                        bullet = k514SystemManager.BulletMgr.CreateBullet<k514PatternedShot>(BULLET_TYPE.TEST2,transform.forward,0.1f,5f,null,12);
                        bullet.position = here = transform.position;
                        bullet.gameObject.name = "Test2";
                        bullet.parent = k514SystemManager.BulletMgr.transform;
                        tmp3 = bullet.GetComponent<k514PatternedShot>();

                        // not loop the pattern
                        tmp3.SetLoop(false);

                        // set math factor
                        theta0 = j*inversed_emit*Mathf.Deg2Rad;
                        theta1 = (j*inversed_emit+30f)*Mathf.Deg2Rad;
                        sin0 = Mathf.Sin(theta0);
                        cos0 = Mathf.Cos(theta0);
                        sin1 = Mathf.Sin(theta1);
                        cos1 = Mathf.Cos(theta1);
                        
                        // get patterns list
                        tmp = tmp3.patterns;

                        // bullet pattern 0 : 나선
                        tmp2 = (k514MoveBezier)k514EnemyPatternFactory.CreatePattern<k514MoveBezier>(tmp3);
                        tmp3.SetInfo(tmp2);
                        tmp2.Init(tmp3,here,here + new Vector3(0.33f*radius*cos1,0.33f*radius*sin1,0f),here + new Vector3(0.66f*radius*cos1,0.66f*radius*sin1,0f),here + new Vector3(radius*cos0,radius*sin0,0f),1f-0.2f*i);
                        tmp.Add(tmp2);

                        // bullet pattern 1 : 대기
                        tmp4 = (k514WaitSec)k514EnemyPatternFactory.CreatePattern<k514WaitSec>(tmp3);
                        tmp3.SetInfo(tmp4);
                        tmp4.Init(tmp3,3f-i*0.3f);
                        tmp.Add(tmp4);

                        if(i == 4){
                            // bullet pattern 2 : KIRA
                            tmp6 = (k514PlaySoundPattern)k514EnemyPatternFactory.CreatePattern<k514PlaySoundPattern>(tmp3);
                            tmp6.Init(tmp3,SFX_TYPE_EFFECT.KIRA);
                            tmp.Add(tmp6);
                        }
                        
                        // bullet pattern 3 : 대기
                        tmp4 = (k514WaitSec)k514EnemyPatternFactory.CreatePattern<k514WaitSec>(tmp3);
                        tmp3.SetInfo(tmp4);
                        tmp4.Init(tmp3,0.1f);
                        tmp.Add(tmp4);

                        // bullet pattern 4 : 발사
                        tmp5 = (k514MoveDirect)k514EnemyPatternFactory.CreatePattern<k514MoveDirect>(tmp3);
                        tmp3.SetInfo(tmp5);
                        tmp5.Init_LateMoving(tmp3);
                        tmp.Add(tmp5);

                        // bullet pattern 5 : 대기
                        tmp4 = (k514WaitSec)k514EnemyPatternFactory.CreatePattern<k514WaitSec>(tmp3);
                        tmp3.SetInfo(tmp4);
                        tmp4.Init(tmp3,0.05f);
                        tmp.Add(tmp4);

                        // regist danmak
                        k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());

                    }
                    yield return new WaitForSeconds(0.2f);
                }
				yield return coolTime;
				yield return coolTime;
		 }
	}

	public override void PopItem(){
        k514ItemController tmp = Random.Range(0,10) > 5 ? k514SystemManager.ItemMgr.GetPooledItem(ITEM_TYPE.SCORE) : k514SystemManager.ItemMgr.GetPooledItem(ITEM_TYPE.POWER);
        Transform tmp2 = null;

        if(tmp != null){
            tmp2 = tmp.transform;
            tmp2.position = transform.position;
        }

        int cnt = Random.Range(2,7);
        for(int i = 0 ; i < cnt ; i++){
            tmp = k514SystemManager.ItemMgr.GetPooledItem(ITEM_TYPE.SCORE);
            if(tmp != null){
                tmp2 = tmp.transform;
                tmp2.position = transform.position + new Vector3(Random.Range(-2f,2f),Random.Range(-2f,2f),Random.Range(-2f,2f));
            }
        }
    }

    public override void DoInteract_Dead(){
        base.DoInteract_Dead();
        k514SystemManager.EnemyMgr.KillEnemy(this,ENEMY_TYPE.FAIRY);
    }
}
