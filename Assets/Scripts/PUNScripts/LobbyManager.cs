using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// 목적: 로비에 방을 만들고 방에 입장한다.
// 필요속성: 방 이름을 넣을 InputField
public class LobbyManager : MonoBehaviourPunCallbacks
{
    // 필요속성: 방 이름을 넣을 InputField
    public TMP_InputField roomNameInput;
    public int maxPlayerNum = 3;
    public TMP_Text logText;
    public string tempTxt;
    public string sceneName = "LoadingScene";


    string updateTxt(out string _tempTxt, string input)
    {
        _tempTxt = input;
        return tempTxt += _tempTxt;
    }

    // 목적: 로비에 방을 만들고 방에 입장한다.
    public void CreateRoom()
    {
        // inputField에 내용이 있을 때, 방을 해당 inputField의 내용으로 만든다.
        if(roomNameInput.text != "")
        {
            PhotonNetwork.JoinOrCreateRoom(roomNameInput.text, new RoomOptions { MaxPlayers = maxPlayerNum }, null);
            print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
    }

    // 방에 입장 후 호출됨
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        logText.text = updateTxt(out tempTxt, "enter success\n");

        SceneManager.LoadScene(sceneName);
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        print("방 생성 완료");
    }

    // 방에 개설 실패시 호출
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        logText.text = updateTxt(out tempTxt, "create room failed\n");
    }

    // 방에 입장 실패시 호출
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);

        logText.text = updateTxt(out tempTxt, "join room failed\n");
    }
}
