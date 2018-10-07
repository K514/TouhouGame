using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514MoveBezier : k514Move {

	protected Vector3[] Destinations;
	protected float max_u = 1f;
	public void Init(IPatternable targetScript,Vector3 Destination0, Vector3 Destination1, Vector3 Destination2, Vector3 Destination3, float max_u = 1f){
		base.Init(targetScript);
		Destinations = new Vector3[4];
		this.Destinations[0] = Destination0;
		this.Destinations[1] = Destination1;
		this.Destinations[2] = Destination2;
		this.Destinations[3] = Destination3;
		this.max_u = max_u;
	}

	public override bool Progress(){
		if(PatternCoroutine == null){
			PatternCoroutine = LifeSpan();
			StartCoroutine(PatternCoroutine);
		}
		return PatternLifeSpanTrig;
	}

	public override void EndProcess(){
		PatternCoroutine = null;
		PatternLifeSpanTrig = true;
	}

	IEnumerator LifeSpan(){
		yield return null;
		float step = max_u/LifeSpanCnt;
		int n = 3; // 3차 베지어 곡선
		for(float i = 0f ; i < max_u ; i+=step){
			localDestination = Vector3.zero;
			for(int j = 0 ; j <= n ; j++){
				// bezier formula : sigma(j : 0 to n) : nCj * i^j * (1-i)^(n-j)
				localDestination += k514SystemManager.MathMgr.Combination(n,j) * Mathf.Pow(i,j) * Mathf.Pow(1f-i,n-j) * Destinations[j];
			}
			while((master.position - localDestination).sqrMagnitude > 0.0001f){
				masterRigid.MovePosition(Vector3.MoveTowards(master.position, localDestination, Speed*inversedMoveTime*Time.deltaTime));
				yield return null;
			}
			yield return null;
		
		}

		PatternLifeSpanTrig = false;
		yield return null;
	}

}
