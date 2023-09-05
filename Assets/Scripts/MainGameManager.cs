using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

//목적1: SpawnPoint에 Player를 위치시킨다.
// 필요속성: SpawnPoints 배열
//목적2: 특정 시간이 지나면 게임 시작 명령을 내린다.
// 필요속성: 특정시간, 현재시간, 게임시작명령 flag
public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance;

    // 필요속성: SpawnPoints 배열
    public Transform[] spawnPoints;

    // 필요속성: 특정시간, 현재시간, 게임시작명령 flag
    public float gameStartTime = 10f;
    public bool isGameStarted = false;

    [PunRPC]
    public void GetGameState(out bool _isGameStarted)
    {
        _isGameStarted = isGameStarted;

        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    void SetSpawnPoints()
    {
        Transform spawnPointsParent = GameObject.Find("Grp_SpawnPoints").transform;

        spawnPoints = new Transform[spawnPointsParent.childCount];
        print(spawnPointsParent.childCount);

        for (int i = 0; i < spawnPointsParent.childCount; i++)
        {
            spawnPoints[i] = spawnPointsParent.GetChild(i);
        }
    }


    // 시간을 기록해서 특정 시간이 지나면 게임 시작을 알린다.
    IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(1);

        SetSpawnPoints();

        //목적2: 특정 시간이 지나면 게임 시작 명령을 내린다.
        yield return new WaitForSeconds(gameStartTime);

        isGameStarted = true;
        print("게임이 시작되었습니다.");
    }

    // MainGameManager의 타이머 코루틴을 시작하는 함수
    public void StartTimer()
    {
        print(System.Reflection.MethodBase.GetCurrentMethod().Name);
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(TimerCoroutine());
        }
    }
}
