using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFLifecycle.Effects;

namespace XFLifecycle
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

	    private void Button_OnClicked(object sender, EventArgs e)
	    {
	        MainContainer.Children.Clear();
	    }

	    private void RoutingLifecycleEffect_OnLoaded(object sender, EventArgs e)
	    {
	        DisplayAlert("LOADED", "Button was added", "OK");
        }

	    private void RoutingLifecycleEffect_OnUnloaded(object sender, EventArgs e)
	    {
	        DisplayAlert("UNLOADED", "Button was removed", "OK");
	    }
	}
}