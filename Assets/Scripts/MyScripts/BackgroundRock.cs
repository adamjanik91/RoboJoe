using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BackgroundRock : IRock {

	private List<Vector3> positionList = new List<Vector3> { 
		(new Vector3(-120.0f, 38.7f, 60f)),
		(new Vector3(-180.0f, 38.7f, 60f)),
		(new Vector3(-250.0f, 38.7f, 60f)),
		(new Vector3(-300.0f, 38.7f, 60f)),
		(new Vector3(-330.0f, 38.7f, 60f)),
		(new Vector3(-370.0f, 38.7f, 60f)),
		(new Vector3(-420.0f, 38.7f, 60f)),
		(new Vector3(-440.0f, 38.7f, 60f)),
		(new Vector3(-500.0f, 38.7f, 60f)),
		(new Vector3(-510.0f, 38.7f, 60f)),
		(new Vector3(-550.0f, 38.7f, 60f)),
		(new Vector3(-580.0f, 38.7f, 60f)),
		(new Vector3(-630.0f, 38.7f, 60f)),
		(new Vector3(-650.0f, 38.7f, 60f)),
		(new Vector3(-700.0f, 38.7f, 60f)),
		(new Vector3(-740.0f, 38.7f, 60f)),
		(new Vector3(-800.0f, 38.7f, 60f)),
		(new Vector3(-850.0f, 38.7f, 60f)),
		(new Vector3(-880.0f, 38.7f, 60f))
	};
	private float distanceToCheck = 6.0f;
	private float timeToDeactivate = 0.30f;
	private float timeToDestroy = 1.0f;

	List<Vector3> IRock.positionList {
		get { return positionList; }
	}

	float IRock.distanceToCheck {
		get { return distanceToCheck; }
	}

	float IRock.timeToDeactivate {
		get { return timeToDeactivate; }
	}

	float IRock.timeToDestroy {
		get { return timeToDestroy; }
	}

}
