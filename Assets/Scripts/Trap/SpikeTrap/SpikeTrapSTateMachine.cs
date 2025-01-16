public class SpikeTrapStateMachine : StateMachine
{
    public SpikeTrap SpikeTrap { get; }
    public SpikeTrapActiveState ActiveState { get; }
    public SpikeTrapInactiveState InactiveState { get; }

    public SpikeTrapStateMachine(SpikeTrap spikeTrap)
    {
        SpikeTrap = spikeTrap;
        ActiveState = new SpikeTrapActiveState(this);
        InactiveState = new SpikeTrapInactiveState(this);
    }
}
