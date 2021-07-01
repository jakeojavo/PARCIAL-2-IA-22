using System;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class ReloadState : MonoBaseState {

    public GameObject myRobot;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;
    public GameObject player;

    public float reloadTime;
    public override event Action OnNeedsReplan;
    
    private void Awake()
    {
        myRobot = gameObject;
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myMovement = GetComponent<EnemyMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
       
    }

    public override void UpdateLoop() {
        
        if (myMovement.myAgent.speed >= 0.3f)
            myMovement.myAgent.speed -= Time.deltaTime / 3;

        if (myMovement.offsetSpeed >= 0f)
            myMovement.offsetSpeed -= Time.deltaTime / 2;

        reloadTime += Time.deltaTime;


    }

    public override IState ProcessInput() {
        
        var sqrDistance = (player.transform.position - transform.position).sqrMagnitude;

        if (reloadTime >= 3f)
        {

            reloadTime = 0;
            
           OnNeedsReplan?.Invoke();

        }

        return this;

    }
}