# Upgrade Options — SoapCore

Assessment: 3 SDK-style projects, current TFMs include netcoreapp3.1/net8.0/netstandard2.x, 2 incompatible packages, 2,134 source-incompatible APIs, and 26k+ LOC.

## Strategy

### Upgrade Strategy
A small 3-project dependency graph keeps this change set manageable, so a single coordinated pass is the best fit.

| Value | Description |
|-------|-------------|
| **All-at-Once** (selected) | Upgrade all projects together in one coordinated pass. |
| Top-Down | Upgrade entry points first, keeping shared libraries temporarily multi-targeted during the transition. |

## Project Structure

### Package Management
The solution uses per-project PackageReference files without CPM, but the current request is focused on TFM consolidation rather than package centralization.

| Value | Description |
|-------|-------------|
| Central Package Management (CPM) | Create Directory.Packages.props and centralize package versions across projects. |
| **Per-Project (defer CPM to post-migration)** (selected) | Keep package versions in each project during this upgrade and reconsider CPM afterward. |

## Compatibility

### Unsupported API Handling
The assessment found source and binary compatibility issues, but this solution is small enough to fix them directly while updating the projects.

| Value | Description |
|-------|-------------|
| **Fix Inline** (selected) | Resolve compatibility issues in the same upgrade tasks without leaving stubbed follow-up work. |
| Defer Complex Changes | Stub complex API changes temporarily and create follow-up subtasks for them. |

## Modernization

### Nullable Reference Types
You asked to try nullable if the work stays small, so it will be enabled as part of this pass and can be revisited if it expands the scope too much.

| Value | Description |
|-------|-------------|
| Leave Disabled | Keep nullable reference types disabled during this upgrade. |
| **Enable Nullable Reference Types** (selected) | Enable nullable reference types as part of this upgrade and fix resulting warnings. |
