using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514PHitPart : MonoBehaviour {
 	private ParticleSystem ps;
	void Start () {
		 ps = GetComponent<ParticleSystem>();
		 var main = ps.main;
		 Destroy(gameObject,2f);
	}
}
