using System.Xml;
using UnityEngine;

public class XmlReader : MonoBehaviour
{
    void Start()
    {
        string path = Application.dataPath + "/stages.xml"; // stages.xml 파일의 경로를 지정합니다.
        XmlDocument xmlDoc = new XmlDocument(); // XmlDocument 객체를 생성합니다.
        xmlDoc.Load(path); // stages.xml 파일의 내용을 불러옵니다.

        XmlNodeList stageNodes = xmlDoc.SelectNodes("/stages/stage"); // 모든 'stage' 노드를 선택합니다.

        foreach (XmlNode stageNode in stageNodes) // 각 스테이지 정보를 출력합니다.
        {
            Debug.Log("Stage Number: " + stageNode.SelectSingleNode("stageNumber").InnerText);
            XmlNodeList monsterNodes = stageNode.SelectNodes("monsters/monster");

            foreach (XmlNode monsterNode in monsterNodes)
            {
                Debug.Log("Monster Name: " + monsterNode.SelectSingleNode("name").InnerText +
                          ", Health: " + monsterNode.SelectSingleNode("health").InnerText);
            }
        }
    }
}
