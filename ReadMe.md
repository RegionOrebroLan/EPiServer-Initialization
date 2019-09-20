# EPiServer-Initialization

Custom EPiServer-initialization.

[![NuGet](https://img.shields.io/nuget/v/RegionOrebroLan.EPiServer.Initialization.svg?label=NuGet)](https://www.nuget.org/packages/RegionOrebroLan.EPiServer.Initialization)

## Features

- [RegionOrebroLan.EPiServer.Initialization.ContentProviderConfigurationInitialization](/Source/Project/ContentProviderConfigurationInitialization.cs)
- [RegionOrebroLan.EPiServer.Initialization.ContentProviderInitialization](/Source/Project/ContentProviderInitialization.cs)
- [RegionOrebroLan.EPiServer.Initialization.DataInitialization](/Source/Project/DataInitialization.cs)
	- [InitializeData](/Source/Project/DataInitialization.cs#L124)
    - [InitializeDatabase](/Source/Project/DataInitialization.cs#L126)
    - [InitializeDataDirectory](/Source/Project/DataInitialization.cs#L136)
- [RegionOrebroLan.EPiServer.Initialization.DefaultSiteContentInitialization](/Source/Project/DefaultSiteContentInitialization.cs)
- [RegionOrebroLan.EPiServer.Initialization.ServiceRegistration](/Source/Project/ServiceRegistration.cs)
- [RegionOrebroLan.EPiServer.Initialization.ServiceScanner](/Source/Project/ServiceScanner.cs)

### Disable features

You can disable initialization by adding appSettings-keys. This applies to initialization implementing [RegionOrebroLan.EPiServer.Initialization.DisableableInitialization](/Source/Project/DisableableInitialization.cs).

Disable all initialization implementing [RegionOrebroLan.EPiServer.Initialization.DisableableInitialization](/Source/Project/DisableableInitialization.cs):

	<configuration>
		<appSettings>
			<add key="da21c762-1466-4e0b-b4c6-25540fe0e8b4:*" value="false" />
		</appSettings>
	</configuration>

Disable [RegionOrebroLan.EPiServer.Initialization.DataInitialization](/Source/Project/DataInitialization.cs):

	<configuration>
		<appSettings>
			<add key="da21c762-1466-4e0b-b4c6-25540fe0e8b4:*.DataInitialization" value="false" />
		</appSettings>
	</configuration>

Disable all except [DataDirectory-initialization](/Source/Project/DataInitialization.cs#L136):

	<configuration>
		<appSettings>
			<add key="da21c762-1466-4e0b-b4c6-25540fe0e8b4:*" value="false" />
			<add key="da21c762-1466-4e0b-b4c6-25540fe0e8b4:RegionOrebroLan.EPiServer.Initialization.DataInitialization:InitializeDataDirectory" value="true" />
		</appSettings>
	</configuration>

## Development

### Integration-tests

**Do not clone this repository to a path longer than 76 characters**.

- This path should work: **C:\Data\Projects\RegionOrebroLan\EPiServer-Initialization** (57 characters)

In the integration-tests, a database is created, leading to the following files are created under the cloned solution-directory:
- **\Source\Integration-tests\App_Data\EPiServer.mdf** (48 characters)
- **\Source\Integration-tests\App_Data\EPiServer_log.ldf** (52 characters, 52 + 76 = 128)

Database-names and database-filenames can not be longer than 128 characters. You can read more here: [CREATE DATABASE/Arguments](https://docs.microsoft.com/en-us/sql/t-sql/statements/create-database-transact-sql?view=sql-server-2017#arguments).

Therefore, if the path to this solution is to long, the integration-tests may fail.