using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrugSpawner : MonoBehaviour {

    GameObject pool;
    private ObjectPool objPool;
    private GameObject last_drug;
    private float _timer, _next_spawntime;

    private string[] drugList = {
        "Bath_salts",
        "Heroin",
        "Ketamine",
        "MDMA",
        "NewDrugs",
        "Amphetamines",
        "Health_bag",
    };

    private float spawnTime = 1.2f; // 產生階梯的時間

	// Use this for initialization
	void Start () {
        pool = GameObject.Find("pool");
        objPool = pool.GetComponent<ObjectPool>();

        _next_spawntime = spawnTime;
	}

	// Update is called once per frame
	void Update () {
		_timer += Time.deltaTime; //時間增加

        if (_timer <= spawnTime)
            return;

        int rnd = Random.Range(0, drugList.Length);
        last_drug = objPool.ReUse( drugList[rnd], transform.position + new Vector3(Random.Range(-6f, 6f), 0, 0), transform.rotation);

        _timer = 0;
	}
}
