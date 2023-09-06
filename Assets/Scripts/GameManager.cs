using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// 목적: 게임의 상태(Ready, Start, GameOver)를 구별하고, 게임의 시작과 끝을 TextUI로 표현하고 싶다.
// 필요속성: 게임상태 열거형 변수, TextUI

// 목적2: 2초 후 게임이 Ready 상태에서 Start 상태로 변경되며 게임이 시작된다. 

// 목적3: 플레이어의 hp가 0보다 작으면 상태텍스트와 상태를 GameOver로 바꿔주고
// 필요속성3: hp가 들어있는 playerMove

// 목적4: 플레이어의 hp가 0 이하라면, 플레이어의 애니메이션을 멈춘다.
// 필요속성4: 플레이어의 애니메이터 컴포넌트

// 목적5: Setting 버튼을 누르면 Option UI가 켜진다. 동시에 게임 속도를 조절한다(0 or 1)
// 필요속성5: OptionUI 게임오브젝트, 일시정지 상태

// 목적6: 게임 오버시 Retry와 Quit 버튼을 활성화한다.

// 목적7: 게임이 시작되면 Player를 PhotonView를 사용하여 생성한다.
// 필요속성: Player PhotonView

// 목적8: Client로서 접속이 되면(GameManager가 태어나면) PhotonNetwork에 접속한 Player들을 확인해서 내 번호를 정한다.
public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    // 필요속성: 게임상태 열거형 변수, TextUI
    public enum GameState
    {
        Ready,
        Start,
        Pause,
        GameOver
    }

    public GameState state = GameState.Ready;
    public TMP_Text stateText;

    // 필요속성3: hp가 들어있는 playerMove
    public PlayerMove player;

    // 필요속성4: 플레이어의 애니메이터 컴포넌트
    Animator animator;

    // 필요속성5: OptionUI 게임오브젝트, 일시정지 상태
    public GameObject optionUI;

    // 필요속성: Player PhotonView
    public PhotonView playerPrefab;
    private int myPlayerNumber = 0;

    // 게임시작시 활성화 할 적그룹
    public GameObject grpEnemies;
    public GameObject enemyPref;
    public NavMeshSurface humanoidSurface;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // 목적8: Client로서 접속이 되면(GameManager가 태어나면) PhotonNetwork에 접속한 Player들을 확인해서 내 번호를 정한다.
        Player[] players = PhotonNetwork.PlayerList;
        foreach (var p in players)
        {
            if (p != PhotonNetwork.LocalPlayer)
            {
                myPlayerNumber++;
            }
        }

        stateText.text = "Ready";
        stateText.color = new Color32(255, 185, 0, 255);

        SetSpawnPoints();

        StartCoroutine(GameStart());
    }

    // 필요속성: SpawnPoints 배열
    public Transform[] spawnPoints;
    void SetSpawnPoints()
    {
        Transform spawnPointsParent = GameObject.Find("Grp_SpawnPoints").transform;

        spawnPoints = new Transform[spawnPointsParent.childCount];

        for (int i = 0; i < spawnPointsParent.childCount; i++)
        {
            spawnPoints[i] = spawnPointsParent.GetChild(i);
        }
    }

    // 목적2: 2초 후 게임이 Ready 상태에서 Start 상태(초록색)로 변경되며 게임이 시작된다. 
    IEnumerator GameStart()
    {
        // <중요> 방에 있는 상태가 네트워크에서 확인 되면 
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        

        // 목적7: 게임이 시작되면 Player를 PhotonView를 사용하여 생성한다.
        GameObject playerObj = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[myPlayerNumber].position, Quaternion.identity);
        print(playerObj.name);

        //GameObject enemyObj = PhotonNetwork.Instantiate(enemyPref.name, spawnPoints[myPlayerNumber].position, Quaternion.identity);

        //enemyObj.GetComponent<EnemyFSM>().player = playerObj.transform;

        //// 모든 적의 HP Bar는 항상 나(컨트롤하는 플레이어)를 향한다.
        //HpBarTarget[] enmies = GameObject.FindObjectsOfType<HpBarTarget>();
        
        //foreach(var enemy in enmies)
        //{
        //    enemy.GetComponent<HpBarTarget>().target = playerObj.transform;
        //}


        //humanoidSurface.BuildNavMesh();


        // 게임이 시작되면 플레이어를 찾고 애니메이터를 가져온다.
        player = playerObj.GetComponent<PlayerMove>();
        animator = player.GetComponentInChildren<Animator>();

        //grpEnemies.SetActive(true);



        stateText.text = "Game Start";
        stateText.color = new Color32(0, 255, 0, 255);

        // 0.5초를 기다린다.
        yield return new WaitForSeconds(0.5f);

        // 상태 text 비활성
        stateText.gameObject.SetActive(false);

        // 상태 변경
        state = GameState.Start;
    }

    // 목적3: 플레이어의 hp가 0보다 작으면 상태텍스트와 상태를 GameOver로 바꿔주고
    void CheckGameOver()
    {
        if(player == null) { return; }

        if(player.hp <= 0)
        {
            // 목적4: 플레이어의 hp가 0 이하라면, 플레이어의 애니메이션을 멈춘다.
            animator.SetFloat("MoveMotion", 0f);

            // 상태 텍스트 ON
            stateText.gameObject.SetActive(true);

            // 상태 텍스트를 GameOver로 변경
            stateText.text = "Game Over";

            // 상태 텍스트의 컬러를 빨간색으로 변경
            stateText.color = new Color32(255, 0, 0, 255);

            // 목적6: 게임 오버시 Retry와 Quit 버튼을 활성화한다.
            GameObject retryBtn = stateText.transform.GetChild(0).gameObject;
            GameObject quitBtn = stateText.transform.GetChild(1).gameObject;
            retryBtn.SetActive(true);
            quitBtn.SetActive(true);

            // 목적7: 게임 오버시, HP Bar와 Weapon Mode Text를 비활성화한다.
            player.hpSlider.gameObject.SetActive(false);
            player.GetComponent<PlayerFire>().weaponModeTxt.gameObject.SetActive(false);

            state = GameState.GameOver;
        }
    }


    // 목적5: Option 버튼을 누르면 Option UI가 켜진다. 동시에 게임 속도를 조절한다(0 or 1)
    // Option 화면 켜기
    public void OpenOptionWindow()
    {
        // Option UI가 켜진다.
        optionUI.SetActive(true);

        // 동시에 게임 속도를 조절한다(0)
        Time.timeScale = 0;

        // 게임 상태를 변경한다.
        state = GameState.Pause;
    }

    // 계속하기 옵션
    public void CloseOptionWindow()
    {
        // Option UI가 꺼진다.
        optionUI.SetActive(false);

        // 동시에 게임 속도를 조절한다(1)
        Time.timeScale = 1;

        // 게임 상태를 변경한다.
        state = GameState.Start;
    }

    // 다시하기 옵션
    public void RestartGame()
    {
        // 동시에 게임 속도를 조절한다(1)
        Time.timeScale = 1;

        // 현재 씬 번호를 다시 로드
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // 게임 종료 옵션
    public void QuitGame()
    {
        Application.Quit();
    }
}
