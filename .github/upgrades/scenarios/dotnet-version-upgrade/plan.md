# .NET Version Upgrade Plan

## Overview

**Target**: Retarget the SoapCore solution to net10.0 as the default modern target while keeping only netstandard2.0, netstandard2.1, net8.0, and net10.0 where they apply.
**Scope**: 3 SDK-style projects, ~26k LOC, with per-project PackageReference management, legacy netcoreapp3.1 targets to remove, and API/package compatibility fixes identified in the .NET 10 assessment.

### Selected Strategy
**All-At-Once** — All projects upgraded simultaneously in a single operation.
**Rationale**: 3 projects, all already on modern .NET or .NET Standard, with a simple dependency structure (SoapCore consumed by tests and benchmark) and no .NET Framework migration boundary.

## Tasks

### 01-toolchain-readiness: Verify .NET 10 toolchain and solution prerequisites

Validate that the repository can be upgraded on the current machine before any project edits begin. This task covers confirming .NET 10 SDK availability, checking whether any global.json constraints or restore settings would block net10.0, and confirming the solution is ready for a coordinated modern-to-modern retargeting pass.

The assessment shows an SDK-style solution with 3 projects and no legacy project-system conversion work, so this task is intentionally narrow: remove toolchain uncertainty up front and document any prerequisite adjustments needed before the project files are rewritten.

**Done when**: .NET 10 SDK support is verified for the workspace, any global.json or toolchain blockers are resolved or documented, and the solution is ready for the retargeting task.

---

### 02-solution-retargeting: Retarget all projects and align package conditions

Update SoapCore, SoapCore.Tests, and SoapCore.Benchmark so the solution keeps only the supported TFMs requested for this repository: netstandard2.0, netstandard2.1, net8.0, and net10.0. This task removes netcoreapp3.1 from all project files, adds net10.0 where appropriate, makes net10.0 the default modern target, and rewrites conditional ItemGroups so package and framework references match the retained TFM set.

This is the core task because the assessment identified target framework changes in every project, 10 package upgrades, and source or binary compatibility issues across all 3 projects. The work also includes the user-approved nullable experiment: enable nullable reference types only if the warning fallout stays reasonably small while preserving progress on the framework migration.

**Done when**: All project files reference only the retained TFMs, net10.0 has been added where applicable, conditional package logic matches the new TFM set, nullable has been enabled only if manageable, and the updated solution builds cleanly with no warnings in the modified projects.

---

### 03-validation-and-tests: Validate the upgraded solution and capture follow-up guidance

Run the complete solution validation pass after the atomic retargeting is complete. This task covers full restore/build/test execution, verifying the tests project against the upgraded SoapCore library, and recording any intentionally deferred follow-up items such as Central Package Management adoption after the repository settles on the new TFM set.

The assessment flagged deprecated and upgrade-recommended packages in the test and benchmark projects, so this task closes the loop by confirming the repository is stable after the coordinated retargeting rather than assuming package and API updates were sufficient.

**Done when**: The full solution restore/build/test flow succeeds, the upgrade result is verified on the retained target frameworks, and any deferred post-migration recommendations are documented.