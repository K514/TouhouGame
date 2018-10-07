using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class k514EnemySpawner : MonoBehaviour {
    // 생성 조건
    [System.NonSerialized]public bool outter_spwCondition = false;
    protected abstract bool spwCondition();
    // 생성 패턴
    protected IEnumerator coroutine = null;
    protected abstract IEnumerator spwSequence();
    // 한번 뮤텍스
    protected bool onceTrig = false;

    protected void reqSpawn(){
        if(spwCondition() || outter_spwCondition){
            coroutine = spwSequence();
            StartCoroutine(coroutine);
            onceTrig = true;
        }
    }

    protected k514EnemyController CreateEnemy(ENEMY_TYPE index, Vector3 pos){
        k514EnemyController tmp = k514SystemManager.EnemyMgr.GetPooledEnemy(index);
        tmp.transform.position = pos;
        return tmp;
    }

    protected void Update(){
        if(!onceTrig)
            reqSpawn();
    }

    public virtual void Init(){
        onceTrig = false;
        outter_spwCondition = false;
    }
}