/*global requirejs*/
/*jslint sloppy:true*/

(function() {
    
    require.config({
        //Default module directory, relative to the file including require.js
        baseUrl: 'js/app',
        //Other module directories, relative to baseUrl
        paths: {
            pat: '../../../../lib'
        }
    });
    require(['pat/pat'], function(pat) {
        var fmt = pat.Formatter.format,
            localizedDateString;
        
        //Use default culture (en-US) and default flavor (java)
        localizedDateString = fmt('Current date: %#tc', new Date());
        if (console) { console.log(localizedDateString); }
        
        //Specific culture
        pat.Formatter.options({
            cultureId: 'deAT'
        }, function() {
            localizedDateString = fmt(
                'Aktuelles Datum: %1$#tA, %1$#te. %1$#tB %1$#tY, %1$#tT',
                new Date());
            if (console) { console.log(localizedDateString); }
        });
    });
    
}());