﻿using System;
using System.Collections.Generic;
using FSM;
using UnityEngine;
using Random = System.Random;

public class PatrolState : MonoBaseState {

    public GameObject myRobot;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;

    public float timeToIdle;
    public override event Action OnNeedsReplan;
    
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

        if (!myMovement.statesTriggers[EStates.PATROL])
        {
            myMovement.SetAllStatesToFalse();
            myMovement.statesTriggers[EStates.PATROL] = true; 
        }
            


        timeToIdle += Time.deltaTime;


    }

    public override IState ProcessInput() {

        if (timeToIdle >= 10) //cada 10 segundos va a IDLE
        {
            Debug.Log("fui a idle");
            timeToIdle = 0;
            return Transitions["IdleState"];
        }

        if (myLineOfSight.playerOnSight && myLineOfSight.playerOnAngle) //si veo al player, replaneo
        {
            OnNeedsReplan?.Invoke();
        }

        return this;
    }
}