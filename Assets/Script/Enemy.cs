﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

	public int id;

	public RectTransform rTransform;

	public void UpdatePos(float x, float y){
		rTransform.localPosition = new Vector3(x*5,y*5,0);
	}
}
