using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514RumiaPattern : k514EnemyController {

    public Transform Vail;
    private Vector3 RumiaOffset = new Vector3(20f,30f,20f);

    protected override IEnumerator GetAttckPatterns(){
        // switch(Pattren){
        //     case 0 :
		//     return AttackPattern();
        //     case 1 :
		//     return AttackPattern1();
        //     case 2 :
		//     return AttackPattern2();
        //     default :
		//     return AttackPattern3();
        // }
        return AttackPattern1();
	}

	protected override void Update(){
        base.Update();
        // if(k514SystemManager.InteractMgr.GetPlayerVector().z - transform.position.z > 1f){
        //     DoInteract_Dead();
        // }
	}

    public void SetVail(bool b){
        Vail.gameObject.SetActive(b);
    }

    // 5연발
    protected IEnumerator AttackPattern1(){
		yield return null;
	}



	public override void PopItem(){
        k514ItemController tmp = Random.Range(0,10) > 7 ? k514SystemManager.ItemMgr.GetPooledItem(ITEM_TYPE.SCORE) : k514SystemManager.ItemMgr.GetPooledItem(ITEM_TYPE.POWER);
        Transform tmp2 = null;

        if(tmp != null){
            tmp2 = tmp.transform;
            tmp2.position = transform.position;
        }

        int cnt = Random.Range(4,14);
        for(int i = 0 ; i < cnt ; i++){
            tmp = k514SystemManager.ItemMgr.GetPooledItem(ITEM_TYPE.SCORE);
            if(tmp != null){
                tmp2 = tmp.transform;
                tmp2.position = transform.position + new Vector3(Random.Range(-3f,3f),Random.Range(-3f,3f),Random.Range(-2f,2f));
            }
        }
    }

    public void StartPattern(){
        isBoss = true;
        // player와 거리를 유지하는 패턴을 수행한다.
        k514SystemManager.InteractMgr.GetCrossHair().Set_Cross_HairRange(15f);
        StartCoroutine(flyAndYConserveDistanse());
        StartCoroutine(ChangeRandomXY());
        StartCoroutine(NormalPattern());

    }

    IEnumerator flyAndYConserveDistanse(){

        // 소나노카-의 포즈
        Vector3 jumpup = transform.position + Vector3.up*1.5f;
        while(( jumpup - transform.position ).magnitude >0.005f){
		        rigid.MovePosition(Vector3.MoveTowards(transform.position, jumpup, Time.deltaTime));
                yield return null;
        }
        anim.SetBool("sonanoka",true);
        yield return new WaitForSeconds(3f);

        k514SystemManager.InteractMgr.GetPlayerTransform().GetComponent<k514PlayerController>().EventEnd();

        while(true){


            transform.LookAt(k514SystemManager.InteractMgr.GetPlayerVector());
            if(( k514SystemManager.InteractMgr.GetPlayerVector().z - transform.position.z ) < RumiaOffset.z){
		        rigid.MovePosition(Vector3.MoveTowards(transform.position, Vector3.Scale(new Vector3(0,0,1),k514SystemManager.InteractMgr.GetPlayerVector()) + RumiaOffset, 15f*Time.deltaTime));
            }

            // 루미아가 뒤쳐지면
            while(k514SystemManager.InteractMgr.GetPlayerVector().z > transform.position.z + RumiaOffset.z){
                transform.LookAt(k514SystemManager.InteractMgr.GetPlayerVector());
		        rigid.MovePosition(Vector3.MoveTowards(transform.position, transform.position + Vector3.forward, 15f*Time.deltaTime));
                yield return null;
            }

            yield return null;
        }
        
    }


    IEnumerator ChangeRandomXY(){
        yield return null;
        yield return new WaitForSeconds(8f);
        while(true){
            RumiaOffset = (k514SystemManager.InteractMgr.GetPlayerVector().y > 20f) ? new Vector3(Random.Range(5f,75f),Random.Range(5f,38f),RumiaOffset.z) : new Vector3(Random.Range(5f,75f),Random.Range(10f,44f),RumiaOffset.z);
            yield return new WaitForSeconds(Random.Range(2f,7f));
        }        
    }


    // 통상패턴
    IEnumerator NormalPattern(){
        yield return null;
        yield return new WaitForSeconds(10f);
        WaitForSeconds cooldown = new WaitForSeconds(4f);
        WaitForSeconds shotInterval = null;
        Transform bullet = null;
        k514PatternedShot patternedShot = null;
        k514BulletBirthPattern moveDirect = null;
        k514WaitSec waitPattern = null;
        k514ChangeScale scalePattern = null;
        k514ChangeRotate rotatePattern = null;
        k514PlaySoundPattern soundPattern = null;
        k514MoveDirect movePattern = null;
        k514BulletBirthPattern birthPattern = null;
        k514MoveBezier bezierPattern = null;

        Vector3 tar ;

        List<k514EnemyBehaviour> list;

		while(this.HP > 500){
            for(int i = 0 ; i < Random.Range(3,6) ; i++){
                k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BULLET.SHOT2,false);
                bullet = k514SystemManager.BulletMgr.CreateBullet<k514PatternedShot>(BULLET_TYPE.THUNDER,transform.forward,0.1f,1f,null,100);
                bullet.position = transform.position + new Vector3(Random.Range(-14f,14f),Random.Range(-6f,6f),0f);
                bullet.Rotate(Vector3.forward,Random.Range(0f,180f));
                bullet.gameObject.name = "Thunder";
                bullet.parent = k514SystemManager.BulletMgr.transform;
                patternedShot = bullet.GetComponent<k514PatternedShot>();

                // not loop the pattern
                patternedShot.SetLoop(false);

                 // get patterns list
                list = patternedShot.patterns;

                // 0.5초 대기
                waitPattern = (k514WaitSec)k514EnemyPatternFactory.CreatePattern<k514WaitSec>(patternedShot);
                        patternedShot.SetInfo(waitPattern);
                        waitPattern.Init(patternedShot,0.5f);
                        list.Add(waitPattern);


                // 스케일 키우기
                scalePattern = (k514ChangeScale)k514EnemyPatternFactory.CreatePattern<k514ChangeScale>(patternedShot);
                        patternedShot.SetInfo(scalePattern);
                        scalePattern.Init(patternedShot,new Vector3(1f,80f,1f));
                        list.Add(scalePattern);

                // 0.5초 대기
                waitPattern = (k514WaitSec)k514EnemyPatternFactory.CreatePattern<k514WaitSec>(patternedShot);
                        patternedShot.SetInfo(waitPattern);
                        waitPattern.Init(patternedShot,0.1f);
                        list.Add(waitPattern);


                // 회전
                rotatePattern = (k514ChangeRotate)k514EnemyPatternFactory.CreatePattern<k514ChangeRotate>(patternedShot);
                        patternedShot.SetInfo(rotatePattern);
                        rotatePattern.Init(patternedShot,new Vector3(0f,0f,1f),30f);
                        list.Add(rotatePattern);

                k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                
               yield return new WaitForSeconds(0.1f);
            }
            yield return cooldown;
        }


//        1스펠 선언 및 체력회복
        k514SystemManager.UI_SpecaMgr.EnemySpellAnimate(0,2f);
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.SPELL_ACTIVE);
        this.HP += 700;


        yield return new WaitForSeconds(2f);
        cooldown = new WaitForSeconds(8f);

        float theta = 0f;
        int step = 0;
        float radius = 8f;
        float r_step = 0f;
		while(this.HP > 700){
            step = Random.Range(3,9);
            
            // 전방사격
            StartCoroutine(ForwardShot());
            yield return new WaitForSeconds(0.3f);
            
            k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.KIRA,false);
            
            for(int i = 0 ; i < step ; i++){
                r_step = 360f / step;
                theta = i * r_step;
                theta *= Mathf.Deg2Rad;

                bullet = k514SystemManager.BulletMgr.CreateBullet<k514PatternedShot>(BULLET_TYPE.THUNDER,transform.forward,0.1f,1f,null,100);
                bullet.position = transform.position + radius*new Vector3(Mathf.Cos(theta),Mathf.Sin(theta),2f);
                bullet.Rotate(Vector3.back,90f-i*r_step);
                bullet.Rotate(Vector3.right,90f);
                //bullet.up = k514SystemManager.InteractMgr.GetPlayerHitVectorFrom(bullet.position);
                Debug.Log(bullet.up);                
                //bullet.LookAt(transform.position);
                bullet.gameObject.name = "Thunder";
                bullet.parent = k514SystemManager.BulletMgr.transform;
                patternedShot = bullet.GetComponent<k514PatternedShot>();


                // not loop the pattern
                patternedShot.SetLoop(false);


                 // get patterns list
                list = patternedShot.patterns;


                // 0.2초 대기
                waitPattern = (k514WaitSec)k514EnemyPatternFactory.CreatePattern<k514WaitSec>(patternedShot);
                        patternedShot.SetInfo(waitPattern);
                        waitPattern.Init(patternedShot,0.5f);
                        list.Add(waitPattern);


                // 스케일 키우기
                scalePattern = (k514ChangeScale)k514EnemyPatternFactory.CreatePattern<k514ChangeScale>(patternedShot);
                        patternedShot.SetInfo(scalePattern);
                        scalePattern.Init(patternedShot,new Vector3(1f,80f,1f));
                        list.Add(scalePattern);

                // 0.2초 대기
                waitPattern = (k514WaitSec)k514EnemyPatternFactory.CreatePattern<k514WaitSec>(patternedShot);
                        patternedShot.SetInfo(waitPattern);
                        waitPattern.Init(patternedShot,0.2f);
                        list.Add(waitPattern);

                        if(i == 0){
                            // bullet pattern 2 : KIRA
                            soundPattern = (k514PlaySoundPattern)k514EnemyPatternFactory.CreatePattern<k514PlaySoundPattern>(patternedShot);
                            soundPattern.Init(patternedShot,SFX_TYPE_BULLET.BEAM);
                            list.Add(soundPattern);
                        }


                k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                
            }

            yield return cooldown;
        }


    


