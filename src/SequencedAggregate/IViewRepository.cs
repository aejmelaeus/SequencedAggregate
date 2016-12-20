namespace SequencedAggregate
{
    public interface IViewRepository
    {
        void Commit<TView>(string id, TView view) where TView : class;
        TView Read<TView>(string id) where TView : class;
    }
}