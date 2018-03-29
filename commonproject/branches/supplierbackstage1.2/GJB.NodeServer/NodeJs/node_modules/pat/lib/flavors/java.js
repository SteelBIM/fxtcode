/**
 * Formatting data described by format specifiers of a certain flavor.
 * 
 * @module pat
 * @submodule flavors
 */
/**
 * Represents a data formatter that parses format specifiers known from Java's
 * Formatter class (java.util.Formatter).
 * 
 * The module includes a Scanner, a Parser and the format function. It exports
 * itself as a Node module, an AMD module or to the global scope, depending on
 * the environment.
 * 
 * @static
 * @class java
 * @namespace flavors
 */
/**
 * @license FreeBSD License
 * @date 2012-06-27
 * @author Michael Pecherstorfer
 */


/*global define, module*/
/*jslint nomen:true plusplus:true maxlen:85*/


(function(root) {
    
    'use strict';
    
    var mod, //module to export
        Formatter,
        Scanner,
        Parser,
        format,
        parser,
        __ = {}; //private section
    
    /*
     * Throws an error with the given message.
     */
    function err(msg) {
        throw new Error(msg);
    }
    /*
     * Throws an error corresponding to invalid Scanner/Parser input.
     */
    function inputErr(input, start, hint) {
        var i = input,
            n = 10, //display input[i] and the next n chars max
            len = input.length,
            msg = ['Invalid format specifier at zero based index ',
                start,
                '.\n',
                input[start]].join('');
                
        if (i + n < len) { len = i + n; }
        while (i < len) { msg += input[i++]; }
        err(msg + '...\n^\n' + hint);
    }
    
    /**
     * Java flavored data formatter.
     * @static
     * @class flavors.java.Formatter
     */
    Formatter = {};
    /**
     * Utility functions.
     * @static
     * @class flavors.java.Formatter.util
     */
    Formatter.util = {};
    /**
     * Appends or prepends spaces to the given array until it has the given
     * length.
     * @static
     * @chainable
     * @method spacePad
     * @param {Array} array
     * @param {Number} width Resulting width of the given array
     * @param {Boolean} [prepend = false] Whether to append or prepend to the
     *        given array
     * @return {Formatter.util}
     */
    Formatter.util.spacePad = function(array, width, prepend) {
        prepend = Boolean(prepend);
        var delta = width - array.length,
            i;
        if (delta > 0) {
            for (i = 0; i < delta; i++) {
                if (prepend) {
                    array.unshift(' ');
                } else {
                    array.push(' ');
                }
            }
        }
        return this;
    };
    /**
     * Java flavored number formatter.
     * @static
     * @class flavors.java.Formatter.number
     */
    Formatter.number = {
        /**
         * Default mantissa precision for number strings in computerized
         * scientific notation.
         * @final @static
         * @property DEFAULT_PRECISION
         * @type {Number}
         */
        DEFAULT_PRECISION: 6,
        /**
         * Default exponent width (excluding 'e' and sign) for number strings
         * in computerized scientific notation.
         * @final @static
         * @property DEFAULT_MIN_EXPONENT_WIDTH
         * @type {Number}
         */
        DEFAULT_MIN_EXPONENT_WIDTH: 2
    };
    /**
     * Java number localization algorithm.
     * @static 
     * @class flavors.java.Formatter.number.localize
     */
    Formatter.number.localize = {
        /**
         * @final @static
         * @property ASCII_ZERO ASCII code for '0' (48)
         * @type {Number}
         */
        ASCII_0: 48,
        /**
         * @final @static
         * @property ASCII_9 ASCII code for '9' (57)
         * @type {Number}
         */
        ASCII_9: 57
    };
    /**
     * Java number localization algorithm.
     * Each digit character d in the string is replaced by a culture-specific
     * digit computed relative to the current culture's zero digit z; that is
     * d - '0' + z.
     * @static
     * @method digits
     * @param {Array} arg Character array representing the Number to be localized
     * @para {Object} culture Culture specific information
     * @return undefined
     */
    Formatter.number.localize.digits = function(arg, culture) {
        var i = arg.length - 1,
            z = culture.zeroDigit.charCodeAt(0),
            c;
        while (i >= 0) {
            c = arg[i].charCodeAt(0);
            if (this.ASCII_0 <= c && c <= this.ASCII_9) { //localize digits
                arg[i] = String.fromCharCode(c - this.ASCII_0 + z);
            }
            i--;
        }
    };
    /**
     * Java number localization algorithm.
     * If a decimal separator is present, a culture-specific decimal separator
     * is substituted.
     * @static
     * @method decimalSeparator
     * @param {Array} arg Character array representing the Number to be localized
     * @param {Object} culture Culture information
     * @return undefined
     */
    Formatter.number.localize.decimalSeparator = function(arg, culture) {
        var pos = arg.indexOf('.');
        if (pos >= 0) {
            arg[pos] = culture.decimalSeparator;
        }
    };
    /**
     * Java number localization algorithm.
     * If the ',' ('\u002c') flag is given, then the culture-specific grouping
     * separator is inserted by scanning the integer part of the string from
     * least significant to most significant digits and inserting a separator
     * at intervals defined by the culture's grouping size.
     * @static
     * @method groupingSeparator
     * @param {Array} arg Character array representing the Number to be localized
     * @param {Object} culture Culture information
     * @result undefined
     */
    Formatter.number.localize.groupingSeparator = function(arg, culture) {
        var i = arg.length - 1,
            j = culture.groupingSize - 1;
        while (i >= 0) {
            if (j === 0 && i !== 0) {
                arg.splice(i, 0, culture.groupingSeparator);
                j = culture.groupingSize;
            }
            i--;
            j--;
        }
    };
    /**
     * Java number localization algorithm.
     * If the '0' flag is given, then the culture-specific zero digits are
     * inserted after the sign character, if any, and before the first non-zero
     * digit, until the length of the string is equal to the requested field
     * width.
     * @static
     * @method localize
     * @param {Array} arg Character array representing the Number to be localized
     * @param {Number} width Length of the resulting string
     * @param {Object} culture Culture information
     * @return undefined
     */
    Formatter.number.localize.zeroPad = function(arg, width, culture) {
        var len = arg.length,
            delta = width - len,
            pos = 0,
            i = 0;
        if (delta > 0) {
            while (i < len && (
                arg[i] === '-' ||
                arg[i] === '+' ||
                arg[i] === '0')) { i++; }
            if (i < len) {
                pos = i;
                for (i = 0; i < delta; i++) {
                    arg.splice(pos, 0, culture.zeroDigit);
                }
            }
        }
    };
    /**
     * Java flavored number localization algorithm.
     * 
     * If the value is negative and the '(' flag is given, then a '(' is
     * prepended and a ')' is appended.
     * 
     * If the value is negative and '(' flag is not given, then a '-' is
     * prepended.
     * 
     * If the '+' flag is given and the value is positive or zero, then a '+'
     * will be prepended.
     * 
     * If the ' ' flag is given and the value is positive or zero, then a ' '
     * will be prepended.
     * 
     * @static
     * @method sign
     * @param {Array} arg Character array representing the Number to be localized
     * @param {Object} opt Sign options
     * @return undefined
     */
    Formatter.number.localize.sign = function(arg, opt) {
        if (arg[0] === '-') { //negative value
            if (opt.parenthesesWhenNegative) {
                arg[0] = '(';
                arg.push(')');
            }
        } else { //positive value
            if (opt.leadingPlusWhenPositive) {
                arg.unshift('+');
            } else if (opt.leadingSpaceWhenPositive) {
                arg.unshift(' ');
            }
        }
    };
    /**
     * Java localization algorithm for number strings.
     * @static
     * @method localizeNumber
     * @param {String} arg Number string to be localized
     * @param {Object} culture Culture-specific information
     * @param {Number} width The result's minimum width
     * @param {Object} options Localization options (Options of a parsed token)
     * @return {String} The localized number string
     */
    Formatter.number.localize.localizeNumber = function(
            arg,
            culture,
            width,
            options) {
        var r = arg.split('');
        this.digits(r, culture);
        this.decimalSeparator(r, culture);
        if (options.localizedGroupingSeparator) {
            this.groupingSeparator(r, culture);
        }
        if (options.zeroPad) {
            this.zeroPad(r, width, culture);
        }
        this.sign(r, options);
        if (!options.zeroPad && width) { //pad with spaces
            Formatter.util.spacePad(r, width, !options.leftJustify);
        }
        return r.join('');
    };
    
    /**
     * Allocates a new Scanner for Java format specifiers.
     * @constructor
     * @class flavors.java.Scanner
     * @param {String|Array} input String or character array to be scanned
     * @return {Scanner} New Scanner instance
     */
    Scanner = function(input) {
        if (!(this instanceof Scanner)) { return new Scanner(input); }
        this.input(input);
    };
    /**
     * Different categories of scanned tokens.
     * @static
     * @class flavors.java.Scanner.tokenCategories
     */
    Scanner.tokenCategories = {
        /**
         * Represents a token of general conversion type.
         * @final @static
         * @property general
         * @type {Number}
         */
        general: 0,
        /**
         * Represents a token to be formatted as a character.
         * @final @static
         * @property character
         * @type {Number}
         */
        character: 1,
        /**
         * Represents a token to be formatted as an integer.
         * @final @static
         * @property integral
         * @type {Number}
         */
        integral: 2,
        /**
         * Represents a token to be formatted as a floating point number.
         * @final @static
         * @property floatingPoint
         * @type {Number}
         */
        floatingPoint: 3,
        /**
         * Represents a token to be formatted as a date.
         * @final @static
         * @property datetime
         * @type {Number}
         */
        datetime: 4,
        /**
         * Represents the percent literal token.
         * @final @static
         * @property percent
         * @type {Number}
         */
        percent: 5,
        /**
         * Represents the line separator literal token.
         * @final @static
         * @property lineSeparator
         * @type {Number}
         */
        lineSeparator: 6,
        /**
         * Represents a string token.
         * @final @static
         * @property text
         * @type {Number}
         */
        text: 7
    };
    /**
     * Resets this Scanner's state.
     * @chainable
     * @method reset
     * @for flavors.java.Scanner
     * @return {Scanner}
     */
    Scanner.prototype.reset = function() {
        this._in = [];
        this._ch = ''; //current input character
        this._iNextCh = 0; //points to the next input character
        this._iArg = -1;
        this._iPrevArg = -1;
        this._token = null;
        return this;
    };
    /**
     * Sets or returns this Scanner's input.
     * @chainable
     * @method input
     * @for flavors.java.Scanner
     * @param {String|Array} [input] String or character array to be scanned
     * @return {Scanner|Array} This Scanner if called as setter, this Scanner's
     *         current input if called as getter.
     */
    Scanner.prototype.input = function(input) {
        if (arguments.length === 0) { return this._in; }
        this.reset();
        if (input) { this._in = __.fmt.util.toArray(input); }
    };
    /**
     * Throws an error using the given hint for the error message.
     * The error message includes the relevant input substring, the index of
     * the error prone character within the input, and the given hint.
     * @method err
     * @for flavors.java.Scanner
     * @param {String} hint To be included in the error message
     */
    Scanner.prototype.err = function(hint) {
        inputErr(this._input, this._iNextCh - 1, hint);
    };
    /*
     * Reads the next character of the input.
     * @chainable
     * @method readCh
     * @for flavors.java.Scanner
     * @return {Scanner}
     */
    Scanner.prototype.readCh = function() {
        this._ch = this._in[this._iNextCh++];
    };
    /*
     * Returns true if the current character is a digit, false otherwise.
     * @method isDigit
     * @for flavors.java.Scanner
     * @return {Boolean}
     */
    Scanner.prototype.isDigit = function() {
        return '0' <= this._ch && this._ch <= '9';
    };
    /*
     * Throws an error if the current character does not represent a digit.
     * @chainable
     * @method expectDigit
     * @for flavors.java.Scanner
     * @return {Scanner}
     */
    Scanner.prototype.expectDigit = function() {
        if (!this.isDigit()) { this.err('Digit expected.'); }
        return this;
    };
    /*
     * Throws an error if the current character is not equal to the given one.
     * @chainable
     * @method expectCh
     * @for flavors.java.Scanner
     * @param {String} val Expected character
     * @return {Scanner}
     */
    Scanner.prototype.expectCh = function(val) {
        if (this._ch !== val) { this.err("'" + val + "'" + ' expected.'); }
        return this;
    };
    /*
     * Throws an error if the current character is falsy, e.g. after the last
     * input character has been read.
     * @chainable
     * @method expectAnyCh
     * @for flavors.java.Scanner
     * @return {Scanner}
     */
    Scanner.prototype.expectAnyCh = function() {
        if (!this._ch) { this.err('Unexpected string end.'); }
        return this;
    };
    /*
     * Reads digit after digit and returns the value as a Number. Expects the
     * current character to be a digit. Stops reading as soon as the current
     * character does not represent a digit.
     * @method readNumber
     * @for flavors.java.Scanner
     * @return {Number}
     */
    Scanner.prototype.readNumber = function() {
        var result = this._ch;
        this.readCh();
        while(this.isDigit()) {
            result += this._ch;
            this.readCh();
        }
        return Number(result);
    };
    /*
     * Reads the character sequence that starts with the current character and
     * ends with the character before the next '%'. If there is not a next '%'
     * sign then it will read the substring from the current character to the
     * input end.
     * @chainable
     * @method readText
     * @for flavors.java.Scanner
     * @return {Scanner}
     */
    Scanner.prototype.readText = function() {
        var end = this._in.indexOf('%', this._iNextCh);
        if (end >= 0) {
            this._token.value = this._in.slice(this._iNextCh - 1, end).join('');
            this._iNextCh = end;
        } else {
            this._token.value = this._in.slice(this._iNextCh - 1).join('');
            this._iNextCh = this._in.length;
        }
        return this;
    };
    /*
     * Extracts the conversion sequence.
     * @chainable
     * @method readConversion
     * @for flavors.java.Scanner
     * @return {Scanner}
     */
    Scanner.prototype.readConversion = function() {
        function set(token, conversion, category, upperCase) {
            token.conversion = conversion;
            token.category = category;
            token.upperCase = Boolean(upperCase);
        }
        switch (this._ch) {
        case 'b': case 's':
            set(this._token, this._ch, Scanner.tokenCategories.general);
            break;
        case 'B': case 'S':
            set(this._token, this._ch.toLowerCase(),
                Scanner.tokenCategories.general, true);
            break;
        case 'c':
            set(this._token, this._ch, Scanner.tokenCategories.character);
            break;
        case 'C':
            set(this._token, this._ch.toLowerCase(),
                Scanner.tokenCategories.character, true);
            break;
        case 'd': case 'o': case 'x':
            set(this._token, this._ch, Scanner.tokenCategories.integral);
            break;        
        case 'X':
            set(this._token, this._ch.toLowerCase(),
                Scanner.tokenCategories.integral, true);
            break;
        case 'e': case 'f': case 'g': case 'a':
            set(this._token, this._ch, Scanner.tokenCategories.floatingPoint);
            break;
        case 'E': case 'G': case 'A':
            set(this._token, this._ch.toLowerCase(),
                Scanner.tokenCategories.floatingPoint, true);
            break;
        case 't': case 'T':
            set(this._token, 't', Scanner.tokenCategories.datetime,
                this._ch === 'T');
            this.readCh();
            this.expectAnyCh();
            switch (this._ch) {
            //time
            case 'H': case 'I': case 'k': case 'l': case 'M': case 'S':
            case 'L': case 'p': case 'z': case 'Z': case 's': case 'Q':
            //date
            case 'B': case 'b': case 'h': case 'A': case 'a': case 'C':
            case 'Y': case 'y': case 'j': case 'm': case 'd': case 'e':
            case 'V':
            //compositions
            case 'R': case 'T': case 'r': case 'D': case 'F': case 'c':
                this._token.conversion += this._ch;
                break;
            default:
                this.err('Unexpected datetime conversion: \'' + this._ch + '\'.');
                break;
            }
            break;
        default:
            this.err('Unexpected conversion: \'' + this._ch + '\'.');
            break;
        }
        return this;
    };
    /*
     * Extracts the precision.
     * @chainable
     * @method readPrecision
     * @for flavors.java.Scanner
     * @return {Scanner}
     */
    Scanner.prototype.readPrecision = function() {
        if (this._ch === '.') {
            this.readCh();
            this.expectDigit();
            this._token.precision = this.readNumber();
        }
        return this;
    };
    /*
     * Extracts the width.
     * @chainable
     * @method readWidth
     * @for flavors.java.Scanner
     * @return {Scanner}
     */
    Scanner.prototype.readWidth = function() {
        if (this.isDigit()) { this._token.width = this.readNumber(); }
        return this;
    };
    /*
     * Extracts the flags.
     * @chainable
     * @method readFlags
     * @for flavors.java.Scanner
     * @return {Scanner}
     */
    Scanner.prototype.readFlags = function() {
        switch(this._ch) {
        case '-':
            this._token.options.leftJustify = true;
            this.readCh();
            this.readFlags();
            break;
        case '#':
            this._token.options.alternateForm = true;
            this.readCh();
            this.readFlags();
            break;
        case '+':
            this._token.options.leadingPlusWhenPositive = true;
            this.readCh();
            this.readFlags();
            break;
        case ' ':
            this._token.options.leadingSpaceWhenPositive = true;
            this.readCh();
            this.readFlags();
            break;
        case '0':
            this._token.options.zeroPad = true;
            this.readCh();
            this.readFlags();
            break;
        case ',':
            this._token.options.localizedGroupingSeparator = true;
            this.readCh();
            this.readFlags();
            break;
        case '(':
            this._token.options.parenthesesWhenNegative = true;
            this.readCh();
            this.readFlags();
            break;
        }
        return this;
    };
    /*
     * Sets the argument index. Used when the format specifier does not include
     * an explicit or a relative index.
     * @chainable
     * @method implicitArgumentIndex
     * @for flavors.java.Scanner
     * @return {Scanner}
     */
    Scanner.prototype.implicitArgumentIndex = function() {
        this._iArg++;
        this._token.argumentIndex = this._iArg;
        this._iPrevArg = this._token.argumentIndex;
        return this;
    };
    /*
     * Extracts the argument index.
     * @chainable
     * @method readArgumentIndex
     * @for flavors.java.Scanner
     * @return {Scanner}
     */
    Scanner.prototype.readArgumentIndex = function() {
        var number = 0,
            iTmp = this._iNextCh;
        if (this.isDigit()) {
            if (this._ch !== 0) {
                number = this.readNumber();
                if (this._ch === '$') { //absolute index
                    this._token.argumentIndex = number - 1;
                    this._iPrevArg = this._token.argumentIndex;
                    this.readCh();
                    this.expectAnyCh();
                } else { //implicit index
                    this.implicitArgumentIndex();
                    // number read was width => rewind
                    this._iNextCh = iTmp;
                    this._ch = this._in[iTmp - 1];
                }
            } else { //implicit index
                this.implicitArgumentIndex();
            }
        } else if (this._ch === '<') { //relative index
            if (this._iPrevArg < 0) {
                this.err('Missing previous format specifier');
            }
            this._token.argumentIndex = this._iPrevArg;
            this.readCh();
            this.expectAnyCh();
        } else { //implicit index
            this.implicitArgumentIndex();
        }
        return this;
    };
    /**
     * Returns true if it's possible to scan another token, false otherwise.
     * @method hasNext
     * @for flavors.java.Scanner
     * @return {Boolean}
     */
    Scanner.prototype.hasNext = function() {
        return this._in && this._in.length > 0 &&
            //iNextCh points to the first char or to any char but the last
            (this._iNextCh === 0 || this._iNextCh < this._in.length);
    };
    /**
     * Returns the next token or undefined if there is no more text to scan.
     * @method next
     * @for flavors.java.Scanner
     * @return {Object}
     */
    Scanner.prototype.next = function() {
        if (!this.hasNext()) { return undefined; }
        this._token = {
            category: Scanner.tokenCategories.general,
            argumentIndex: -1,
            options: {
                leftJustify: false,
                alternateForm: false,
                leadingPlusWhenPositive: false,
                leadingSpaceWhenPositive: false,
                zeroPad: false,
                localizedGroupingSeparator: false,
                parenthesesWhenNegative: false
            },
            width: false,
            precision: false,
            conversion: '',
            upperCase: false,
            startIndex: 0,
            value: null
        };
        this._token.startIndex = this._iNextCh;
        this.readCh();
        if (this._ch === '%') {
            this.readCh();
            this.expectAnyCh();
            switch (this._ch) {
            case '%':
                this._token.conversion = this._ch;
                this._token.category = Scanner.tokenCategories.percent;
                break;
            case 'n':
                this._token.conversion = this._ch;
                this._token.category = Scanner.tokenCategories.lineSeparator;
                break;
            default:
                this.readArgumentIndex();
                this.readFlags();
                this.readWidth();
                this.readPrecision();
                this.readConversion();
                break;
            }
        } else {
            this._token.category = Scanner.tokenCategories.text;
            this.readText();
        }
        return this._token;
    };
    
    /**
     * Allocates a new Parser for Java format specifiers.
     * @constructor
     * @class flavors.java.Parser
     * @return {Parser} New Parser instance
     */
    Parser = function() {
        if (!(this instanceof Parser)) { return new Parser(); }
        this._scanner = new Scanner();
        this._target = [];
    };
    /**
     * Parses a format specifier of conversion type 'general'.
     * @chainable
     * @method parseGeneral
     * @for flavors.java.Parser
     * @param {Object} token Token to be parsed
     * @param {any} fmtArg Value to be formatted
     * @return {Parser}
     */
    Parser.prototype.parseGeneral = function(token, fmtArg) {
        var val = null;
        switch (token.conversion) {
        case 'b': case 'B':
            val = fmtArg ? 'true' : 'false';
            break;
        case 's': case 'S':
            val = String(fmtArg);
            break;
        }
        if (token.upperCase) {
            val = val.toUpperCase();
        }
        if (token.precision && token.precision < val.length) {
            val = val.substr(0, token.precision);
        }
        if (token.width) {
            val = __.fmt.util.pad(val, ' ', token.width, !token.options.leftJustify);
        }
        this._target.push(val);
        return this;
    };
    /**
     * Parses a format specifier of conversion type 'character'
     * @chainable
     * @method parseCharacter
     * @for flavors.java.Parser
     * @param {Object} token Token to be parsed
     * @param {String|Number} fmtArg String or Unicode code point
     * @return {Parser}
     */
    Parser.prototype.parseCharacter = function(token, fmtArg) {
        var val = null;
        if (typeof fmtArg === 'string') {
            val = fmtArg.charAt(0);
        } else if (typeof fmtArg === 'number') {
            val = String.fromCharCode(Math.floor(fmtArg));
        } else {
            err('Invalid argument. String or Number expected.');
        }
        if (token.upperCase) {
            val = val.toUpperCase();
        }
        if (token.width) {
            val = __.fmt.util.pad(val, ' ', token.width, !token.options.leftJustify);
        }
        this._target.push(val);
        return this;
    };
    /**
     * Parses a symbolic number.
     * @chainable
     * @method parseSymbolicNumber
     * @for flavors.java.Parser
     * @param {Object} token Token to be parsed
     * @param {any} fmtArg Value to be formatted
     * @return {Parser}
     */
    Parser.prototype.parseSymbolicNumber = function(token, fmtArg) {
        if (Number(fmtArg) === Number.NEGATIVE_INFINITY &&
                token.options.parenthesesWhenNegative) {
            this._target.push('(Infinity)');
        } else {
            this._target.push(String(Number(fmtArg)));
        }
        return this;
    };
    /**
     * Parses a decimal integer.
     * @chainable
     * @method parseDecInt
     * @for flavors.java.Parser
     * @param {Object} token Token to be parsed
     * @param {Number} fmtArg Value to be formatted
     * @param {Object} fmtOpt Format options
     * @return {Parser}
     */
    Parser.prototype.parseDecInt = function(token, fmtArg, fmtOpt) {
        this._target.push(
            Formatter.number.localize.localizeNumber(
                __.fmt.number.toDecimal(Math.floor(fmtArg)),
                fmtOpt.culture(),
                token.width,
                token.options));
        return this;
    };
    /**
     * Parses an octal integer.
     * @chainable
     * @method parseOctInt
     * @for flavors.java.Parser
     * @param {Object} token Token to be parsed
     * @param {Number} fmtArg Value to be formatted
     * @return {Parser}
     */
    Parser.prototype.parseOctInt = function(token, fmtArg) {
        var val = null;
        if (fmtArg >= 0) {
            val = Math.floor(fmtArg).toString(8);
        } else {
            val = Number(__.fmt.util.number.twosComplement(-1 * fmtArg))
                .toString(8);
        }
        if (token.options.alternateForm) { //with '0' prefix
            val = '0' + val;
        }
        if (token.width) { //space or zero pads
            if (token.options.zeroPad) {
                val = __.fmt.util.padLeft(val, '0', token.width);
            } else {
                val = __.fmt.util.padLeft(val, ' ', token.width);
            }
        }
        this._target.push(val);
        return this;
    };
    /**
     * Parses a hexadecimal integer.
     * @chainable
     * @method parseHexInt
     * @for flavors.java.Parser
     * @param {Object} token Token to be parsed
     * @param {Number} fmtArg Value to be formatted
     * @return {Parser}
     */
    Parser.prototype.parseHexInt = function(token, fmtArg) {
        var val = null;
        if (fmtArg >= 0) {
            val = Math.floor(fmtArg).toString(16);
        } else {
            val = Number(__.fmt.util.number.twosComplement(-1 * fmtArg))
                .toString(16);
        }
        //alternate form: with '0x' prefix
        if (token.options.alternateForm) {
            if (token.width) {
                val = token.options.zeroPad ?
                    '0x' + __.fmt.util.padLeft(val, '0', token.width - 2) :
                    __.fmt.util.padLeft('0x' + val, ' ', token.width);
            } else {
                val = '0x' + val;
            }
        } else { //default form: naked value
            if (token.width) { //space or zero pad
                val = token.options.zeroPad ?
                    __.fmt.util.padLeft(val, '0', token.width) :
                    __.fmt.util.padLeft(val, ' ', token.width);
            }
        }
        if (token.upperCase) { val = val.toUpperCase(); }
        this._target.push(val);
        return this;
    };
    /**
     * Parses a format specifier of conversion type 'integral'.
     * @chainable
     * @method parseIntegral
     * @for flavors.java.Parser
     * @param {Object} token Token to be parsed
     * @param {any} fmtArg Value to be formatted
     * @param {Object} fmtOpt Format options
     * @return {Parser}
     */
    Parser.prototype.parseIntegral = function(token, fmtArg, fmtOpt) {
        if (__.fmt.util.number.isSymbolicNumber(fmtArg)) {
            this.parseSymbolicNumber(token, fmtArg);
        } else {
            switch (token.conversion) {
            case 'd': //decimal notation
                this.parseDecInt(token, Number(fmtArg), fmtOpt);
                break;
            case 'o': //octal notation
                this.parseOctInt(token, Number(fmtArg));
                break;
            case 'x': //hex notation
                this.parseHexInt(token, Number(fmtArg));
                break;
            }
        }
        return this;
    };
    /**
     * Parses decimal floats.
     * @chainable
     * @method parseDecimalFloat
     * @param {Object} token Token to be parsed
     * @param {Number} fmtArg Value to be formatted
     * @param {Object} fmtOpt Format options
     * @return {Parser}
     */
    Parser.prototype.parseDecimalFloat = function(token, fmtArg, fmtOpt) {
        this._target.push(
            Formatter.number.localize.localizeNumber(
                __.fmt.number.toDecimal(fmtArg, {
                    precision: token.precision || token.precision === 0 ?
                        token.precision :
                        Formatter.number.DEFAULT_PRECISION,
                    considerZeroSign: true
                }),
                fmtOpt.culture(),
                token.width,
                token.options));
        return this;
    };
    /**
     * Parses computerized scientific floats.
     * @chainable
     * @method parseComputerizedScientificFloat
     * @param {Object} token Token to be parsed
     * @param {Number} fmtArg Value to be formatted
     * @param {Object} fmtOpt Format options
     * @return {Parser}
     */
    Parser.prototype.parseScientificFloat = function(token, fmtArg, fmtOpt) {
        //numbers in scientific notation do not have groups 
        token.options.localizedGroupingSeparator = false;
        this._target.push(Formatter.number.localize.localizeNumber(
            __.fmt.number.toScientific(fmtArg, {
                precision: token.precision || token.precision === 0 ?
                    token.precision :
                    Formatter.number.DEFAULT_PRECISION,
                expMinWidth: Formatter.number.DEFAULT_MIN_EXPONENT_WIDTH,
                upperCase: token.upperCase,
                considerZeroSign: true
            }),
            fmtOpt.culture(),
            token.width,
            token.options));
    };
    /**
     * Parses computerized scientific floats.
     * @chainable
     * @method parseGeneralScientificFloat
     * @param {Object} token Token to be parsed
     * @param {Number} fmtArg Value to be formatted
     * @param {Object} fmtOpt Format options
     * @return {Parser}
     */
    Parser.prototype.parseGeneralScientificFloat = function(token, fmtArg, fmtOpt) {
        var r,
            nIntDigits,
            lowerBound,
            upperBound,
            roundPrec;
        r = Math.abs(Number(fmtArg));
        nIntDigits = r === 0 ?
            1 :
            Math.floor(Math.log(r) / Math.log(10)) + 1;
        if (token.precision === 0) { token.precision = 1; }
        if (!token.precision) {
            token.precision = Formatter.number.DEFAULT_PRECISION;
        }
        lowerBound = Math.pow(10, -4);
        upperBound = Math.pow(10, token.precision);
        roundPrec = 0 < r && r < 1 ?
            token.precision :
            token.precision - nIntDigits;
        if (roundPrec < 0) { roundPrec = 0; }
        r = __.fmt.util.number.round(fmtArg, roundPrec);
        if (r === 0 || (lowerBound <= Math.abs(r) && Math.abs(r) < upperBound)) {
            token.precision = roundPrec;
            this.parseDecimalFloat(token, r, fmtOpt);
        } else {
            token.precision--;
            this.parseScientificFloat(token, r, fmtOpt);
        }
        return this;
    };
    /**
     * Parses hexadecimal exponential floats.
     * @chainable
     * @method parseGeneralScientificFloat
     * @param {Number} fmtArg Value to be formatted
     * @return {Parser}
     */
    Parser.prototype.parseHexExp = function(fmtArg) {
        this._target.push(__.fmt.number.toHexExp(fmtArg));
        return this;
    };
    /**
     * Parses a format specifier of conversion type 'floatingPoint'.
     * @chainable
     * @method parseFloatingPoint
     * @for flavors.java.Parser
     * @param {Object} token Token to be parsed
     * @param {any} fmtArg Value to be formatted
     * @param {Object} fmtOpt Format options
     * @return {Parser}
     */
    Parser.prototype.parseFloatingPoint = function(token, fmtArg, fmtOpt) {
        if (__.fmt.util.number.isSymbolicNumber(fmtArg)) {
            this.parseSymbolicNumber(token, fmtArg);
        } else {
            switch (token.conversion) {
            case 'f': //decimal format
                this.parseDecimalFloat(token, Number(fmtArg), fmtOpt);
                break;
            case 'e': //computerized scientific notation
                this.parseScientificFloat(token, Number(fmtArg), fmtOpt);
                break;
            case 'g': //general scientific notation
                this.parseGeneralScientificFloat(token, Number(fmtArg), fmtOpt);
                break;
            case 'a': //hexadecimal exponential form
                this.parseHexExp(token, Number(fmtArg), fmtOpt);
                break;
            }
        }
        return this;
    };
    /**
     * Parses a format specifier of conversion type 'datetime'
     * @chainable
     * @method parseDatetime
     * @for flavors.java.Parser
     * @param {Object} token Token to be parsed
     * @param {Date} fmtArg Value to be formatted
     * @param {Object} fmtOpt Format options
     * @return {Parser}
     */
    Parser.prototype.parseDatetime = function(token, fmtArg, fmtOpt) {
        var val = null;
        if (!fmtArg || !(fmtArg instanceof Date)) {
            err('Invalid argument. Date expected.');
        }
        //Should the token be formatted based on the time zone provided by the
        //JavaScript interpreter's host OS?
        if (token.options.alternateForm) {
            fmtArg = new Date(fmtArg.valueOf() +
                //tz offset from UTC in milliseconds
                fmtArg.getTimezoneOffset() * -60000);
        }
        switch (token.conversion) {
        //time
        case 'tH':
            val = __.fmt.date.hours(fmtArg, true, false);
            break;
        case 'tI':
            val = __.fmt.date.hours(fmtArg, true, true);
            break;
        case 'tk':
            val = __.fmt.date.hours(fmtArg, false, false);
            break;
        case 'tl':
            val = __.fmt.date.hours(fmtArg, false, true);
            break;
        case 'tM':
            val = __.fmt.date.minutes(fmtArg, true);
            break;
        case 'tS':
            val = __.fmt.date.seconds(fmtArg, true);
            break;
        case 'tL':
            val = __.fmt.date.milliseconds(fmtArg, true);
            break;
        case 'tp':
            val = (token.upperCase ?
                __.fmt.date.timeDesignator(fmtArg, fmtOpt.culture()).toUpperCase() :
                __.fmt.date.timeDesignator(fmtArg, fmtOpt.culture()));
            break;
        case 'tz':
            val = (token.options.alternateForm ?
                __.fmt.date.timezoneOffset(fmtArg) :
                '+0000');
            break;
        case 'tZ':
            val = (token.options.alternateForm ?
                __.fmt.date.abbreviatedTimezone(fmtArg) :
                'GMT');
            break;
        case 'ts':
            val = __.fmt.util.date.timestamp(fmtArg);
            break;
        case 'tQ':
            val = fmtArg.valueOf();
            break;
        //date
        case 'tB':
            val = (token.upperCase ?
                __.fmt.date.monthName(fmtArg, fmtOpt.culture()).toUpperCase() :
                __.fmt.date.monthName(fmtArg, fmtOpt.culture()));
            break;
        case 'tb': //fall through
        case 'th':
            val = (token.upperCase ?
                __.fmt.date.abbreviatedMonthName(fmtArg, fmtOpt.culture())
                        .toUpperCase() :
                __.fmt.date.abbreviatedMonthName(fmtArg, fmtOpt.culture()));
            break;
        case 'tA':
            val = (token.upperCase ?
                __.fmt.date.weekdayName(fmtArg, fmtOpt.culture()).toUpperCase() :
                __.fmt.date.weekdayName(fmtArg, fmtOpt.culture()));
            break;
        case 'ta':
            val = (token.upperCase ?
                __.fmt.date.abbreviatedWeekdayName(fmtArg, fmtOpt.culture())
                        .toUpperCase() :
                __.fmt.date.abbreviatedWeekdayName(fmtArg, fmtOpt.culture()));
            break;
        case 'tC':
            val = String(__.fmt.util.date.pastCenturies(fmtArg));
            if (val.length === 1) {
                val = __.fmt.util.padLeft(val, '0', 2);
            }
            break;
        case 'tY':
            val = __.fmt.date.year(fmtArg, {
                maxDigits: 4,
                leadingZeros: true
            });
            break;
        case 'ty':
            val = __.fmt.date.year(fmtArg, {
                maxDigits: 2,
                leadingZeros: true
            });
            break;
        case 'tj':
            val = __.fmt.date.dayOfYear(fmtArg, true);
            break;
        case 'tm':
            val = __.fmt.date.month(fmtArg, true);
            break;
        case 'td':
            val = __.fmt.date.dayOfMonth(fmtArg, true);
            break;
        case 'te':
            val = __.fmt.date.dayOfMonth(fmtArg, false);
            break;
        case 'tV':
            val = String(__.fmt.util.date.isoWeek(fmtArg));
            break;
        //compositions
        case 'tR': //"%tH:%tM"
            val = __.fmt.date.hours(fmtArg, true, false) + ':' +
                __.fmt.date.minutes(fmtArg, true);
            break;
        case 'tT': //"%tH:%tM:%tS"
            val = __.fmt.date.time(fmtArg, true);
            break;
        case 'tr': //"%tI:%tM:%tS %Tp"
            val = __.fmt.date.hours(fmtArg, true, true) + ':' +
                __.fmt.date.minutes(fmtArg, true) + ':' +
                __.fmt.date.seconds(fmtArg, true) + ' ' +
                __.fmt.date.timeDesignator(fmtArg, fmtOpt.culture()).toUpperCase();
            break;
        case 'tD': //"%tm/%td/%ty"
            val = __.fmt.date.month(fmtArg, true) + '/' +
                __.fmt.date.dayOfMonth(fmtArg, true) + '/' +
                __.fmt.date.year(fmtArg, {
                    maxDigits: 2,
                    leadingZeros: true
                });
            break;
        case 'tF': //"%tY-%tm-%td"
            val = __.fmt.date.year(fmtArg, {
                    maxDigits: 4,
                    leadingZeros: true
                }) + '-' +
                __.fmt.date.month(fmtArg, true) + '-' +
                __.fmt.date.dayOfMonth(fmtArg, true);
            break;
        case 'tc': //"%ta %tb %td %tT %tZ %tY"
            val = [
                __.fmt.date.abbreviatedWeekdayName(fmtArg, fmtOpt.culture()),
                __.fmt.date.abbreviatedMonthName(fmtArg, fmtOpt.culture()),
                __.fmt.date.dayOfMonth(fmtArg, true),
                __.fmt.date.time(fmtArg, true),
                (token.options.alternateForm ?
                    __.fmt.date.abbreviatedTimezone(fmtArg, fmtOpt.culture()) :
                    'GMT'),
                __.fmt.date.year(fmtArg, {
                    maxDigits: 4,
                    leadingZeros: true
                })
            ].join(' ');
            break;
        }
        if (token.width) {
            val = __.fmt.util.pad(val, ' ', token.width, !token.options.leftJustify);
        }
        this._target.push(val);
        return this;
    };
    /**
     * Applies the given token to the given format argument.
     * @chainable
     * @method parseToken
     * @for flavors.java.Parser
     * @param {Object} token Token to be parsed
     * @param {any} [fmtArg] Value to be formatted
     * @param {Object} [fmtOpt] Format options
     * @return {Parser}
     */
    Parser.prototype.parseToken = function(token, fmtArg, fmtOpt) {
        switch (token.category) {
        case Scanner.tokenCategories.general:
            this.parseGeneral(token, fmtArg);
            break;
        case Scanner.tokenCategories.character:
            this.parseCharacter(token, fmtArg);
            break;
        case Scanner.tokenCategories.integral:
            this.parseIntegral(token, fmtArg, fmtOpt);
            break;
        case Scanner.tokenCategories.floatingPoint:
            this.parseFloatingPoint(token, fmtArg, fmtOpt);
            break;
        case Scanner.tokenCategories.datetime:
            this.parseDatetime(token, fmtArg, fmtOpt);
            break;
        case Scanner.tokenCategories.percent:
            this._target.push('%');
            break;
        case Scanner.tokenCategories.lineSeparator:
            this._target.push(fmtOpt.lineSeparator());
            break;
        case Scanner.tokenCategories.text:
            this._target.push(token.value);
            break;
        }
        return this;
    };
    /**
     * Parses the given input.
     * @method parse
     * @for flavors.java.Parser
     * @param {String|Array} input String or character array to be parsed
     * @param {Array} [fmtArgs] Data to be formatted
     * @param {Object} [fmtOpt] Format options
     * @return {String} Formatted data
     */
    Parser.prototype.parse = function(input, fmtArgs, fmtOpt) {
        var t = null; //token
        this._target = [];
        this._scanner.input(input);
        while (this._scanner.hasNext()) {
            t = this._scanner.next();
            if (t.argumentIndex >= 0 &&
                    t.argumentIndex > fmtArgs.length) {
                inputErr(input, t.startIndex, 'Invalid argument index');
            }
            try {
                this.parseToken(t,
                    t.argumentIndex >= 0 ? fmtArgs[t.argumentIndex] : undefined,
                    fmtOpt);
            } catch (e) {
                inputErr(input, t.startIndex, e.message);
            }
        }
        return this._target.join('');
    };
    
    /**
     * Formats the given arguments described by the given formatstring.
     * @method format
     * @for flavors.java
     * @param {String} fstr Format string
     * @param {Array} [args] Data to be formatted
     * @return {String} Formatted data
     */
    format = function(fstr, formatter, args, options) {
        if (!fstr || !formatter) { return undefined; }
        if (!parser) { parser = new Parser(); }
        __.fmt = formatter;
        fstr = __.fmt.util.toArray(fstr);
        return parser.parse(fstr, args, options);
    };
    
    mod = {
        id: 'java',
        Formatter: Formatter,
        Scanner: Scanner,
        Parser: Parser,
        format: format,
        _private: __
    };
    
    /* Node.js */
    if (typeof module !== 'undefined') {
        module.exports = mod;
    }
    /* AMD */
    else if (typeof define !== 'undefined' && define.amd) { //AMD
        define(mod);
    }
    /* Browser */
    else if (root) {
        if (!root.pat) { root.pat = {}; }
        if (!root.pat._private) { root.pat._private = {}; }
        if (!root.pat._private.flavors) {
            root.pat._private.flavors = {};
            root.pat._private.preferredFlavorId = mod.id;
        }
        root.pat._private.flavors.java = mod;
    }
    
}(this));