//        통상 2
        while(this.HP > 500){

            StartCoroutine(ForwardShot());

            for(int i = 0 ; i < Random.Range(5,8) ; i++){
                k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BULLET.SHOT2,false);
                bullet = k514SystemManager.BulletMgr.CreateBullet<k514PatternedShot>(BULLET_TYPE.THUNDER,transform.forward,0.1f,1f,null,100);
                bullet.position = transform.position + new Vector3(Random.Range(-14f,14f),Random.Range(-6f,6f),0f);
                bullet.Rotate(Vector3.forward,Random.Range(0f,180f));
                bullet.gameObject.name = "Thunder";
                bullet.parent = k514SystemManager.BulletMgr.transform;
                patternedShot = bullet.GetComponent<k514PatternedShot>();

                // not loop the pattern
                patternedShot.SetLoop(false);

                 // get patterns list
                list = patternedShot.patterns;

                // 0.5초 대기
                waitPattern = (k514WaitSec)k514EnemyPatternFactory.CreatePattern<k514WaitSec>(patternedShot);
                        patternedShot.SetInfo(waitPattern);
                        waitPattern.Init(patternedShot,0.5f);
                        list.Add(waitPattern);


                // 스케일 키우기
                scalePattern = (k514ChangeScale)k514EnemyPatternFactory.CreatePattern<k514ChangeScale>(patternedShot);
                        patternedShot.SetInfo(scalePattern);
                        scalePattern.Init(patternedShot,new Vector3(1f,80f,1f));
                        list.Add(scalePattern);

                // 0.5초 대기
                waitPattern = (k514WaitSec)k514EnemyPatternFactory.CreatePattern<k514WaitSec>(patternedShot);
                        patternedShot.SetInfo(waitPattern);
                        waitPattern.Init(patternedShot,0.1f);
                        list.Add(waitPattern);


                // 회전
                rotatePattern = (k514ChangeRotate)k514EnemyPatternFactory.CreatePattern<k514ChangeRotate>(patternedShot);
                        patternedShot.SetInfo(rotatePattern);
                        rotatePattern.Init(patternedShot,new Vector3(0f,0f,1f),30f);
                        list.Add(rotatePattern);

                k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                
               yield return new WaitForSeconds(0.1f);
            }
            yield return cooldown;
        }


