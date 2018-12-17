using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPiServer.Approvals;

namespace RegionOrebroLan.EPiServer.Initialization.IntegrationTests.Business.Approvals
{
	public class FakedApprovalEngine : IApprovalEngine
	{
		#region Methods

		public virtual Task AbortAsync(IEnumerable<int> ids, string username)
		{
			return Task.CompletedTask;
		}

		public virtual Task ApproveAsync(int id, string username, int stepIndex, ApprovalDecisionScope scope, string comment)
		{
			return Task.CompletedTask;
		}

		public virtual Task RejectAsync(int id, string username, int stepIndex, ApprovalDecisionScope scope, string comment)
		{
			return Task.CompletedTask;
		}

		public virtual Task<IEnumerable<Approval>> StartAsync(IEnumerable<Uri> references, string username, bool throwOnMissingDefinition)
		{
			return Task.FromResult(Enumerable.Empty<Approval>());
		}

		#endregion
	}
}