using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514RotateOff : k514Move {

	public override bool Progress(){
		masterScript.RotateTrig = false;
		PatternLifeSpanTrig = false;		
		return PatternLifeSpanTrig;
	}

	public override void EndProcess(){
		PatternLifeSpanTrig = true;
	}
}
