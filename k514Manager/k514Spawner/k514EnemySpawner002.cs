using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 S컴포넌트를 가지는 게임 오브젝트는 레이 캐스트의 시작점이 된다.
public class k514EnemySpawner002 : k514EnemySpawner {

    public int RegenCnt = 10, start = 0 , end = 100;
    private Vector3[] targetPlaceDirection,targetPlace;
    private List<KeyValuePair<int,float>>sortedTargetPlaceIndex; // targetPlace's Index , z-differ

    // for  compare
    int Compare(KeyValuePair<int, float> a, KeyValuePair<int, float> b)
    {
        return a.Value.CompareTo(b.Value);
    }

    void Start(){
        targetPlaceDirection = new Vector3[RegenCnt];
        targetPlace = new Vector3[RegenCnt];
        sortedTargetPlaceIndex = new List<KeyValuePair<int,float>>();
    }

    int GetStart(){
        return start;
    }
     
    int GetEnd(){
        return end;
    }

    // 1번 스테이지 n번 블록, 레이캐스트 용
    // 생성 조건 :: 블록 체크에 플레이어가 진입한 경우
    protected override bool spwCondition(){
            // outter_spwContion에 의존
            return false;
    }
    
    public override void Init(){
        base.Init();
        sortedTargetPlaceIndex.Clear();
    }

    // 생성 패턴 : 모든 targetPlace에서 전방으로 요정 생성.
    protected override IEnumerator spwSequence(){
        yield return null;
        int offset = 100*k514SystemManager.BlockMgr.nowBlockCount;
        if(!(offset >= GetStart()*100 && offset <= GetEnd()*100)) yield break;
        // coroutine
        WaitForSeconds cooldown = new WaitForSeconds(3f);

        // for raycast
		int layerMask = -1;
        int Radius = 2;
        int FIND_RANGE = 60;
		RaycastHit hit;
        bool isHit = false;
        // for enemy pattern
         Vector3 start,end;
        k514EnemyController spawnedEnemy = null;
        List<k514EnemyBehaviour> patterns = null;
        k514MoveDirect tmp = null;

        for(int i = 0 ; i < targetPlaceDirection.Length ; i++){
            
		    targetPlaceDirection[i] = (new Vector3(Random.Range(10f,70f),-5f,offset + Random.Range(30f,99f)) - transform.position).normalized;//(k514SystemManager.InteractMgr.GetCameraCurrentLookVector());
            isHit = Physics.SphereCast (transform.position, Radius, targetPlaceDirection[i], out hit, FIND_RANGE, layerMask);
            if (isHit) {
                targetPlace[i] = hit.point;
                targetPlace[i].y = Random.Range(targetPlace[i].y,35f);
                sortedTargetPlaceIndex.Add(new KeyValuePair<int, float>(i, targetPlace[i].z));
            }
        }

        // sorting
        sortedTargetPlaceIndex.Sort(Compare);
        //for (int i = 0; i < targetPlaceDirection.Length; i++) Debug.Log(sortedTargetPlaceIndex[i]);


        // regen enemy
        for(int j = 0 ; j < 1 ; j++){
            for(int i = 0 ; i < targetPlace.Length ; i++){
                start = targetPlace[i];
                end = targetPlace[sortedTargetPlaceIndex[RegenCnt/2].Key] + new Vector3(Random.Range(-10f,10f),0f,Random.Range(-10f,10f));
                spawnedEnemy = CreateEnemy(ENEMY_TYPE.FAIRY,start);
                patterns = spawnedEnemy.patterns;
                tmp = (k514MoveDirect)k514EnemyPatternFactory.CreatePattern<k514MoveDirect>(spawnedEnemy);
                tmp.Init(spawnedEnemy,end);
                patterns.Add(tmp);
                tmp = (k514MoveDirect)k514EnemyPatternFactory.CreatePattern<k514MoveDirect>(spawnedEnemy);
                tmp.Init(spawnedEnemy,start);
                patterns.Add(tmp);
            }
            yield return cooldown;
        }
    }


    // void OnDrawGizmos() {

    //     if(!outter_spwCondition) return;

	// 	int layerMask = -1;
    //     int Radius = 2;
    //     int FIND_RANGE = 60;
	// 	RaycastHit hit;
    //     bool isHit = false;
		
    //     for(int i = 0 ; i < targetPlaceDirection.Length ; i++){
    //         // Physics.SphereCast (레이저를 발사할 위치, 구의 반경, 발사 방향, 충돌 결과, 최대 거리)
    //         isHit = Physics.SphereCast (transform.position, Radius, targetPlaceDirection[i], out hit, FIND_RANGE, layerMask);
    //         Gizmos.color = Color.red;
    //         if (isHit) {
    //             Gizmos.DrawRay (transform.position, targetPlaceDirection[i] * hit.distance);
    //             Gizmos.DrawWireSphere (transform.position + targetPlaceDirection[i] * hit.distance, Radius);
    //         } else {
    //             Gizmos.DrawRay (transform.position, targetPlaceDirection[i] * FIND_RANGE);
    //         }
    //     }
	// }



}