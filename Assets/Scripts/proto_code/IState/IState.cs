public interface IState
{
    void OnEnter(EntityBrain brain);
    void OnUpdate();
    void OnExit();
}