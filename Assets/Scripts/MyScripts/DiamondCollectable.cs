using UnityEngine;
using System.Collections;

public class DiamondCollectable : ICollectable {

	private AudioClip audioClip = Resources.Load("Sounds/collect") as AudioClip;
	private int points = 10;

	AudioClip ICollectable.audioClip {
		get { return audioClip; }
	}

	int ICollectable.points {
		get { return points; }	
	}

}
