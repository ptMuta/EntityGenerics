#!/usr/bin/env bash
source ~/.dnx/dnvm/dnvm.sh

SOURCES=(
  'src/EntityGenerics.Annotations'
  'src/EntityGenerics.Core.Abstractions'
  'src/EntityGenerics.Core'
)

TESTS=(
  'test/EntityGenerics.Core.UnitTests'
)

get_sources() {
  echo $(IFS=' '; echo "${SOURCES[*]}")
}

get_tests() {
  echo $(IFS=' '; echo "${TESTS[*]}")
}

get_all() {
  echo $(get_sources) $(get_tests)
}

update_runtimes_and_restore() {
  echo 'Updating to project CoreCLR version...'
  dnvm install latest -r coreclr -a x64 -alias default >/dev/null
  echo 'Updating to project Mono version...'
  dnvm install latest -r mono -a x64 >/dev/null
  echo 'Restoring packages...'
  dnu restore >/dev/null
}

build_framework() {
  dnu build $(get_all) --framework $1 --configuration $2 --out $3
}

build() {
  update_runtimes_and_restore
  echo 'Building .NET CoreCLR version...'
  build_framework dnxcore50 $1 $2 >/dev/null
  echo 'Building .NET 451 version...'
  build_framework dnx451 $1 $2 >/dev/null
}

test() {
  update_runtimes_and_restore
  for TEST in $TESTS
  do
    cd $TEST
    dnx test
  done
}

update_version() {
  for SOURCE in ${SOURCES[@]}
  do
    sed -i "s/.*\"version\".*/  \"version\": \""$1"\",/" $SOURCE/project.json
  done
}

case "$1" in
  update_version)
    update_version $2
    ;;
  build)
    update_version $2
    build $3 $4
    ;;
  test)
    test
    ;;
  build_and_test)
    update_version $2
    build $3 $4
    test
    ;;
  *)
    echo "Usage: {update_version|build|test|build_and_test}"
    exit 1
    ;;
esac
exit $?