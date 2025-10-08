﻿using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotNetReleaser.Changelog;
using DotNetReleaser.Configuration;
using DotNetReleaser.Logging;

namespace DotNetReleaser.Tests;

public class MockDevHosting : IDevHosting
{
    public MockDevHosting(ISimpleLogger logger, DevHostingConfiguration configuration)
    {
        Logger = logger;
        Configuration = configuration;
        ApiToken = string.Empty;
        ApiTokenUsage = string.Empty;
    }
    
    public ISimpleLogger Logger { get; }

    public DevHostingConfiguration Configuration { get; }
    public string ApiToken { get; }
    public string ApiTokenUsage { get; }

    public Task<bool> Connect()

    {
        return Task.FromResult(true);
    }

    public Task CreateOrUpdateGist(string gistId, string fileName, string content)
    {
        return Task.CompletedTask;
    }

    public Task<List<ReleaseVersion>> GetAllReleaseTags(string user, string repo, string tagPrefix)
    {
        throw new System.NotImplementedException();
    }

    public Task CreateOrUpdateRelease(string user, string repo, ReleaseVersion version, ChangelogResult? changelog)
    {
        return Task.CompletedTask;
    }
    
    public delegate ChangelogCollection? GetChangesDelegate(IDevHosting hosting, string user, string repo, string tagPrefix, string version);


    public GetChangesDelegate? GetChangesImpl { get; set; }

    public Task<ChangelogCollection?> GetChanges(string user, string repo, string tagPrefix, string version)
    {
        return Task.FromResult(GetChangesImpl?.Invoke(this, user, repo, tagPrefix, version));
    }



    public Task UpdateChangelogAndUploadPackages(string user, string repo, ReleaseVersion version, ChangelogResult? changelog, List<AppPackageInfo> entries, bool enablePublishPackagesInDraft, bool forceUpload)
    {
        return Task.CompletedTask;
    }

    public Task UploadHomebrewFormula(string user, string repo, ProjectPackageInfo projectPackageInfo, string brewFormula)
    {
        return Task.CompletedTask;
    }

    public Task UploadScoopManifest(string user, string repo, ProjectPackageInfo projectPackageInfo, string scoopManifest)
    {
        return Task.CompletedTask;
    }

    private static readonly Regex VersionRegex = new(@"^\d+(\.\d+)+");

    public string GetCompareUrl(string user, string repo, string fromRef, string toRef)
    {
        var fromVersionOrCommit = VersionRegex.IsMatch(fromRef) ? $"{Configuration.VersionPrefix}{fromRef}" : fromRef;
        var toVersionOrCommit = VersionRegex.IsMatch(toRef) ? $"{Configuration.VersionPrefix}{toRef}" : toRef;
        return $"{Configuration.Base}/{user}/{repo}/compare/{fromVersionOrCommit}...{toVersionOrCommit}";
    }

    public string GetDownloadReleaseUrl(string version, string fileEntry)
    {
        return $"{Configuration.Base}/{Configuration.User}/{Configuration.Repo}/releases/download/{Configuration.VersionPrefix}{version}/{Path.GetFileName(fileEntry)}";
    }

    public Task<List<string>> GetBranchNamesForCommit(string user, string repo, string sha)
    {
        return Task.FromResult(new List<string>());
    }
}