using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFFECT_TYPE{
	PHIT,HIT,MUSOU,END
}

public class k514EffectManager : MonoBehaviour {

	public static k514EffectManager singleton = null;
	public Transform[] pfab;

	// Use this for initialization
	void Awake () {
		if(singleton == null){
			singleton = this;
		}
		else if(singleton != this) Destroy(gameObject);
	}

    public void CastEffect(EFFECT_TYPE e,Vector3 pos){
        Transform tmp = Instantiate(pfab[(int)e]);
        tmp.position = pos;
    }

}