//        2스펠 선언 및 체력회복
        k514SystemManager.UI_SpecaMgr.EnemySpellAnimate(1,2f);
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.SPELL_ACTIVE);
        this.HP += 700;

        yield return new WaitForSeconds(2f);
        cooldown = new WaitForSeconds(3f);
        shotInterval = new WaitForSeconds(0.2f);

		while(this.HP > 700){
            for(int i = 0 ; i < 5 ; i++){
                k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BULLET.SHOT2,true);
                theta = Random.Range(2f,12f);
                step = Random.Range(12,23);
                radius = Random.Range(-0.23f,0.23f);
                r_step = theta/step;
                for(int j = 0 ; j < step ; j++){
                    bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.SHADOW,((k514SystemManager.InteractMgr.GetPlayerHitVector() + new Vector3(theta - 2f*j*r_step,radius,0f) - transform.position)).normalized,0.05f,14f);
                    bullet.position = transform.position;
                    bullet.gameObject.name = "Shadow";
                    bullet.parent = k514SystemManager.BulletMgr.transform;
                    k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());

                    bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.TEST,((k514SystemManager.InteractMgr.GetPlayerHitVector() + new Vector3(-theta + 2f*j*r_step,-radius,0f) - transform.position)).normalized,0.05f,14f);
                    bullet.position = transform.position;
                    bullet.gameObject.name = "Test";
                    bullet.parent = k514SystemManager.BulletMgr.transform;
                    k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                    yield return new WaitForSeconds(0.02f);
                }

				yield return shotInterval;
            }
            yield return cooldown;
        }




    cooldown = new WaitForSeconds(5f);
	while(this.HP > 500){
            step = Random.Range(5,8);
            for(int i = 0 ; i < step ; i++){
                theta = Random.Range(0f,180f);
                tar = new Vector3(Random.Range(-14f,14f),Random.Range(-14f,14f),0f);
                k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.KIRA,false);
                bullet = k514SystemManager.BulletMgr.CreateBullet<k514PatternedShot>(BULLET_TYPE.THUNDER,transform.forward,0.1f,1f,null,100);
                bullet.position = transform.position + tar;
                bullet.Rotate(Vector3.forward,theta);
                bullet.gameObject.name = "Thunder";
                bullet.parent = k514SystemManager.BulletMgr.transform;
                patternedShot = bullet.GetComponent<k514PatternedShot>();

                // not loop the pattern
                patternedShot.SetLoop(false);

                 // get patterns list
                list = patternedShot.patterns;



                // 스케일 키우기
                scalePattern = (k514ChangeScale)k514EnemyPatternFactory.CreatePattern<k514ChangeScale>(patternedShot);
                        patternedShot.SetInfo(scalePattern);
                        scalePattern.Init(patternedShot,new Vector3(1f,80f,1f));
                        list.Add(scalePattern);


                // 회전
                rotatePattern = (k514ChangeRotate)k514EnemyPatternFactory.CreatePattern<k514ChangeRotate>(patternedShot);
                        patternedShot.SetInfo(rotatePattern);
                        rotatePattern.Init(patternedShot,new Vector3(0f,0f,1f),45f);    
                        list.Add(rotatePattern);

                k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                

                bullet = k514SystemManager.BulletMgr.CreateBullet<k514PatternedShot>(BULLET_TYPE.THUNDER,transform.forward,0.1f,1f,null,100);
                bullet.position = transform.position + tar;
                bullet.Rotate(Vector3.forward,theta+90f);
                bullet.gameObject.name = "Thunder";
                bullet.parent = k514SystemManager.BulletMgr.transform;
                patternedShot = bullet.GetComponent<k514PatternedShot>();

                // not loop the pattern
                patternedShot.SetLoop(false);

                 // get patterns list
                list = patternedShot.patterns;



                // 스케일 키우기
                scalePattern = (k514ChangeScale)k514EnemyPatternFactory.CreatePattern<k514ChangeScale>(patternedShot);
                        patternedShot.SetInfo(scalePattern);
                        scalePattern.Init(patternedShot,new Vector3(1f,80f,1f));
                        list.Add(scalePattern);


                // 회전
                rotatePattern = (k514ChangeRotate)k514EnemyPatternFactory.CreatePattern<k514ChangeRotate>(patternedShot);
                        patternedShot.SetInfo(rotatePattern);
                        rotatePattern.Init(patternedShot,new Vector3(0f,0f,1f),45f);
                        list.Add(rotatePattern);

                k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());


                for(int j = 0 ; j < 10 ; j++){
                    bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.TEST,((k514SystemManager.InteractMgr.GetPlayerHitVector() - transform.position)).normalized,0.05f,14f);
                    bullet.position = transform.position + tar + new Vector3(5 - j , 0 , 0);
                    bullet.gameObject.name = "Test";
                    bullet.parent = k514SystemManager.BulletMgr.transform;
                    k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                }
                for(int k = 0 ; k < 10 ; k++){
                    bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.TEST,((k514SystemManager.InteractMgr.GetPlayerHitVector() - transform.position)).normalized,0.05f,14f);
                    bullet.position = transform.position + tar + new Vector3(0, 5 -k , 0);
                    bullet.gameObject.name = "Test";
                    bullet.parent = k514SystemManager.BulletMgr.transform;
                    k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                }


               yield return new WaitForSeconds(0.15f);
            }
            yield return cooldown;
        }



