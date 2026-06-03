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
- Nullable Reference Types: Enable Nullable Reference Types

## Strategy
**Selected**: All-At-Once
**Rationale**: 3 SDK-style projects, a shallow dependency graph, and a focused framework consolidation request make a single coordinated retargeting pass the best fit.

### Execution Constraints
- Single atomic upgrade pass across all projects; do not phase by dependency tier.
- Update all project files before resolving package and source compatibility issues.
- Restore and build after the coordinated retargeting, then fix all remaining errors and warnings in the modified projects.
- Run the full solution test suite only after the atomic project and package updates build cleanly.
- Keep CPM deferred; if the solution lands on a stable common TFM set, record CPM as a post-migration recommendation rather than adding it during this pass.

## User Preferences
### Technical Preferences
- Nullable reference types: Try enabling them if the required fixes stay reasonably small; otherwise prioritize the framework retargeting work.
