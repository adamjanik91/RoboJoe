using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectableFactory {

	Dictionary<int, ICollectable> list = new Dictionary<int, ICollectable>();

	public void SaveCollectable(int index, ICollectable collectable) {
		list.Add(index, collectable);
	}

	public ICollectable GetCollectable(int index) {
		return list[index];
	}

}
