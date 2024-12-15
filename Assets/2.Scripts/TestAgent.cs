using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TestAgent : MonoBehaviour
{
    public Transform target;

    NavMeshAgent agent;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            agent.SetDestination(target.position);
        }
    }
}
