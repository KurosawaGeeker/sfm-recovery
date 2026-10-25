# License scope and provenance

The root MIT license applies only to original documentation and tools that SFM Recovery contributors have the right to license.

The Unity source snapshot contains reconstructed game code, recovered declarations, adapters, and types associated with third-party dependencies. These files are not represented as the original developer's source release, and publication does not relicense the original game or third-party software under MIT. Their respective rights and license terms remain with their owners. No blanket ownership or redistribution license for the entire Unity tree is claimed.

Original contributor tooling is identified by its provenance, notably the standalone publication/usage utilities in `tools/`. The `SfmMvp`, `MvpBridge`, and Editor recovery helpers were created during this work but may reference third-party APIs and reconstructed code; those references do not change third-party rights.

The source snapshot includes names/types associated with Easy Save (ES3), Unity packages, UniGLTF/VRM, Decal and other components. This is an inventory of dependencies and provenance, not a claim that all third-party licensing has been exhaustively resolved. Consumers must obtain needed assets and dependencies with appropriate rights. Game executables, DLLs, scenes, textures, models, audio, fonts, and save files are deliberately excluded.
