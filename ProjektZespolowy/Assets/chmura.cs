﻿using UnityEngine;
using System.Collections;

public class chmura : MonoBehaviour {

	public Transform kamera;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update() {
		transform.rotation = kamera.rotation;

	}
}
