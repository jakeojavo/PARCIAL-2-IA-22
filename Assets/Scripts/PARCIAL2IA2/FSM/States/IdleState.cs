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

        if (myMovement.myAgent.speed <= 0f)
        {
            time += Time.deltaTime;  
        }

        


    }

    public override IState ProcessInput() {
        
        if (time >= 5f) //si pasan 5 segundos sale de idle y replanea
        {
            OnNeedsReplan?.Invoke();
            time = 0;
        }

        if (myLineOfSight.playerOnSight) //si ve al player, replanea
        {
            OnNeedsReplan?.Invoke();
            time = 0;
        }

        return this;
    }
}