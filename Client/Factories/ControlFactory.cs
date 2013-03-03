using Microsoft.Practices.Unity;
using Client.ViewModels;
using Client.ViewModels.Controls;

namespace Client.Factories
{
    public class ControlFactory : IControlFactory
    {

        private IUnityContainer _Container;


        public ControlFactory(IUnityContainer container)
        {
            _Container = container;
        }


        public IBugTableViewModel CreateBugTablePanel(ProjectViewModel project)
        {
            return _Container.Resolve<IBugTableViewModel>(new ParameterOverride("activeProj", project));
        }


        public ICommandPanelViewModel CreateCommandPanel(ProjectViewModel project)
        {
            return _Container.Resolve<ICommandPanelViewModel>(new ParameterOverride("activeProj", project));
        }


        public BugPanelViewModel CreateBugViewPanel(ProjectViewModel project, BugViewModel bug)
        {
            ParameterOverrides parameters = new ParameterOverrides() 
            { 
                { "activeProj",  project },
                { "selectedBug", bug     }
            };

            return _Container.Resolve<BugPanelViewModel>("ViewPanel",  parameters);
        }


        public BugPanelViewModel CreateBugAddPanel(ProjectViewModel project)
        {
            return _Container.Resolve<BugPanelViewModel>("AddPanel", new ParameterOverride("activeProj", project));
        }

    }
}
