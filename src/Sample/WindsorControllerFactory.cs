namespace MvcContrib.Samples.BrailViewEngine
{
	using System;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Routing;
	using Castle.Windsor;

	public class WindsorControllerFactory : DefaultControllerFactory
	{
		private readonly IWindsorContainer _container;

		public WindsorControllerFactory(IWindsorContainer container)
		{
			_container = container;
		}

		protected override IController GetControllerInstance(RequestContext context, Type controllerType)
		{
			if (controllerType == null)
			{
				throw new HttpException(404,string.Format("The controller for path '{0}' could not be found or it does not implement IController.",
				                        	context.HttpContext.Request.Path));
			}

			return (IController) _container.Resolve(controllerType);
		}

		public override void ReleaseController(IController controller)
		{
			var disposable = controller as IDisposable;

			if (disposable != null)
			{
				disposable.Dispose();
			}

			_container.Release(controller);
		}
	}
}