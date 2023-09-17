using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Monster
{
    public string name;
    public int health;
}

[System.Serializable]
public class StageData
{
    public int stageNumber;
    public List<Monster> monsters;
}

[System.Serializable]
public class StageListData
{
    public List<StageData> stages;
}

public class JsonReader : MonoBehaviour
{
    void Start()
    {
        string path = Application.dataPath + "/stages.json"; // stages.json 파일의 경로를 지정합니다.
        string json = File.ReadAllText(path); // stages.json 파일의 내용을 문자열로 읽어옵니다.

        StageListData stageList = JsonUtility.FromJson<StageListData>(json); // JSON 문자열을 StageList 타입의 객체로 변환합니다.

        foreach (var stage in stageList.stages) // 각 스테이지 정보를 출력합니다.
        {
            Debug.Log($"Stage Number: {stage.stageNumber}");
            foreach (var monster in stage.monsters)
            {
                Debug.Log($"Monster Name: {monster.name}, Health: {monster.health}");
            }
        }

    }
}
