
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPatternable : MonoBehaviour {

	// pattern relatives
	[System.NonSerialized]public Vector3 RotationAxis;
	[System.NonSerialized]public bool RotateTrig = false,LoopTrig = true;
    
	public void SetLoop(bool b){
		this.LoopTrig = b;
	}
}