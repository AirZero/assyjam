using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {
	public float time = 5.0f;
	
	// Use this for initialization
	void Start () {
		Destroy (gameObject, time);
	}
}
