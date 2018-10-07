using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514Particle : MonoBehaviour {
 	private ParticleSystem ps;
	void Start () {
		 ps = GetComponent<ParticleSystem>();
		 var main = ps.main;
		 main.startColor = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0.6f,1f));
		main.startSize = Random.Range(1f,2f);
		 Destroy(gameObject,2f);
	}
}
