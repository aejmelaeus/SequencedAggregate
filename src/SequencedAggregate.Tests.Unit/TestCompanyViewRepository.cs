using System.Collections.Generic;

namespace SequencedAggregate.Tests.Unit
{
    internal class TestCompanyViewRepository : IViewRepository
    {
        private readonly Dictionary<string, object> _views = new Dictionary<string, object>();

        public void Commit<TView>(string id, TView view) where TView : class
        {
            if (_views.ContainsKey(id))
            {
                _views[id] = view;
            }
            else
            {
                _views.Add(id, view);
            }
        }

        public TView Read<TView>(string id) where TView : class
        {
            if (_views.ContainsKey(id))
            {
                return _views[id] as TView;
            }

            return null;
        }

        public void WithExistingView(string id, CompanyView view)
        {
            Commit(id, view);
        }
    }
}