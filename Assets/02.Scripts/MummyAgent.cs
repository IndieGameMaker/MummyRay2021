using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MummyAgent : Agent
{
    private Transform tr;
    private Rigidbody rb;

    public float moveSpeed = 1.5f;
    public float turnSpeed = 200.0f;
    public StageManager stageManager;

    public Renderer floorRd;
    public Material goodMt;
    public Material badMt;
    private Material originMt;

    public override void Initialize()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        originMt = floorRd.material;

        // 한 에피스드 당 시도횟수
        MaxStep = 5000;
    }

    public override void OnEpisodeBegin()
    {
        // 스테이지 초기화
        stageManager.SetStateObject();

        // 물리엔진 초기화
        rb.velocity = rb.angularVelocity = Vector3.zero;

        // 에이전트의 위치를 초기화
        tr.localPosition = new Vector3(Random.Range(-22.0f, 22.0f),
                                       0.05f,
                                       Random.Range(-22.0f, 22.0f));

        // 회전 불규칙하게 변경
        tr.localRotation = Quaternion.Euler(Vector3.up * Random.Range(0, 360));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
    }
}
