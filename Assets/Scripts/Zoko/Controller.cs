using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rig;
    public Transform zokoBody;
    public BoxCollider2D upperCollider;
    ZokoAudio zokoAudio;
    bool dead = false;
    bool finished = false;
    bool canFlip = true;
    bool canRun = true;
    bool canJump = true;
    bool canMoveAir = true;
    bool canWall = false;
    public bool canPerformJump = true;
    public bool canPerformSlopeJump = true;
    bool canSlide = true;
    bool canPerformSlide = true;
    bool sliding = false;
    bool slidingSlope = false;
    bool performSlidingSlope = false;
    bool checkWallStick = false;
    public float horzSpeed = 5f;
    public float verSpeed = 5f;
    public float airSpeed = 5f;
    public float slideSpeed = 5f;
    public float wallFriction = 0.3f;
    public float maxSlopeAngle;
    float slopeAngle;
    public LayerMask groundMask;
    public LayerMask wallMask;
    public Transform gCL;
    public Transform gCC;
    public Transform gCR;
    public Transform wallCheckerUp;
    public Transform wallCheckerDown;
    bool grounded = false;
    float rigXVelo = 0f;
    float rigYVelo = 0f;
    float facing = 1;
    float xInput;
    bool jumpInput = false;
    bool slideInput = false;
    float jumpTime = 0.3f;
    float jumpCountTime = 0f;
    float slopeJumpTime = 1f;
    float slopeJumpCountTime = 0;
    float jumpLowFric = 1f;
    float jumpHighFric = 1.5f;
    ZokoVFX vfx;
    bool playedSmoke = false;
    bool slopeDetected = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        vfx = GetComponent<ZokoVFX>();
        zokoAudio = GetComponent<ZokoAudio>();

        transform.position = GameManager.Instance.getCheckPoint();
    }

    private void Update()
    {
        xInput = ArteemInputs.Instance.horizontal;
        jumpInput = ArteemInputs.Instance.jumpInput;
        slideInput = ArteemInputs.Instance.slideInput;

        checkFacing();

        anim.SetBool("Grounded", grounded);
        anim.SetFloat("HorzSpeed", Mathf.Abs(rigXVelo));
        anim.SetFloat("VerSpeed", rigYVelo);
        anim.SetBool("Slide", sliding);
        anim.SetBool("Slope", slidingSlope);
        anim.SetBool("Wall", canWall);

        ZokoAnimManager.Instance.setAnimSpeed(Mathf.Abs(rigXVelo), rigYVelo);
        
    }

    private void FixedUpdate()
    {
        checkGround();

        rigYVelo = rig.velocity.y;
        rigXVelo = rig.velocity.x;

        if(!dead && !finished)
        {
            if (canFlip)
            {
                if (rigXVelo > 0.1f && facing == -1)
                {
                    flip();
                }
                if (rigXVelo < -0.1f && facing == 1)
                {
                    flip();
                }
            }

            movement();

        }
        else
        {
            canFlip = false;
            canJump = false;
            canMoveAir = false;
            canRun = false;
            canSlide = false;

            rig.velocity = new Vector2(0, rigYVelo);
        }

        Debug.DrawRay(wallCheckerUp.position, Vector2.right * 0.04f, Color.green);
        Debug.DrawRay(wallCheckerDown.position, Vector2.right * 0.04f, Color.green);
    }

    void checkGround()
    {
        bool groundedL = Physics2D.Raycast(gCL.position, Vector2.down, 0.2f, groundMask);
        bool groundedC = Physics2D.Raycast(gCC.position, Vector2.down, 0.2f, groundMask);
        bool groundedR = Physics2D.Raycast(gCR.position, Vector2.down, 0.2f, groundMask);

        if(groundedL || groundedC || groundedR)
        {
            grounded = true;
            canWall = false;
            canMoveAir = true;

            checkSlope();
 

            if (!performSlidingSlope)
            {
                rig.gravityScale = jumpLowFric;
                rig.drag = 0;
            }

            if (!canPerformJump)
            {
                if(jumpCountTime < jumpTime)
                {
                    jumpCountTime += Time.deltaTime;
                }
                else
                {
                    canPerformJump = true;
                }
            }

            if (!canPerformSlopeJump)
            {
                if(slopeJumpCountTime < slopeJumpTime)
                {
                    slopeJumpCountTime += Time.deltaTime;
                }
                else
                {
                    canPerformSlopeJump = true;
                }
            }

            if (!playedSmoke)
            {
                vfx.createVFX(0);//smoke
                playedSmoke = true;
            }

            RaycastHit2D hit = Physics2D.Raycast(gCC.position, Vector2.down, 0.2f, groundMask);

            if(hit.collider != null)
            {
                if (hit.collider.CompareTag("Elevator"))
                {
                    rig.velocity = new Vector2(xInput * horzSpeed, -2);
                }
            }

            ZokoAnimManager.Instance.setGround(true);
            vfx.stopVFX(2);

            if(!sliding || !slidingSlope)
            {
                vfx.stopVFX(1);
            }
        }
        else
        {
            grounded = false;

            
            //zokoBody.localEulerAngles = new Vector3(0, 0, 0);

            checkWall();

            if (performSlidingSlope)
            {
                StartCoroutine(resetSlideSlope());
            }
            else
            {
                upperCollider.enabled = true;
            }

            playedSmoke = false;

            ZokoAnimManager.Instance.setGround(false);
        }
    }

    void checkSlope()
    {
        RaycastHit2D hit = Physics2D.Raycast(gCC.position, Vector2.down, 0.5f, groundMask);
        Debug.DrawRay(gCC.position, Vector2.down * 0.5f, Color.red);

        if(hit.collider != null)
        {
            slopeAngle = Mathf.Abs(Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg);

            if(slopeAngle > maxSlopeAngle)
            {
                slopeDetected = true;
                upperCollider.enabled = false;
                performSlidingSlope = true;
                slidingSlope = true;
                canRun = false;
                canSlide = false;
                canJump = false;
                rig.drag = 2;
                //zokoBody.localEulerAngles = new Vector3(0, 0, -slopeAngle);
                //slopeDetected = false;
                if (canPerformSlopeJump)
                {
                    if (jumpInput)
                    {
                        slopeDetected = false;
                        rig.velocity = new Vector2(0, 0);
                        rig.velocity = new Vector2(facing * 10, 10);
                        slopeJumpCountTime = 0;
                        canPerformSlopeJump = false;
                    }
                }

                vfx.createVFX(1);

            }
            else
            {
                //zokoBody.localEulerAngles = new Vector3(0, 0, 0);

                if (performSlidingSlope)
                {
                    vfx.stopVFX(1);
                    StartCoroutine(resetSlideSlope());
                }
            }
        }
        else
        {
            //zokoBody.localEulerAngles = new Vector3(0, 0, 0);
            vfx.stopVFX(1);
        }
    }

    void checkWall()
    {
        bool wallUp = Physics2D.Raycast(wallCheckerUp.position, facing * Vector2.right, 0.04f, wallMask);
        bool wallDown = Physics2D.Raycast(wallCheckerDown.position, facing * Vector2.right, 0.04f, wallMask);

        if(wallUp && wallDown)
        {
            canWall = true;
        }
        else
        {
            canWall = false;
            checkWallStick = false;
        }
    }

    void checkFacing()
    {
        if(transform.eulerAngles.y < 180)
        {
            facing = 1;
        }
        else
        {
            facing = -1;
        }
    }

    void flip()
    {
        if(facing == 1)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    void movement()
    {
        if (grounded)
        {
            if (!slopeDetected)
            {
                if (canRun)
                {
                    if (!sliding && !performSlidingSlope)
                    {
                        rig.velocity = new Vector2(xInput * horzSpeed, rigYVelo);
                    }
                }
                if (canJump && !performSlidingSlope)
                {
                    if (canPerformJump)
                    {
                        if (jumpInput)
                        {
                            rig.velocity = new Vector2(rigXVelo, verSpeed);
                            jumpCountTime = 0;
                            //zokoAudio.playAudOnce(9);//jump sfx
                            canPerformJump = false;
                        }
                    }
                }
                if (canSlide)
                {
                    if (canPerformSlide)
                    {
                        if (slideInput)
                        {
                            upperCollider.enabled = false;
                            rig.velocity = new Vector2(facing * slideSpeed, rigYVelo);
                            sliding = true;
                            canPerformSlide = false;
                            vfx.createVFX(1);
                            StartCoroutine(endSlide());
                        }
                    }
                }
            }
            else
            {
                rig.velocity = new Vector2(rigXVelo, -2);
                vfx.stopVFX(1);
            }
            
        }
        else
        {
            if (!slopeDetected)
            {
                if (canMoveAir && !performSlidingSlope)
                {
                    rig.velocity = new Vector2(xInput * airSpeed, rigYVelo);
                }

                if (!performSlidingSlope && !canWall)
                {
                    if (jumpInput)
                    {
                        if (rigYVelo > 0)
                        {
                            rig.gravityScale = jumpLowFric;
                        }
                        else
                        {
                            rig.gravityScale = jumpHighFric;
                        }
                    }
                    else
                    {
                        rig.gravityScale = jumpHighFric;
                    }
                }

                if (rigYVelo < 0)
                {
                    if (canWall)
                    {
                        checkWallStick = true;
                    }
                }

                if (checkWallStick)
                {
                    wallMovement();

                }
                else
                {
                    rig.drag = 0;
                    vfx.stopVFX(2);
                }
            }
            else
            {
                rig.velocity = new Vector2(rigXVelo, -2);
            }
        }
    }

    void wallMovement()
    {
        canMoveAir = false;

        if (jumpInput)
        {
            rig.gravityScale = 0;

            if (facing == 1 && xInput < 0)
            {
                rig.velocity = new Vector2(-facing * 10, 10);
            }
            if (facing == -1 && xInput > 0)
            {
                rig.velocity = new Vector2(-facing * 10, 10);
            }

            vfx.stopVFX(2);
            //zokoAudio.playAudLoop(9);//jump
        }
        else
        {

            rig.gravityScale = 1;
            rig.drag = 20;

            if (facing == 1 && xInput < 0)
            {
                rig.velocity = new Vector2(-facing * 5, 5);
                //zokoAudio.playAudOnce(9);//jump
            }
            if (facing == -1 && xInput > 0)
            {
                rig.velocity = new Vector2(-facing * 5, 5);
                //zokoAudio.playAudOnce(9);//jump
            }

            vfx.createVFX(2);
            zokoAudio.playAudLoop(3);
        }

    }

    IEnumerator endSlide()
    {
        yield return new WaitForSeconds(0.5f);
        sliding = false;
        vfx.stopVFX(1);
        StartCoroutine(resetSlide());
    }

    IEnumerator resetSlide()
    {
        yield return new WaitForSeconds(0.5f);
        upperCollider.enabled = true;
        canPerformSlide = true;
    }

    IEnumerator resetSlideSlope()
    {
        yield return new WaitForSeconds(0.3f);
        performSlidingSlope = false;
        slidingSlope = false;
        canFlip = true;
        canJump = true;
        canRun = true;
        canSlide = true;
        upperCollider.enabled = true;
        slopeDetected = false;

    }

    public void setFinished()
    {
        StartCoroutine(doFinished());
    }

    IEnumerator doFinished()
    {
        yield return new WaitForSeconds(1);
        finished = true;
    }

}
