using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JSONExample : MonoBehaviour
{
    [Serializable]
    public class Monster
    {
        public string name;
        public int hp;
        public int attackPower;
        public int speed;
    }

    [Serializable]
    public class StageData
    {
        public int stageNumber;
        public List<Monster> monsters;
    }

    [Serializable]
    public class TotalStageData
    {
        public List<StageData> stages;
    }

    public TotalStageData totalData;


    public string StageDataReader()
    {
        string path = Application.dataPath + "/stages.json";
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader streamReader = new StreamReader(fileStream);
        string json = streamReader.ReadToEnd();
        
        print(json);
        
        streamReader.Close();
        fileStream.Close();

        return json;
    }

    void ReadJson(string json)
    {
        totalData = JsonUtility.FromJson<TotalStageData>(json);

        foreach(var stage in totalData.stages)
        {
            print($"Stage number: {stage.stageNumber}");

            foreach(var monster in stage.monsters)
            {
                print($"monster name: {monster.name}, hp: {monster.hp}, attackPower: {monster.attackPower}, speed: {monster.speed}");

            }
        }
    }


    [Serializable]
    public class MyClass
    {
        public int level;
        public float timeElapsed;
        public string playerName;
    }

    // Start is called before the first frame update
    void Start()
    {
        Monster monster1 = new Monster();
        monster1.name = "Hulk";
        monster1.hp = 100;
        monster1.attackPower = 1000;
        monster1.speed = 50;

        string json = JsonUtility.ToJson(monster1);


        // NewtonJson 예시
        //string newtonJson = JsonConvert.SerializeObject(json);
        //JObject keyValuePairs = JObject.Parse(newtonJson);
        //JObject stageHead = (JObject)keyValuePairs["stages"];
        //if (stageHead.ContainsKey("monsters"))
        //{
        //    JObject monsters = (JObject)stageHead["monsters"];

        //    if (monsters["name"].ToString() == "Goblin")
        //    {
        //        monsters["hp"] = 10;
        //    }
        //}

        //print(json);

        //WrtieJson();

        string monsterJson = StageDataReader();
        ReadJson(monsterJson);
    }

    void WrtieJson()
    {
        string json =
            "{\r\n  \"level\": 11,\r\n  \"timeElapsed\": 30.55,\r\n  \"playerName\": \"New York\"\r\n}";

        MyClass myClass = JsonUtility.FromJson<MyClass>(json);
        print(myClass.level);
    }
}
