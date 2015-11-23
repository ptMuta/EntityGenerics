#!/usr/bin/env bash
source ~/.dnx/dnvm/dnvm.sh

PROJECTS=(
  'src/EntityGenerics.Annotations'
  'src/EntityGenerics.Core.Abstractions'
  'src/EntityGenerics.Core'
  'test/EntityGenerics.Core.UnitTests'
)

build_framework() {
  dnu build $(IFS=' '; echo "${PROJECTS[*]}") --framework $1
}

dnvm install latest -r coreclr -a x64 -alias default
dnvm install latest -r mono -a x64
dnu restore
build_framework dnxcore50
build_framework dnx451
