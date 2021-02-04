using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public GameManager gameManager;
    private int DamagedDuration = 2;

    public float maxSpeed;
    public float jumpPower;

    //sound
    public AudioClip audioJump;
    public AudioClip audioAttack;
    public AudioClip audioDamaged;
    public AudioClip audioItem;
    public AudioClip audioDie;
    public AudioClip audioFinish;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;
    AudioSource audioSource;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        //Player Jump
        if (Input.GetButtonDown("Jump") && !anim.GetBool("IsJump"))
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("IsJump", true);
            audioSource.clip = audioJump;
            audioSource.Play();
        }
        
        //Stop Speed
        if (Input.GetButtonUp("Horizontal")){
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.2f, rigid.velocity.y);
        }
        //Dirction Sprite
        if(Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //Walking Animation, Abs == 절대값
        if(Mathf.Abs(rigid.velocity.x) < 0.3)
            anim.SetBool("IsWalk", false);
        else
            anim.SetBool("IsWalk", true);

    }
    private void FixedUpdate()
    {
        //Move Speed
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //Max Speed
        if(rigid.velocity.x > maxSpeed) // right Max Speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // Left Max Speed
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //Landing Platform
        if(rigid.velocity.y < 0)
        {
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));

            if (rayHit.collider != null)
            {
                if (rayHit.distance < 0.5f)
                    anim.SetBool("IsJump", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            //Atack
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }else
                OnDamaged(collision.transform.position);
        }
    }

     void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            //point
            bool isBronze = collision.gameObject.name.Contains("BronzeCoin");
            bool isSliver = collision.gameObject.name.Contains("SliverCoin");
            bool isGold = collision.gameObject.name.Contains("GoldCoin");

            if (isBronze)
                gameManager.statePoint += 50;
            else if (isSliver)
                gameManager.statePoint += 100;
            else if (isGold)
                gameManager.statePoint += 300;
            //Deactive
            collision.gameObject.SetActive(false);
        }else if (collision.gameObject.tag == "Finish")
        {
            //Next Stage
            gameManager.NextStage();
        }
    }


    void OnAttack(Transform enemy)
    {
        //point
        gameManager.statePoint += 100;

        //Attack Reaction
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        //Attack Enemy
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();
    }

    void OnDamaged(Vector2 targetPosition)
    {
        //health down
        gameManager.HealthDown();
        //On Damaged
        gameObject.layer = 11;

        //View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        //Reaction Force
        int dirc = transform.position.x - targetPosition.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 7,ForceMode2D.Impulse);
        //Animation
        anim.SetTrigger("DoDamaged");

        //Off Damaged
        Invoke("OffDamaged", DamagedDuration);
    }

    void OffDamaged()
    {
        //무적 풀기
        gameObject.layer = 10;
        //View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        capsuleCollider.enabled = false;

        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
