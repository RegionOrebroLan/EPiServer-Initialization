using System.Web.Hosting;
using EPiServer.Web.Hosting;

namespace RegionOrebroLan.EPiServer.Initialization.IntegrationTests.Business.Web.Hosting
{
	public class FakedHostingEnvironment : IHostingEnvironment
	{
		#region Fields

		private const string _applicationId = "Integration-test";
		private static readonly string _applicationPhysicalPath = Global.ProjectDirectoryPath;
		private const string _applicationVirtualPath = "/";
		private static readonly IHostingEnvironment _hostingEnvironmentInternal = new AspNetHostingEnvironment();

		#endregion

		#region Properties

		public virtual string ApplicationID => _applicationId;
		public virtual string ApplicationPhysicalPath => _applicationPhysicalPath;
		public virtual string ApplicationVirtualPath => _applicationVirtualPath;
		protected internal virtual IHostingEnvironment HostingEnvironmentInternal => _hostingEnvironmentInternal;
		public virtual VirtualPathProvider VirtualPathProvider => this.HostingEnvironmentInternal.VirtualPathProvider;

		#endregion

		#region Methods

		public virtual string MapPath(string virtualPath)
		{
			return this.HostingEnvironmentInternal.MapPath(virtualPath);
		}

		public virtual void RegisterVirtualPathProvider(VirtualPathProvider virtualPathProvider)
		{
			this.HostingEnvironmentInternal.RegisterVirtualPathProvider(virtualPathProvider);
		}

		#endregion
	}
}