using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FSM;
using UnityEngine;

public class ChaseState : MonoBaseState {
    
    public EnemyWorldState myWorldState;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;

    public GameObject player;
    public float counter;
    public float sqrDistance;
    
    private void Awake()
    {
        
        myWorldState = GetComponent<EnemyWorldState>();
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myMovement = GetComponent<EnemyMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
    }


    public override void UpdateLoop()
    {
        
        sqrDistance = (player.transform.position - transform.position).sqrMagnitude;
        
        if (myLineOfSight.playerOnSight && myLineOfSight.playerOnAngle)
        {
            if (!myWorldState.seenPlayer) myWorldState.seenPlayer = true;

            if (!myMovement.statesTriggers[EStates.CHASE])
            {
                myMovement.SetAllStatesToFalse();
                myMovement.statesTriggers[EStates.CHASE] = true;
            }

        }

    }    

    public override IState ProcessInput() {

        if (myWorldState.seenPlayer) //comportamiento si vio al player
        {
            if (sqrDistance <= 4f)
            {
                myWorldState.seenPlayer = false;
                return  Transitions["AttackState"];
            }
            
            if (!myLineOfSight.playerOnSight && !myLineOfSight.playerOnAngle)
            {
                return  Transitions["AttackState"];
            }
        }

        else //si no lo vi sigo con las transiciones
        {
            return Transitions["AttackState"];
        }

            return this;

    }
}