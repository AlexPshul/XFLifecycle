using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XFLifecycle.Droid.Effects;
using XFLifecycle.Effects;
using View = Android.Views.View;

[assembly: ResolutionGroupName(RoutingLifecycleEffect.EffectGroupName)]
[assembly: ExportEffect(typeof(LifecycleEffect), RoutingLifecycleEffect.EffectName)]
namespace XFLifecycle.Droid.Effects
{
    public class LifecycleEffect : PlatformEffect
    {
        private View _nativeView;
        private AttachStateListener _attachStateListener;
        private RoutingLifecycleEffect _routingLifecycleEffect;

        protected override void OnAttached()
        {
            _routingLifecycleEffect = Element.Effects.OfType<RoutingLifecycleEffect>().FirstOrDefault();
            _attachStateListener = new AttachStateListener(_routingLifecycleEffect, Element);

            _nativeView = Control ?? Container;
            _nativeView.AddOnAttachStateChangeListener(_attachStateListener);
        }

        protected override void OnDetached()
        {
            View view = Control ?? Container;
            view.RemoveOnAttachStateChangeListener(_attachStateListener);
            _routingLifecycleEffect.RaiseUnloaded(Element);
        }

        private class AttachStateListener : Java.Lang.Object, View.IOnAttachStateChangeListener
        {
            private readonly RoutingLifecycleEffect _routingRoutingLifecycleEffect;
            private readonly Element _xfElement;

            public AttachStateListener(RoutingLifecycleEffect routingRoutingLifecycleEffect, Element xfElement)
            {
                _routingRoutingLifecycleEffect = routingRoutingLifecycleEffect;
                _xfElement = xfElement;
            }

            public void OnViewAttachedToWindow(View attachedView) => _routingRoutingLifecycleEffect.RaiseLoaded(_xfElement);
            public void OnViewDetachedFromWindow(View detachedView) => _routingRoutingLifecycleEffect.RaiseUnloaded(_xfElement);
        }
    }
}