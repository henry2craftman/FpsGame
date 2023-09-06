using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// 목적: Photon 게임 서버에 연결한다.
// 목적2: 로비에 입장한다.
public class ConnectionManager : MonoBehaviourPunCallbacks
{
    public TMP_Text logText;
    string tempTxt;

    string updateTxt(out string _tempTxt, string input)
    {
        _tempTxt = input;
        return tempTxt += _tempTxt;
    }

    // 목적: Photon 게임 서버에 연결한다.
    public void Connect()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.SendRate = 30; // Default: 30
        PhotonNetwork.SerializationRate = 30; // Default: 10

        PhotonNetwork.ConnectUsingSettings();
    }
    // 포톤 서버 연결시 호출됨
    public override void OnConnected()
    {
        base.OnConnected();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        logText.text = updateTxt(out tempTxt, "photon server connected");
    }

    // 마스터 서버 연결시 호출됨
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        logText.text = updateTxt(out tempTxt, "master server connected");

        // 마스터 접속시 자동으로 로비 입장
        JoinLobby();
    }

    // 목적2: 로비에 입장한다.
    public void JoinLobby() => PhotonNetwork.JoinLobby();

    // 로비에 입장시 호출됨

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        logText.text = updateTxt(out tempTxt, "lobby enter success");

        // LobbyScene 으로 이동
        SceneManager.LoadScene("LobbyScene");
    }


    // LoadingScene을 Load한다.
    public void EnterSoloPlayMode()
    {
        SceneManager.LoadScene("LoadingScene");
    }
}
