using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514RotateOn : k514Move {

	public override bool Progress(){
		masterScript.RotationAxis = this.localDestination;
		masterScript.RotateTrig = true;
		PatternLifeSpanTrig = false;		
		return PatternLifeSpanTrig;
	}

	public override void EndProcess(){
		PatternLifeSpanTrig = true;
	}
}
