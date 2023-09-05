using System;
using Photon.Pun;
using UnityEngine;

// 목적: 플레이어를 키입력에 따라 이동시킨다.
// 필요속성: 스피드
public class PunPlayerMove : MonoBehaviour, IPunObservable
{
    public static int startFlag = 0;

    // 필요속성: 스피드
    public float speed = 3;
    PhotonView pv;
    float currentTime = 0;
    public float startTime = 10;

    public bool isFiring = false;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            // 목적: 플레이어를 키입력에 따라 이동시킨다.
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(-speed * Time.deltaTime, 0, 0);
            }

            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
            }

            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(speed * Time.deltaTime, 0, 0);
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                pv.RPC("CreateSphere", RpcTarget.All, $"I'm Ready! - {DateTime.Now}");
                isFiring = true;
            }
            else if(Input.GetKeyUp(KeyCode.Space))
            {
                isFiring = false;
            }
        }
        else
        {
            //transform.position = currPos;
            //transform.rotation = currRot;

            transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * 20f);
            transform.rotation = Quaternion.Lerp(transform.rotation, currRot, Time.deltaTime * 20f);

            RoomManager.Instance.infoText.text = currPos.ToString();
        }
    }

    [PunRPC]
    void CreateSphere(string info)
    {
        GameObject go = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere)
            , transform.position, Quaternion.identity);
        go.transform.localScale = Vector3.one * 0.1f;

        RoomManager.Instance.infoText.text = info;
    }

    //클론이 통신을 받는 변수 설정
    private Vector3 currPos;
    private Quaternion currRot;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            this.currPos = (Vector3)stream.ReceiveNext();
            this.currRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
