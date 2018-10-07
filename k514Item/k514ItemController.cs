using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514ItemController : MonoBehaviour {

	private Rigidbody Rigid = null;
	private Transform player = null;
	private bool OnceTrig = false, traceMode = false;
	private float z_differ = 0f,speed= 20f;

	public ITEM_TYPE my_type;

	void Start(){
		Rigid = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(!OnceTrig){
			OnceTrig = true;
			player = k514SystemManager.InteractMgr.GetPlayerTransform();
		}

		if(k514SystemManager.EnemyMgr.ENEMY_CNT == 0){
			traceMode = true;
			speed = 40f;
		}
		if(!traceMode){
			z_differ = player.position.z - transform.position.z;
			if(z_differ > 1f){
				// 제거
				EndProcess();
			}else if(z_differ <= 1f && z_differ > -4f){
				// 타겟 확인
				if((player.position - transform.position).sqrMagnitude < 49f){
					traceMode = true;	
				}else{
					Rigid.MovePosition(Vector3.MoveTowards(transform.position, transform.position - Vector3.forward*10, 10*Time.deltaTime));
				}

			}else{
				Rigid.MovePosition(Vector3.MoveTowards(transform.position, transform.position - Vector3.forward*10, 10*Time.deltaTime));
			}
		}else{
			Rigid.MovePosition(Vector3.MoveTowards(transform.position, player.position + Vector3.up, speed*Time.deltaTime));
		}
	}

	void EndProcess(){
		traceMode = false;
		OnceTrig = false;
		k514SystemManager.ItemMgr.KillItem(this,my_type);
	}

	void OnTriggerEnter(Collider co){
		if(co.CompareTag("PlayerHit")){
			k514SystemManager.SoundMgr.PlayAudioClip(my_type,true);
			switch(my_type){
				case ITEM_TYPE.POWER :
					player.GetComponent<k514PlayerController>().sPP += Random.Range(2,8);
					k514SystemManager.UI_SpecaMgr.Power_UI_Update();
				break;
				case ITEM_TYPE.SCORE :
					player.GetComponent<k514PlayerController>().SCORE_STORAGE += Random.Range(50,300);
				break;
			}
			EndProcess();
		}
	}

}
