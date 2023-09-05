using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.IO.Pipes;

public class FileStreamExample : MonoBehaviour
{
    FileStream fileStream;

    // Start is called before the first frame update
    void Start()
    {
        string path = Application.dataPath + "/fileSteam.txt";
        fileStream = new FileStream(path, FileMode.OpenOrCreate);

        //for(int i = 65; i < 90; i++)
        //{
        //    fileStream.WriteByte((byte)i);
        //}

        //WriteStream();

        ReadStream();

        fileStream.Close();
    }

    void ReadStream()
    {
        StreamReader streamReader = new StreamReader(fileStream);
        string line = streamReader.ReadLine();

        print(line);
        streamReader.Close();
    }

    void WriteStream()
    {
        StreamWriter streamWriter = new StreamWriter(fileStream);
        streamWriter.WriteLine("Hello Everyone");

        streamWriter.Close();
    }

    void WriteCSVFormat()
    {
        string monsterName = "Hulk";
        int hp = 10;
        int speed = 50;
        string csvFormat = string.Format("{0}, {1}, {2}", monsterName, hp, speed);
    }

    void ParseCSV()
    {
        string csvFormat = "Hulk, 10, 50";
        char[] sep = { ',' };
        string[] result = csvFormat.Split(sep);
        
        foreach(var s in result)
        {
            print(s);
        }
    }
}
