using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514CrossHair : MonoBehaviour {

	public Transform CrossHair;
	public float MAX_DISTANCE = 4f, FIND_RANGE = 10f;
	public float Radius = 10f;
	[System.NonSerialized]public Vector3 targetPlace = Vector3.zero;
	[System.NonSerialized]public Transform targetTransform;
	

	public void Set_Cross_HairRange(float r){
		Radius = r;
	}

	// Update is called once per frame
	void Update () {
		RayCast();
	}

	void RayCast(){
		RaycastHit hit;
		targetPlace = (transform.position - Camera.main.transform.position + Vector3.up * 1.2f).normalized;
		CrossHair.forward = targetPlace;			
		int layerMask = 1 << LayerMask.NameToLayer("Enemy");
		bool isHit = Physics.SphereCast (Camera.main.transform.position-Vector3.forward*6, Radius, targetPlace, out hit, FIND_RANGE, layerMask);
			if(isHit){
				targetPlace = hit.point;
				targetTransform = hit.collider.transform;				
				k514SystemManager.InteractMgr.SetTargetDetectedTrig(true);
			}else{
				// not collide
				targetPlace = transform.position + targetPlace*MAX_DISTANCE;
				targetTransform = null;								
				k514SystemManager.InteractMgr.SetTargetDetectedTrig(false);
			}
		CrossHair.position = targetPlace;
	}

	// void OnDrawGizmos() {
	// 	int layerMask = 1 << LayerMask.NameToLayer("Enemy");
	// 	Vector3 targetPlace = (transform.position - Camera.main.transform.position + Vector3.up * 1.2f).normalized;//(k514SystemManager.InteractMgr.GetCameraCurrentLookVector());
	// 	RaycastHit hit;
	// 	// Physics.SphereCast (레이저를 발사할 위치, 구의 반경, 발사 방향, 충돌 결과, 최대 거리)
	// 	bool isHit = Physics.SphereCast (Camera.main.transform.position-Vector3.forward*6, Radius, targetPlace, out hit, FIND_RANGE, layerMask);

	// 	Gizmos.color = Color.red;
	// 	if (isHit) {
	// 		Debug.Log(hit.collider.name);
	// 		Gizmos.DrawRay (Camera.main.transform.position, targetPlace * hit.distance);
	// 		Gizmos.DrawWireSphere (Camera.main.transform.position + targetPlace * hit.distance, Radius);
	// 	} else {
	// 		Gizmos.DrawRay (Camera.main.transform.position, targetPlace * MAX_DISTANCE);
	// 	}
	// }

}
