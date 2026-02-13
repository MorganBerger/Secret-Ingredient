using UnityEngine;
using System.Linq;

public class BatChaseState: BatState
{
    private Vector2 dirToPlayer;
    private bool hasLineOfSight = false;
    private GameObject player;
    private Collider2D[] rejectedColliders;


    // Custom pathfinding-like based on platforms avoidance
    private float avoidanceDistance = 1f;
    private bool isAvoiding = false;
    private float minAvoidanceTime = 0.5f;
    private float timeSinceAvoidanceStart = 0f;
    private Vector2 avoidanceDirection;
    private Vector2 lastKnownPlayerDirection;

    public BatChaseState(Bat _bat, string _animationName)
        : base(_bat, _animationName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player = bat.targetPlayer;
        rejectedColliders = bat.GetComponentsInChildren<Collider2D>();
    }

    public override void TransitionChecks()
    {
        base.TransitionChecks();

        if (bat.targetPlayer == null)
        {
            stateMachine.ChangeState(bat.idleState);
            return;
        }

        float distToPlayer = Vector2.Distance(bat.transform.position, bat.targetPlayer.transform.position);
        if (distToPlayer <= bat.attackRange)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(bat.transform.position, (bat.targetPlayer.transform.position - bat.transform.position).normalized, bat.attackRange);
            if (hits.Any(hit => hit.collider.gameObject.layer != LayerMask.NameToLayer("Player") && !rejectedColliders.Contains(hit.collider))) return;

            stateMachine.ChangeState(bat.attackState);
            return;
        }
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (isExitingState) return;

        dirToPlayer = player.transform.position - bat.transform.position;
        if ((dirToPlayer.x > 0 && bat.transform.localScale.x < 0) || (dirToPlayer.x < 0 && bat.transform.localScale.x > 0))
        {
            bat.Flip();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        CheckLOS();
    }

    private void CheckLOS()
    {
        Vector2 directionToPlayer = (player.transform.position - bat.transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(bat.transform.position, player.transform.position);

        // If player is out of detection range, don't even check for LOS
        if (distanceToPlayer > bat.detectRange)
        {
            return;
        }

        // Check for ground layer between bat and player
        RaycastHit2D hit = Physics2D.Raycast(bat.transform.position, directionToPlayer, distanceToPlayer, LayerMask.GetMask("Ground"));

        hasLineOfSight = hit.collider == null;

        // If no LOS, try to get around the obstacle
        if (hit.collider != null)
        {
            GetAroundObstacle(directionToPlayer);
            timeSinceAvoidanceStart += Time.deltaTime;
        }
        // Else directly move to player
        else
        {
            MoveToPlayer(directionToPlayer);
        }

        Debug.DrawLine(bat.transform.position, player.transform.position, hasLineOfSight ? Color.green : isAvoiding ? Color.blue : Color.red);
    }

    /// <summary>
    /// Move directly towards the player, if the bat has LOS (line of sight) on the player
    /// </summary>
    /// <param name="directionToPlayer">Vector2 direction to the player</param>
    private void MoveToPlayer(Vector2 directionToPlayer)
    {
        // Keep avoiding for a little while even if we regained LOS, to prevent jittery movement
        if (isAvoiding && timeSinceAvoidanceStart < minAvoidanceTime)
        {
            Vector2 moveDirection = (avoidanceDirection + directionToPlayer * 0.5f).normalized;
            bat.transform.position += bat.chaseSpeed * Time.deltaTime * (Vector3)moveDirection;
            timeSinceAvoidanceStart += Time.deltaTime;
        }

        isAvoiding = false;
        timeSinceAvoidanceStart = 0f;
        bat.transform.position += bat.chaseSpeed * Time.deltaTime * (Vector3)directionToPlayer ;
    }

    /// <summary>
    /// Manage bat movement, trying to reach the player behind an obstacle.<br />
    /// * The bat will try to go around the obstacle by going up or down, depending on which way is free.<br />
    /// * If both ways are free, it will choose the one that brings it closer to the player.<br />
    /// * If both ways are blocked, it will try to back off a little bit.
    /// </summary>
    /// <param name="directionToPlayer">Vector2 direction to the player</param>
    void GetAroundObstacle(Vector2 directionToPlayer)
    {
        // Check need of avoidance or big change in player direction
        if (!isAvoiding || Vector2.Angle(lastKnownPlayerDirection, directionToPlayer) > 45f)
        {
            isAvoiding = true;
            lastKnownPlayerDirection = directionToPlayer;
            timeSinceAvoidanceStart = 0f;

            Vector2 perpendiculaire = new(-directionToPlayer.y, directionToPlayer.x);

            RaycastHit2D hitUp = Physics2D.Raycast(
                bat.transform.position,
                perpendiculaire,
                avoidanceDistance,
                LayerMask.GetMask("Ground")
            );

            RaycastHit2D hitDown = Physics2D.Raycast(
                bat.transform.position,
                -perpendiculaire,
                avoidanceDistance,
                LayerMask.GetMask("Ground")
            );

            if (hitUp.collider == null && hitDown.collider != null)
            {
                avoidanceDirection = perpendiculaire;
            }
            else if (hitDown.collider == null && hitUp.collider != null)
            {
                avoidanceDirection = -perpendiculaire;
            }
            else if (hitUp.collider == null && hitDown.collider == null)
            {
                Vector2 playerPos = player.transform.position;
                Vector2 posUp = (Vector2)player.transform.position + perpendiculaire;
                Vector2 posDown = (Vector2)player.transform.position - perpendiculaire;

                if (Vector2.Distance(posUp, playerPos) < Vector2.Distance(posDown, playerPos))
                {
                    avoidanceDirection = perpendiculaire;
                }
                else
                {
                    avoidanceDirection = -perpendiculaire;
                }
            }
            else
            {
                avoidanceDirection = -directionToPlayer;
            }
        }

        // Move in avoidance direction, with a little influence of the player
        // direction to prevent going the wrong way when the player is moving
        // Reduce player influence if we just started avoiding
        float playerInfluence = timeSinceAvoidanceStart > minAvoidanceTime ? 0.5f : 0.2f;
        Vector2 moveDirection = (avoidanceDirection + directionToPlayer * playerInfluence).normalized;
        bat.transform.position += (Vector3)moveDirection * bat.chaseSpeed * Time.deltaTime;

        // Debug
        Debug.DrawRay(bat.transform.position, avoidanceDirection * 0.5f, Color.yellow);
        Debug.DrawRay(bat.transform.position, moveDirection * 0.5f, Color.cyan);
    }
}