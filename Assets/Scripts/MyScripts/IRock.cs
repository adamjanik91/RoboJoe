using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IRock {

	List<Vector3> positionList { get; }
	float distanceToCheck { get; }
	float timeToDeactivate { get; }
	float timeToDestroy { get; }

}
