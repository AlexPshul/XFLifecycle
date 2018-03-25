using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using XFLifecycle.Effects;
using XFLifecycle.UWP.Effects;

[assembly:ResolutionGroupName(ViewLifecycleEffect.EffectGroupName)]
[assembly:ExportEffect(typeof(UwpViewLifecycleEffect), ViewLifecycleEffect.EffectName)]
namespace XFLifecycle.UWP.Effects
{
    public class UwpViewLifecycleEffect : PlatformEffect
    {
        private FrameworkElement _nativeView;
        private ViewLifecycleEffect _viewLifecycleEffect;

        protected override void OnAttached()
        {
            _viewLifecycleEffect = Element.Effects.OfType<ViewLifecycleEffect>().FirstOrDefault();
            _nativeView = Container;
            
            _nativeView.Loaded += NativeViewOnLoaded;
            _nativeView.Unloaded += NativeViewOnUnloaded;
        }

        protected override void OnDetached()
        {
            _viewLifecycleEffect?.RaiseUnloaded(Element);
            _nativeView.Loaded -= NativeViewOnLoaded;
            _nativeView.Unloaded -= NativeViewOnUnloaded;
        }

        private void NativeViewOnLoaded(object sender, RoutedEventArgs routedEventArgs) => _viewLifecycleEffect?.RaiseLoaded(Element);
        private void NativeViewOnUnloaded(object sender, RoutedEventArgs routedEventArgs) => _viewLifecycleEffect?.RaiseUnloaded(Element);
    }
}