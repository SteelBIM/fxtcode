#pat
A data formatter. Data to be formatted is described by format specifiers of a
certain flavor. Currently supported flavors:

+ [Java](https://github.com/mpecherstorfer/pat/blob/master/lib/flavors/java.md) (java.util.Formatter)

##Node environment
### Installation

        $ npm install pat

### Usage

        var pat = require('pat'),
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

## Browser environment: AMD

        //Optionally configure require
        require.config({
            //Default module directory, relative to the file including require.js
            baseUrl: 'js/lib',
            //Specific module directories, relative to baseUrl
            paths: {
                pat: 'pat/lib'
            }
        });
        //Include pat and format...
        require(['pat/pat'], function(pat) {
            var fmt = pat.Formatter.format,
                localizedDateString;
        
            //Use default culture (en-US) and default flavor (java)
            localizedDateString = fmt('Current date: %#tc', new Date());
            console.log(localizedDateString);
        
            //Specific culture
            pat.Formatter.options({
                cultureId: 'deAT'
            }, function() {
                localizedDateString = fmt(
                    'Aktuelles Datum: %1$#tA, %1$#te. %1$#tB %1$#tY, %1$#tT',
                    new Date());
                console.log(localizedDateString);
            });
        });

## Browser environment: global scope
Include the cultures and flavors to use (any order). Make sure to include pat.js
after all the culture and flavor scripts.

        <script type="text/javascript" src="pat/lib/cultures/enUS.js"></script>
        <script type="text/javascript" src="pat/lib/flavors/java.js"></script>
        <script type="text/javascript" src="pat/lib/pat.js"></script>
        <script type="text/javascript">
            var fmt = pat.Formatter.format,
                localizedDateString = fmt('Current date: %#tc', new Date());
            console.log(localizedDateString);
        </script>

+ If a single culture file is included, that culture is used as the default culture.
When several culture files are included, the default culture is 'en-US' or, if not
included, the culture represented by the first included culture file.

+ If a single flavor file is included, that flavor is used as the default flavor.
When several flavor files are included, the default flavor is 'java' or, if not
included, the flavor represented by the first included flavor file.

## Multiple formatters
To create additional formatters apply Formatter as a constructor function:

        fmt = new Formatter();
        fmt.options({ cultureId: 'deAT' });
        Formatter.format(...) //Default formatter instance
        fmt.format(...) //Specific instance

## Formatter options
Getter/setter: ```Formatter.options()``` or ```formatterInstance.options()```

The setter expects an object with one or more of the following properties:

+ ```cultureId```: Id of the culture module to use, defaults to ```'enUS'```.
+ ```flavorId```: Id of the flavor module to use, defaults to ```'java'```.
+ ```lineSeparator```: Line separator to use when parsing the line separator format
  specifier (e.g. '%n'). Defaults to '\n'.

## Flavor documentation
[Java flavored format specifiers](https://github.com/mpecherstorfer/pat/blob/master/lib/flavors/java.md)

