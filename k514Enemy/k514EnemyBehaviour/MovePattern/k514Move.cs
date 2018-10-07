using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class k514Move : k514EnemyBehaviour {

	protected Transform Destination;
	protected Vector3 localDestination;
	protected int MoveType = 0;

	public virtual void Init(IPatternable targetScript,Transform Destination){
		base.Init(targetScript);
		this.Destination = Destination;
	}

	public virtual void Init(IPatternable targetScript,Vector3 localDestination){
		base.Init(targetScript);
		this.localDestination = localDestination;
		this.MoveType = 1;
	}

	public virtual void Init_LateMoving(IPatternable targetScript){
		base.Init(targetScript);
		this.MoveType = 2;
	}
}
