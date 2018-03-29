#!/bin/bash
#
# Configuration.
#

#####################################################################
# General settings

# source code root directory
codeBaseDir="../lib"

# webkit browser used for debugging
webkitBrowser="chromium-browser"

# secondary browser used to display code documentation
secondaryBrowser="firefox"

#####################################################################
# Unit testing
#

# directory containing the test suites
testDir="../test"

# this project's test suites
testSuites=(
	"../test/pat.js"
	"../test/flavors/java.js"
)

# path to mocha
mocha="../node_modules/mocha/bin/mocha"

# default mocha options
mochaOptions="--bail --reporter list"

# mocha options to use when running all test suites
mochaRunAllOptions="$mochaDefaultOptions --globals module,define"

#####################################################################
# Debugging

# path to node-inspector
debugInterface="../node_modules/node-inspector/bin/inspector.js"

# browser
debugFrontEnd=$webkitBrowser

# debug front end URL
debugUrl="http://127.0.0.1:8080/debug?port=5858"

#####################################################################
# Code documentation
#

# doc root directory
docDir="../doc"

# YUI doc command line interface
yuiDoc="../node_modules/yuidocjs/lib/cli"

# YUI doc server: protocol, IP address, and port
yuiDocServerUrl="http://127.0.0.1"
yuiDocServerPort="3000"

# YUI doc viewer
yuiDocViewer=$secondaryBrowser

#####################################################################
# Code metrics

# path to unified code count
ucc="../metrics/UCC2010.07/UCC"

# directory containing the metrics collected by UCC 
uccTargetDir="../metrics"

# UCC file name to display in temp mode
uccResultName="JavaScript_outfile.txt"
