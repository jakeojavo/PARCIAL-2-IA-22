using System.Collections.Generic;
using FSM;
using UnityEngine;

public class AttackState : MonoBaseState {

    public EnemyLineOfSight myLineOfSight;
    public GameObject playerHealth;
    public GameObject myRobot;
    public PlayerHealth myPlayerHealth;
    public float shootCount = 4;
    
    public EnemyMovement myMovement;
    public ShootPlayer myShootPlayer;
    
    public GameObject bullet;
    public GameObject player;
    public float distance;
    
    private void Awake() {
        
        myShootPlayer = GetComponent<ShootPlayer>();
        player = GameObject.FindGameObjectWithTag("Player");
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        playerHealth = GameObject.FindGameObjectWithTag("PlayerHealth");
        myPlayerHealth = playerHealth.GetComponent<PlayerHealth>();
        myMovement = GetComponent<EnemyMovement>();
    }

    public override void UpdateLoop()
    {

        if (myMovement.target != player.transform)
            myMovement.target = player.transform;
        
        distance = (player.transform.position - transform.position).sqrMagnitude;
        

        if (distance <= 4f)
            {
                myShootPlayer.shootCount += Time.deltaTime;
            }

            if (distance > 4f)
            {
                myShootPlayer.shootCount = 2.5f;
            }
            
        
     
    }

    public override IState ProcessInput() {
        
        if (distance >= 4f)
        {
            Debug.Log("fui a chase");
            return Transitions["OnChaseState"];
        }

        if (distance <= 2f)
        {
            Debug.Log("fui a reload");
             //var newBullet = GameObject.Instantiate(bullet);
             //newBullet.transform.position = transform.position;
          if (Transitions.ContainsKey("ReloadState1")) 
          {
              Debug.Log("contiene");
              return Transitions["ReloadState1"];
              
          }
          else Debug.Log("nocon");
            
        }

        if (myPlayerHealth.health <= 0)
        {
            Debug.Log("voy a idle");
            return Transitions["IdleState"];
        }

        return this;
    }
}