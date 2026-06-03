# .NET Version Upgrade Progress

## Overview

Upgrading the SoapCore solution to use net10.0 as the default modern target while retaining only netstandard2.0, netstandard2.1, net8.0, and net10.0 where applicable. The work will be executed with an all-at-once strategy across the 3 SDK-style projects.

**Progress**: 0/3 tasks complete <progress value="0" max="100"></progress> 0%

## Tasks

- 🔄 01-toolchain-readiness: Verify .NET 10 toolchain and solution prerequisites ([Content](tasks/01-toolchain-readiness/task.md))
- 🔲 02-solution-retargeting: Retarget all projects and align package conditions
- 🔲 03-validation-and-tests: Validate the upgraded solution and capture follow-up guidance