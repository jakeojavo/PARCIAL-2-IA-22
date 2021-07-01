using System.Collections.Generic;
using FSM;
using UnityEngine;

public class PatrolState : MonoBaseState {

    public GameObject myRobot;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;

    public float timeToIdle;
    
    private void Awake()
    {
        myRobot = gameObject;
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myMovement = GetComponent<EnemyMovement>();
       
    }

    public override void UpdateLoop() {
        
        if (myMovement.myAgent.speed >= 0.5f)
            myMovement.myAgent.speed -= Time.deltaTime / 3;

        if (myMovement.myAgent.speed < 0.5f)
            myMovement.myAgent.speed += Time.deltaTime;

        if (myMovement.offsetSpeed >= 0f)
            myMovement.offsetSpeed -= Time.deltaTime / 2;
        
        if (myMovement.statesTriggers[EStates.PATROL] == false)
            myMovement.statesTriggers[EStates.PATROL] = true;


        timeToIdle += Time.deltaTime;


    }

    public override IState ProcessInput() {

        if (myLineOfSight.playerOnSight)
        {
            Debug.Log("fui a chase");
            return Transitions["ChaseState"];
        }

        if (timeToIdle >= 10)
        {
            Debug.Log("fui a idle");
            timeToIdle = 0;
            return Transitions["IdleState"];
        }

        return this;
    }
}