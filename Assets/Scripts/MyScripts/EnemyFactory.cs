using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFactory {

	Dictionary<int, IEnemy> list = new Dictionary<int, IEnemy>();

	public void SaveEnemy(int index, IEnemy enemy) {
		list.Add(index, enemy); 
	}

	public IEnemy GetEnemy(int index) {
		return list[index];
	}

}
