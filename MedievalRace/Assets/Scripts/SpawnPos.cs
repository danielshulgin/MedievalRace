using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPos : MonoBehaviour {

	public static List<SpawnPos> list = new List<SpawnPos>();

    void Awake() {
        list.Add(this);
    }

    void OnDestroy()
    {
        list.Remove(this);
    }
}
