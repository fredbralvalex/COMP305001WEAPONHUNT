using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController: HittableController, IBoundaryElementController
{

    public bool Dummy;
    public enum PlayerAction { Move, Idle, Jump, Punch, Kick, Pike, Axe, Fall, Defeated, End, GetUp, Won };

    public enum Weapon { Bare, Pike, Axe};
    public Weapon chosenWeapon = Weapon.Bare;

    public enum PlayerDummyAction { MoveToCenter, Won, Nothing };

    public PlayerDummyAction playerDummyState = PlayerDummyAction.Nothing;
    public float PlayerInitialPosition = 0;


    public PlayerAction playerState = PlayerAction.Idle;

    public bool InPosition = false;

    private double time;
    float maxJumpHigh;
    public bool moving = false;
    public bool facingRight = true;
    bool StateMovement = true;
    public int playerJumpPower = 1250;
    public float moveX;
    Animator animator;
    SpriteRenderer sprite;
    Rigidbody2D playerRB;
    GameController gameController;

    private Transform lastPosition;
    private Vector3 lastGroundPosition;
    private Boolean grounded = false;

    //[Header("Audio Sources")]

    public GameController.PassNextLevelDelegate Del { get; set; }

    private void LateUpdate()
    {
        lastPosition = transform;
    }

    void Start() {
        animator = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        playerRB = gameObject.GetComponent<Rigidbody2D>();
        FindGameBarInScene();
    }
	

    private void FindGameBarInScene()
    {
        GameObject gObj = GameObject.FindGameObjectWithTag("GameBar");
        if (gObj != null)
        {
            gameController = gObj.GetComponent<GameController>();
        }
    }

	void FixedUpdate() {
        time += Time.deltaTime;

        if (gameController == null)
        {
            FindGameBarInScene();
        }

        if (Dummy)
        {
            if (playerDummyState == PlayerDummyAction.MoveToCenter)
            {
                PutPlayerInPosition();
            }
            else
            {
                if (playerState == PlayerAction.Won)
                {
                    if(time <= 3)
                    {
                        return;
                    }
                    //print("Won!!!");
                    playerDummyState = PlayerDummyAction.Won;
                    playerState = PlayerAction.Idle;
                    Del.Invoke();
                    //goto to the next level

                }
                /*
                if (playerState == PlayerAction.Won)
                {
                    playerDummyState = PlayerDummyAction.Won;


                }*/
            }
         } else {
            ChooseWeapon();
            Blinking();
            //playerRB.mass = 1;
            //playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (playerState == PlayerAction.Jump)
            {
                PerformeJumpFall();
            }
            
            if (ValidateTimeToWait())
            {
                return;
            }
            time = 0;

            if (playerState == PlayerAction.Defeated)
            {
                playerState = PlayerAction.End;
            }
            if (playerState == PlayerAction.GetUp && Hits == 0)
            {
                //New Chancd
                playerState = PlayerAction.Idle;
            }

            if (gameController!= null && GameController.LifeAmount - Hits <= 0)
            {
                if (playerState == PlayerAction.End)
                {
                    //gameObject.SetActive(false);
                    //Destroy(gameObject);
                    if (gameController.NewChancePlayer())
                    {
                        playerState = PlayerAction.GetUp;
                        Hits = 0;
                        UpdateLifeBar();
                        PlayGetUp();
                    }

                }
                else
                {
                    playerState = PlayerAction.Defeated;
                    PlayDefeated();
                }
            }
            else
            {
                Move();
                Attack();
                Jump();
            }
        }
    }

    private void ChooseWeapon()
    {
        if (Input.GetKeyDown(GameController.NO_WEAPON))
        {
            chosenWeapon = Weapon.Bare;
        } else if (Input.GetKeyDown(GameController.WEAPON_1) && GameStateController.level > 1)
        {
            chosenWeapon = Weapon.Pike;
        } else if (Input.GetKeyDown(GameController.WEAPON_2) && GameStateController.level > 2)
        {
            chosenWeapon = Weapon.Axe;
        }
    }

    internal bool isMovingBack()
    {
        return moving && !facingRight;
    }

    bool ValidateTimeToWait()
    {
        bool wait = false;
        if (playerState == PlayerAction.Punch && GameController.LifeAmount - Hits > 0)
        {
            wait = time <= GameController.TIME_PUNCH;
        } else if (playerState == PlayerAction.Kick && GameController.LifeAmount - Hits > 0)
        {
            wait = time <= GameController.TIME_KICK;
        }else if (playerState == PlayerAction.Pike && GameController.LifeAmount - Hits > 0)
        {
            wait = time <= GameController.TIME_PIKE;
        }
        else if (playerState == PlayerAction.Axe && GameController.LifeAmount - Hits > 0)
        {
            wait = time <= GameController.TIME_AXE;
        }
        else if (playerState == PlayerAction.Jump && GameController.LifeAmount - Hits > 0) {
            wait = time <= GameController.TIME_JUMP;
        }
        else if (playerState == PlayerAction.Defeated)
        {
            wait = time <= GameController.TIME_DEFEATED;
        }
        else if (playerState == PlayerAction.GetUp)
        {
            wait = time <= GameController.TIME_GET_UP;
        }        
        else
        {
            time = 0;
        }       

        return wait;
    }

    private void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (!Dummy && (Input.GetKeyDown(GameController.LEFT) || moveHorizontal < 0))
        {
            playerState = PlayerAction.Move;
            //move left
            facingRight = false;
            Animator animation = animator.GetComponent<Animator>();
            animation.Play(RUN_L);
            MoveTransform(Vector2.left);
            moving = true;
        }
        else if (!Dummy && (Input.GetKeyDown(GameController.RIGHT) || moveHorizontal > 0))
        {
            playerState = PlayerAction.Move;
            //move right
            facingRight = true;
            Animator animation = animator.GetComponent<Animator>();
            animation.Play(RUN);
            MoveTransform(Vector2.right);
            moving = true;
        }
        else
        {
            moving = false;
            playerState = PlayerAction.Idle;
            Animator animation = animator.GetComponent<Animator>();
            if (facingRight)
            {
                animation.Play(IDLE);
            }
            else
            {
                animation.Play(IDLE_L);
            }
        }
    }

    private void Attack()
    {
        if (!Dummy)
        {
            if (Weapon.Bare == chosenWeapon)
            {
                if (Input.GetKeyDown(GameController.ATTACK_1))
                {
                    playerState = PlayerAction.Punch;
                    Attack(true);

                }
                else if (Input.GetKeyDown(GameController.ATTACK_2))
                {
                    playerState = PlayerAction.Kick;
                    Attack(false);
                    MoveTransform(facingRight? Vector2.right: Vector2.left, 3);
                }
            }
            else
            {
                if (Input.GetKeyDown(GameController.ATTACK_1) || Input.GetKeyDown(GameController.ATTACK_2))
                {
                    if (Weapon.Pike == chosenWeapon)
                    {
                        
                        playerState = PlayerAction.Pike;
                        AttackWeapon(true);
                    } else
                    {
                        playerState = PlayerAction.Axe;
                        AttackWeapon(false);
                    }
                }
            }
        }        
    }

    private void AttackWeapon(bool isPike)
    {
        Animator animation = animator.GetComponent<Animator>();
        if (facingRight)
        {
            if (isPike)
            {
                animation.Play(PIKE);
            }
            else
            {
                animation.Play(AXE);
            }            
        }
        else
        {
            if (isPike)
            {
                animation.Play(PIKE_L);
            }
            else
            {
                animation.Play(AXE_L);
            }
        }
        DrawBack();
    }

    public void DrawBack ()
    {
        GettingHit(1);
        GameObject gObj = GameObject.FindGameObjectWithTag("GameBar");
        GameController gController = gObj.GetComponent<GameController>();
        GameObject blood = gController.CreateBlood();
        blood.transform.localPosition = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z);
        Vector3 force = new Vector3(gameObject.transform.position.x - 5, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z);
        Rigidbody2D rb = blood.GetComponent<Rigidbody2D>();
        rb.AddForceAtPosition(force, blood.transform.localPosition);
    }

    private void Attack(bool isPunch)
    {
        Animator animation = animator.GetComponent<Animator>();
        if (facingRight)
        {
            if (isPunch)
            {
                animation.Play(PUNCH);
            }
            else
            {
                animation.Play(KICK);
            }
        }
        else
        {
            if (isPunch)
            {
                animation.Play(PUNCH_L);
            }
            else
            {
                animation.Play(KICK_L);
            }
        }
        /*
        HitController[] hcontrollers = gameObject.GetComponentsInChildren<HitController>();
        HitController hcontrollerL = null;
        HitController hcontrollerR = null;

        foreach (HitController hcontroller in hcontrollers)
        {
            
            if (isPunch)
            {
                if (hcontroller.gameObject.tag == "hitRightP")
                {
                    hcontrollerR = hcontroller;
                }
                else if (hcontroller.gameObject.tag == "hitLeftP")
                {
                    hcontrollerL = hcontroller;
                }
            }
            else
            {
                if (hcontroller.gameObject.tag == "hitRightK")
                {
                    hcontrollerR = hcontroller;
                }
                else if (hcontroller.gameObject.tag == "hitLeftK")
                {
                    hcontrollerL = hcontroller;
                }
            }
        }            
        if (CanHit && AimHit != null)
        {
            HittableController hController = AimHit.GetComponent<HittableController>();
            if (hController != null)
            {
                if (facingRight)
                {
                    if (hcontrollerR != null)
                    {
                        hcontrollerR.GetActionHit();
                    }
                }
                else
                {
                    if (hcontrollerL != null)
                    {
                        hcontrollerL.GetActionHit();
                    }
                }

                if (isPunch)
                {
                    hController.GettingHit(GameController.POWER_ATTACK_1);
                }
                else
                {
                    hController.GettingHit(GameController.POWER_ATTACK_2);
                }
            }
        }*/
    }

    private void MoveTransform(Vector2 direction, float var = 1)
    {        
        //float var = 1;

        Vector2 nextPosition = direction * var * GameController.SPEED_CONSTANT * (gameObject.transform.localScale) * Time.deltaTime;
        Move(nextPosition);
    }
    private void Move(Vector3 nextPosition)
    {
        if (StateMovement)
        {
            transform.localPosition += (Vector3)nextPosition;
        }
        else
        {
            //print("NOT state movement");
            transform.localPosition -= (Vector3)nextPosition;
            //gameObject.SetActive(false);
            StateMovement = true;
        }
    }

    private void Jump()
    {
        if (!Dummy && Input.GetKeyDown(GameController.JUMP))
        {
            //Jump
            GameSaveStateController.GetInstance().GeneratePlayJumpAudio();
            //obj.GetComponent<PlaySoundController>().PlayAudioSource();
            playerState = PlayerAction.Jump;
            Animator animation = animator.GetComponent<Animator>();
            if (facingRight)
            {
                animation.Play(JUMP_AN);
            }
            else
            {
                animation.Play(JUMP_AN_L);
            }            
        }        
    }

    private void PerformeJumpFall()
    {
        float varRun = 1;

        Vector2 nextPositionHorizontal;
        if (Input.GetKey(GameController.LEFT))
        {
            facingRight = false;
            moving = true;
            nextPositionHorizontal = Vector2.left * GameController.SPEED_CONSTANT * varRun * Time.deltaTime * gameObject.transform.localScale;
        }
        else if (Input.GetKey(GameController.RIGHT))
        {
            facingRight = true;
            moving = true;
            nextPositionHorizontal = Vector2.right * GameController.SPEED_CONSTANT * varRun * Time.deltaTime * gameObject.transform.localScale;
        }
        else
        {
            moving = false;
            nextPositionHorizontal = Vector2.zero;
        }

        Vector2 nextPositionVertical = Vector2.zero;

        //if (playerState == PlayerAction.Jump)
        //{
        //}        
        if (maxJumpHigh >= nextPositionVertical.y)
        {
            grounded = false;
            //going high       
            //state = Movement.Jump;
            nextPositionVertical = Vector2.up * GameController.SPEED_JUMP_CONSTANT * Time.deltaTime * gameObject.transform.localScale * 1.2f;
            //print("Up");
        }
        else
        {
            print("Down");
            //Falling
        }

        transform.localPosition += (Vector3)nextPositionVertical;
        if (StateMovement)
        {
            Move(nextPositionHorizontal);
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        //print(other.gameObject.tag);
        if (other.gameObject.tag == "Ground")
        {
            Vector3 boundsSize = gameObject.GetComponent<Renderer>().bounds.size;
            lastGroundPosition = new Vector3 (gameObject.transform.localPosition.x - boundsSize.x, gameObject.transform.localPosition.y + boundsSize.y, gameObject.transform.localPosition.z);
            //print("last" + lastGroundPosition);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        //print(other.gameObject.tag);
        if (other.gameObject.tag == "Ground")
        {
            Vector3 boundsSize = gameObject.GetComponent<Renderer>().bounds.size;
            lastGroundPosition = new Vector3(gameObject.transform.localPosition.x - boundsSize.x, gameObject.transform.localPosition.y + boundsSize.y, gameObject.transform.localPosition.z);
            //print("last" + lastGroundPosition);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //print(other.gameObject.tag);
        if (other.gameObject.tag == "Ground")
        {
            CalcMaxJumpHigh();
            playerState= PlayerAction.Idle;
            grounded = true;            
        }
        else if (other.gameObject.tag == "Gangman")
        {
            //Rigidbody2D rigidbody = other.gameObject.GetComponent<Rigidbody2D>();
            //rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            //playerRB.mass = 100;
        }
        else if (other.gameObject.tag == "Blood")
        {
            PlaySoundController c = other.gameObject.GetComponent<PlaySoundController>();
            c.PlayAudioSource();
            Hits--;
            UpdateLifeBar();
            //other.gameObject.SetActive(false);
            //Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Coin")
        {
            PlaySoundController c = other.gameObject.GetComponent<PlaySoundController>();
            c.PlayAudioSource();
            GameStateController.coins++;
            //other.gameObject.SetActive(false);
            //Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Water")
        {
            GettingHit(GameController.LifeAmount);//Remove all life

            Position gameRespawnPosition = other.gameObject.GetComponent<Position>();
            //Transform respawnPosition = gameRespawnPosition.GetComponent<Transform>();
            gameObject.transform.localPosition = new Vector3(gameRespawnPosition.TransformPosition.position.x, gameRespawnPosition.TransformPosition.position.y, gameObject.transform.position.z);
            /*print("older" + gameObject.transform.position);
            gameObject.transform.position = new Vector3(lastGroundPosition.x, lastGroundPosition.y, lastGroundPosition.z);
            print("new" + gameObject.transform.position);*/
            //
        }
        else if (other.gameObject.tag == "Fire")
        {
            GettingHit(1);
        }
        else if (other.gameObject.tag == "Mush")
        {
            GettingHit(1);
        }
        else if (other.gameObject.tag == "FallingStone")
        {
            GettingHit(1);
        }
    }

    private void CalcMaxJumpHigh()
    {
        float varRun = 2;
        float varSlide = 1;
        maxJumpHigh = transform.localPosition.y + sprite.bounds.size.y * varRun * varSlide * gameObject.transform.localScale.y;
       
    }

    public override bool IsHitting()
    {
        return playerState == PlayerAction.Punch || playerState == PlayerAction.Kick;
    }

    public override void GettingHit(float power)
    {
        GameObject  gObj = GameObject.FindGameObjectWithTag("GameBar");
        GameController gController = gObj.GetComponent<GameController>();
        gController.EnemiesScoreN++;
        Hits++;
        //print(gameObject.tag + " is getting Hit : " + power);
        Blink = true;
        UpdateLifeBar();
        PushedBack(power);
    }

    public override void GettingHit(int hits)
    {
        GameObject gObj = GameObject.FindGameObjectWithTag("GameBar");
        GameController gController = gObj.GetComponent<GameController>();
        gController.EnemiesScoreN++;
        Hits = Hits + hits;
        //print(gameObject.tag + " is getting Hit : " + power);
        Blink = true;
        UpdateLifeBar();
        PushedBack(1);
    }

    private void PushedBack(float power)
    {
        Vector2 direction;
        if(facingRight)
        {
            direction = Vector2.left;
        } else
        {
            direction = Vector2.right;

        }

        Vector2 nextPosition = direction * power/10;
        Move(nextPosition);
    }

    public void UpdateLifeBar()
    {
        GameObject gObj = GameObject.FindGameObjectWithTag("GameBar");
        GameController gController = gObj.GetComponent<GameController>();

        if ((float)Hits / GameController.LifeAmount <= 1)
        {
            gController.LifeBar.fillAmount = 1.0f - (float)Hits / GameController.LifeAmount;
            if (gController.LifeBar.fillAmount > 0.75)
            {
                gController.LifeBar.color = Color.green;
            }
            else if (gController.LifeBar.fillAmount > 0.25 && gController.LifeBar.fillAmount <= 0.75)
            {
                gController.LifeBar.color = Color.yellow;
            }
            else if (gController.LifeBar.fillAmount > 0 && gController.LifeBar.fillAmount <= 0.25)
            {
                gController.LifeBar.color = Color.red;
            }
        }
    }

    protected override SpriteRenderer GetSprite()
    {
        return sprite;
    }

    private void PlayDefeated()
    {
        Animator animation = animator.GetComponent<Animator>();
        if (facingRight)
        {
            animation.Play(CHAR_FALL);
        }
        else
        {
            animation.Play(CHAR_FALL_L);
        }
    }

    private void PlayGetUp()
    {
        Animator animation = animator.GetComponent<Animator>();
        if (facingRight)
        {
            animation.Play(CHAR_GET_UP);
        }
        else
        {
            animation.Play(CHAR_GET_UP_L);
        }
    }

    public void PlayWinning()
    {
        playerState = PlayerAction.Won;
        //playerDummyState = PlayerDummyAction.Won;
        Animator animation = animator.GetComponent<Animator>();
        Dummy = true;
        animation.Play(CHAR_WINNING);
    }

    void IBoundaryElementController.TouchesBoundaries()
    {
        StateMovement = false;
    }

    Transform IBoundaryElementController.GetLastValidPosition()
    {
        return lastPosition;
    }

    //public CameraController.OffSetPlayer delegat;

    public void PutPlayerInPosition()
    {
        if (transform.position.x < 0 && grounded)
        {
            playerState = PlayerAction.Move;
            //move right
            facingRight = true;
            Animator animation = animator.GetComponent<Animator>();
            animation.Play(RUN);
            MoveTransform(Vector2.right);
            moving = true;
        } else if (transform.position.x >= 0 && grounded)
        {
            playerState = PlayerAction.Idle;
            playerDummyState = PlayerDummyAction.Nothing;
            Dummy = false;
            InPosition = true;
            offsetdel.Invoke();
            //GameObject.Find("/MainCamera").GetComponent<CameraController>().UpdateOffSetPlayer();
            //GetComponent<Camera>().GetComponent<CameraController>().UpdateOffSetPlayer();
        }
    }
    public CameraController.OffSetPlayer offsetdel;
    public const string IDLE = "CharIdle";
    public const string IDLE_L = "CharIdle_l";

    public const string RUN = "CharRun";
    public const string RUN_L = "CharRun_l";

    public const string JUMP_AN = "CharJump";
    public const string JUMP_AN_L = "CharJump_l";

    public const string PUNCH = "CharPunch";
    public const string PUNCH_L = "CharPunch_l";

    public const string KICK = "CharKick";
    public const string KICK_L = "CharKick_l";

    public const string PIKE = "CharPike";
    public const string PIKE_L = "CharPike_l";

    public const string AXE = "CharAxe";
    public const string AXE_L = "CharAxe_l";

    public const string CHAR_FALL = "CharDefeated";
    public const string CHAR_FALL_L = "CharDefeated_l";

    public const string CHAR_GET_UP = "CharGetUp";
    public const string CHAR_GET_UP_L = "CharGetUp_l";

    public const string CHAR_WINNING = "CharWon";
}
