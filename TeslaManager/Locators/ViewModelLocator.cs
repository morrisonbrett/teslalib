using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

// Toolkit namespace
using SimpleMvvmToolkit;
using TeslaManager.ViewModels;

namespace TeslaManager.Locators
{
    /// <summary>
    /// This class creates ViewModels on demand for Views, supplying a
    /// ServiceAgent to the ViewModel if required.
    /// <para>
    /// Place the ViewModelLocator in the App.xaml resources:
    /// </para>
    /// <code>
    /// &lt;Application.Resources&gt;
    ///     &lt;vm:ViewModelLocator xmlns:vm="clr-namespace:TeslaManager"
    ///                                  x:Key="Locator" /&gt;
    /// &lt;/Application.Resources&gt;
    /// </code>
    /// <para>
    /// Then use:
    /// </para>
    /// <code>
    /// DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
    /// </code>
    /// <para>
    /// Use the <strong>mvvmlocator</strong> or <strong>mvvmlocatornosa</strong>
    /// code snippets to add ViewModels to this locator.
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        public TeslaVehicleViewModel TeslaVehicleViewModel
        {
            get
            {
                return new TeslaVehicleViewModel();
            }
        }

        public TeslaManagerViewModel TeslaManagerViewModel
        {
            get
            {
                return new TeslaManagerViewModel();
            }
        }

        public LoginViewModel LoginViewModel
        {
            get
            {
                return new LoginViewModel();
            }
        }
    }
}