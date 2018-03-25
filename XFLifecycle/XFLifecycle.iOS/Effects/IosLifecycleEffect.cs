using System;
using System.ComponentModel;
using System.Linq;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XFLifecycle.Effects;
using XFLifecycle.iOS.Effects;

[assembly:ResolutionGroupName(ViewLifecycleEffect.EffectGroupName)]
[assembly:ExportEffect(typeof(IosLifecycleEffect), ViewLifecycleEffect.EffectName)]
namespace XFLifecycle.iOS.Effects
{
    public class IosLifecycleEffect : PlatformEffect
    {
        private const NSKeyValueObservingOptions ObservingOptions = NSKeyValueObservingOptions.Initial | NSKeyValueObservingOptions.OldNew | NSKeyValueObservingOptions.Prior;

        private ViewLifecycleEffect _viewLifecycleEffect;
        private IDisposable _isLoadedObserverDisposable;

        protected override void OnAttached()
        {
            _viewLifecycleEffect = Element.Effects.OfType<ViewLifecycleEffect>().FirstOrDefault();
            
            UIView nativeView = Control ?? Container;
            _isLoadedObserverDisposable = nativeView?.AddObserver("superview", ObservingOptions, IsViewLoadedObserver);
        }

        protected override void OnDetached()
        {
            _viewLifecycleEffect.RaiseUnloaded(Element);
            _isLoadedObserverDisposable.Dispose();
        }

        private void IsViewLoadedObserver(NSObservedChange nsObservedChange)
        {
            if (!nsObservedChange.NewValue.Equals(NSNull.Null))
                _viewLifecycleEffect?.RaiseLoaded(Element);
            else if (!nsObservedChange.OldValue.Equals(NSNull.Null))
                _viewLifecycleEffect?.RaiseUnloaded(Element);
        }
    }
}