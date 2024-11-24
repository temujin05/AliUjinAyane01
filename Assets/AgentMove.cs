using UnityEngine;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentController : Agent
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 4f;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-5f, 6.6f), 0f, Random.Range(-2.7f, 8.8f));
        target.localPosition = new Vector3(Random.Range(-5, 6.6f), 0f, Random.Range(-2.7f, 8.8f));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(target.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 velocity = new Vector3(moveX, 0f, moveZ);
        velocity = velocity.normalized * Time.deltaTime * moveSpeed;
        transform.localPosition += velocity;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reward")
        {
            AddReward(2f);
            EndEpisode();
        }
        if (other.gameObject.tag == "Wall")
        {
            AddReward(-1f);
            EndEpisode();
        }
    }

}
