using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class k514MathManager : MonoBehaviour {

	public static k514MathManager singleton = null;

	void Awake(){
		if(singleton == null) singleton = this;
		else if(singleton != this) Destroy(gameObject);
	}

	public int Combination(int n, int r){
		return Factorial(n,n-r)/Factorial(r,1);
	}

	public int Factorial(int n,int bound){
		int current = 1;
		while(n > bound){
			current = Factorial_Trempoline(n,current);
			n--;
		}
		return current;
	}

	public int Factorial_Trempoline(int n, int current){
		return n * current;
	}

	public int[] Permutation(int size){
		int[] result = new int[size];
		int j,tmp;
		for(int i = 0 ; i < size ; i++){
			result[i] = i;
		}
		for(int i = 0 ; i < size ; i++){
			j = Random.Range(0,size);
			tmp = result[i];
			result[i] = result[j];
			result[j] = tmp;
		}
		return result;	
	}

	public int Pow(int basei, int exponent){
		int result = 1;
		for(int i = 0 ; i < exponent ; i++){
			result *= basei;
		}
		return result;
	}
}
