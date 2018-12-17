using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPiServer.Approvals;

namespace RegionOrebroLan.EPiServer.Initialization.IntegrationTests.Business.Approvals
{
	public class FakedApprovalRepository : IApprovalRepository
	{
		#region Methods

		public virtual Task DeleteAsync(IEnumerable<int> ids)
		{
			return Task.CompletedTask;
		}

		public virtual Task<IEnumerable<Approval>> GetItemsAsync(IEnumerable<Uri> references)
		{
			return Task.FromResult(Enumerable.Empty<Approval>());
		}

		public virtual Task<IEnumerable<Approval>> GetItemsAsync(IEnumerable<int> ids)
		{
			return Task.FromResult(Enumerable.Empty<Approval>());
		}

		public virtual Task<PagedApprovalResult> ListAsync(ApprovalQuery query, long startIndex, int maxRows)
		{
			return Task.FromResult(new PagedApprovalResult(Enumerable.Empty<Approval>(), 0));
		}

		public virtual Task<IEnumerable<ApprovalStepDecision>> ListDecisionsAsync(int id, int? stepIndex)
		{
			return Task.FromResult(Enumerable.Empty<ApprovalStepDecision>());
		}

		public virtual Task SaveAsync(IEnumerable<Approval> approvals, string username)
		{
			return Task.CompletedTask;
		}

		public virtual Task SaveDecisionAsync(int id, ApprovalStepDecision decision)
		{
			return Task.CompletedTask;
		}

		#endregion
	}
}