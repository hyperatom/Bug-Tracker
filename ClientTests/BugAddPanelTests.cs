using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using Client.ViewModels;
using Client.Helpers;
using Client.ServiceReference;
using System.Runtime.Serialization;
using Microsoft.Practices.Unity;
using Client.Views.Controls.Notifications;

namespace ClientTests
{
    [TestFixture]
    public class BugAddPanelTests
    {

        private BugAddPanelViewModel _AddPanel;

        // Dependencies
        private ProjectViewModel _ActiveProjectStub;
        private IMessenger _Messenger;
        private ITrackerService _ServiceMock;

        private ProjectViewModel _MockActiveProject;

        private List<string> _MockPriorityList;
        private List<string> _MockStatusList;
        private List<User> _MockProjectUsersList;
        private User _AssignedUserMock;

        [SetUp]
        public void Init()
        {
            var messengerMock = new Mock<IMessenger>();
            var serviceMock   = new Mock<ITrackerService>();

            _Messenger = messengerMock.Object;

            _MockPriorityList = new List<string>() { "High", "Low" };
            _MockStatusList = new List<string>() { "In Progress", "Closed" };

            _MockActiveProject = new ProjectViewModel(new Project { Id = 1, Code = "GGGGG", Description = "", Name = "TestProject" });

            _AssignedUserMock = new User() { Id = 5, FirstName = "Adam", Username = "adam", Password = "password" };

            _MockProjectUsersList = new List<User>() { _AssignedUserMock};

            serviceMock.Setup<List<string>>(p => p.GetBugPriorityList()).Returns(_MockPriorityList);
            serviceMock.Setup<List<string>>(p => p.GetBugStatusList()).Returns(_MockStatusList);
            serviceMock.Setup<List<User>>(p => p.GetUsersByProject(_MockActiveProject.ToProjectModel())).Returns(_MockProjectUsersList);
            serviceMock.Setup<User>(p => p.GetMyUser()).Returns(_AssignedUserMock);

            var _MockNotifier = new Mock<IGrowlNotifiactions>();

            _ServiceMock = serviceMock.Object;

            _ActiveProjectStub = new ProjectViewModel(new Project { Id = 5, Description = "Stub Project", Name = "Stub Title" });
            
            _AddPanel = new BugAddPanelViewModel(_Messenger, _ServiceMock, _MockActiveProject, _MockNotifier.Object);
        }


        /*[Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestExceptionThrownWhenMessengerNull()
        {
            _AddPanel = new BugAddPanelViewModel(null, _ServiceMock, _ActiveProjectStub);
        }*/


        [Test]
        public void TestDefaultPriorityIsSecondInList()
        {
            Assert.AreEqual(_MockPriorityList[1], _AddPanel.EditedBug.Priority);
        }

        [Test]
        public void TestDefaultStatusIsSecondInList()
        {
            Assert.AreEqual(_MockStatusList[1], _AddPanel.EditedBug.Status);
        }

        [Test]
        public void TestAssignedUserIsCorrect()
        {
            Assert.IsNull(_AddPanel.AssignedUser);
        }

        [Test]
        public void TestPanelIsNotVisibleByDefault()
        {
            Assert.IsFalse(_AddPanel.IsVisible);
        }

        [Test]
        public void TestUserInActiveProjectAreCorrect()
        {
            if (_MockProjectUsersList != null)
                Assert.AreEqual(_MockProjectUsersList, _AddPanel.UsersInActiveProject);
            else
                Assert.True(true);
        }

    }
}
