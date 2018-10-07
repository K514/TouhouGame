using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class k514EigenAbillity : MonoBehaviour {
	
	protected Transform target;
	protected k514PlayerController targetScript;
	protected Animator anim;
	protected bool ActivateTrig = false , DeactivateTrig = false;

	public void Init(Transform target){
		this.target = target;
		this.anim = this.target.GetComponent<Animator>();
		this.targetScript = this.target.GetComponent<k514PlayerController>();
	}
	public abstract void Activate();
	public abstract void Deactivate();
	public abstract void EndProcess();
}
