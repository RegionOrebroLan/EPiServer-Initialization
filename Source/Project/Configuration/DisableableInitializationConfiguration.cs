using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace RegionOrebroLan.EPiServer.Initialization.Configuration
{
	public class DisableableInitializationConfiguration : IDisableableInitializationConfiguration
	{
		#region Fields

		private IReadOnlyDictionary<string, bool> _settings;
		private const char _wildcardCharacter = '*';
		public const string KeyPrefix = "da21c762-1466-4e0b-b4c6-25540fe0e8b4:";

		#endregion

		#region Constructors

		public DisableableInitializationConfiguration() : this(ConfigurationSystem.ApplicationSettings()) { }

		public DisableableInitializationConfiguration(NameValueCollection applicationSettings)
		{
			this.ApplicationSettings = applicationSettings ?? new NameValueCollection();
		}

		#endregion

		#region Properties

		protected internal virtual NameValueCollection ApplicationSettings { get; }

		protected internal virtual IReadOnlyDictionary<string, bool> Settings
		{
			get
			{
				// ReSharper disable InvertIf
				if(this._settings == null)
				{
					var settings = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

					foreach(var key in this.ApplicationSettings.AllKeys)
					{
						if(key == null || !key.StartsWith(KeyPrefix, StringComparison.OrdinalIgnoreCase))
							continue;

						if(!bool.TryParse(this.ApplicationSettings[key], out var enabled))
							enabled = true;

						settings.Add(key.Substring(KeyPrefix.Length), enabled);
					}

					this._settings = new ReadOnlyDictionary<string, bool>(settings);
				}
				// ReSharper restore InvertIf

				return this._settings;
			}
		}

		protected internal virtual char WildcardCharacter => _wildcardCharacter;

		#endregion

		#region Methods

		protected internal virtual Regex CreateRegularExpression(string pattern)
		{
			pattern = "^" + Regex.Escape(pattern ?? string.Empty).Replace("\\*", ".*") + "$";

			return new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
		}

		public virtual bool? IsDisabled(string initializationKey)
		{
			if(initializationKey == null)
				throw new ArgumentNullException(nameof(initializationKey));

			// ReSharper disable LoopCanBeConvertedToQuery
			foreach(var key in this.Settings.Keys.Reverse())
			{
				if(this.IsMatch(key, initializationKey))
					return !this.Settings[key];
			}
			// ReSharper restore LoopCanBeConvertedToQuery

			return null;
		}

		protected internal virtual bool IsMatch(string pattern, string value)
		{
			if(value == null)
				throw new ArgumentNullException(nameof(value));

			return this.CreateRegularExpression(pattern).IsMatch(value);
		}

		#endregion
	}
}