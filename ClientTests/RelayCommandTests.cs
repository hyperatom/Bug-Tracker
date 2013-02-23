using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Client.Helpers;
using NUnit.Framework;

namespace ClientTests
{
    [TestFixture]
    public class RelayCommandTests
    {

        private RelayCommand _RelayCommand;
        private String _ActualString;
        private bool _HasCommandExecuted;


        [TestFixtureSetUp]
        public void Init()
        {
            _HasCommandExecuted = false;
            CanStubCommandExecute = false;

            _RelayCommand = new RelayCommand(p => ExecuteStubCommand(p), p => CanStubCommandExecute);
        }


        public bool CanStubCommandExecute { get; set; }


        private void ExecuteStubCommand(object param)
        {
            _HasCommandExecuted = true;

            if (param != null && param.GetType() == typeof(String))
                _ActualString = (String)param;
        }

        
        [Test]
        public void TestStubCommandExecutes()
        {
            _RelayCommand.Execute(null);

            Assert.True(_HasCommandExecuted);
        }


        [Test]
        public void TestStubCommandCannotExecuteIfCanExecuteIsFalse()
        {
            CanStubCommandExecute = false;

            Assert.False(_RelayCommand.CanExecute(null));
        }


        [Test]
        public void TestStubCommandCanExecuteIfCanExecuteIsTrue()
        {
            CanStubCommandExecute = true;

            Assert.True(_RelayCommand.CanExecute(null));
        }


        [Test]
        public void TestStubCommandCanExecuteWithParameters()
        {
            const String ExpectedString = "TestingParam";

            _RelayCommand.Execute(ExpectedString);

            Assert.AreEqual(ExpectedString, _ActualString);
        }

    }
}
