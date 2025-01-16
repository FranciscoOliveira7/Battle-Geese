using System;
using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public enum TargetType
{
    moving,
    fix,
    custom
}

public class Unit : MonoBehaviour
{
    const float minPathUpdateTime = 0.3f;
    
    public event Action OnPathFailed;
    
    public bool isStopped = false;
    public Transform movingTarget;
    public float speed = 10;
    private float turnDst = 0;
    public float turnSpeed = 3;
    public float stoppingDst = 1f;
    public bool isDecelerating = true;
    public float acceleration = 3f;
    
    NodePath _path;

    [HideInInspector] public Rigidbody rb;

    [HideInInspector] public TargetType targetType = TargetType.moving;
    private bool isUsingFixedTarget;
    private Vector3 _target;
    private Vector3 _lastTargetPosition;
    private Vector3 moveDirection;
    
    public Func<Vector3> targetFunc;

    public bool disabled;

    [ContextMenu("Test Set Target")]
    private void TestSetTarget()
    {
        SetTarget(MainManager.Instance.ClosestPlayer(transform.position).transform);
    }
    
    public void SetCustomPosition(Func<Vector3> target)
    {
        if (disabled) return;
        StopCoroutine(nameof(FollowPath));
        targetType = TargetType.custom;
        targetFunc = target;
        _target = targetFunc();
        
        PathRequestManager.RequestPath(new PathRequest(transform.position, _target, OnPathFound, this));
    }
    
    public void UpdateCustomPosition()
    {
        if (disabled) return;
        StopCoroutine(nameof(FollowPath));
        targetType = TargetType.custom;
        _target = targetFunc();
        
        PathRequestManager.RequestPath(new PathRequest(transform.position, _target, OnPathFound, this));
    }
    
    public void SetTarget(Vector3 newTarget)
    {
        StopCoroutine(nameof(FollowPath));
        _target = newTarget;
        targetType = TargetType.fix;
        
        PathRequestManager.RequestPath(new PathRequest(transform.position, _target, OnPathFound, this));
    }
    
    public void SetTarget(Transform newTarget)
    {
        if (disabled) return;
        if (targetType == TargetType.moving) return;
        StopCoroutine(nameof(FollowPath));
        movingTarget = newTarget;
        targetType = TargetType.moving;
    }

    private void Start()
    {
        disabled = PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.IsMasterClient;
        rb = GetComponent<Rigidbody>();
        
        if (disabled) return;
        
        StartCoroutine(UpdatePath());
    }

    private void FixedUpdate()
    {
        // if (isStopped) return;
        // if (isDecelerating && isStopped) Decelerate();
        if (!isDecelerating) return;
        
        Vector3 speedDif = Vector3.Lerp(rb.velocity, speed * moveDirection, Time.deltaTime * acceleration);
        
        Vector3 movement = speedDif - rb.velocity;
        movement.y = 0f;
        
        rb.AddForce(movement, ForceMode.VelocityChange);
    }

    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful) {
        if (pathSuccessful) {
            _path = new NodePath(waypoints, transform.position, turnDst, stoppingDst);
            
            StopCoroutine(nameof(FollowPath));
            StartCoroutine(nameof(FollowPath));
        }
        else
        {
            OnPathFailed?.Invoke();
            Debug.Log("Path failed");
            if (targetType == TargetType.custom) UpdateCustomPosition();
        }
    }

    IEnumerator UpdatePath()
    {
        // Give a small delay on the start cuz yes
        if (Time.timeSinceLevelLoad < 0.3f)
        {
            yield return new WaitForSeconds(0.3f);
        }
        
        _target = movingTarget.position;
        PathRequestManager.RequestPath(new PathRequest(transform.position, _target, OnPathFound, this));

        while (true) {
            yield return new WaitForSeconds(minPathUpdateTime);
            if (targetType != TargetType.moving) continue;
            
            _target = movingTarget.position;
            
            if (Vector3.Distance(_lastTargetPosition, _target) > 0.1f)
            {
                PathRequestManager.RequestPath(new PathRequest(transform.position, _target, OnPathFound, this));
                _lastTargetPosition = _target;
            }
        }
    }

    IEnumerator FollowPath() {
        bool followingPath = true;
        int pathIndex = 0;
        
        float speedPercent;
        
        while (followingPath) {
            if (isStopped)
            {
                moveDirection = Vector3.zero;
                yield break;
            }
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (_path.turnBounderies[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == _path.finishLineIndex)
                {
                    followingPath = false;
                    if (targetType == TargetType.custom) _target = targetFunc();
                    break;
                }
                pathIndex++;
            }

            if (followingPath)
            {
                if (pathIndex >= _path.slowDownIndex && stoppingDst > 0f) {
                    speedPercent = Mathf.Clamp01(_path.turnBounderies[_path.finishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }
                
                Vector3 targetDirection = (_path.lookPoints[pathIndex] - transform.position).normalized;
                moveDirection = Vector3.Lerp(moveDirection, targetDirection, Time.deltaTime * turnSpeed).normalized;
                moveDirection.y = 0f;

                // transform.Translate(moveDirection * speed * Time.deltaTime);
            }
            
            yield return null;
        }
        if (targetType == TargetType.custom) UpdateCustomPosition();
        moveDirection = Vector3.zero;
    }

    public void OnDrawGizmos() {
        _path?.DrawWithGizmos();
        
        Gizmos.color = Color.green;
        Gizmos.DrawCube(_target, Vector3.one / 4);
    }
}