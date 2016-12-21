using SequencedAggregate.Tests.Unit.Events;

namespace SequencedAggregate.Tests.Unit
{
    internal class CompanyNamesProjectionBuilder : ProjectionBuilderBase<EventBase, CompanyNamesView>
    {
        public CompanyNamesProjectionBuilder()
        {
            RegisterHandler<CompanyCreated>(Handle);
            RegisterHandler<CompanyNameUpdated>(Handle);
        }

        private CompanyNamesView Handle(CompanyCreated e, CompanyNamesView view)
        {
            view.Id = e.Id;
            view.Names.Add(e.Name);

            return view;
        }

        private CompanyNamesView Handle(CompanyNameUpdated e, CompanyNamesView view)
        {
            view.Names.Add(e.NewName);

            return view;
        }
    }
}