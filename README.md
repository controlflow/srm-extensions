# srm-extensions

An attempt to build less low-level metadata reader library on top of new 'System.Reflection.Metadata' reader from Roslyn.
Hides some complexity of traversing metadata, exposes access to strings (names, namespaces, etc), debugger presentations,
defines equality, provides IL opcodes reader.

Roadmap:
* Complete metadata API
* TypeReference resolution
* Control flow builder
