# Java flavored formatter
This module includes an sprintf-style data formatter that parses format specifiers known from ```java.util.Formatter```.

## Format strings
The ```format``` method requires a format string and the arguments to be formatted. A format string includes normal text and format specifiers. The format specifiers describe how to format given arguments.

Example:

        format('Another %s', 'string'); // -> 'Another string'
        format('%.2f', 1/3); // -> '0.33' (rounded to two fractional digits)

## Format specifier syntax

        %[argIndex$][flags][width][.precision]conversion

+ ```argIndex$``` (number): Format specifier for the n-th argument (1$ describes the first argument)
+ ```flags``` (string): Format options
+ ```width``` (number): Minimum width of the resulting string
+ ```.precision``` (number): Limits the width of the resulting string
+ ```conversion``` (string): Format identifier

## Argument index
A format specifier's corresponding argument is defined with a one-based index. The argument index may be defined explicitly, implicitly, or relative to the argument of the previous format specifier.

+ Explicit indexing: The format specifier contains the index followed by the dollar sign.

        format('%1$tH:%1$tM', new Date());

+ Relative indexing is enabled with the symbol '<' and re-uses the argument of the previous format specifier.

        format('%1$tH:%<tM', new Date());

+ Implicit indexing: Any format specifier without an explicit or relative index gets an implicit index.

        format('%tH:%<tM', new Date());

It's possible to combine the three forms of indexing:

        format('%2$s %s %<s %s %3$s', 'One', 'Two', 'Three'); // -> 'Two One One Two Three'

## General format specifiers
<table>
  <thead>
    <tr>
      <td>Conversion</td>
      <td>Description</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>'b', 'B'</td>
      <td>
        Result is 'false' if Boolean(argument) is false, 'true' otherwise. Uppercase when 'B'.
      </td>
    </tr>
    <tr>
      <td>'s', 'S'</td>
      <td>
        Result is the string argument. Uppercase when 'S'.
      </td>
    </tr>
  </tbody>
</table>

*Format specifier details:*

+ General format specifiers may be applied to arguments of any type
+ ```width``` is the resulting string's minimum width (space padding if necessary)
+ ```precision``` is the resulting string's maximum width (truncation if necessary), applied before width

*Applicable flags:*

+ '-': Left justification when space padding (default is right justification)
+ Other flags are ignored

*Example:*

        format('%.1B', undefined); // -> 'F'

## Character format specifiers
<table>
  <thead>
    <tr>
      <td>Conversion</td>
      <td>Description</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>'c', 'C'</td>
      <td>
        Result is a Unicode character. The corresponding argument is a numeric Codepoint (UTF-16 or UCS-2, depending on the environment) or a string. Numeric codepoints are resolved by ```String.fromCharCode```, a string is mapped to it's first character. Uppercase when 'C' and available.
      </td>
    </tr>
  </tbody>
</table>

*Format specifier details:*

+ May be applied to arguments of type ```Number``` and ```String```
+ ```width``` is the resulting string's minimum width (space padding if necessary)
+ ```precision``` is not available and will be ignored if defined

*Applicable flags:*

+ '-': Left justification when space padding (default is right justification)
+ Other flags are ignored

*Example:*

        format('%c', 0x27EB); // -> &#x27EB;

## Integer format specifiers
<table>
  <thead>
    <tr>
      <td>Conversion</td>
      <td>Description</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>'d'</td>
      <td>
        Decimal integer format (culture-specific). 
      </td>
    </tr>
    <tr>
      <td>'o'</td>
      <td>
        Octal integer format.
      </td>
    </tr>
    <tr>
      <td>'x', 'X'</td>
      <td>
        Hexadecimal integer format. Uppercase when 'X'.
      </td>
    </tr>
  </tbody>
</table>

*Format specifier details:*

+ May be applied to arguments of type ```Number```
+ Fractional digits are truncated before applying the format
+ ```width``` is the resulting string's minimum width (space padding if necessary) including signs, digits, grouping separators, radix, and parentheses
+ ```precision``` is not available and will be ignored if defined

*Applicable flags:*

+ '-': Left justification when space padding (default is right justification)
+ '0': Zero padding instead of space padding to guarantee ```width```
+ ',': Include the culture-specific grouping separator (default is to format numbers without the grouping separator)
+ '+': Positive numbers have a leading '+' (default is to format positive numbers without any prefix)
+ ' ': Positive numbers have a leading space (default is to format positive numbers without any prefix)
+ '(': Negative values are enclosed by parantheses (default is to format negative numbers with '-' prefix)