//        3스펠 선언 및 체력회복
        k514SystemManager.UI_SpecaMgr.EnemySpellAnimate(2,2f);
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.SPELL_ACTIVE);
        this.HP += 900;

        yield return new WaitForSeconds(2f);
        cooldown = new WaitForSeconds(3f);
        shotInterval = new WaitForSeconds(0.2f);

		while(this.HP > 700){

            k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.KIRA,true);
            for(int i = 0 ; i < 20 ; i ++){
                bullet = k514SystemManager.BulletMgr.CreateBullet<k514PatternedShot>(BULLET_TYPE.THUNDER,transform.forward,0.1f,1f,null,100);
                bullet.position = transform.position + Vector3.up*(20f - 2*i);
                bullet.Rotate(Vector3.back,90f);
                bullet.gameObject.name = "Thunder";
                bullet.parent = k514SystemManager.BulletMgr.transform;
                patternedShot = bullet.GetComponent<k514PatternedShot>();


                // not loop the pattern
                patternedShot.SetLoop(false);


                 // get patterns list
                list = patternedShot.patterns;


                // 0.2초 대기
                waitPattern = (k514WaitSec)k514EnemyPatternFactory.CreatePattern<k514WaitSec>(patternedShot);
                        patternedShot.SetInfo(waitPattern);
                        waitPattern.Init(patternedShot,0.1f);
                        list.Add(waitPattern);


                // 스케일 키우기
                scalePattern = (k514ChangeScale)k514EnemyPatternFactory.CreatePattern<k514ChangeScale>(patternedShot);
                        patternedShot.SetInfo(scalePattern);
                        scalePattern.Init(patternedShot,new Vector3(0.3f,80f,0.3f));
                        list.Add(scalePattern);

                // 0.2초 대기
                waitPattern = (k514WaitSec)k514EnemyPatternFactory.CreatePattern<k514WaitSec>(patternedShot);
                        patternedShot.SetInfo(waitPattern);
                        waitPattern.Init(patternedShot,0.2f);
                        list.Add(waitPattern);

                        if(i == 0){
                            // bullet pattern 2 : KIRA
                            soundPattern = (k514PlaySoundPattern)k514EnemyPatternFactory.CreatePattern<k514PlaySoundPattern>(patternedShot);
                            soundPattern.Init(patternedShot,SFX_TYPE_BULLET.BEAM);
                            list.Add(soundPattern);
                        }


                //  이동
                    movePattern = (k514MoveDirect)k514EnemyPatternFactory.CreatePattern<k514MoveDirect>(patternedShot);
                    movePattern.SetSpeed(5f);
                    if(i%2 == 0) {
                        movePattern.Init(patternedShot,Vector3.up*100f);
                    }else{
                        movePattern.Init(patternedShot,Vector3.down*100f);                        
                    }
                    list.Add(movePattern);

                k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
            }


                for(int i = 0 ; i < 5 ; i++){
                    k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BULLET.SHOT2,true);
                    theta = Random.Range(2f,12f);
                    step = Random.Range(12,23);
                    radius = Random.Range(-0.23f,0.23f);
                    r_step = theta/step;
                    for(int j = 0 ; j < step ; j++){
                        bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.SHADOW,((k514SystemManager.InteractMgr.GetPlayerHitVector() + new Vector3(theta - 2f*j*r_step,radius+Random.Range(-0.2f,0.2f),0f) - transform.position)).normalized,0.05f,14f);
                        bullet.position = transform.position;
                        bullet.gameObject.name = "Shadow";
                        bullet.parent = k514SystemManager.BulletMgr.transform;
                        k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());

                        bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.SHADOW,((k514SystemManager.InteractMgr.GetPlayerHitVector() + new Vector3(-theta + 2f*j*r_step,-radius+Random.Range(-0.2f,0.2f),0f) - transform.position)).normalized,0.05f,14f);
                        bullet.position = transform.position;
                        bullet.gameObject.name = "Shadow";
                        bullet.parent = k514SystemManager.BulletMgr.transform;
                        k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                        yield return new WaitForSeconds(0.005f);
                    }
                    yield return shotInterval;
                }

            yield return cooldown;
        }


