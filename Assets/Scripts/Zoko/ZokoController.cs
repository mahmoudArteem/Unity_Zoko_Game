using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZokoController : MonoBehaviour
{

    Animator anim;
    Rigidbody2D rig;
    public int runSpeed = 5;
    public int jumpSpeed = 5;
    public float airSpeed = 5f;
    float jumpLowFric = 1f;
    float jumpHighFric = 1.5f;
    Vector2 rigVelo;
    int facing = 1;
    bool grounded = false;

    public Transform zokoBody;
    public LayerMask groundMask;
    public LayerMask wallMask;
    public Transform gCL;
    public Transform gCC;
    public Transform gCR;

    public Transform wallCheckerUp;
    public Transform wallCheckerDown;
    public bool canWall = false;
    public bool checkWallStick = false;


    float xInput = 0;
    bool jumpInput = false;
    bool slideInput = false;

    float jumpTime = 0.4f;
    float jumpCount = 0;
    bool jumping = false;

    public bool sliding = false;
    bool slopeSliding = false;

    public bool canFlip = true;
    bool canMove = true;
    bool canMoveAir = true;
    public bool canRun = true;
    bool canJump = true;
    bool canDoJump = true;
    bool canSlide = true;
    bool canDoSlide = true;

    public float slopeAngle = 0;
    public float slopeAngleB = 0;
    float maxSlopeAngle = 359f;
    bool calledResetSlope = false;
    bool calledSetSlope = false;

    bool playedSmoke = false;

    //public bool water = false;
    //bool doingWaterJump = false;

    public CapsuleCollider2D topCollider;
    //public CapsuleCollider2D downCollider;
    //public CapsuleCollider2D swimCollider;

    public PhysicsMaterial2D lowFric;
    public PhysicsMaterial2D highFric;

    ZokoVFX vfx;
    ZokoAudio zokoAudio;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        vfx = GetComponent<ZokoVFX>();
        zokoAudio = GetComponent<ZokoAudio>();

        transform.position = GameManager.Instance.getCheckPoint();
    }

    private void Start()
    {
        //downCollider.isTrigger = true;
        //swimCollider.isTrigger = true;
        //topCollider.isTrigger = false;
        vfx.stopVFX(0);
        vfx.stopVFX(1);
        vfx.stopVFX(2);
        ArteemInputs.Instance.AxisChanged(1);
    }

    private void Update()
    {
        xInput = ArteemInputs.Instance.horizontal;
        jumpInput = ArteemInputs.Instance.jumpInput;
        slideInput = ArteemInputs.Instance.slideInput;

        checkFacing();

        anim.SetBool("Grounded", grounded);
        anim.SetFloat("HorzSpeed", Mathf.Abs(rigVelo.x));
        anim.SetFloat("VerSpeed", rigVelo.y);
        anim.SetBool("Slide", sliding);
        anim.SetBool("Slope", slopeSliding);
        anim.SetBool("Wall", canWall);
        //anim.SetBool("Water", water);

        ZokoAnimManager.Instance.setAnimSpeed(Mathf.Abs(rigVelo.x), rigVelo.y);

    }

    private void FixedUpdate()
    {
        rigVelo = new Vector2(rig.velocity.x, rig.velocity.y);

        checkGround();

        if (canMove)
        {
            if (grounded)
            {
                movement();
            }
            else
            {
                /*if (!water)
                {
                    airMovement();
                }
                else
                {
                    waterMovement();
                }*/

                airMovement();
            }
        }
        else
        {
            rig.velocity = new Vector2(0, rigVelo.y);
        }
    }

    private void LateUpdate()
    {

        if (canFlip)
        {
            if (facing == -1 && xInput > 0.1f)
            {
                flip();
            }
            if (facing == 1 && xInput < -0.1f)
            {
                flip();
            }
        }

        //zokoBody.localEulerAngles = new Vector3(0, 0, -facing * slopeAngleB);
    }

    void checkGround()
    {

        bool groundedL = Physics2D.Raycast(gCL.position, Vector2.down, 0.15f, groundMask);
        bool groundedC = Physics2D.Raycast(gCC.position, Vector2.down, 0.15f, groundMask);
        bool groundedR = Physics2D.Raycast(gCR.position, Vector2.down, 0.15f, groundMask);

        if (groundedL || groundedC || groundedR)
        {
            grounded = true;
            canWall = false;
            if (!slopeSliding)
            {
                canMoveAir = true;
            }


            if (!playedSmoke)
            {
                vfx.createVFX(0);//smoke
                playedSmoke = true;
            }
        }
        else
        {
            grounded = false;
            playedSmoke = false;
        }
    }

    void checkFacing()
    {
        if (transform.eulerAngles.y < 180)
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
        if (facing == 1)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    void checkSlope()
    {
        RaycastHit2D hit = Physics2D.Raycast(gCC.position, Vector2.down, 0.3f, groundMask);

        if (hit.collider != null)
        {
            slopeAngleB = Mathf.Atan2(hit.normal.x, hit.normal.y) * Mathf.Rad2Deg;
            slopeAngle = Mathf.Abs(slopeAngleB);

            if (slopeAngle >= maxSlopeAngle)
            {
                if (!calledSetSlope)
                {
                    setSlope();
                }
            }
            else
            {
                if (slopeSliding)
                {
                    if (!calledResetSlope)
                    {
                        StartCoroutine(resetSlope());
                    }
                }

                if (slopeAngle > 1 && slopeAngle < maxSlopeAngle)
                {
                    if (!jumping)
                    {
                        if (sliding)
                        {
                            rig.drag = 0;
                            if (slopeAngleB < 0)
                            {
                                rig.velocity = new Vector2(Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * facing * runSpeed, Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * facing * runSpeed);
                            }
                            else
                            {
                                if (facing == -1)
                                {
                                    rig.velocity = new Vector2(Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * facing * runSpeed, Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * facing * runSpeed);
                                }
                                else
                                {
                                    rig.velocity = new Vector2(facing * runSpeed, -8);
                                }
                            }
                        }
                        else
                        {
                            if (xInput != 0 && !sliding)
                            {
                                //rig.drag = 0;
                                rig.sharedMaterial = lowFric;
                                rig.velocity = new Vector2(Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * xInput * runSpeed, Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * xInput * runSpeed);
                            }
                            else
                            {
                                //rig.drag = 2000;
                                rig.sharedMaterial = highFric;
                            }
                        }
                    }
                }
                else
                {
                    rig.drag = 0;
                    rig.sharedMaterial = lowFric;
                }
            }
        }
        else
        {
            if (slopeSliding)
            {
                if (!calledResetSlope)
                {
                    StartCoroutine(resetSlope());
                }
            }
        }
    }
    void movement()
    {
        checkSlope();

        if (!canDoJump)
        {
            if (jumpCount < jumpTime)
            {
                jumpCount += Time.deltaTime;
            }
            else
            {
                jumping = false;
                canDoJump = true;
            }
        }

        if (canRun)
        {
            rig.velocity = new Vector2(xInput * runSpeed, rigVelo.y);
        }

        rig.gravityScale = jumpLowFric;

        if (canJump)
        {
            if (canDoJump)
            {
                if (jumpInput)
                {
                    rig.drag = 0;
                    jumping = true;
                    rig.velocity = (new Vector2(rigVelo.x, jumpSpeed));
                    jumpCount = 0;
                    canDoJump = false;
                }
            }
        }

        if (canSlide)
        {
            if (canDoSlide)
            {
                if (slideInput)
                {
                    canRun = false;
                    canFlip = false;
                    sliding = true;
                    //downCollider.isTrigger = false;
                    //topCollider.isTrigger = true;
                    if (slopeAngle <= 1)
                    {
                        rig.velocity = new Vector2(runSpeed * facing, rigVelo.y);
                    }
                    vfx.createVFX(1);
                    StartCoroutine(resetSliding());
                    canDoSlide = false;
                }
            }
        }

    }

    void airMovement()
    {
        slopeAngle = 0;
        slopeAngleB = 0;

        if (canMoveAir)
        {
            rig.velocity = new Vector2(xInput * airSpeed, rigVelo.y);
        }

        if (jumpInput)
        {
            if (rigVelo.y > 0)
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

        if (slopeSliding)
        {
            calledResetSlope = false;
            calledSetSlope = false;
            canRun = true;
            canSlide = true;
            canMoveAir = true;
            rig.drag = 0;
            canFlip = true;
            slopeSliding = false;

            StartCoroutine(resetSlope());
        }

        checkWall();

        if (rigVelo.y < 0)
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

    IEnumerator resetSliding()
    {
        yield return new WaitForSeconds(1f);
        sliding = false;
        canRun = true;
        if (!slopeSliding)
        {
            canFlip = true;
        }

        //downCollider.isTrigger = true;
        //topCollider.isTrigger = false;
        vfx.stopVFX(1);
        canDoSlide = true;
    }

    void setSlope()
    {
        slopeSliding = true;
        canFlip = false;
        canMoveAir = false;
        canRun = false;
        canMoveAir = false;
        canSlide = false;
        rig.drag = 0;
        calledResetSlope = false;

        if (facing == 1 && slopeAngleB < -1)
        {
            flip();
        }
        if (facing == -1 && slopeAngleB > 1)
        {
            flip();
        }

        //downCollider.isTrigger = false;
        //topCollider.isTrigger = true;

        rig.velocity = new Vector2(facing * runSpeed, -8);
        vfx.createVFX(1);
        calledSetSlope = true;
    }

    IEnumerator resetSlope()
    {
        calledResetSlope = true;

        yield return new WaitForSeconds(0.5f);

        slopeSliding = false;
        canRun = true;
        canSlide = true;
        canMoveAir = true;
        rig.drag = 0;
        canFlip = true;
        //downCollider.isTrigger = true;
        //topCollider.isTrigger = false;
        calledSetSlope = false;
        vfx.stopVFX(1);
    }

    void checkWall()
    {
        bool wallUp = Physics2D.Raycast(wallCheckerUp.position, facing * Vector2.right, 0.04f, wallMask);
        bool wallDown = Physics2D.Raycast(wallCheckerDown.position, facing * Vector2.right, 0.04f, wallMask);

        if (wallUp && wallDown)
        {
            canWall = true;
        }
        else
        {
            canWall = false;
            checkWallStick = false;
        }
    }

    void wallMovement()
    {
        canMoveAir = false;
        canFlip = false;

        if (jumpInput)
        {
            rig.gravityScale = 0;

            if (facing == 1 && xInput < 0)
            {
                rig.velocity = new Vector2(-facing * 6, 6);
                canFlip = true;
            }
            if (facing == -1 && xInput > 0)
            {
                rig.velocity = new Vector2(-facing * 6, 6);
                canFlip = true;
            }

            vfx.stopVFX(2);
        }
        else
        {

            rig.gravityScale = 1;
            rig.drag = 20;

            if (facing == 1 && xInput < 0)
            {
                rig.velocity = new Vector2(-facing * 4, 4);
                canFlip = true;
            }
            if (facing == -1 && xInput > 0)
            {
                rig.velocity = new Vector2(-facing * 4, 4);
                canFlip = true;
            }

            vfx.createVFX(2);
            zokoAudio.playAudLoop(3);
        }

    }

    /*public void setWater(bool b)
    {
        water = b;

        if (b == true)
        {
            swimCollider.isTrigger = false;
            topCollider.isTrigger = true;
            downCollider.isTrigger = true;
            ArteemInputs.Instance.AxisChanged(0);
        }
        else
        {
            swimCollider.isTrigger = true;
            topCollider.isTrigger = false;
            downCollider.isTrigger = true;
            ArteemInputs.Instance.AxisChanged(1);
        }
    }

    void waterMovement()
    {
        float yInput = ArteemInputs.Instance.vertical;

        bool waterJump = ArteemInputs.Instance.jumpInput;
        rig.velocity = new Vector2(xInput * 2, yInput * 2);

        if (waterJump)
        {
            if (!doingWaterJump)
            {
                StartCoroutine(resetWaterJump());
            }
        }
        if (doingWaterJump)
        {
            rig.gravityScale = 1.5f;
            rig.drag = 1;
            rig.velocity = new Vector2(rigVelo.x, 5);
        }
        else
        {
            if (yInput == 0 && xInput == 0)
            {
                rig.drag = 10;
                rig.gravityScale = 1.5f;
                rig.velocity = new Vector2(rigVelo.x, rigVelo.y);
            }
            else
            {
                rig.drag = 0;
                rig.gravityScale = 3;
            }
        }

        anim.SetFloat("WaterVert", yInput * 2);
        anim.SetFloat("WaterHorz", Mathf.Abs(xInput * 2));
    }

    IEnumerator resetWaterJump()
    {
        doingWaterJump = true;
        yield return new WaitForSeconds(1);
        doingWaterJump = false;
    }*/

    public void setFinished()
    {
        StartCoroutine(doFinished());
    }

    IEnumerator doFinished()
    {
        yield return new WaitForSeconds(1);
        canMove = false;
    }
}
