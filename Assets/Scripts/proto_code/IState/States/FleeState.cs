// 파일 이름: FleeState.cs

using UnityEngine;

public class FleeState : IState
{
    private EntityBrain brain;
    private EntityCore predator;
    private float safeDistance = 8f;

    public void OnEnter(EntityBrain brain)
    {
        this.brain = brain;
        if (brain.Core.Data.predatorFactions.Count > 0)
            this.predator = brain.Sense.FindClosestTargetByFaction(brain.Core.Data.predatorFactions[0]);
    }

    public AIIntent OnTick(out EntityCore outTarget, out Vector2 outDirection)
    {
        outDirection = Vector2.zero;

        if (predator == null || predator.IsDead ||
            Vector2.Distance(brain.transform.position, predator.transform.position) > safeDistance)
        {
            brain.ChangeState(new WanderState());
            outTarget = null;
            return AIIntent.Wander;
        }

        outTarget = this.predator;
        return AIIntent.Flee;
    }

    public void OnExit() { }
}