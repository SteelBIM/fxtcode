#!/bin/bash
#
# Views the project documentation in the specified browser in
# YUI-doc's server mode
#

# change directory to that containing this script
cd "$(dirname "$0")"

# import configuration and library
source config.sh

# create code documentation
rm -r $docDir
mkdir $docDir
node $yuiDoc -o $docDir $codeBaseDir


#node ./node_modules/yuidocjs/lib/cli --o './doc' './lib'