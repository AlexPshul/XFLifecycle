using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using XFLifecycle.Effects;
using XFLifecycle.UWP.Effects;

[assembly:ResolutionGroupName(RoutingLifecycleEffect.EffectGroupName)]
[assembly:ExportEffect(typeof(LifecycleEffect), RoutingLifecycleEffect.EffectName)]
namespace XFLifecycle.UWP.Effects
{
    public class LifecycleEffect : PlatformEffect
    {
        private FrameworkElement _nativeView;
        private RoutingLifecycleEffect _routingEffect;

        protected override void OnAttached()
        {
            _routingEffect = Element.Effects.OfType<RoutingLifecycleEffect>().FirstOrDefault();
            _nativeView = Container;
            
            _nativeView.Loaded += NativeViewOnLoaded;
            _nativeView.Unloaded += NativeViewOnUnloaded;
        }

        protected override void OnDetached()
        {
            _routingEffect?.RaiseUnloaded(Element);
            _nativeView.Loaded -= NativeViewOnLoaded;
            _nativeView.Unloaded -= NativeViewOnUnloaded;
        }

        private void NativeViewOnLoaded(object sender, RoutedEventArgs routedEventArgs) => _routingEffect?.RaiseLoaded(Element);
        private void NativeViewOnUnloaded(object sender, RoutedEventArgs routedEventArgs) => _routingEffect?.RaiseUnloaded(Element);
    }
}