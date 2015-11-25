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

update_version() {
  echo 'Updating versions...'
  for SOURCE in ${SOURCES[@]}
  do
    echo "Updating version for ${SOURCE}..."
    sed -i "s/.*\"version\".*/  \"version\": \""$1"\",/" $SOURCE/project.json
  done
}

update_environment() {
  echo 'Updating to project CoreCLR version...'
  dnvm install latest -r coreclr -a x64 -alias default >/dev/null
  echo 'Updating to project Mono version...'
  dnvm install latest -r mono -a x64 >/dev/null -alias mono
  echo 'Restoring packages...'
  dnu restore >/dev/null
}

build_framework() {
  dnu build $(get_all) --framework $1 --configuration $2
}

build() {
  echo 'Building .NET 451 version...'
  dnvm use mono >/dev/null
  build_framework dnx451 $1 >/dev/null
  echo 'Building .NET CoreCLR version...'
  dnvm use default >/dev/null
  build_framework dnxcore50 $1 >/dev/null
}

test() {
  echo 'Running tests...'
  for TEST in ${TESTS[@]}
  do
    echo "Running tests for ${TEST}..."
    cd $TEST
    dnx test
  done
}

pack() {
  echo 'Packaging projects...'
  for SOURCE in ${SOURCES[@]}
  do
    echo "Packaging ${SOURCE}..."
    dnvm use mono >/dev/null
    dnu pack $SOURCE --configuration $1 --framework dnx451 >/dev/null
    dnvm use default >/dev/null
    dnu pack $SOURCE --configuration $1 --framework dnxcore50 >/dev/null
  done
  echo 'Packing build as a NuGet package...'
}

publish() {
  nuget setApiKey $3
  echo 'Publishing packages to NuGet...'
  for SOURCE in ${SOURCES[@]}
  do
    echo "Publishing ${SOURCE}..."
	nuget push "${SOURCE#src/}/bin/$1/${SOURCE#src/}.$2.nupkg"
  done
  echo 'Packages published to NuGet'
}

case "$1" in
  update_environment)
    update_environment
    ;;
  update_version)
    update_version $2
    ;;
  build)
    build $2 $3
    ;;
  test)
    test
    ;;
  pack)
    pack $2
    ;;
  publish)
    publish $2 $3 $4
	;;
  *)
    echo "Usage: {update_environment|update_version|build|test|pack}"
    exit 1
    ;;
esac
exit $?
