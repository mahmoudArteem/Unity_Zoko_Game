using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pc2 : MonoBehaviour
{

    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float groundCheckRadius;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    private float maxSlopeAngle;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFriction;

    private float xInput;
    public float slopeDownAngle;
    public float slopeSideAngle;
    public float lastSlopeAngle;

    private int facingDirection = 1;

    public bool isGrounded;
    public bool isOnSlope;
    public bool isJumping;
    public bool canWalkOnSlope;
    private bool canJump;

    private Vector2 newVelocity;
    private Vector2 newForce;

    private Vector2 slopeNormalPerp;

    private Rigidbody2D rb;
    public CapsuleCollider2D mainCollider;
    public CircleCollider2D slideCollider;
    private Animator anim;

    //mine

    private Vector2 rigVelo;
    public Transform centerpos;
    public Transform horzPos;
    public float ang = 0;
    private bool canFlip = true;
    private bool slopeSliding = false;
    public Transform zokoBody;
    public bool canMoveAir = true;
    public bool canSlopeJump = false;
    private bool slideInput = false;
    private bool isSliding = false;
    public float ang2 = 0;
    private bool canSlide = false;
    public Transform wallCheckerUp;
    public Transform wallCheckerDown;
    public LayerMask wallMask;
    public bool canWall;
    public bool checkWallStick;
    private bool jumpInput;
    public float airSpeed = 5;
    public float jumpHighFric = 1.5f;
    public float jumpLowFric = 1f;
    private bool jumping;
    private bool canDoJump;
    private float jumpCount;
    public float jumpTime = 0.4f;
    private bool canMove = true;
    private bool calledSlopeJump = false;
    private float sjTime = 0.5f;
    private float sjtimeCount = 0;

    ZokoVFX vfx;
    ZokoAudio zokoAudio;
    private bool playedSmoke = false;

    private void Awake()
    {
        vfx = GetComponent<ZokoVFX>();
        zokoAudio = GetComponent<ZokoAudio>();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        mainCollider.isTrigger = false;
        slideCollider.isTrigger = true;

        vfx.stopVFX(0);
        vfx.stopVFX(1);
        vfx.stopVFX(2);
    }

    private void Update()
    {
        CheckInput();
        applyAnimations();
    }

    private void FixedUpdate()
    {
        rigVelo = rb.velocity;

        ApplyMovement();
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        slideInput = Input.GetKeyDown(KeyCode.S);
        jumpInput = Input.GetButton("Jump");

        if (canFlip)
        {
            if (xInput == 1 && facingDirection == -1)
            {
                Flip();
            }
            else if (xInput == -1 && facingDirection == 1)
            {
                Flip();
            }
        }

    }
    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (rb.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if (isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
        {
            canJump = true;
            canSlide = true;

            if (!playedSmoke)
            {
                vfx.createVFX(0);//smoke
                playedSmoke = true;
            }
        }

        if (!isGrounded)
        {
            canSlide = false;
            playedSmoke = false;
        }

    }

    private void SlopeCheck()
    {
        Vector2 checkPos = centerpos.position;

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(horzPos.position, transform.right, 0.1f, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(horzPos.position, -transform.right, 0.1f, whatIsGround);

        if (slopeHitFront)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

        }
        else if (slopeHitBack)
        {
            isOnSlope = true;

            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit)
        {

            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != lastSlopeAngle)
            {
                isOnSlope = true;
            }

            lastSlopeAngle = slopeDownAngle;


            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);

        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;

            ang = highSlopeAngle(hit.normal, Vector2.up);

            forcedSlopeSlide(ang);
        }
        else
        {
            canWalkOnSlope = true;

            if (slopeSliding)
            {

                resetSlopeSliding();
            }

        }

        if (isOnSlope && canWalkOnSlope && xInput == 0.0f && !isSliding)
        {
            rb.sharedMaterial = fullFriction;
        }
        else
        {
            rb.sharedMaterial = noFriction;
        }
    }

    void movement()
    {
        if (!isOnSlope && !isJumping && !isSliding) //if not on slope
        {
            newVelocity.Set(movementSpeed * xInput, 0.0f);
            rb.velocity = newVelocity;

            if (!canMoveAir)
            {
                canFlip = true;
                canMoveAir = true;
            }
        }
        else if (isOnSlope && canWalkOnSlope && !isJumping && !isSliding) //If on slope
        {
            newVelocity.Set(movementSpeed * slopeNormalPerp.x * -xInput, movementSpeed * slopeNormalPerp.y * -xInput);
            rb.velocity = newVelocity;

            if (!canMoveAir)
            {
                canFlip = true;
                canMoveAir = true;
            }
        }

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

        rb.gravityScale = jumpLowFric;

        if (canJump && !slopeSliding)
        {
            if (canDoJump)
            {
                if (jumpInput)
                {
                    rb.drag = 0;
                    jumping = true;
                    newVelocity.Set(rigVelo.x, jumpForce);
                    rb.velocity = newVelocity;
                    jumpCount = 0;
                    canDoJump = false;
                }
            }
        }

        if (slideInput && canSlide)
        {
            slide();
        }

        if (canSlopeJump)
        {
            if (!calledSlopeJump)
            {
                if (sjtimeCount < sjTime)
                {
                    sjtimeCount += Time.deltaTime;
                }
                else
                {
                    calledSlopeJump = true;
                }
            }
        }

        if (calledSlopeJump)
        {
            if (jumpInput)
            {
                slopeJump();
                calledSlopeJump = false;
            }
        }

    }
    void airMovement()
    {

        if (canMoveAir)
        {
            newVelocity.Set(xInput * airSpeed, rigVelo.y);
            rb.velocity = newVelocity;
        }

        if (jumpInput)
        {
            if (rigVelo.y > 0)
            {
                rb.gravityScale = jumpLowFric;
            }
            else
            {
                rb.gravityScale = jumpHighFric;
            }
        }
        else
        {
            rb.gravityScale = jumpHighFric;
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
            rb.drag = 0;
            vfx.stopVFX(2);
        }

        vfx.stopVFX(1);
    }

    private void ApplyMovement()
    {

        CheckGround();

        if (canMove)
        {
            if (isGrounded)
            {
                SlopeCheck();
                movement();

                if (isSliding)
                {
                    normalSlopeSliding();
                }
            }
            else
            {
                isOnSlope = false;
                slideCollider.isTrigger = true;
                mainCollider.isTrigger = false;
                airMovement();
            }
        }
        else
        {
            newVelocity.Set(0, rigVelo.y);
            rb.velocity = newVelocity;
        }

    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void applyAnimations()
    {
        anim.SetBool("Grounded", isGrounded);
        anim.SetFloat("HorzSpeed", Mathf.Abs(rigVelo.x));
        anim.SetFloat("VerSpeed", rigVelo.y);
        anim.SetBool("Slope", slopeSliding);
        anim.SetBool("Slide", isSliding);
        anim.SetBool("Wall", canWall);
    }

    private void forcedSlopeSlide(float angle)
    {
        slopeSliding = true;
        canFlip = false;
        canMoveAir = false;
        canSlopeJump = true;

        slideCollider.isTrigger = false;
        mainCollider.isTrigger = true;

        newVelocity.Set(5 * slopeNormalPerp.x * -facingDirection, 5 * slopeNormalPerp.y * -facingDirection);
        rb.velocity = newVelocity;

        if (facingDirection == -1 && angle < 0)
        {
            Flip();
        }

        else if (facingDirection == 1 && angle > 0)
        {
            Flip();
        }

        if (angle < 0)
        {
            zokoBody.localEulerAngles = new Vector3(0, 0, -facingDirection * -angle);
        }
        else
        {
            zokoBody.localEulerAngles = new Vector3(0, 0, facingDirection * angle);
        }
    }

    private void resetSlopeSliding()
    {
        ang = 0;
        zokoBody.localEulerAngles = new Vector3(0, 0, 0);

        slideCollider.isTrigger = true;
        mainCollider.isTrigger = false;

        if (!canWall)
        {
            canFlip = true;
        }
        canSlopeJump = false;
        canMoveAir = true;
        slopeSliding = false;
    }

    private float highSlopeAngle(Vector2 a, Vector2 b)
    {
        var an = a.normalized;
        var bn = b.normalized;
        var x = an.x * bn.x + an.y * bn.y;
        var y = an.y * bn.x - an.x * bn.y;
        return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
    }

    private void slopeJump()
    {
        canDoJump = false;

        slideCollider.isTrigger = true;
        mainCollider.isTrigger = false;

        newVelocity.Set(facingDirection * 5, 7);
        rb.velocity = newVelocity;
        zokoBody.localEulerAngles = new Vector3(0, 0, 0);
        sjtimeCount = 0;
    }


    private void slide()
    {
        if (!isSliding && !slopeSliding)
        {
            isSliding = true;
            canFlip = false;
            canJump = false;

            slideCollider.isTrigger = false;
            mainCollider.isTrigger = true;

            if (!isOnSlope)
            {
                newVelocity.Set(7 * facingDirection, 0.0f);
                rb.velocity = newVelocity;
            }

            vfx.createVFX(1);

            StartCoroutine(resetSlide());
        }
    }

    IEnumerator resetSlide()
    {
        isSliding = true;

        yield return new WaitForSeconds(1f);

        if (!slopeSliding)
        {
            if (!canWall)
            {
                canFlip = true;
            }
            canJump = true;

            slideCollider.isTrigger = true;
            mainCollider.isTrigger = false;

            newVelocity.Set(0.0f, 0.0f);
            rb.velocity = newVelocity;
        }

        ang2 = 0;
        zokoBody.localEulerAngles = new Vector3(0, 0, 0);

        vfx.stopVFX(1);

        isSliding = false;
    }

    private void normalSlopeSliding()
    {
        RaycastHit2D hit = Physics2D.Raycast(centerpos.position, Vector2.down, slopeCheckDistance, whatIsGround);

        ang2 = highSlopeAngle(hit.normal, Vector2.up);

        if (isOnSlope && Mathf.Abs(ang2) < maxSlopeAngle)
        {

            if ((ang2 > 0 && facingDirection > 0) || (ang2 < 0 && facingDirection < 0))
            {
                newVelocity.Set(7 * slopeNormalPerp.x * -facingDirection, 7 * slopeNormalPerp.y * -facingDirection);
                rb.velocity = newVelocity;
            }

            else if ((ang2 < 0 && facingDirection > 0) || (ang2 > 0 && facingDirection < 0))
            {
                newVelocity.Set(5 * slopeNormalPerp.x * -facingDirection, 5 * slopeNormalPerp.y * -facingDirection);
                rb.velocity = newVelocity;
            }

            if (ang2 < 0)
            {
                zokoBody.localEulerAngles = new Vector3(0, 0, -facingDirection * -ang2);
            }
            else
            {
                zokoBody.localEulerAngles = new Vector3(0, 0, facingDirection * ang2);
            }
        }
    }

    void checkWall()
    {
        bool wallUp = Physics2D.Raycast(wallCheckerUp.position, facingDirection * Vector2.right, 0.04f, wallMask);
        bool wallDown = Physics2D.Raycast(wallCheckerDown.position, facingDirection * Vector2.right, 0.04f, wallMask);

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
            rb.gravityScale = 0;

            if (facingDirection == 1 && xInput < 0)
            {
                newVelocity.Set(-facingDirection * 6, 8);
                rb.velocity = newVelocity;
                canFlip = true;
            }
            if (facingDirection == -1 && xInput > 0)
            {
                newVelocity.Set(-facingDirection * 6, 8);
                rb.velocity = newVelocity;
                canFlip = true;
            }

            vfx.stopVFX(2);
        }
        else
        {

            rb.gravityScale = 1;
            rb.drag = 20;

            if (facingDirection == 1 && xInput < 0)
            {
                newVelocity.Set(-facingDirection * 4, 4);
                rb.velocity = newVelocity;
                canFlip = true;
            }
            if (facingDirection == -1 && xInput > 0)
            {
                newVelocity.Set(-facingDirection * 4, 4);
                rb.velocity = newVelocity;
                canFlip = true;
            }

            vfx.createVFX(2);
            //zokoAudio.playAudLoop(3);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

}

