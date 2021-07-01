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
    public EnemyWorldState myWorldState;
    public override event Action OnNeedsReplan;

    public bool onState;
    
    private void Awake()
    {
        myRobot = gameObject;
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myMovement = GetComponent<EnemyMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
        myMovement = GetComponent<EnemyMovement>();
        myWorldState = GetComponent<EnemyWorldState>();

    }
    
    public override void Enter(IState @from, Dictionary<string, object> transitionParameters = null)
    {
        base.Enter(@from, transitionParameters);

        onState = true;
        
        Debug.Log("a");

        reloadTime = 0;
        
    }

    private void Update()
    {
        if (onState)
        {
            reloadTime += Time.deltaTime;
        }
        
        if (myWorldState.seenPlayer)
        {
            if (myMovement.myAgent.speed >= 0.2f)
                myMovement.myAgent.speed -= Time.deltaTime / 3;

            if (myMovement.offsetSpeed >= 0f)
                myMovement.offsetSpeed -= Time.deltaTime / 2;
            
            reloadTime += Time.deltaTime;

        }
    }

    public override IState ProcessInput() {
        
        var sqrDistance = (player.transform.position - transform.position).sqrMagnitude;

        if (myWorldState.seenPlayer) //si ya vio previamente al player
        {
            if (myLineOfSight.playerOnSight && myLineOfSight.playerOnAngle && reloadTime >= 3
            ) //si paso el tiempo de recarga y lo ve, replanea
            {
                reloadTime = 0;

                OnNeedsReplan?.Invoke();
            }

            if (!myLineOfSight.playerOnSight && !myLineOfSight.playerOnAngle && reloadTime >= 3
            ) //si paso el tiempo de recarga y no lo ve, va a alerta
            {
                reloadTime = 0;
                return Transitions["AlertState"];
            }
        }

        else return Transitions["AlertState"]; //si no vio al player, pasa a alerta

      

        return this;

    }
}