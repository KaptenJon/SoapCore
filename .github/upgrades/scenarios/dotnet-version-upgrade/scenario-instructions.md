# .NET Version Upgrade

## Preferences
- **Flow Mode**: Automatic
- **Target Framework**: net10.0
- **Supported Target Frameworks to Keep**: netstandard2.0, netstandard2.1, net8.0, net10.0

## Source Control
- **Source Branch**: Modernize-from-scratch
- **Working Branch**: Modernize-from-scratch
- **Commit Strategy**: Single Commit at End
- **Branch Sync**: Auto (Merge)

## Upgrade Options
**Source**: .github/upgrades/scenarios/dotnet-version-upgrade/upgrade-options.md

### Strategy
- Upgrade Strategy: All-at-Once

### Project Structure
- Package Management: Per-Project (defer CPM to post-migration)

### Compatibility
- Unsupported API Handling: Fix Inline

### Modernization
- Nullable Reference Types: Leave Disabled

## Strategy
**Selected**: All-At-Once
**Rationale**: 3 SDK-style projects, a shallow dependency graph, and a focused framework consolidation request make a single coordinated retargeting pass the best fit.

### Execution Constraints
- Single atomic upgrade pass across all projects; do not phase by dependency tier.
- Update all project files before resolving package and source compatibility issues.
- Restore and build after the coordinated retargeting, then fix all remaining errors and warnings in the modified projects unless the user explicitly defers an existing warning backlog.
- Run the full solution test suite only after the atomic project and package updates build cleanly.
- Keep CPM deferred; if the solution lands on a stable common TFM set, record CPM as a post-migration recommendation rather than adding it during this pass.

## User Preferences
### Technical Preferences
- Nullable reference types: Try enabling them if the required fixes stay reasonably small; otherwise prioritize the framework retargeting work.
- Existing StyleCop warning backlog: Defer cleanup for now and continue the upgrade work.
- StyleCop cleanup ordering: Take StyleCop warnings last and, if cleaned, place that work in its own commit.
- Test-project compatibility cleanup: Remove obsolete `#if !NETCOREAPP3_0_OR_GREATER` branches from `SoapCore.Tests`, but keep compatibility code in `SoapCore` where `netstandard2.0` support may still require it.

## Key Decisions Log
- Nullable reference types were evaluated on SoapCore and deferred after the build surfaced 411 nullable errors in the library, which exceeded the agreed scope for this pass.
- Existing StyleCop warnings in SoapCore were accepted as deferred backlog for this pass so the framework retargeting can continue.
- Any eventual StyleCop cleanup should be handled after the framework retargeting work and separated into its own commit.
- Obsolete pre-.NET Core 3.0 conditional branches may be removed from `SoapCore.Tests` because the test project now targets only `net8.0` and `net10.0`; equivalent compatibility branches in `SoapCore` remain in scope for library compatibility.

## Build Tool Decisions
- **SoapCore.sln**: dotnet build (all projects are SDK-style and current work stays within modern .NET/.NET Standard project types)
- **SoapCore.csproj**: dotnet build (SDK-style library with no special MSBuild-only requirements detected)
- **SoapCore.Tests.csproj**: dotnet build (SDK-style test project)
- **SoapCore.Benchmark.csproj**: dotnet build (SDK-style console benchmark project)
