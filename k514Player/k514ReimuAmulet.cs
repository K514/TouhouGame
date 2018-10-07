using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514ReimuAmulet : MonoBehaviour {
	
	WaitForSeconds coolTime = null;
	Transform bullet = null;
	k514PlayerController target = null;
	int ShotMotionSpan = 0;
	public bool isLeft = true;
	private IEnumerator shot_patterns = null;

	void Start(){
		coolTime = new WaitForSeconds(0.1f);
		target = GameObject.Find("Player").GetComponent<k514PlayerController>();
		shot_patterns = Shot();
		OnEnable();
	}

	void OnEnable(){
		if(shot_patterns!=null)
			StartCoroutine(Shot());
	}

	void OnDisable(){
		StopCoroutine(Shot());
	}

	IEnumerator Shot(){
		yield return null;
		
		Vector3 targetPlace = Vector3.zero;
		Transform targetTransform;
		while(true){
			if(Input.GetKey(KeyCode.X) && !target.EventTrig && target.ShotableTrig){

				if(isLeft && ShotMotionSpan == 0){
					target.animator.SetBool("Shot",true);
				}

				ShotMotionSpan = 20;
				
				if(target.isShottable()){
					//targetPlace = (k514SystemManager.InteractMgr.GetTargetDetectedTrig()) ? (k514SystemManager.InteractMgr.GetCrossHairTargetVector()- transform.position).normalized : Vector3.forward;
					targetTransform = k514SystemManager.InteractMgr.GetCrossHairTargetTransform();
					k514SystemManager.SoundMgr.PlayAudioClip(SFX_TYPE_BULLET.SHOT3,false);
					k514PlayerController.CURRENT_BULLET++;
					bullet = k514SystemManager.BulletMgr.CreateBullet<k514TargetGuideShot>(0,Vector3.forward,0.005f,10f,targetTransform,15,2f,false);
					
					bullet.position = transform.position + Vector3.forward*1f;
					bullet.forward = targetTransform == null ? Vector3.forward : targetTransform.forward;
					
					bullet.gameObject.name = "Amulet";
					bullet.parent = k514SystemManager.BulletMgr.transform;
					k514SystemManager.BulletMgr.bulletPool.Add(bullet.GetComponent<k514BulletBridge>());
				}else{
					// bulelt full fail
				}

			}else if(!Input.GetKey(KeyCode.Z) && !target.animator.GetBool("Shot")){
				ShotMotionSpan = 0;
			}

			AnimatorStateInfo stateInfo = target.animator.GetCurrentAnimatorStateInfo(2);	
			if (stateInfo.IsName("Arm Layer.Garm") && ShotMotionSpan == 19)
			{	
				target.animator.SetBool("Shot",false);
			}

			if(ShotMotionSpan > 0){
				ShotMotionSpan -- ;
			}
			yield return coolTime;
		}
	}

}

