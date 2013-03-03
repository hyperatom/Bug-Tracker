using System;
using Client.Helpers;
using Moq;
using NUnit.Framework;

namespace ClientTests
{
    [TestFixture]
    public class WindowLoaderTests
    {

        [TestFixtureSetUp]
        public void Init()
        {
            var mockMessenger = new Mock<IMessenger>();

            //_WindowLoader = new WindowLoader();
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestExeptionThrownWhenUnknownWindowUsed()
        {
            //_WindowLoader.ShowView(new StubWindow());
        }


        private class StubWindow : IWindow
        {
            public StubWindow() { }

            public void ShowView(IWindow viewModel) { }

            public EventHandler RequestClose { get; set; }
        }

    }
}
