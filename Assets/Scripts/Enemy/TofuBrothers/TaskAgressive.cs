using BehaviourTree;
using System.Collections;
using UnityEngine;

public class TaskAgressive : Node
{
    private Unit _unit;
    private DashAttackTask _dashTask;
    private WeaponAttackTask _weaponAttackTask;
    private bool _hasDashed;
    private Transform _target;
    private Animator _animator;

    public TaskAgressive(Unit unit, Transform target, Weapon primaryWeapon, Animator animator)
    {
        _animator = animator;
        _unit = unit;
        _target = target;
        // Dash task with 2 seconds duration
        _dashTask = new DashAttackTask(_unit, 2f, 0.4f, 15f, _animator);
        _weaponAttackTask = new WeaponAttackTask(unit, primaryWeapon, target, animator);
        _hasDashed = false;
    }

    public void ResetDash()
    {
        _hasDashed = false;
        _dashTask = new DashAttackTask(_unit, 2f, 0.4f, 15f, _animator); // Reinitialize the dash task
    }

    public override NodeState Evaluate()
    {
        if (!_hasDashed)
        {
            NodeState dashState = _dashTask.Evaluate();

            if (dashState == NodeState.SUCCESS)
            {
                _hasDashed = true;
                _unit.StartCoroutine(DelayedResetDash());
            }
        }
        else
        {
            _weaponAttackTask.Evaluate();
        }

        state = NodeState.RUNNING;
        return state;
    }

    private IEnumerator DelayedResetDash()
    {
        yield return new WaitForSeconds(2.5f);
        ResetDash();
    }
}
