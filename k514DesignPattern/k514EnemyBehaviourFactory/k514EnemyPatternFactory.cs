using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514EnemyPatternFactory : MonoBehaviour {

	public static k514EnemyBehaviour CreatePattern<T>(IPatternable k) where T : k514EnemyBehaviour{
		GameObject tmp = new GameObject();
		T t = tmp.AddComponent<T>();
		tmp.name = "Pattern";
		tmp.transform.parent = k.transform;
		return t;
	}

}
