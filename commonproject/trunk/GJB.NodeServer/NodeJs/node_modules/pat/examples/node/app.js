/*global require:true*/
/*jslint sloppy:true*/

var pat = require('../../lib/pat.js'),
    fmt = pat.Formatter.format,
    localizedDateString;

//Optionally set a culture and/or a flavor.
//Default flavor is 'java', default culture 'enUS'.
pat.Formatter.options({
    cultureId: 'deAT'
});

localizedDateString = fmt(
    'Aktuelles Datum: %1$#tA, %1$#te. %1$#tB %1$#tY, %1$#tT',
    new Date());

console.log(localizedDateString);