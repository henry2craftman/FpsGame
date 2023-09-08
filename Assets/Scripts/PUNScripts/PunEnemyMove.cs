using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;

// 목적: 레이더를 이용하여 특정 반경 내에 검색된 가장 가까운 플레이어를 저장하고 따라간다.
// 플레이어를 저장하면, 다른 플레이어를 탐지해도 처음 탐지한 플레이어만 따라간다.
// 처음 탐지한 플레이어가 레이더 상에 없으면, 가장 가까이에 있는 플레이어를 저장하고 따라간다.
public class PunEnemyMove : MonoBehaviour, IPunObservable
{
    public Transform playerFound;
    public float detectionRange = 3;
    public float chaseSpeed = 2;
    PhotonView pv;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FindObject(ref playerFound);

        ChaseObject(ref playerFound);
    }

    void FindObject(ref Transform obj)
    {
        if (obj != null)
        {
            print("Player already Found");
            return;
        }

        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRange);

        // 거리 순 정렬
        detectedObjects = detectedObjects.OrderBy
            (eachObj => Vector3.Distance(transform.position, eachObj.transform.position)).ToArray();

        foreach (Collider collider in detectedObjects)
        {
            if (collider.name.Contains("Player"))
            {
                obj = collider.transform;
                print("Player Found!");
                break;
            }
        }
    }

    void ChaseObject(ref Transform obj)
    {
        if (obj == null)
        {
            print("No player to Chase");
            return;
        }
        float distance = (obj.position - transform.position).magnitude;
        Vector3 dir = (obj.position - transform.position).normalized;
        if(distance > detectionRange)
        {
            obj = null;
            print("Player Disappeared");
            return;
        }

        if(pv.IsMine)
        {
            transform.position += dir * chaseSpeed * Time.deltaTime;
            print("Chasing Player");
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivedPos, Time.deltaTime * 20);
        }
    }

    //void OnDrawGizmosSelected()
    //{
    //    // Draw a yellow sphere at the transform's position
    //    Gizmos.color = new Color(1, 0, 0, 0.2f);
    //    Gizmos.DrawSphere(transform.position, detectionRange);
    //}


    Vector3 receivedPos;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            receivedPos = (Vector3)stream.ReceiveNext();
        }
    }
}
