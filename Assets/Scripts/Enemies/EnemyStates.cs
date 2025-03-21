using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStates
{
    public enum STATE
    {
        IDLE, PATROL, CHASE
    }

    public enum EVENT
    {
        ENTER, UPDATE, EXIT
    }

    public STATE stateName;
    protected EVENT stage;

    protected GameObject enemy;
    protected Animation anim;
    protected Transform playerPos;
    protected EnemyStates nextState;

    public EnemyStates(GameObject enemy, Animation anim, Transform playerPos)
    {
        enemy = this.enemy;
        anim = this.anim;
        playerPos = this.playerPos;
    }

    public virtual void Enter()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Update()
    {
        stage = EVENT.UPDATE;
    }

    public virtual void Exit()
    {
        stage = stage = EVENT.EXIT;
    }

    public EnemyStates Process()
    {
        if (stage == EVENT.ENTER)
        {
            Enter();
        }
        else if (stage == EVENT.UPDATE)
        {
            Update();
        }
        else
        {
            Exit();
            return nextState;
        }
        return this;
    }
}

public class Idle : EnemyStates
{
    float timer; //
    
    public Idle(GameObject enemy, Animation anim, Transform playerPos) : base(enemy, anim, playerPos)
    {
        stateName = STATE.IDLE;
    }

    public override void Enter()
    {
        
        base.Enter();
    }

    public override void Update()
    {
        timer += Time.deltaTime;

        if (timer > 5)
        {
            // definir nextState 
            stage = EVENT.EXIT;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
