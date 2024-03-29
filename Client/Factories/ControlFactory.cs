﻿using Microsoft.Practices.Unity;
using Client.ViewModels;
using Client.ViewModels.Controls;
using Client.ServiceReference;
using Client.Views.Controls;
using Client.ViewModels.Controls.ProjectPanel;
using Client.ViewModels.Controls.Dialogs;
using Client.ViewModels.Windows;
using System;

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
            if (project == null)
                return _Container.Resolve<IBugTableViewModel>();

            return _Container.Resolve<IBugTableViewModel>(new ParameterOverride("activeProj", project));
        }


        public ICommandPanelViewModel CreateCommandPanel(ProjectViewModel project)
        {
            if (project == null)
                return _Container.Resolve<ICommandPanelViewModel>();

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


        public IProjectManagerViewModel CreateProjectManagerPanel(User currentUser)
        {
            return _Container.Resolve<IProjectManagerViewModel>(new ParameterOverride("currentUser", currentUser));
        }


        public ProjectPanelViewModel CreateProjectAddPanel()
        {
            return _Container.Resolve<ProjectPanelViewModel>("Add");
        }


        public ProjectPanelViewModel CreateProjectViewPanel(ProjectViewModel vm)
        {
            return _Container.Resolve<ProjectPanelViewModel>("View", new ParameterOverride("selectedProj", vm));
        }


        public IDeleteProjectDialogViewModel CreateDeleteProjectDialog(ProjectViewModel vm)
        {
            return _Container.Resolve<IDeleteProjectDialogViewModel>(new ParameterOverride("proj", vm));
        }


        public IAssignedProjectsPanelViewModel CreateAssignedProjectsPanel(User currentUser)
        {
            return _Container.Resolve<IAssignedProjectsPanelViewModel>(new ParameterOverride("currentUser", currentUser));
        }


        public IManagedProjectsPanelViewModel CreateManagedProjectsPanel(User currentUser)
        {
            return _Container.Resolve<IManagedProjectsPanelViewModel>(new ParameterOverride("currentUser", currentUser));
        }


        public IProjectUsersPanelViewModel CreateProjectUsersPanel(ProjectViewModel proj)
        {
            if (proj == null)
                return _Container.Resolve<IProjectUsersPanelViewModel>();

            return _Container.Resolve<IProjectUsersPanelViewModel>(new ParameterOverride("proj", proj));
        }


        public IJoinProjectPanelViewModel CreateJoinProjectPanel()
        {
            return _Container.Resolve<IJoinProjectPanelViewModel>();
        }


        public IAccountSettingsViewModel CreateAccountSettingsPanel()
        {
            return _Container.Resolve<IAccountSettingsViewModel>();
        }


        public IWestSideBarViewModel CreateWestSideBar(ProjectViewModel project)
        {
            if (project == null)
                return _Container.Resolve<IWestSideBarViewModel>();

            return _Container.Resolve<IWestSideBarViewModel>(new ParameterOverride("activeProj", project));
        }


        public IRegistrationSuccessPanelViewModel CreateRegistrationSuccessPanel(String username, IRegistrationViewModel window)
        {
            ParameterOverrides parameters = new ParameterOverrides() 
            { 
                { "username",  username },
                { "regwindow", window     }
            };


            return _Container.Resolve<IRegistrationSuccessPanelViewModel>(parameters);
        }
    }
}
