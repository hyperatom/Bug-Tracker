using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using Client.Controllers;
using Client.ViewModels;

namespace Client
{
    /// <summary>
    /// This is the first class loaded by the client. The constructor
    /// immediately creates an instance of the window controller
    /// to initiate the user interface flow.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Default constructor creates a new instance of window controller
        /// which controls the flow of user interfaces in the client.
        /// </summary>
        public App()
        {
            //new WindowController();
            new WindowLoader().ShowView(new LoginViewModel());
        }

    }
}
