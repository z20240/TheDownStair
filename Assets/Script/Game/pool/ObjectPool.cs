using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class ObjPool {

}

public class ObjectPool : MonoBehaviour {
    // Use this for initialization
    public GameObject stone_stair_1; // 石階1
    public GameObject stone_stair_2; // 石階2

    public GameObject Bath_salts; // 浴鹽
    public GameObject Heroin; // 海洛因
    public GameObject Ketamine; // Ｋ他命
    public GameObject MDMA; // 搖頭丸
    public GameObject NewDrugs; // 新興毒品
    public GameObject Amphetamines; // 安非他命
    public GameObject Health_bag; // 救命包
    public GameObject Player; // 玩家(分身)

    private IDictionary<string, Queue<GameObject>> dict = new Dictionary<string, Queue<GameObject>>();

    private ArrayList QueueList = new ArrayList();

    void Awake() {

        string[] pool_name_ary = {
            "stone_stair_1", "stone_stair_2",

            "Bath_salts", "Heroin", "Ketamine", "MDMA", "NewDrugs", "Amphetamines",

            "Health_bag",

            "Player",
        };

        foreach (string name in pool_name_ary) {
            dict[name] = new Queue<GameObject>();
        }

        InitPool(dict["stone_stair_1"], stone_stair_1, 21);
        InitPool(dict["stone_stair_2"], stone_stair_2, 21);

        InitPool(dict["Bath_salts"], Bath_salts, 21);
        InitPool(dict["Heroin"], Heroin, 21);
        InitPool(dict["Ketamine"], Ketamine, 21);
        InitPool(dict["MDMA"], MDMA, 21);
        InitPool(dict["NewDrugs"], NewDrugs, 21);
        InitPool(dict["Amphetamines"], Amphetamines, 21);
        InitPool(dict["Health_bag"], Health_bag, 21);
        InitPool(dict["Player"], Player, 3);
    }

    public GameObject ReUse(string pool_name, Vector3 position, Quaternion rotation, int degree = 90) {
        return Use(dict[pool_name], position, rotation, degree);
    }


    public void Recovery(string pool_name, GameObject recovery) {
        Reback(dict[pool_name], recovery);
    }

    void InitPool(Queue<GameObject> pool, GameObject obj, int amount) {
        for ( int i = 0; i < amount ; i++ ) {
            GameObject go = Instantiate( obj ) as GameObject;
            pool.Enqueue( go );
            go.SetActive( false );
        }
    }

    GameObject Use(Queue<GameObject> pool, Vector3 position, Quaternion rotation, int degree = 90) {
        if (pool.Count > 1) {
            GameObject reuse = pool.Dequeue();
            reuse.transform.position = position;
            reuse.transform.rotation = rotation;
            reuse.SetActive( true );
            return reuse;
        } else {
            GameObject reuse = pool.Peek();
            reuse.name = getName(reuse.name);
            GameObject go = Instantiate( reuse ) as GameObject;
            go.transform.position = position;
            go.transform.rotation = rotation;
            go.SetActive( true );
            return go;
        }
    }

    void Reback(Queue<GameObject> pool, GameObject recovery) {
        pool.Enqueue ( recovery );
        recovery.SetActive ( false );
    }


    string getName(string name) {
        if (name.IndexOf("Clone") == -1)
            return name;
        return name.Substring(0, name.IndexOf("Clone")-1);
    }
}
