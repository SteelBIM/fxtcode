#!/bin/bash
#
# Debugs a mocha test suite.
#

# change directory to that containing this script
cd "$(dirname "$0")"

# import configuration and library
source config.sh
source lib.sh

# testsuite to run
testsuite="$1"

# delayed calls
delay=0.8 #seconds

# start node-inspector if not already running
if [ ! $(isActiveProcess "node-inspector") ]
then
	$debugInterface &
fi

# run all test suites or a particular one
sleep $delay
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
	$mocha $mochaOptions --debug-brk "../test/"$suite".js" &
else
	$mocha $mochaOptions --debug-brk ${testSuites[@]} &
fi

# start front end
sleep $delay
$debugFrontEnd $debugUrl

# wait until chromium exits before sending SIGTERM to node-inspect,
# mocha, and mocha child processes
pids=$(ps -ef \
	| grep -v grep \
	| grep -v /scripts/debug.sh \
	| grep "node-inspect\|mocha" \
	| awk '{ print $2 }')
echo -n "Sending SIGTERM to the following processes: "
echo $pids
kill $pids
echo "Finished."
