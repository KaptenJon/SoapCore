# 03-validation-and-tests: Validate the upgraded solution and capture follow-up guidance

Run the complete solution validation pass after the atomic retargeting is complete. This task covers full restore/build/test execution, verifying the tests project against the upgraded SoapCore library, and recording any intentionally deferred follow-up items such as Central Package Management adoption after the repository settles on the new TFM set.

The assessment flagged deprecated and upgrade-recommended packages in the test and benchmark projects, so this task closes the loop by confirming the repository is stable after the coordinated retargeting rather than assuming package and API updates were sufficient.

**Done when**: The full solution restore/build/test flow succeeds, the upgrade result is verified on the retained target frameworks, and any deferred post-migration recommendations are documented.
