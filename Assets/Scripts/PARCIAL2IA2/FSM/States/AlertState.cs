using System.Collections.Generic;
using FSM;
using UnityEngine;

public class AlertState : MonoBaseState {

    public GameObject myRobot;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;

    public float timer;
    
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

        timer += Time.deltaTime;


    }

    public override IState ProcessInput() {

        if (myLineOfSight.playerOnSight)
        {
            Debug.Log("fui a chase");
            return Transitions["ChaseState"];
        }

        if (timer >= 10f)
        {
            Debug.Log("fui a patrol");
            timer = 0f;
            return Transitions["PatrolState"];
        }

        return this;
    }
}