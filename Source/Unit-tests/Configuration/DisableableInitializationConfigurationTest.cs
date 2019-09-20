using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionOrebroLan.EPiServer.Initialization.Configuration;

namespace RegionOrebroLan.EPiServer.Initialization.UnitTests.Configuration
{
	[TestClass]
	public class DisableableInitializationConfigurationTest
	{
		#region Methods

		[TestMethod]
		public void Constructor_WithOneParameter_IfTheApplicationSettingsParameterIsNull_ShouldNotThrowAnException()
		{
			Assert.IsNotNull(new DisableableInitializationConfiguration(null));
		}

		[TestMethod]
		public void Constructor_WithOneParameter_IfTheApplicationSettingsParameterIsNull_ShouldResultInAnEmptyApplicationSettingsProperty()
		{
			Assert.AreEqual(0, new DisableableInitializationConfiguration(null).ApplicationSettings.Count);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsDisabled_IfTheInitializationKeyParameterIsNull_ShouldThrowAnArgumentNullException()
		{
			try
			{
				var disableableInitializationConfiguration = new DisableableInitializationConfiguration(new NameValueCollection {{DisableableInitializationConfiguration.KeyPrefix, "false"}});

				disableableInitializationConfiguration.IsDisabled(null);
			}
			catch(ArgumentNullException argumentNullException)
			{
				if(argumentNullException.ParamName.Equals("initializationKey", StringComparison.Ordinal))
					throw;
			}
		}

		[TestMethod]
		public void IsDisabled_Test()
		{
			var disableableInitializationConfiguration = new DisableableInitializationConfiguration(new NameValueCollection {{DisableableInitializationConfiguration.KeyPrefix, "false"}});
			var disabled = disableableInitializationConfiguration.IsDisabled(string.Empty);
			Assert.IsNotNull(disabled);
			Assert.IsTrue(disabled.Value);

			disableableInitializationConfiguration = new DisableableInitializationConfiguration(new NameValueCollection {{DisableableInitializationConfiguration.KeyPrefix + "*", "false"}});
			disabled = disableableInitializationConfiguration.IsDisabled(Guid.NewGuid().ToString());
			Assert.IsNotNull(disabled);
			Assert.IsTrue(disabled.Value);

			disableableInitializationConfiguration = new DisableableInitializationConfiguration(new NameValueCollection {{DisableableInitializationConfiguration.KeyPrefix + "RegionOrebroLan*", "false"}});
			disabled = disableableInitializationConfiguration.IsDisabled(this.GetType().FullName);
			Assert.IsNotNull(disabled);
			Assert.IsTrue(disabled.Value);

			disableableInitializationConfiguration = new DisableableInitializationConfiguration(new NameValueCollection {{DisableableInitializationConfiguration.KeyPrefix + "RegionOrebroLan*", "true"}});
			disabled = disableableInitializationConfiguration.IsDisabled(this.GetType().FullName);
			Assert.IsNotNull(disabled);
			Assert.IsFalse(disabled.Value);

			disableableInitializationConfiguration = new DisableableInitializationConfiguration(new NameValueCollection {{DisableableInitializationConfiguration.KeyPrefix + "RegionOrebroLan*", "true"}});
			disabled = disableableInitializationConfiguration.IsDisabled("Test");
			Assert.IsNull(disabled);
		}

		[TestMethod]
		public void IsMatch_IfThePatternParameterIsAWildcardAndTheValueParameterIsNotNull_ShouldReturnTrue()
		{
			const string wildcard = "*";

			var disableableInitializationConfiguration = new DisableableInitializationConfiguration();

			Assert.IsTrue(disableableInitializationConfiguration.IsMatch(wildcard, string.Empty));
			Assert.IsTrue(disableableInitializationConfiguration.IsMatch(wildcard, " "));
			Assert.IsTrue(disableableInitializationConfiguration.IsMatch(wildcard, "Test"));
		}

		[TestMethod]
		public void IsMatch_IfThePatternParameterIsNullAndTheValueParameterIsAnEmptyString_ShouldReturnTrue()
		{
			var disableableInitializationConfiguration = new DisableableInitializationConfiguration();

			Assert.IsTrue(disableableInitializationConfiguration.IsMatch(null, string.Empty));
		}

		[TestMethod]
		public void IsMatch_IfThePatternParameterIsNullAndTheValueParameterIsAWhiteSpace_ShouldReturnFalse()
		{
			var disableableInitializationConfiguration = new DisableableInitializationConfiguration();

			Assert.IsFalse(disableableInitializationConfiguration.IsMatch(null, " "));
		}

		[TestMethod]
		public void IsMatch_IfThePatternParameterIsNullAndTheValueParameterIsTest_ShouldReturnFalse()
		{
			var disableableInitializationConfiguration = new DisableableInitializationConfiguration();

			Assert.IsFalse(disableableInitializationConfiguration.IsMatch(null, "Test"));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		[SuppressMessage("Design", "CA1031:Do not catch general exception types")]
		public void IsMatch_IfTheValueParameterIsNull_ShouldThrowAnArgumentNullException()
		{
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
		public void Settings_ShouldIncludeTheEntriesFromApplicationSettingsWithTheRightPrefix()
		{
			Assert.IsFalse(new DisableableInitializationConfiguration().Settings.Any());

			Assert.IsFalse(new DisableableInitializationConfiguration(new NameValueCollection {{"Test", "true"}}).Settings.Any());

			Assert.IsFalse(new DisableableInitializationConfiguration(new NameValueCollection {{"da21c762-1466-4e0b-b4c6-25540fe0e8b4", "true"}}).Settings.Any());

			Assert.IsTrue(new DisableableInitializationConfiguration(new NameValueCollection {{DisableableInitializationConfiguration.KeyPrefix, "true"}}).Settings.Any());
		}

		[TestMethod]
		public void WildcardCharacter_Test()
		{
			Assert.AreEqual('*', new DisableableInitializationConfiguration().WildcardCharacter);
		}

		#endregion
	}
}