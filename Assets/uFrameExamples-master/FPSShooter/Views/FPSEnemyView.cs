using System;
using UnityEngine;
using UniRx;
using Random = UnityEngine.Random;
public partial class FPSEnemyView
{
    public NavMeshAgent _NavAgent;
    public float Health, Speed;
    public bool Decoy, Tank, Fast, Nomal; 
    private GameObject player;
    private float dis;
    private Animator ani;
    private bool die;

    private Renderer ForestGolem;

    private bool nowAttack = false;

    private bool hitPlayer;

    private float color = 1.0f;
    //private SphereCollider sp;

    public void Start()
    {
        //sp = GetComponent<SphereCollider>();
        foreach(Transform FG in this.gameObject.transform)
        {
            if (FG.name == "Forest Golem")
            {
                ForestGolem = FG.GetComponent<Renderer>();
            }
        }
        FPSEnemy.Health = Health;
        FPSEnemy.Speed = Speed;
        player = GameObject.FindGameObjectWithTag("Player"); 
        //player.transform.position = FPSEnemy.ParentFPSGame.CurrentPlayer.Position;
        ani = gameObject.GetComponent<Animator>();
        ani.SetBool("Walk", true);
        int i = Random.Range(0, 10);
        if (i == 0)
            Decoy = true;
        else if (i == 1)
            Tank = true;
        else if (i == 2)
            Fast = true;
        else if (i >= 3)
            Nomal = true;

        
    }

    //void OnTriggerStay(Collider other)
    //{
    //    if(other.tag == "Player")
    //    {
    //        //Debug.Log("find!!");
    //        SeekroFlee(FPSEnemy.Health, other.transform);
    // //       this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
    // //Quaternion.LookRotation(other.transform.position - this.transform.position), 5 * Time.deltaTime);
    //    }
    //}

    public void SeekroFlee(float hp)
    {
        dis = Vector3.Distance(this.transform.position, player.transform.position);

        if (dis <= 2.5f)
        {
            FPSEnemy.Speed = 0;
            //Debug.Log(dis);
            if (!nowAttack && !Tank)
            {
                nowAttack = true;
                Attack();
            }

            return;
        }
        else
            FPSEnemy.Speed = Speed;

        if (hp <= 2 && Nomal)
        {
            //   this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
            //Quaternion.LookRotation(-this.transform.position - player.transform.position), 10 * Time.deltaTime);
            Vector3 targetRotation = Quaternion.LookRotation(-player.transform.position - this.transform.position).eulerAngles;
            targetRotation.x = 0;
            targetRotation.z = 0;
            
            if(dis > 2.5f)
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(targetRotation), 5 * Time.deltaTime);
        }
        else
        {
            //   this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
            //Quaternion.LookRotation(player.transform.position - this.transform.position), 5 * Time.deltaTime);

            if (dis > 2.5f)
            {
                Vector3 targetRotation = Quaternion.LookRotation(player.transform.position - this.transform.position).eulerAngles;
                targetRotation.x = 0;
                targetRotation.z = 0;

                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(targetRotation), 500 * Time.deltaTime);
            }
        }

            if (Decoy)
            {
                FPSEnemy.Speed = Speed * 2;
                decoy();
            }
            else if (Tank)
            {
                FPSEnemy.Health = Health * 2;
                tank();
            }
            else if (Fast)
            {
                FPSEnemy.Speed = Speed * 2;
                fast();
            }

        //}
    }

    void Attack()
    {
        ani.SetBool("Walk", false);
        ani.SetBool("Punch", true);
        Invoke("ExitAttack", 0.8f);
    }

    void ExitAttack()
    {
        if(hitPlayer)
        {
            player.SendMessage("ApplyDamage");
        }

        ani.SetBool("Punch", false);
        ani.SetBool("Walk", true);

        nowAttack = false;
    }

    private void fast()
    {
        if(dis <= 30)
        {
            FPSEnemy.Speed = (dis) + 1;
        }
    }

    private void tank()
    {

        ForestGolem.material.SetColor("_Color", new Color(1.0f, 0.0f, 0.0f));
        if (dis <= 20)
        {
            FPSEnemy.Speed = dis / 2;
            FPSEnemy.Health = dis - 1;
        }
        else
            FPSEnemy.Speed = Speed;
    }

    private void decoy()
    {
        if (dis <= 10)
        {
            FPSEnemy.Speed = 0;
            Invoke("SetDecoy", 2);
        }
        else
            FPSEnemy.Speed = Speed * 2;
    }

    private void SetDecoy()
    {
        Decoy = false;
        Fast = true;
        FPSEnemy.Speed = Speed;
    }
  
    public override void Bind()
    {
        base.Bind();
        //FPSEnemy.ParentFPSGame.CurrentPlayer.PositionProperty.Subscribe(_ =>
        //{
        //    _NavAgent.SetDestination(_);
        //    transform.LookAt(_);
        //}).DisposeWith(this.gameObject);

        UpdateAsObservable()
            .Subscribe(_ =>
            {
                
                if (!ani.GetBool("Die"))
                {
                    if (FPSEnemy.Health <= 1)
                    {
                        DieAction();
                    }
                    transform.Translate(0, 0, Time.deltaTime * FPSEnemy.Speed);
                    SeekroFlee(FPSEnemy.Health);
                }

            }).DisposeWith(this.gameObject);
    }

    void DieAction()
    {
        ani.SetBool("Punch", false);
        ani.SetBool("Walk", false);
        ani.SetBool("Die", true);
        FPSEnemy.Health = 10;
        Invoke("ExitDie", 2.5f);
        
    }

    void ExitDie()
    {
        FPSEnemy.Health = 0;
    }

    public override void HealthChanged(float value)
    {
        base.HealthChanged(value);
        //gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(1.0f, 1.0f - (1.0f - value), 1.0f - (1.0f - value)));
    }
    public override void HealthStateChanged(FPSPlayerState value)
    {
        gameObject.SetActive(value != FPSPlayerState.Dead);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            hitPlayer = false;
            return;
        }
        else
        {
            hitPlayer = true;
            return;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            hitPlayer = false;
            return;
        }
    }

}