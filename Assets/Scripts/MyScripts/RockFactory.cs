using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RockFactory {

	private Dictionary<int, IRock> list = new Dictionary<int, IRock>();

	public void SaveRock(int index, IRock rock) {
		list.Add(index, rock);
	}

	public IRock GetRock(int index) {
		return list[index];
	}

}