//    4통상
    cooldown = new WaitForSeconds(5f);
	while(this.HP > 600){
            step = Random.Range(5,8);
            for(int i = 0 ; i < step ; i++){
                theta = Random.Range(0f,180f);
                tar = new Vector3(Random.Range(-14f,14f),Random.Range(-14f,14f),0f);
                k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.KIRA,false);
                bullet = k514SystemManager.BulletMgr.CreateBullet<k514PatternedShot>(BULLET_TYPE.THUNDER,transform.forward,0.1f,1f,null,100);
                bullet.position = transform.position + tar;
                bullet.Rotate(Vector3.forward,theta);
                bullet.gameObject.name = "Thunder";
                bullet.parent = k514SystemManager.BulletMgr.transform;
                patternedShot = bullet.GetComponent<k514PatternedShot>();

                // not loop the pattern
                patternedShot.SetLoop(false);

                 // get patterns list
                list = patternedShot.patterns;



                // 스케일 키우기
                scalePattern = (k514ChangeScale)k514EnemyPatternFactory.CreatePattern<k514ChangeScale>(patternedShot);
                        patternedShot.SetInfo(scalePattern);
                        scalePattern.Init(patternedShot,new Vector3(1f,80f,1f));
                        list.Add(scalePattern);


                // 회전
                rotatePattern = (k514ChangeRotate)k514EnemyPatternFactory.CreatePattern<k514ChangeRotate>(patternedShot);
                        patternedShot.SetInfo(rotatePattern);
                        rotatePattern.Init(patternedShot,new Vector3(0f,0f,1f),45f);    
                        list.Add(rotatePattern);

                k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                

                bullet = k514SystemManager.BulletMgr.CreateBullet<k514PatternedShot>(BULLET_TYPE.THUNDER,transform.forward,0.1f,1f,null,100);
                bullet.position = transform.position + tar;
                bullet.Rotate(Vector3.forward,theta+90f);
                bullet.gameObject.name = "Thunder";
                bullet.parent = k514SystemManager.BulletMgr.transform;
                patternedShot = bullet.GetComponent<k514PatternedShot>();

                // not loop the pattern
                patternedShot.SetLoop(false);

                 // get patterns list
                list = patternedShot.patterns;



                // 스케일 키우기
                scalePattern = (k514ChangeScale)k514EnemyPatternFactory.CreatePattern<k514ChangeScale>(patternedShot);
                        patternedShot.SetInfo(scalePattern);
                        scalePattern.Init(patternedShot,new Vector3(1f,80f,1f));
                        list.Add(scalePattern);


                // 회전
                rotatePattern = (k514ChangeRotate)k514EnemyPatternFactory.CreatePattern<k514ChangeRotate>(patternedShot);
                        patternedShot.SetInfo(rotatePattern);
                        rotatePattern.Init(patternedShot,new Vector3(0f,0f,1f),45f);
                        list.Add(rotatePattern);

                k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());


                for(int j = 0 ; j < 10 ; j++){
                    bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.TEST,((k514SystemManager.InteractMgr.GetPlayerHitVector() - transform.position)).normalized,0.05f,14f);
                    bullet.position = transform.position + tar + new Vector3(5 - j , 0 , 0);
                    bullet.gameObject.name = "Test";
                    bullet.parent = k514SystemManager.BulletMgr.transform;
                    k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                }
                for(int k = 0 ; k < 10 ; k++){
                    bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.TEST,((k514SystemManager.InteractMgr.GetPlayerHitVector() - transform.position)).normalized,0.05f,14f);
                    bullet.position = transform.position + tar + new Vector3(0, 5 -k , 0);
                    bullet.gameObject.name = "Test";
                    bullet.parent = k514SystemManager.BulletMgr.transform;
                    k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                }


               yield return new WaitForSeconds(0.15f);
            }
            yield return cooldown;
        }



        // 4스펠 선언 및 체력회복
        k514SystemManager.UI_SpecaMgr.EnemySpellAnimate(3,2f);
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.SPELL_ACTIVE);
        this.HP += 900;

        yield return new WaitForSeconds(2f);
        cooldown = new WaitForSeconds(3f);
        shotInterval = new WaitForSeconds(0.2f);
        float theta0 , theta1 , sin0 , cos0 , sin1 , cos1 , inversed_emit = 0f;

        // Vail scale down
        SetVail(true);
        Vector3 here;
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.MUSOU_START,true);
        float time = 2f, inversedTime = 1f/time;
        float startTime = 0f, normalize = 0f;
        while(startTime < time){
            startTime += Time.deltaTime;
            normalize = startTime * inversedTime;
            Vail.localScale = Vector3.Lerp(Vector3.one*0.1f,Vector3.one*4f,normalize);
            yield return null;
        }


		while(this.HP > 700){

                // 나선
                k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.MUSOU_START,false);
                step = Random.Range(2,8);
                radius = Random.Range(15,25);

                for(int i = 0 ; i < step ; i++){
                    inversed_emit = 360f/step;
                    theta0 = i*inversed_emit*Mathf.Deg2Rad;
                    theta1 = (i*inversed_emit+30f)*Mathf.Deg2Rad;
                    sin0 = Mathf.Sin(theta0);
                    cos0 = Mathf.Cos(theta0);
                    sin1 = Mathf.Sin(theta1);
                    cos1 = Mathf.Cos(theta1);


                    // gene bullet
                    k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BULLET.SHOT1,false);
                    bullet = k514SystemManager.BulletMgr.CreateBullet<k514PatternedShot>(BULLET_TYPE.RUMIA,transform.forward,0.1f,1f,null,10);
                    bullet.position = here = transform.position + Vector3.forward*5f;
                    bullet.gameObject.name = "Rumia";
                    bullet.parent = k514SystemManager.BulletMgr.transform;
                    patternedShot = bullet.GetComponent<k514PatternedShot>();

                    // not loop the pattern
                    patternedShot.SetLoop(false);

                    // get patterns list
                    list = patternedShot.patterns;

                    // bullet pattern 0 : 나선
                    bezierPattern = (k514MoveBezier)k514EnemyPatternFactory.CreatePattern<k514MoveBezier>(patternedShot);
                    patternedShot.SetInfo(bezierPattern);
                    bezierPattern.Init(patternedShot,here,here + new Vector3(0.33f*radius*cos1,0.33f*radius*sin1,1f),here + new Vector3(0.66f*radius*cos1,0.66f*radius*sin1,2f),here + new Vector3(radius*cos0,radius*sin0,3f),1f);
                    list.Add(bezierPattern);
        
                    // p1 : birth p
                    birthPattern = (k514BulletBirthPattern)k514EnemyPatternFactory.CreatePattern<k514BulletBirthPattern>(patternedShot);
                    patternedShot.SetInfo(birthPattern);
                    birthPattern.Init(patternedShot); 
                    list.Add(birthPattern);
                
                    k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                }


                yield return cooldown;
        }

        // Vail scale down
        time = 2f;
        inversedTime = 1f/time;
        startTime = 0f;
         normalize = 0f;
        while(startTime < time){
            startTime += Time.deltaTime;
            normalize = startTime * inversedTime;
            Vail.localScale = Vector3.Lerp(Vector3.one*4f,Vector3.one*0.1f,normalize);
            yield return null;
        }
        
        SetVail(false);
        this.HP += 400;
        yield return new WaitForSeconds(2f);
        
        // 5 스펠
        k514SystemManager.UI_SpecaMgr.EnemySpellAnimate(4,2f);
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.SPELL_ACTIVE);
        yield return new WaitForSeconds(2f);
        
        cooldown = new WaitForSeconds(3f);
        shotInterval = new WaitForSeconds(0.2f);
        int clip = -1;
		while(this.HP > 0){
            for(int i = 0 ; i < 3 ; i++){
                k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BULLET.BEAM,true);
                theta = Random.Range(2f,12f);
                step = Random.Range(1,4);
                radius = Random.Range(-0.23f,0.23f);
                r_step = theta/step;
                for(int j = 0 ; j < step ; j++){
                    bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.RUMIA2,((k514SystemManager.InteractMgr.GetPlayerHitVector() + new Vector3((theta - 2f*j*r_step)*clip,0f,0f) - transform.position)).normalized,0.05f,14f);
                    bullet.position = transform.position;
                    bullet.localScale = Vector3.one * 0.2f;
                    bullet.gameObject.name = "Rumia2";
                    bullet.parent = k514SystemManager.BulletMgr.transform;
                    k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());

                    bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.RUMIA2,((k514SystemManager.InteractMgr.GetPlayerHitVector() + new Vector3(0f,(-theta + 2f*j*r_step)*clip,0f) - transform.position)).normalized,0.05f,14f);
                    bullet.position = transform.position;
                    bullet.localScale = Vector3.one * 0.2f;
                    bullet.gameObject.name = "Rumia2";
                    bullet.parent = k514SystemManager.BulletMgr.transform;
                    k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                    clip *= -1;
                    yield return new WaitForSeconds(0.02f);
                }

				yield return shotInterval;
            }
            
                int x = 1 + ((int)MAX_HP-(int)HP)/120;
                Debug.Log(MAX_HP +" : "+HP+" : "+x );
                for(int xi = 0 ; xi < x ; xi++){
                    k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BULLET.SHOT2,false);
                    bullet = k514SystemManager.BulletMgr.CreateBullet<k514PatternedShot>(BULLET_TYPE.THUNDER,transform.forward,0.1f,1f,null,100);
                    bullet.position = transform.position + new Vector3(Random.Range(-20f,20f),Random.Range(-9f,9f),Random.Range(10f,20f));
                    bullet.Rotate(Vector3.forward,Random.Range(0f,180f));
                    bullet.gameObject.name = "Thunder";
                    bullet.parent = k514SystemManager.BulletMgr.transform;
                    patternedShot = bullet.GetComponent<k514PatternedShot>();

                    // not loop the pattern
                    patternedShot.SetLoop(false);

                    // get patterns list
                    list = patternedShot.patterns;

                    // 0.5초 대기
                    waitPattern = (k514WaitSec)k514EnemyPatternFactory.CreatePattern<k514WaitSec>(patternedShot);
                            patternedShot.SetInfo(waitPattern);
                            waitPattern.Init(patternedShot,0.5f);
                            list.Add(waitPattern);


                    // 스케일 키우기
                    scalePattern = (k514ChangeScale)k514EnemyPatternFactory.CreatePattern<k514ChangeScale>(patternedShot);
                            patternedShot.SetInfo(scalePattern);
                            scalePattern.Init(patternedShot,new Vector3(0.5f,80f,0.5f));
                            list.Add(scalePattern);

                    // 0.5초 대기
                    waitPattern = (k514WaitSec)k514EnemyPatternFactory.CreatePattern<k514WaitSec>(patternedShot);
                            patternedShot.SetInfo(waitPattern);
                            waitPattern.Init(patternedShot,0.1f);
                            list.Add(waitPattern);


                    // 회전
                    rotatePattern = (k514ChangeRotate)k514EnemyPatternFactory.CreatePattern<k514ChangeRotate>(patternedShot);
                            patternedShot.SetInfo(rotatePattern);
                            rotatePattern.Init(patternedShot,new Vector3(0f,0f,Random.Range(-1f,1f)),30f);
                            list.Add(rotatePattern);

                    k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                }
            yield return cooldown;
        }
        


    }


    protected IEnumerator ForwardShot(){
		yield return null;
		yield return new WaitForSeconds(firstAttackInterval);
        WaitForSeconds cooldown = new WaitForSeconds(7f), shotInterval = new WaitForSeconds(0.2f);
            for(int i = 0 ; i < 5 ; i++){
                k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BULLET.SHOT1,false);
                for(int j = 0 ; j < Random.Range(4f,6f) ; j++){
                    bullet = k514SystemManager.BulletMgr.CreateBullet<k514OneDirectionShot>(BULLET_TYPE.SHADOW,((k514SystemManager.InteractMgr.GetPlayerHitVector() + new Vector3(Random.Range(-2f,2f),Random.Range(-2f,2f),0f) - transform.position)).normalized,0.05f,7f);
                    bullet.position = transform.position;
                    bullet.gameObject.name = "Shadow";
                    bullet.parent = k514SystemManager.BulletMgr.transform;
                    k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
                }
				yield return shotInterval;
            }
	}




    public override void DoInteract_Dead(){
        base.DoInteract_Dead();
        Time.timeScale = 0.1f;
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.BOSS_RETIRE);
        k514SystemManager.BulletMgr.KillAllBullet();
        k514SystemManager.InteractMgr.GetCamera().trig = true;
        k514SystemManager.InteractMgr.GetCrossHair().Set_Cross_HairRange(10f);
        k514SystemManager.EnemyMgr.KillEnemy(this,ENEMY_TYPE.RUMIA);
        k514SystemManager.GameMgr.GoEnd();
    }
}
