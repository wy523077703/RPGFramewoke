﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestPool : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject go = new GameObject("ObjectPool");
        ObjectPool pool= go.AddComponent<ObjectPool>();
        List<string> names = new List<string>();
        //names.Add("Prefabs/Capsule");
        //names.Add("Prefabs/Cube");
        //names.Add("Prefabs/Sphere");
        pool.AllPreRes = names;
        DontDestroyOnLoad(go);

        foreach (object s in Test())
            Debug.Log(s);
        List<int> list = new List<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);
        list.Add(1);
	}
	
        IEnumerable Test()
        {
          yield return 5;
          yield return 1000;
          yield break;
          yield return 15;
        }

	// Update is called once per frame
	void Update () {
	    
	}

    void OnGUI() { 
        if(GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height - 150, 300, 50), "初始化物体")){
            GameObject capsule=ObjectPool.instance.GetObjByName("Prefabs/Capsule");
            GameObject cube = ObjectPool.instance.GetObjByName("Prefabs/Cube");
            GameObject sphere = ObjectPool.instance.GetObjByName("Prefabs/Sphere");
            int range1=Random.Range(-4, 0);
            capsule.transform.position = new Vector3(range1, range1, range1);
            int range2 = Random.Range(0, 4);
            cube.transform.position = new Vector3(range2, range2, range2);
            int range3 = Random.Range(4, 8);
            sphere.transform.position = new Vector3(range3, range3, range3);
        }
        if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height - 300, 300, 50), "切换场景"))
        {
            Application.LoadLevelAsync("test");
        }
    }
}
