using System;
using System.ComponentModel;
using System.Linq;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XFLifecycle.Effects;
using XFLifecycle.iOS.Effects;

[assembly:ResolutionGroupName(RoutingLifecycleEffect.EffectGroupName)]
[assembly:ExportEffect(typeof(LifecycleEffect), RoutingLifecycleEffect.EffectName)]
namespace XFLifecycle.iOS.Effects
{
    public class LifecycleEffect : PlatformEffect
    {
        private UIView _nativeView;
        private SuperviewListener _myObserver;
        private RoutingLifecycleEffect _routingLifecycleEffect;

        protected override void OnAttached()
        {
            _routingLifecycleEffect = Element.Effects.OfType<RoutingLifecycleEffect>().FirstOrDefault();
            _myObserver = new SuperviewListener(_routingLifecycleEffect, Element);
            
            _nativeView = Control ?? Container;
            _nativeView.AddObserver(_myObserver, new NSString(nameof(Control.Superview)), NSKeyValueObservingOptions.Initial, new IntPtr());
        }

        protected override void OnDetached()
        {
            _nativeView.RemoveObserver(_myObserver, new NSString(nameof(Control.Superview)));
            _routingLifecycleEffect.RaiseUnloaded(Element);
        }

        private class SuperviewListener : NSObject
        {
            private readonly RoutingLifecycleEffect _routingLifecycleEffect;
            private readonly Element _element;

            public SuperviewListener(RoutingLifecycleEffect routingLifecycleEffect, Element element)
            {
                _routingLifecycleEffect = routingLifecycleEffect;
                _element = element;
            }

            public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
            {
                if (!(ofObject is UIView view))
                    return;

                if (view.Superview != null)
                    _routingLifecycleEffect.RaiseLoaded(_element);
                else
                    _routingLifecycleEffect.RaiseUnloaded(_element);
            }
        }
    }
}