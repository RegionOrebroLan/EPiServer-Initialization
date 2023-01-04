using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.EPiServer.Initialization;
using RegionOrebroLan.EPiServer.Initialization.Configuration;

namespace UnitTests.Configuration
{
	[TestClass]
	public class DisableableInitializationConfigurationTest
	{
		#region Methods

		[TestMethod]
		public async Task Constructor_WithOneParameter_IfTheApplicationSettingsParameterIsNull_ShouldNotThrowAnException()
		{
			await Task.CompletedTask;

			Assert.IsNotNull(new DisableableInitializationConfiguration(null));
		}

		[TestMethod]
		public async Task Constructor_WithOneParameter_IfTheApplicationSettingsParameterIsNull_ShouldResultInAnEmptyApplicationSettingsProperty()
		{
			await Task.CompletedTask;

			Assert.AreEqual(0, new DisableableInitializationConfiguration(null).ApplicationSettings.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task IsDisabled_IfTheInitializationKeyParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			await Task.CompletedTask;

			try
			{
				var disableableInitializationConfiguration = new DisableableInitializationConfiguration(new NameValueCollection { { DisableableInitializationConfiguration.KeyPrefix, "false" } });

				disableableInitializationConfiguration.IsDisabled(null);
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(string.Equals(argumentNullException.ParamName, "initializationKey", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		public async Task IsDisabled_Test()
		{
			await Task.CompletedTask;

			var disableableInitializationConfiguration = new DisableableInitializationConfiguration(new NameValueCollection { { DisableableInitializationConfiguration.KeyPrefix, "false" } });
			var disabled = disableableInitializationConfiguration.IsDisabled(string.Empty);
			Assert.IsNotNull(disabled);
			Assert.IsTrue(disabled.Value);

			disableableInitializationConfiguration = new DisableableInitializationConfiguration(new NameValueCollection { { DisableableInitializationConfiguration.KeyPrefix + "*", "false" } });
			disabled = disableableInitializationConfiguration.IsDisabled(Guid.NewGuid().ToString());
			Assert.IsNotNull(disabled);
			Assert.IsTrue(disabled.Value);

			disableableInitializationConfiguration = new DisableableInitializationConfiguration(new NameValueCollection { { DisableableInitializationConfiguration.KeyPrefix + "RegionOrebroLan*", "false" } });
			disabled = disableableInitializationConfiguration.IsDisabled(typeof(ContentProviderInitialization).FullName);
			Assert.IsNotNull(disabled);
			Assert.IsTrue(disabled.Value);

			disableableInitializationConfiguration = new DisableableInitializationConfiguration(new NameValueCollection { { DisableableInitializationConfiguration.KeyPrefix + "RegionOrebroLan*", "true" } });
			disabled = disableableInitializationConfiguration.IsDisabled(typeof(ContentProviderInitialization).FullName);
			Assert.IsNotNull(disabled);
			Assert.IsFalse(disabled.Value);

			disableableInitializationConfiguration = new DisableableInitializationConfiguration(new NameValueCollection { { DisableableInitializationConfiguration.KeyPrefix + "RegionOrebroLan*", "true" } });
			disabled = disableableInitializationConfiguration.IsDisabled("Test");
			Assert.IsNull(disabled);
		}

		[TestMethod]
		public async Task IsMatch_IfThePatternParameterIsAWildcardAndTheValueParameterIsNotNull_ShouldReturnTrue()
		{
			await Task.CompletedTask;

			const string wildcard = "*";

			var disableableInitializationConfiguration = new DisableableInitializationConfiguration();

			Assert.IsTrue(disableableInitializationConfiguration.IsMatch(wildcard, string.Empty));
			Assert.IsTrue(disableableInitializationConfiguration.IsMatch(wildcard, " "));
			Assert.IsTrue(disableableInitializationConfiguration.IsMatch(wildcard, "Test"));
		}

		[TestMethod]
		public async Task IsMatch_IfThePatternParameterIsNullAndTheValueParameterIsAnEmptyString_ShouldReturnTrue()
		{
			await Task.CompletedTask;

			var disableableInitializationConfiguration = new DisableableInitializationConfiguration();

			Assert.IsTrue(disableableInitializationConfiguration.IsMatch(null, string.Empty));
		}

		[TestMethod]
		public async Task IsMatch_IfThePatternParameterIsNullAndTheValueParameterIsAWhiteSpace_ShouldReturnFalse()
		{
			await Task.CompletedTask;

			var disableableInitializationConfiguration = new DisableableInitializationConfiguration();

			Assert.IsFalse(disableableInitializationConfiguration.IsMatch(null, " "));
		}

		[TestMethod]
		public async Task IsMatch_IfThePatternParameterIsNullAndTheValueParameterIsTest_ShouldReturnFalse()
		{
			await Task.CompletedTask;

			var disableableInitializationConfiguration = new DisableableInitializationConfiguration();

			Assert.IsFalse(disableableInitializationConfiguration.IsMatch(null, "Test"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public async Task IsMatch_IfTheValueParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			await Task.CompletedTask;

			var exceptions = new List<Exception>();

			try
			{
				new DisableableInitializationConfiguration().IsMatch(null, null);
			}
			catch(Exception exception)
			{
				exceptions.Add(exception);
			}

			try
			{
				new DisableableInitializationConfiguration().IsMatch(string.Empty, null);
			}
			catch(Exception exception)
			{
				exceptions.Add(exception);
			}

			Assert.AreEqual(2, exceptions.Count, "The test failed regarding number of exceptions.");

			for(var i = 0; i < exceptions.Count; i++)
			{
				var exception = exceptions[i];

				if(exception is ArgumentNullException argumentNullException && string.Equals("value", argumentNullException.ParamName, StringComparison.Ordinal))
					continue;

				Assert.Fail("Exception with index \"{0}\" failed: {1}", i, exception);
			}

			throw exceptions.First();
		}

		[TestMethod]
		public async Task Settings_ShouldIncludeTheEntriesFromApplicationSettingsWithTheRightPrefix()
		{
			await Task.CompletedTask;

			Assert.IsFalse(new DisableableInitializationConfiguration().Settings.Any());

			Assert.IsFalse(new DisableableInitializationConfiguration(new NameValueCollection { { "Test", "true" } }).Settings.Any());

			Assert.IsFalse(new DisableableInitializationConfiguration(new NameValueCollection { { "da21c762-1466-4e0b-b4c6-25540fe0e8b4", "true" } }).Settings.Any());

			Assert.IsTrue(new DisableableInitializationConfiguration(new NameValueCollection { { DisableableInitializationConfiguration.KeyPrefix, "true" } }).Settings.Any());
		}

		[TestMethod]
		public async Task WildcardCharacter_Test()
		{
			await Task.CompletedTask;

			Assert.AreEqual('*', new DisableableInitializationConfiguration().WildcardCharacter);
		}

		#endregion
	}
}