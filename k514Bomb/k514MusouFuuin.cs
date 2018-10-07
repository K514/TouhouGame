using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514MusouFuuin : IPatternable {

    private int id = 0;
    private bool ActivateTrig = false;
    private Transform targetPlace = null,master = null;
    private Vector3 targetVector;
	[System.NonSerialized]public List<k514EnemyBehaviour> patterns;
	protected k514EnemyBehaviour currentPatterns = null;
	protected int patternsIndex = 0;


    void Start(){
		patterns = new List<k514EnemyBehaviour>();
        master = GameObject.Find("ReimuBomb").transform;
        SetLoop(false);
    }

    public int GetID(){
        return id;
    }

    public bool isAlive(){
        return ActivateTrig;
    }

    public float GetDamage(){
        return 40f;
    }

    public void InitPattern(int id){
        this.id = id;
        transform.parent = null;
        float RandomCubric = 3;
        float Radius1 = Random.Range(7f,12f), Radius2 = Random.Range(7f,16f);
        float theta = Random.Range(-45f,225f) * Mathf.Deg2Rad;
        float distance = 0f;

        targetPlace = k514SystemManager.InteractMgr.GetCrossHairTargetTransform();
        targetVector = (targetPlace==null) ? transform.position+Vector3.forward * 30f : targetPlace.position ;
        // randomize
        targetVector += new Vector3(Random.Range(-RandomCubric,RandomCubric),Random.Range(-2,2),Random.Range(-RandomCubric,RandomCubric+2));

        // get distance
        distance = (transform.position - targetVector).magnitude;

        // add patterns

        List<k514EnemyBehaviour> tmp;
        k514PatternedShot tmp3 = null;
        k514WaitSec tmp4 = null;
        k514MoveDirect tmp5 = null;
        Vector3 here = k514SystemManager.InteractMgr.GetPlayerVector();

        float sin = Mathf.Sin(theta), cos = Mathf.Cos(theta);

        k514MoveBezier tmp2 = (k514MoveBezier)k514EnemyPatternFactory.CreatePattern<k514MoveBezier>(this);
        tmp2.Init(this,here,here + new Vector3(Radius1*cos,Radius1*sin,distance*0.1f),here + new Vector3(Radius2*cos,Radius2*sin,distance*0.66f),targetVector);
        
        // set Speed
        tmp2.SetSpeed(1000f);
        tmp2.SetLifeSpanCnt(20);
        tmp2.SetMoveTime(0.005f);

        patterns.Add(tmp2);

        ActivateTrig = true;
    }


    void Update(){
        if(!ActivateTrig) return;
        
		if(currentPatterns == null && patternsIndex == patterns.Count){
			Interactive_Burst();
            return;
		};

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

    }


    void Interactive_Burst(){
        k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_EFFECT.MUSOU_HIT,true);
        k514SystemManager.EffectMgr.CastEffect(EFFECT_TYPE.MUSOU,transform.position);
        EndProcess();
    }


    void EndProcess(){
        ActivateTrig = false;
        targetPlace = null;
        patternsIndex = 0;
        currentPatterns = null;
        
		// 패턴 제거
        Transform tmp = null;
		for(int i = 0 ; i < transform.childCount ; i++){
			tmp =   transform.GetChild(i);
			if(tmp.name == "Pattern") Destroy(tmp.gameObject);
		}
        
        transform.parent = master;
        patterns.Clear();

    }

}
