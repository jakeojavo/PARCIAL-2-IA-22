using System;
using System.Collections.Generic;
using FSM;
using UnityEngine;

public class AttackState : MonoBaseState
{

    public EnemyLineOfSight myLineOfSight;
    public GameObject playerHealth;
    public GameObject myRobot;
    public PlayerHealth myPlayerHealth;
    public EnemyMovement myMovement;
    public ShootPlayer myShootPlayer;
    public float shootCount;

    public GameObject bullet;
    public GameObject player;
    public float distance;
    public EnemyWorldState myWorldState;

    public override event Action OnNeedsReplan;

    public bool onState;

    private void Awake()
    {

        myShootPlayer = GetComponent<ShootPlayer>();
        player = GameObject.FindGameObjectWithTag("Player");
        myLineOfSight = GetComponent<EnemyLineOfSight>();
        playerHealth = GameObject.FindGameObjectWithTag("PlayerHealth");
        myPlayerHealth = playerHealth.GetComponent<PlayerHealth>();
        myMovement = GetComponent<EnemyMovement>();
        myWorldState = GetComponent<EnemyWorldState>();
    }

    public override void Enter(IState from, Dictionary<string, object> transitionParameters = null)
    {
        shootCount = 0;
        onState = true;
    }

    private void Update()
    {
        if (onState)
        {

            if (distance <= 4f)
            {
                shootCount += Time.deltaTime;
            }

            if (distance > 4f)
            {
                shootCount = 0f;
            }
        }
    }

    public override void UpdateLoop()
    {
        if (myMovement.target != player.transform)
            myMovement.target = player.transform;

        distance = (player.transform.position - transform.position).sqrMagnitude;

    }

    public override IState ProcessInput()
    {

        if (myWorldState.seenPlayer)
        {
            if (distance >= 4f) //si me alejo del player, replaneo
            {
                OnNeedsReplan?.Invoke();
            }

            if (distance <= 2f && shootCount >= 3f)
            {
                var newBullet = GameObject.Instantiate(bullet);
                newBullet.transform.position = transform.position;

                Debug.LogError("Cuanto se ejecuta la trans a Reload");
                return Transitions["ReloadState"];

            }
        }

        if (!myWorldState.seenPlayer)
        {
            return Transitions["ReloadState"];
        }
        return this;

    }
}