*Example:*

        format('%d', 3.5); // -> '3'
        format('%x', 255); // -> 'ff'

## Floating point format specifiers
<table>
  <thead>
    <tr>
      <td>Conversion</td>
      <td>Description</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>'f'</td>
      <td>
        Decimal number format (culture-specific).
      </td>
    </tr>
    <tr>
      <td>'e', 'E'</td>
      <td>
        Computerized scientific notation (culture-specific).
      </td>
    </tr>
    <tr>
      <td>'g', 'G'</td>
      <td>
        General scientific notation (culture-specific).
      </td>
    </tr>
    <tr>
      <td>'a', 'A'</td>
      <td>
        Hexadecimal exponential notation.
      </td>
    </tr>
  </tbody>
</table>

*Format specifier details:*

+ May be applied to arguments of type ```Number```
+ ```width``` is the resulting string's minimum width (space padding if necessary) including signs, digits, grouping separators, radix, and parentheses
+ If the conversion is 'e', 'E' or 'f', then the precision is the number of digits after the decimal separator (defaults to 6)
+ If the conversion is 'g' or 'G', then the precision is the total number of significant digits in the resulting mantissa after rounding (defaults to 6, precision 0 is replaced by 1)

*Applicable flags:*

+ '-': Left justification when space padding (default is right justification)
+ '0': Zero padding instead of space padding to guarantee ```width```
+ ',': Include the culture-specific grouping separator (default is to format numbers without the grouping separator)
+ '+': Positive numbers have a leading '+' (default is to format positive numbers without any prefix)
+ ' ': Positive numbers have a leading space (default is to format positive numbers without any prefix)
+ '(': Negative values are enclosed by parantheses (default is to format negative numbers with '-' prefix)

*Example:*

        format('%06.3f', 3.14159); // -> '03.142'
        format('%e', 1400); // -> '1.400000e+03'

