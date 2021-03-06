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
        var action = actions.DiscreteActions;
        Debug.Log($"[0]={action[0]}, [1]={action[1]}");

        Vector3 dir = Vector3.zero;
        Vector3 rot = Vector3.zero;

        // Branch 0 전진 후진을 판단
        switch (action[0])
        {
            case 1: dir = tr.forward; break;
            case 2: dir = -tr.forward; break;
        }

        // Branch 1 좌우 회전을 판단
        switch (action[1])
        {
            case 1: rot = -tr.up; break;
            case 2: rot = tr.up; break;
        }

        tr.Rotate(rot, Time.fixedDeltaTime * turnSpeed);
        rb.AddForce(dir * moveSpeed, ForceMode.VelocityChange);

        AddReward(-1 / (float)MaxStep);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.DiscreteActions;

        actions.Clear();

        // Branch 0
        // 전진/후진  Non, W, S (0, 1, 2)
        if (Input.GetKey(KeyCode.W))
        {
            actions[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actions[0] = 2;
        }

        // Branch 1
        // 좌우 회전 Non, A, D (0, 1, 2)
        if (Input.GetKey(KeyCode.A))
        {
            actions[1] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            actions[1] = 2;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("SLIME"))
        {
            floorRd.material = goodMt;

            rb.velocity = rb.angularVelocity = Vector3.zero;
            Destroy(coll.gameObject);
            AddReward(1.0f);
            StartCoroutine(RevertMaterial());
        }

        if (coll.collider.CompareTag("BAD"))
        {
            floorRd.material = badMt;

            AddReward(-1.0f);
            EndEpisode();
            StartCoroutine(RevertMaterial());
        }

        if (coll.collider.CompareTag("WALL"))
        {
            //rb.velocity = rb.angularVelocity = Vector3.zero;
            AddReward(-0.1f);
        }
    }

    IEnumerator RevertMaterial()
    {
        yield return new WaitForSeconds(0.2f);
        floorRd.material = originMt;
    }
}
