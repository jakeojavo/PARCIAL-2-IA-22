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

    public bool onState;
    
    private void Awake()
    {
        myRobot = gameObject;
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myMovement = GetComponent<EnemyMovement>();
        myWorldState = GetComponent<EnemyWorldState>();

    }
    
    public override void Enter(IState @from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(@from, transitionParameters);
        
        if (!myMovement.statesTriggers[EStates.ALERT])
        {
            myMovement.SetAllStatesToFalse();
            myMovement.statesTriggers[EStates.ALERT] = true;
        }

        onState = true;

        timer = 0;
    }

    public override void UpdateLoop() {

    }

    public void Update()
    {
        if (myWorldState.seenPlayer)
        {
            timer += Time.deltaTime;
        }
    }


    public override IState ProcessInput() {

        if (myLineOfSight.playerOnSight && myLineOfSight.playerOnAngle) //si ve al player, replanea
        {
            onState = false;
            OnNeedsReplan?.Invoke();
        }

        if (!myWorldState.seenPlayer) //si no vio al player previamente, va directo a patrol
        {
            onState = false;
            Debug.Log("fui a patrol, no vi al player");
            return Transitions["PatrolState"];
        }

        if (timer >= 10f) //pero si lo vio suma el timer, y pasados 10 segundos va a patrol
        {
            onState = false;
            Debug.Log("fui a patrol, vi al player");
            myWorldState.seenPlayer = false;
            return Transitions["PatrolState"];
        }

        return this;
    }
}