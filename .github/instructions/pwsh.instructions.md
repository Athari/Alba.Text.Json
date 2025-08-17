---
applyTo: '**/*.{ps1,psm1}'
description: ''
---

# Coding style

* Use modern PowerShell 7.4 features.

   * Use ternary operator instead of `if` for conditional assignments: `a ? b : c`.

   * Use null-coalescing operators: `$a ?? $b.c ?? 'foo'` and `$a ??= $b.c`.

   * Use null-conditional operators (refer to variables as `${name}` if needed): `${a}?.b?.c` and `(Get-ChildItem)?[2]`.

   * Use pipeline chain operators: `Write-Output 'A' && Write-Output 'B'`.

   * Use redirects instead of `Out-File`: `command -arg >> $env:out.txt`.

   * Import namespaces when using types in it: `using namespace System.Diagnostics.CodeAnalysis`.

* Always use the following aliases, but use full names for all other cmdlets: `foreach`, `group`, `select`, `sort`, `tee`, `where`.