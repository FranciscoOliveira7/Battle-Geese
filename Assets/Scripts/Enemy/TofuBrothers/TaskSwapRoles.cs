using System.Collections;
using UnityEngine;
using BehaviourTree;

public class TaskSwapRoles : Node
{
    private Unit _tofuBrother1;
    private Unit _tofuBrother2;
    private Transform _avoidant;
    private float _swapInterval;
    private float _lastSwapTime;
    private bool _isBrother1Attacking;
    private Unit _attackingBrother;
    private Unit _avoidingBrother;
    private LayerMask _wallLayerMask;
    private WaitTask _waitTask;
    private TaskAgressive _taskAgressive1;
    private TaskAgressive _taskAgressive2;
    private TaskAvoid _taskAvoid1;
    private TaskAvoid _taskAvoid2;

    public TaskSwapRoles(Unit tofuBrother1, Unit tofuBrother2, Transform avoidant, float swapInterval, LayerMask wallLayerMask, TaskAgressive taskAgressive1, TaskAgressive taskAgressive2, TaskAvoid taskAvoid1, TaskAvoid taskAvoid2)
    {
        _tofuBrother1 = tofuBrother1;
        _tofuBrother2 = tofuBrother2;
        _avoidant = avoidant;
        _swapInterval = swapInterval;
        _isBrother1Attacking = true;
        _attackingBrother = _tofuBrother1;
        _avoidingBrother = _tofuBrother2;
        _wallLayerMask = wallLayerMask;
        _waitTask = new WaitTask(5f); // Wait for 5 seconds
        _taskAgressive1 = taskAgressive1;
        _taskAgressive2 = taskAgressive2;
        _taskAvoid1 = taskAvoid1;
        _taskAvoid2 = taskAvoid2;
    }

    public override NodeState Evaluate()
    {
        if (Time.time - _lastSwapTime >= _swapInterval)
        {
            NodeState waitState = _waitTask.Evaluate();
            if (waitState == NodeState.SUCCESS)
            {
                SwapRoles();
                _lastSwapTime = Time.time;
                state = NodeState.SUCCESS;
                return state;
            }
            else
            {
                state = NodeState.RUNNING;
                return state;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }

    private void SwapRoles()
    {
        _isBrother1Attacking = !_isBrother1Attacking;
        _attackingBrother = _isBrother1Attacking ? _tofuBrother1 : _tofuBrother2;
        _avoidingBrother = _isBrother1Attacking ? _tofuBrother2 : _tofuBrother1;

        Transform player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            _attackingBrother.movingTarget = player;
            if (_avoidant != null)
            {
                _avoidingBrother.movingTarget = _avoidant;
            }
        }

        // Reset the dash state for both brothers
        ResetDashState();
    }

    private void ResetDashState()
    {
        _taskAgressive1.ResetDash();
        _taskAgressive2.ResetDash();
        //_taskAvoid1.ResetDash();
        //_taskAvoid2.ResetDash();
    }

    public Unit GetAttackingBrother()
    {
        return _attackingBrother;
    }

    public Unit GetAvoidingBrother()
    {
        return _avoidingBrother;
    }
}
