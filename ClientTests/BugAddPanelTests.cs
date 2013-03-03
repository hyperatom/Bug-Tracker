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

namespace ClientTests
{
    [TestFixture]
    public class BugAddPanelTests
    {

        private BugAddPanelViewModel _AddPanel;
        private ProjectViewModel _ActiveProjectStub;
        private IMessenger _Messenger;
        private ITrackerService _ServiceMock;


        [TestFixtureSetUp]
        public void Init()
        {
            var messengerMock = new Mock<IMessenger>();
            var serviceMock   = new Mock<ITrackerService>();

            _Messenger = messengerMock.Object;

            List<string> mockPriorities = new List<string>(){"High", "Low"};
            List<string> mockStatus     = new List<string>() { "In Progress", "Closed" };

            User userMock = new User() { Id = 5, FirstName = "Adam", Username = "adam", Password = "password" };

            serviceMock.Setup<List<string>> (p => p.GetBugPriorityList()).Returns(mockPriorities);
            serviceMock.Setup<List<string>> (p => p.GetBugStatusList()).Returns(mockStatus); 
            serviceMock.Setup<User>         (p => p.GetMyUser()).Returns(userMock);

            _ServiceMock = serviceMock.Object;

            _ActiveProjectStub = new ProjectViewModel(new Project { Id = 5, Description = "Stub Project", Name = "Stub Title" });
            
            _AddPanel = new BugAddPanelViewModel(_Messenger, _ServiceMock, _ActiveProjectStub);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestExceptionThrownWhenMessengerNull()
        {
            _AddPanel = new BugAddPanelViewModel(null, _ServiceMock, _ActiveProjectStub);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestExceptionThrownWhenProjectNull()
        {
            _AddPanel = new BugAddPanelViewModel(_Messenger, _ServiceMock, null);
        }


        [Test]
        public void TestValidBugAdds()
        {
            //_AddPanel.add
        }

    }
}
