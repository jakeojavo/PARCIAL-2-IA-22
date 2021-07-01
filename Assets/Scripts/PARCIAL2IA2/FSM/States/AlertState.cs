using System;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class AlertState : MonoBaseState {

    public GameObject myRobot;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;
    public EnemyWorldState myWorldState;

    public float timer;
    
    public override event Action OnNeedsReplan;
    
    private void Awake()
    {
        myRobot = gameObject;
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myMovement = GetComponent<EnemyMovement>();
        myWorldState = GetComponent<EnemyWorldState>();

    }

    public override void UpdateLoop() {

        if (myWorldState.seenPlayer)
        {
            if (myMovement.myAgent.speed >= 0.5f)
                myMovement.myAgent.speed -= Time.deltaTime / 3;

            if (myMovement.myAgent.speed < 0.5f)
                myMovement.myAgent.speed += Time.deltaTime;

            if (myMovement.offsetSpeed >= 0f)
                myMovement.offsetSpeed -= Time.deltaTime / 2;

            timer += Time.deltaTime;

            if (!myMovement.statesTriggers[EStates.ALERT])
            {
                myMovement.SetAllStatesToFalse();
                myMovement.statesTriggers[EStates.ALERT] = true;
            }
          
        }
        

    }

    public override IState ProcessInput() {

        if (myLineOfSight.playerOnSight && myLineOfSight.playerOnAngle) //si ve al player, replanea
        {
            OnNeedsReplan?.Invoke();
        }

        if (!myWorldState.seenPlayer) //si no vio al player previamente, va directo a patrol
        {
            Debug.Log("fui a patrol, no vi al player");
            return Transitions["PatrolState"];
        }

        if (timer >= 10f) //pero si lo vio suma el timer, y pasados 10 segundos va a patrol
        {
            Debug.Log("fui a patrol, vi al player");
            timer = 0f;
            myWorldState.seenPlayer = false;
            return Transitions["PatrolState"];
        }

        return this;
    }
}