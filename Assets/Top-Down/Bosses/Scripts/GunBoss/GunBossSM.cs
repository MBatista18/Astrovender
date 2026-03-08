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

    public override void InstantiateComponents()
    {
        base.InstantiateComponents();
        animator = GetComponent<Animator>();
    }

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
        animator.Play("Torso_Knockback", 0);
        animator.Play("Head_Knockback", 1);

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

    public bool GetIsShooting() { return isShooting; }

    bool isShooting;

    public IEnumerator shoot()
    {
        isShooting = true;

        int i = 0;
        while (i < 3)
        {
            i++;
            var a = Instantiate(bullet, transform.position, Quaternion.identity);
            switch (base.GetFacingDirection())
            {
                case AstrovenderStructs.facingDirection.up:
                    animator.Play("Head_ShootLeft", 1, 0);
                    a.transform.position += Vector3.left * .5f;
                    a.GetComponent<EnemyBullet>().SetDirection(AstrovenderStructs.facingDirection.left);
                    break;
                case AstrovenderStructs.facingDirection.left:
                    animator.Play("Head_ShootDown", 1, 0);
                    a.transform.position += Vector3.down * .5f;
                    a.GetComponent<EnemyBullet>().SetDirection(AstrovenderStructs.facingDirection.down);
                    break;
                case AstrovenderStructs.facingDirection.down:
                    animator.Play("Head_ShootRight", 1, 0);
                    a.transform.position += Vector3.right * .5f;
                    a.GetComponent<EnemyBullet>().SetDirection(AstrovenderStructs.facingDirection.right);
                    break;
                case AstrovenderStructs.facingDirection.right:
                    animator.Play("Head_ShootUp", 1, 0);
                    a.transform.position += Vector3.up * .5f;
                    a.GetComponent<EnemyBullet>().SetDirection(AstrovenderStructs.facingDirection.up);
                    break;
            }
            yield return new WaitForSeconds(i <= 1 ? 0.2f : 0.1f);
        }

        isShooting = false;

        if (GetCurrentState() == stateMove)
        {
            stateMove.animate();
        }
    }
}
