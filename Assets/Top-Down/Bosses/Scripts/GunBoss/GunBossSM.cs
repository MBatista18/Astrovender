using UnityEngine;
using System.Collections;
public class GunBossSM : EnemySM
{
    [SerializeField] GameObject[] DungeonWalls;
    [SerializeField] GameObject bullet;

    public override void OnEnableFunctions()
    {
        base.OnEnableFunctions();

        foreach (GameObject a in DungeonWalls)
        {
            a?.SetActive(true);
        }
    }

    public override void OnDisableFunctions()
    {
        base.OnDisableFunctions();

        foreach (GameObject a in DungeonWalls)
        {
            a?.SetActive(false);
        }
    }


    Animator animator;
    public Animator GetAnimator() { return animator; }

    GunBossStateIdle stateIdle;
    public override StateBase InitialState()
    {
        return stateIdle;
    }

    GunBossStateMove stateMove;
    public override StateBase AttackState()
    {
        return stateMove;
    }

    public override void PainReactions() // makes it so the boss starts shooting as it moves
    {
        stateMove.SetWasAttackedTrue();
    }

    public override StateBase DeathState()
    {
        GameManager.Instance.defeatedGunsBoss = true;
        return base.DeathState();
    }

    public override void InstantiateStates()
    {
        base.InstantiateStates();

        stateIdle = new GunBossStateIdle(this);
        stateMove = new GunBossStateMove(this);
    }

    public IEnumerator shoot()
    {
        int i = 0;
        while (i < 3)
        {
            i++;
            var a = Instantiate(bullet, transform.position, Quaternion.identity);
            switch (base.GetFacingDirection())
            {
                case AstrovenderStructs.facingDirection.up:
                    a.GetComponent<EnemyBullet>().SetDirection(AstrovenderStructs.facingDirection.left);
                    break;
                case AstrovenderStructs.facingDirection.left:
                    a.GetComponent<EnemyBullet>().SetDirection(AstrovenderStructs.facingDirection.down);
                    break;
                case AstrovenderStructs.facingDirection.down:
                    a.GetComponent<EnemyBullet>().SetDirection(AstrovenderStructs.facingDirection.right);
                    break;
                case AstrovenderStructs.facingDirection.right:
                    a.GetComponent<EnemyBullet>().SetDirection(AstrovenderStructs.facingDirection.up);
                    break;
            }
            yield return new WaitForSeconds(i <= 1 ? 0.2f : 0.1f);
        }
    }
}