## Date format specifiers
<p>
<strong>%t</strong> or <strong>%T</strong> (for uppercase result) with one of the following postfixes:
</p>
<table>
  <thead>
    <tr>
      <td>Conversion</td>
      <td>Description</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td colspan="2" align="center">*Time components*</td>
    </tr>
    <tr>
      <td>'H'</td>
      <td>
        Hours of the specified day, from '00' to '23'.
      </td>
    </tr>
    <tr>
      <td>'k'</td>
      <td>
        Hours of the specified day, from '0' to '23'.
      </td>
    </tr>
    <tr>
      <td>'I'</td>
      <td>
        Hours of the specified day, from '01' to '12'.
      </td>
    </tr>
    <tr>
      <td>'l'</td>
      <td>
        Hours of the specified day, from '1' to '12'.
      </td>
    </tr>
    <tr>
      <td>'p'</td>
      <td>
        Culture specific morning/afternoon string (e.g. 'am' or 'pm'). '%Tp' for upper case letters.
      </td>
    </tr>
    <tr>
      <td>'M'</td>
      <td>
        Minutes of the specified hour, from '00' to '59'.
      </td>
    </tr>
    <tr>
      <td>'S'</td>
      <td>
        Seconds of the specified minute, from '00' to '59'.
      </td>
    </tr>
    <tr>
      <td>'L'</td>
      <td>
        Milliseconds of the specified second, from '001' to '999'.
      </td>
    </tr>
    <tr>
      <td>'s'</td>
      <td>
        UNIX timestamp: seconds since the beginning of the UNIX epoche (1970-01-01T00:00:00Z)
      </td>
    </tr>
    <tr>
      <td>'Q'</td>
      <td>
        UNIX timestamp in milliseconds.
      </td>
    </tr>
    <tr>
      <td>'z'</td>
      <td>
        Timezone offset from the corresponding date to GMT in hours and minutes. -HHMM if the current date is "ahead", +HHMM if it is "behind" GMT. The timezone of the corresponding date is provided by the host OS of the JavaScript interpreter.
      </td>
    </tr>
    <tr>
      <td>'Z'</td>
      <td>
        Abbreviated time zone, e.g. 'CEST' for 'Central European Summer Time'. The timezone of the corresponding date is provided by the host OS of the JavaScript interpreter.
      </td>
    </tr>
    <tr>
      <td colspan="2" align="center"><br/>*Date components*</td>
    </tr>
    <tr>
      <td>'Y'</td>
      <td>
        Year, formatted as at least four digits with leading zeros as necessary.
      </td>
    </tr>
    <tr>
      <td>'y'</td>
      <td>
        Last two digits of the year, formatted with leading zeros as necessary, from '00' to '99'.
      </td>
    </tr>
    <tr>
      <td>'m'</td>
      <td>
        Month, formatted as two digits with leading zeros as necessary, from '01' to '12'. 
      </td>
    </tr>
    <tr>
      <td>'d'</td>
      <td>
        Day of month, formatted as two digits with leading zeros as necessary, from '01' to '31'.
      </td>
    </tr>
    <tr>
      <td>'e'</td>
      <td>
        Day of month, formatted as two digits, from '1' to '31'. 
      </td>
    </tr>
    <tr>
      <td>'j'</td>
      <td>
        Day of year, formatted as three digits with leading zeros as necessary, from '001' to '366'. 
      </td>
    </tr>
    <tr>
      <td>'A'</td>
      <td>
        Culture-specific weekday name.
      </td>
    </tr>
    <tr>
      <td>'a'</td>
      <td>
        Culture-specific abbreviated weekday name.
      </td>
    </tr>
    <tr>
      <td>'B'</td>
      <td>
        Culture-specific month name.
      </td>
    </tr>
    <tr>
      <td>'b', 'h'</td>
      <td>
        Culture-specific abbreviated month name.
      </td>
    </tr>
    <tr>
      <td>'C'</td>
      <td>
        Past centuries: year / 100, formatted as two digits with leading zero as necessary.
      </td>
    </tr>
    <tr>
      <td>'V'</td>
      <td>
        The ISO 8601 week number of the corresponding date, from '01' to '53'. Week 1 is the first week that has at least 4 days in the new year.
      </td>
    </tr>
    <tr>
      <td colspan="2" align="center"><br/>*Combinations*</td>
    </tr>
    <tr>
      <td>'R'</td>
      <td>
        24-hour time format: '%tH:%tM', e.g. '20:15'
      </td>
    </tr>
    <tr>
      <td>'T'</td>
      <td>
        24-hour time format: '%tH:%tM:%tS', e.g. '20:15:00'
      </td>
    </tr>
    <tr>
      <td>'r'</td>
      <td>
        12-hour time format: '%tI:%tM:%tS %Tp', e.g. '08:15:00 pm'
      </td>
    </tr>
    <tr>
      <td>'F'</td>
      <td>
        ISO 8601 formatted date: '%tY-%tm-%td', e.g. '2012-07-21'
      </td>
    </tr>
    <tr>
      <td>'D'</td>
      <td>
        Date formatted as '%tm/%td/%ty', e.g. '07/21/12'
      </td>
    </tr>
    <tr>
      <td>'c'</td>
      <td>
        Date and time formatted as '%ta %tb %td %tT %tZ %tY', e.g. 'Sat Jul 21 20:15:00 CEST 2012'
      </td>
    </tr>
  </tbody>
</table>

*Format specifier details:*

+ May be applied to arguments of type ```Date```
+ ```width``` is the resulting string's minimum width (space padding if necessary)
+ ```precision``` is not available and will be ignored if defined

*Applicable flags:*

+ '-': Left justification when space padding (default is right justification)
+ '#': If set, the date argument is interpreted as locale date, which means the date is interpreted with the timezone provided by the host OS of the JavaScript interpreter. If not set, a date is interpreted as UTC date
+ Other flags are ignored

*Example:*

        format('%1$tFT%1$tTZ', new Date('2012-07-21T20:15:00Z')); // -> '2012-07-21T20:15:00Z';

## Format specifiers not corresponding to any arguments
<table>
  <thead>
    <tr>
      <td>Conversion</td>
      <td>Description</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>'%'</td>
      <td>
        Result is '%' (escape sequence).
      </td>
    </tr>
    <tr>
      <td>'n'</td>
      <td>
        Result is the newline string as defined in the Formatter's options (defaults to '\n').
      </td>
    </tr>
  </tbody>
</table>

## Java format specifiers not supported by pat
<table>
  <thead>
    <tr>
      <td>Conversion</td>
      <td>Description</td>
    </tr>
  </thead>
  <tbody>
    <tr>
      <td>'h', 'H'</td>
      <td>
        Hexadecimal hash code of the object argument or null.
      </td>
    </tr>
    <tr>
      <td>'N'</td>
      <td>
        Nanoseconds of the corresponding date.
      </td>
    </tr>
  </tbody>
</table>
