#!/bin/bash
#
# Views the project documentation in the specified browser in
# YUI-doc's server mode
#

# change directory to that containing this script
cd "$(dirname "$0")"

# import configuration and library
source config.sh

# start YUI doc server
node $yuiDoc --server $yuiDocServerPort $codeBaseDir &

# display documentation
sleep 0.8
$yuiDocViewer "$yuiDocServerUrl:$yuiDocServerPort"
