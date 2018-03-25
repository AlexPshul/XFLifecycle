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
        private UIView _nativeView;
        private SuperviewListener _myObserver;
        private ViewLifecycleEffect _viewLifecycleEffect;

        protected override void OnAttached()
        {
            _viewLifecycleEffect = Element.Effects.OfType<ViewLifecycleEffect>().FirstOrDefault();
            _myObserver = new SuperviewListener(_viewLifecycleEffect, Element);
            
            _nativeView = Control ?? Container;
            _nativeView.AddObserver(_myObserver, new NSString(nameof(Control.Superview)), NSKeyValueObservingOptions.Initial, new IntPtr());
        }

        protected override void OnDetached()
        {
            _nativeView.RemoveObserver(_myObserver, new NSString(nameof(Control.Superview)));
            _viewLifecycleEffect.RaiseUnloaded(Element);
        }

        private class SuperviewListener : NSObject
        {
            private readonly ViewLifecycleEffect _viewLifecycleEffect;
            private readonly Element _element;

            public SuperviewListener(ViewLifecycleEffect viewLifecycleEffect, Element element)
            {
                _viewLifecycleEffect = viewLifecycleEffect;
                _element = element;
            }

            public override void ObserveValue(NSString keyPath, NSObject ofObject, NSDictionary change, IntPtr context)
            {
                if (!(ofObject is UIView view))
                    return;

                if (view.Superview != null)
                    _viewLifecycleEffect.RaiseLoaded(_element);
                else
                    _viewLifecycleEffect.RaiseUnloaded(_element);
            }
        }
    }
}