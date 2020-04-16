using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmallRock : IRock {

	private List<Vector3> positionList = new List<Vector3> { 
		(new Vector3(-450.0f, 60.0f, 2.250298f)),
		(new Vector3(-500.0f, 60.0f, 2.250298f)),
		(new Vector3(-560.0f, 60.0f, 2.250298f)),
		(new Vector3(-620.0f, 60.0f, 2.250298f)),
		(new Vector3(-800.0f, 60.0f, 2.250298f)),
		(new Vector3(-820.0f, 60.0f, 2.250298f)),
		(new Vector3(-1248.204f, 60.0f, 2.250298f)),
		(new Vector3(-1666.757f, 60.0f, 2.250298f)),
		(new Vector3(-1751.88f, 60.0f, 2.250298f)),
		(new Vector3(-1829.175f, 60.0f, 2.250298f)),
		(new Vector3(-1907.775f, 60.0f, 2.250298f)),
		(new Vector3(-1963.783f, 60.0f, 2.250298f)),
		(new Vector3(-2057.21f, 60.0f, 2.250298f)),
		(new Vector3(-2359.204f, 60.0f, 2.250298f)),
		(new Vector3(-2673.957f, 60.0f, 2.250298f))
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
