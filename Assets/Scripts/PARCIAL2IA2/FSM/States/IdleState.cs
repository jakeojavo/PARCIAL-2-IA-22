using System;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class IdleState : MonoBaseState {

    public GameObject myRobot;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;
    public float time;
    
    public override event Action OnNeedsReplan;
    
    private void Awake()
    {
        myRobot = gameObject;
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myMovement = GetComponent<EnemyMovement>();
       
    }

    public override void UpdateLoop() {
        
        if (myMovement.myAgent.speed >= 0f)
            myMovement.myAgent.speed -= Time.deltaTime / 3;

        if (myMovement.offsetSpeed >= 0f)
            myMovement.offsetSpeed -= Time.deltaTime / 2;

        time += Time.deltaTime;


    }

    public override IState ProcessInput() {
        
        if (time >= 5f)
        {
            OnNeedsReplan?.Invoke();
            time = 0;
           // return Transitions["PatrolState"];
        }

        if (myLineOfSight.playerOnSight)
        {
            OnNeedsReplan?.Invoke();
            time = 0;
        }

        return this;
    }
}