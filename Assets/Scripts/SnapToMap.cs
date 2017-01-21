using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SnapToMap : MonoBehaviour {

	
	const float MAP_WIDTH = 0.64f;
	const float MAP_HEIGHT = 0.64f;
	
	// Update is called once per frame
	void Update () {
		if(Application.isEditor) {
			SnapToBlock();
		}
		
	}

	void SnapToBlock() {
		if(transform.hasChanged) {
			Vector3 currentPos = transform.position;
			transform.position = new Vector3(
				Mathf.RoundToInt(currentPos.x / MAP_WIDTH) * MAP_WIDTH,
				Mathf.RoundToInt(currentPos.y / MAP_HEIGHT) * MAP_HEIGHT,
				0	
			);
		}
	}
}
