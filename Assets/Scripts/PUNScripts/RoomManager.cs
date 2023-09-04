using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

// 목적: 버튼을 눌러서 방의 정보를 보여주고, Leave room 버튼을 눌러서 방을 나갈 수 있다.
// 필요속성: 방 정보 Text

// 목적2: Photon view를 가진 플레이어를 생성한다.
// 필요속성2: PhotonView 플레이어
public class RoomManager : MonoBehaviourPunCallbacks
{
    // 필요속성: 방 정보 Text
    public TMP_Text infoText;

    // 필요속성2: PhotonView 플레이어
    public PhotonView playerPrefab;

    private void Start()
    {

    }

    // 방의 정보를 보여주고 싶다.
    public void ShowRoomInfo()
    {
        if (PhotonNetwork.InRoom)
        {
            // 필요 내용: 방 이름, 방 인원수, 최대 인원수, 플레이어 이름
            string roomName = PhotonNetwork.CurrentRoom.Name;
            int playerCnt = PhotonNetwork.CurrentRoom.PlayerCount;
            int maxPlayersCnt = PhotonNetwork.CurrentRoom.MaxPlayers;

            string playerNames = "< Player 목록 >\n";
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerNames += PhotonNetwork.PlayerList[i].NickName + "\n";
            }

            infoText.text = string.Format("Room: {0}\nPlayer number: {1}\nMax player number: {2}\n{3}", roomName, playerCnt, maxPlayersCnt, playerNames);
        }

        // 목적2: Photon view를 가진 플레이어를 생성한다.
        PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
    }

    // Leave room 버튼을 눌러서 방을 나갈 수 있다.
    public void LeaveRoom(){
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();

        SceneManager.LoadScene(1);
    }
}

