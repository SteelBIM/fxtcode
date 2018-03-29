#
# Runs a mocha test suite.
#
# Usage:
# 	make test                       run all test suites
# 	make test suite=pat             run ./test/pat.js
# 	make test suite=flavors/java    run ./test/flavors/java.js
#   #set mocha options
# 	make test suite=pat options="--reporter list"
#	#complement default options (as specified in config.sh)
#	make test suite=pat addOptions="--globals module,define"
#
test:
	./scripts/test.sh "$(suite)" "$(options)" "$(addOptions)"
	
pat_test:
	make test suite=pat addOptions="--globals module,define"

cultures_test:
	make test suite=cultures
	
java_test:
	make test suite=flavors/java
	
java_formatter_test:
	make test suite=flavors/java addOptions="--grep Java-Formatter"
	
java_scanner_test:
	make test suite=flavors/java addOptions="--grep Java-Scanner"
	
java_parser_test:
	make test suite=flavors/java addOptions="--grep Java-Parser"

#
# Debugs a mocha test suite. Will break at the first line of the mocha
# script. To pause script execution at the next break point defined by
# the 'debugger' statements in your code do the following:
#
# 	1. Step out of current function to exit mocha's module function
#	2. Step out of current function to exit node's Module.prototype._compile
#	3. Resume to next break point
# 	=> In Chrome: Shift+F11, Shift+F11, F8
#
# Usage: make debug (same options as in make test)
#
debug:
	./scripts/debug.sh "$(suite)" "$(options)" "$(addOptions)"

debug_pat:
	make debug suite=pat addOptions="--globals module,define"

debug_java:
	make debug suite=flavors/java

#
# Creates unified code count metrics.
#
ucc:
	./scripts/create_metrics.sh "$(files)" "$(viewOnly)"

ucc_all:
	make ucc files="../lib ../test"

ucc_temp:
	make ucc files="../lib ../test" viewOnly="true"

#
# Create code documentation.
#
doc:
	./scripts/create_doc.sh

#
# YUI doc server mode.
#
livedoc:
	./scripts/livedoc.sh


.PHONY: test debug ucc doc livedoc
