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
  dnu build $(get_all) --framework $1 --configuration $2
}

run_build() {
  echo 'Building .NET CoreCLR version...'
  build_framework dnxcore50 $1 $2 >/dev/null
  echo 'Building .NET 451 version...'
  build_framework dnx451 $1 $2 >/dev/null
}

run_test() {
  echo 'Running tests...'
  for TEST in $TESTS
  do
    echo "Running tests for ${TEST}..."
    cd $TEST
    dnx test
  done
}

run_update_version() {
  echo 'Updating versions...'
  for SOURCE in ${SOURCES[@]}
  do
    echo "Updating version for ${SOURCE}..."
    sed -i "s/.*\"version\".*/  \"version\": \""$1"\",/" $SOURCE/project.json
  done
}

case "$1" in
  update_version)
    run_update_version $2
    ;;
  build)
    update_runtimes_and_restore
    run_update_version $2
    run_build $3 $4
    ;;
  test)
    update_runtimes_and_restore
    run_test
    ;;
  build_and_test)
    update_runtimes_and_restore
    run_update_version $2
    run_build $3 $4
    run_test
    ;;
  *)
    echo "Usage: {update_version|build|test|build_and_test}"
    exit 1
    ;;
esac
exit $?
