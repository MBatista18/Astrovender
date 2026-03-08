using UnityEngine;

public class MultiShotRobotStateFire : StateBase
{
    float maxTimer = 2f;
    float timer;

    MultiShotRobotSM sm;

    public MultiShotRobotStateFire (StateMachineBase _sm) : base (_sm)
    {
        sm = (MultiShotRobotSM) _sm;

        timer = maxTimer;
    }

    bool cardinalDirection = true;

    public override void thisUpdate()
    {
        base.thisUpdate();

        timer -= Time.deltaTime;

        if (timer > maxTimer / 2)
        {
            sm.SwapRenderer(!cardinalDirection);
        }
        else
        {
            sm.SwapRenderer(cardinalDirection);
        }

        if (timer <= 0)
        {
            Shoot();
            timer = maxTimer;
        }
    }

    void Shoot() 
    {
        switch (cardinalDirection)
        {
            case true:
                Object.Instantiate(sm.GetBullet(), sm.transform.position, Quaternion.identity).GetComponent<EnemyBullet>().SetDirection(Vector2.up);
                Object.Instantiate(sm.GetBullet(), sm.transform.position, Quaternion.identity).GetComponent<EnemyBullet>().SetDirection(Vector2.right);
                Object.Instantiate(sm.GetBullet(), sm.transform.position, Quaternion.identity).GetComponent<EnemyBullet>().SetDirection(Vector2.down);
                Object.Instantiate(sm.GetBullet(), sm.transform.position, Quaternion.identity).GetComponent<EnemyBullet>().SetDirection(Vector2.left);
                break;
            case false:
                Object.Instantiate(sm.GetBullet(), sm.transform.position, Quaternion.identity).GetComponent<EnemyBullet>().SetDirection(new Vector2(-1,1));
                Object.Instantiate(sm.GetBullet(), sm.transform.position, Quaternion.identity).GetComponent<EnemyBullet>().SetDirection(new Vector2(-1, -1));
                Object.Instantiate(sm.GetBullet(), sm.transform.position, Quaternion.identity).GetComponent<EnemyBullet>().SetDirection(new Vector2(1, 1));
                Object.Instantiate(sm.GetBullet(), sm.transform.position, Quaternion.identity).GetComponent<EnemyBullet>().SetDirection(new Vector2(1, -1));
                break;
        }

        cardinalDirection = !cardinalDirection;
    }
}
