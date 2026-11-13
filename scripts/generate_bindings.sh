
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
