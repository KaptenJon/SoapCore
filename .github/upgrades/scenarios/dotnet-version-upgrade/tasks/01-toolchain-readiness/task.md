# 01-toolchain-readiness: Verify .NET 10 toolchain and solution prerequisites

Validate that the repository can be upgraded on the current machine before any project edits begin. This task covers confirming .NET 10 SDK availability, checking whether any global.json constraints or restore settings would block net10.0, and confirming the solution is ready for a coordinated modern-to-modern retargeting pass.

The assessment shows an SDK-style solution with 3 projects and no legacy project-system conversion work, so this task is intentionally narrow: remove toolchain uncertainty up front and document any prerequisite adjustments needed before the project files are rewritten.

**Done when**: .NET 10 SDK support is verified for the workspace, any global.json or toolchain blockers are resolved or documented, and the solution is ready for the retargeting task.
