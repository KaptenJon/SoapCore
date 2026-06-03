# 02-solution-retargeting: Retarget all projects and align package conditions

Update SoapCore, SoapCore.Tests, and SoapCore.Benchmark so the solution keeps only the supported TFMs requested for this repository: netstandard2.0, netstandard2.1, net8.0, and net10.0. This task removes netcoreapp3.1 from all project files, adds net10.0 where appropriate, makes net10.0 the default modern target, and rewrites conditional ItemGroups so package and framework references match the retained TFM set.

This is the core task because the assessment identified target framework changes in every project, 10 package upgrades, and source or binary compatibility issues across all 3 projects. The work also includes the user-approved nullable experiment: enable nullable reference types only if the warning fallout stays reasonably small while preserving progress on the framework migration.

**Done when**: All project files reference only the retained TFMs, net10.0 has been added where applicable, conditional package logic matches the new TFM set, nullable has been enabled only if manageable, and the updated solution builds cleanly with no warnings in the modified projects.
