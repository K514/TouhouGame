using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514RumiaEncounterEvent : MonoBehaviour {

    private k514AudioSource RumiaFly = null;

    public IEnumerator RumiaStageOn(float delay){
        yield return new WaitForSeconds(delay);
        k514SystemManager.BlockMgr.MakeNight();
        // 화면 페이드 인
        StartCoroutine(k514SystemManager.UI_SpecaMgr.UI_Black_out(3f,2f));
        yield return new WaitForSeconds(2.5f);
        
        // 초기화
        k514SystemManager.InteractMgr.GetPlayerTransform().position = Vector3.Scale(new Vector3(0,1f,1f),k514SystemManager.InteractMgr.GetPlayerVector()) + Vector3.right*40f;
        k514EnemyController Rumia = k514SystemManager.EnemyMgr.GetPooledEnemy(ENEMY_TYPE.RUMIA);
        Transform RumiaTransform = Rumia.transform;
        Rigidbody RumiaRigid = RumiaTransform.GetComponent<Rigidbody>();
        k514RumiaPattern RumiaPattern = Rumia.GetComponent<k514RumiaPattern>();
        Animator RumiaAnim = Rumia.GetComponent<Animator>();
        
        RumiaTransform.position = k514SystemManager.InteractMgr.GetPlayerVector() + new Vector3(0,0f,6f);
        RumiaTransform.position = k514SystemManager.InteractMgr.GetPlayerVector() + new Vector3(-9f,3f,6f);
        RumiaFly = k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.FLYING, false, true);
    
        // 십자가 포즈
        RumiaAnim.SetBool("sonanoka",true);

        // 루미아 집중 조명
        Camera.main.GetComponent<uThirdPersonCamera>().SetTarget(RumiaTransform);
        
        // 루미아 베일 켠다.
        RumiaPattern.SetVail(true);

        // Rumia Move
        Vector3 targetPlace = k514SystemManager.InteractMgr.GetPlayerVector() + new Vector3(-6f,4f,8f);

        while((targetPlace - RumiaTransform.position).magnitude > 0.005f){
		    RumiaRigid.MovePosition(Vector3.MoveTowards(RumiaTransform.position, targetPlace, Time.deltaTime));
            yield return null;
        }

        // Rumia Move
        targetPlace = k514SystemManager.InteractMgr.GetPlayerVector() + new Vector3(8f,8f,9f);

        while((targetPlace - RumiaTransform.position).magnitude > 0.005f){
		    RumiaRigid.MovePosition(Vector3.MoveTowards(RumiaTransform.position, targetPlace, 3f*Time.deltaTime));
            yield return null;
        }

        RumiaFly.StopClip();


        // Rumia Move
        targetPlace = k514SystemManager.InteractMgr.GetPlayerVector() + new Vector3(-4f,6.5f,8f);
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.SWING);
        
        while((targetPlace - RumiaTransform.position).magnitude > 0.005f){
		    RumiaRigid.MovePosition(Vector3.MoveTowards(RumiaTransform.position, targetPlace, 30f*Time.deltaTime));
            yield return null;
        }


        // Rumia Move
        targetPlace = k514SystemManager.InteractMgr.GetPlayerVector() + new Vector3(2f,4f,7f);
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.SWING);

        while((targetPlace - RumiaTransform.position).magnitude > 0.005f){
		    RumiaRigid.MovePosition(Vector3.MoveTowards(RumiaTransform.position, targetPlace, 30f*Time.deltaTime));
            yield return null;
        }


        // Rumia Move
        targetPlace = k514SystemManager.InteractMgr.GetPlayerVector() + new Vector3(-1f,1.5f,7f);
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.SWING);

        while((targetPlace - RumiaTransform.position).magnitude > 0.005f){
		    RumiaRigid.MovePosition(Vector3.MoveTowards(RumiaTransform.position, targetPlace, 30f*Time.deltaTime));
            yield return null;
        }
        
        // Rumia Move
        targetPlace = k514SystemManager.InteractMgr.GetPlayerVector() + new Vector3(0.5f,0.5f,7f);
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.SWING);

        while((targetPlace - RumiaTransform.position).magnitude > 0.005f){
		    RumiaRigid.MovePosition(Vector3.MoveTowards(RumiaTransform.position, targetPlace, 30f*Time.deltaTime));
            yield return null;
        }

        // Rumia Move
        targetPlace = k514SystemManager.InteractMgr.GetPlayerVector() + new Vector3(0f,0f,5f);
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BODY.SWING);

        while((targetPlace - RumiaTransform.position).magnitude > 0.005f){
		    RumiaRigid.MovePosition(Vector3.MoveTowards(RumiaTransform.position, targetPlace, 30f*Time.deltaTime));
            yield return null;
        }



         // LookAt player
         RumiaTransform.LookAt(k514SystemManager.InteractMgr.GetPlayerVector());


        // Vail scale down
        float time = 2f, inversedTime = 1f/time;
        float startTime = 0f, normalize = 0f;
        Transform Vail = RumiaPattern.Vail;
        while(startTime < time){
            startTime += Time.deltaTime;
            normalize = startTime * inversedTime;
            Vail.localScale = Vector3.Lerp(Vector3.one*1.5f,Vector3.one*0.1f,normalize);
            yield return null;
        }

        // 루미아 베일 끈다.
        RumiaPattern.SetVail(false);
        Vail.localScale = Vector3.one * 1.5f;
        
        // 십자가 포즈 해제
        RumiaAnim.SetBool("sonanoka",false);
        
        // 2초 대기
        yield return new WaitForSeconds(2f);
        
        
        // 대화 이벤트
        k514SystemManager.SerifuMgr.Act();
        while(!k514SystemManager.SerifuMgr.isEnd()){
            yield return null;
        }
        k514SystemManager.SerifuMgr.EndProcess();
        

        // 2초 대기
        yield return new WaitForSeconds(1f);
        

           
        StartCoroutine(k514SystemManager.UI_SpecaMgr.UI_Slide_out(1f));
        Camera.main.GetComponent<uThirdPersonCamera>().ReleaseTarget();

        k514SystemManager.UI_SpecaMgr.ToggleUpperBar(RumiaPattern);
        // 4초 있다가 루미아 패턴시작
        yield return new WaitForSeconds(3f);
		k514SystemManager.UI_SpecaMgr.PumpBgmName(1);
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BGM.BGM2,false,true);
        RumiaPattern.StartPattern();
        


    }

}
