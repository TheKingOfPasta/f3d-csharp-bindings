
#!/bin/sh
set -e

RESOURCE_DIR=$(clang -print-resource-dir)

set --
for file in include/f3d/*.h; do
    set -- "$@" --file "$file"
done

dotnet tool run ClangSharpPInvokeGenerator \
  "$@" \
  --namespace F3D.Native \
  --output ./src/F3D/Native/NativeMethods.g.cs \
  --method-class-name NativeMethods \
  --library-path libf3d_c_api \
  --generate macro-bindings \
  --generate helper-types \
  --resource-directory "$RESOURCE_DIR" \
  --define-macro F3D_STATIC_DEFINE \
  -x c++ \
  -I ./include/f3d

# The generator emits sbyte* for every char*. Turn the C string inputs of functions into C# strings,
# converted to UTF-8 by .NET:
#   const char*  -> string
#   const char** -> string[] (LPStr: .NET does not allow LPUTF8Str here. UTF-8 on Linux/macOS, ANSI on Windows)
# Functions with "destroy" in their name keep pointers: they free strings that F3D allocated.
# Return values, char* outputs and struct fields also keep pointers.
OUT=./src/F3D/Native/NativeMethods.g.cs
sed -E '/static extern/{
/destroy/!s/\[NativeTypeName\("const char \*"\)\] sbyte\* /[MarshalAs(UnmanagedType.LPUTF8Str)] string /g
/destroy/!s/\[NativeTypeName\("const char \*\*"\)\] sbyte\*\* /[MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] /g
}' "$OUT" > "$OUT.tmp"
mv "$OUT.tmp" "$OUT"
