#!/bin/bash
#
# Saves code metrics collected by Unified Code Count.
#

# change directory to that containing this script
cd "$(dirname "$0")"

# import configuration
source config.sh

# check UCC installation
if [ ! -e $ucc ]
then
	echo ERROR: MISSING UCC INSTALLATION
	echo Visit http://sunset.usc.edu/research/CODECOUNT/ to get more information.
	exit 1
fi

# arguments
files=$1
view=$2
tmpFile="$uccTargetDir/tmp.txt"
tmpDir="$uccTargetDir/tmp"

# create UCC filelist
find $files -type f > $tmpFile

# create metric files or view metrics
if [ $view ]
then
	$ucc -i1 $tmpFile -outdir $tmpDir -ascii
	echo
	echo
	cat "$tmpDir/$uccResultName"
	rm -r $tmpDir
else
	$ucc -i1 $tmpFile -outdir $uccTargetDir -ascii
fi
rm $tmpFile
