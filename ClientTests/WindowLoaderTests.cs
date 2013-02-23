using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Client.Controllers;
using Client.Helpers;
using Moq;

namespace ClientTests
{
    [TestFixture]
    public class WindowLoaderTests
    {

        private IWindowLoader _WindowLoader;

        [TestFixtureSetUp]
        public void Init()
        {
            var mockMessenger = new Mock<IMessenger>();

            _WindowLoader = new WindowLoader(mockMessenger.Object);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestExeptionThrownWhenUnknownWindowUsed()
        {
            _WindowLoader.ShowView(new StubWindow());
        }


        private class StubWindow : IWindow
        {
            public StubWindow() { }

            public void ShowView(IWindow viewModel) { }

            public IWindowLoader WindowLoader { get; set; }

            public EventHandler RequestClose { get; set; }
        }

    }
}
