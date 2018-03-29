#!/bin/bash
#
# Runs a mocha test suite.
#

# change directory to that containing this script
cd "$(dirname "$0")"

# import configuration
source config.sh

# testsuite to run
testsuite="$1"

# run all test suites or a particular one
if [ $testsuite ]
then
	# overwrite default mocha options with the given options
	if [ "$2" ]
	then mochaOptions="$2"
	fi
	# complement default mocha options with given additional options
	if [ "$3" ]
	then mochaOptions="$mochaOptions $3"
	fi
	$mocha $mochaOptions $testDir/$testsuite".js"
else
	$mocha $mochaRunAllOptions ${testSuites[@]}
fi
