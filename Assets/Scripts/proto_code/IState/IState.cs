public interface IState
{
    void OnEnter(EntityBrain brain);
    void OnTick();
    void OnExit();
}