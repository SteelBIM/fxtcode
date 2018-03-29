/*global requirejs*/
/*jslint sloppy:true*/

(function() {
    
    var fmt = pat.Formatter.format,
        fstr = 'Aktuelles Datum: %1$#tA, %1$#te. %1$#tB %1$#tY, %1$#tT',
        date = new Date('2012-01-01T00:00Z');
    if (console) {
        console.log(pat.Formatter.format(fstr, date));
        pat.Formatter.options({ cultureId: 'deDE' });
        console.log(pat.Formatter.format(fstr, date));
    }
    
}());