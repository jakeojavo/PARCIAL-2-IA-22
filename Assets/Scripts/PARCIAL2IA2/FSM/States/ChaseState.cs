using System.Collections.Generic;
using FSM;
using UnityEngine;

public class ChaseState : MonoBaseState {
    
    public EnemyBehaviours myBehaviours;
    public EnemyLineOfSight myLineOfSight;
    public EnemyMovement myMovement;

    public GameObject player;
    public float counter;
    
    private void Awake()
    {
        
        myBehaviours = GetComponent<EnemyBehaviours>();
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        myMovement = GetComponent<EnemyMovement>();
        player = GameObject.FindGameObjectWithTag("Player");
        counter = 0;
    }
    

    public override void UpdateLoop() {
        
        if (myMovement.myAgent.speed <= 1f)
            myMovement.myAgent.speed += Time.deltaTime;

        if (myMovement.offsetSpeed <= 3f)
            myMovement.offsetSpeed += Time.deltaTime;

        if (myMovement.statesTriggers[EStates.CHASE] == false)
            myMovement.statesTriggers[EStates.CHASE] = true;

        if (!myLineOfSight.playerOnSight)
        {
            counter += Time.deltaTime;
        }
        else
        {
            counter = 0;
        }

        Debug.Log(counter);

    }

    public override IState ProcessInput() {
        
        var sqrDistance = (player.transform.position - transform.position).sqrMagnitude;
      
        if (counter >= 4f)
        {
            Debug.Log("fui a Alert");
            return  Transitions["AlertState"];
        }
        
        if (myLineOfSight.playerOnSight == true && sqrDistance <= 4f)
        {
            Debug.Log("ataco");
            return  Transitions["AttackState"];
        }
        
        return this;

    }
}