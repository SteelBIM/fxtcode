/**
 * Represents a data formatter. Data to be formatted is described by format
 * specifiers of a certain flavor.
 * 
 * Supported format specifiers:
 * 
 * +    Java (java.util.Formatter)
 * 
 * @module pat
 * 
 * 
 * @license FreeBSD License
 * @date 2012-06-27
 * @author Michael Pecherstorfer
 */


/*global define, module*/
/*jslint nomen:true plusplus:true maxlen:85*/


(function(root) {
    
    'use strict';
    
    var mod = null,
        //private section
        __ = {
            cultures: {}, //culture module cache
            flavors: {} //flavor module cache
        },
        Formatter = null;
            
    /* Directory containing the culture modules */
    __.DIR_CULTURES = 'cultures/';
    
    /* Directory containing the flavor modules */
    __.DIR_FLAVORS = 'flavors/';
    
    /* Default formatter instance */
    __.instance = null;
    
    /* Path to this module, relative to require's baseUrl */
    __.modulePath = '';
    
    /* This module's inital state */
    __.initialState = {
        cultures: {},
        flavors: {},
        modulePath: '',
        set: function() {
            this.cultures = __.deepCopy(__.cultures);
            this.flavors = __.deepCopy(__.flavors);
            this.modulePath = __.modulePath;
        },
        establish: function() {
            Formatter.defaultOptions = __.initialDefaultFormatterOptions();
            __.instance = new Formatter();
            __.cultures = __.deepCopy(this.cultures);
            __.flavors = __.deepCopy(this.flavors);
            __.modulePath = this.modulePath;
        }
    };
    
    /*
     * Returns the initial default options for a new Formatter.
     */
    __.initialDefaultFormatterOptions = function() {
        return {
            flavorId: 'java',
            flavor: null,
            cultureId: 'enUS',
            culture: null,
            lineSeparator: '\n'
        };
    };
    /*
     * Conveniance function throwing an error with the given message.
     */
    __.err = function(msg) {
        throw new Error(msg);
    };
    /*
     * Binds the given context to the given function.
     */
    __.bind = function(fn, context) {
        return function() {
            fn.apply(context, Array.prototype.slice.call(arguments));
        };
    };
    /*
     * Counts the given object's own properties.
     */
    __.countOwnProperties = function(obj) {
        var r = 0,
            key;
        for (key in obj) {
            if (obj.hasOwnProperty(key)) { r++; }
        }
        return r;
    };
    /*
     * Returns the given object's own property names.
     */
    __.ownPropertyNames = function(obj) {
        var r = [],
            key;
        for (key in obj) {
            if (obj.hasOwnProperty(key)) { r.push(key); }
        }
        return r;
    };
    /*
     * Returns a deep copy of the argument.
     * Properties of the prototype chain are not considered.
     */
    __.deepCopy = function(arg) {
        if (typeof arg !== 'object') { return arg; }
        var key,
            result = {};
        for (key in arg) {
            if (arg.hasOwnProperty(key)) {
                result[key] = __.deepCopy(arg[key]);
            }
        }
        return result;
    };
    /*
     * Returns an accessor for subject. The accessor behaves like a setter or
     * a getter, depending whether it is called with or without an argument.
     * 
     * When called as a setter, the given new value is passed to the accessor's
     * 'syncSetter' or 'asyncSetter', depending whether one of those properties
     * is defined.
     * 
     * If 'syncSetter' is defined, the result of this function represents the
     * value to be set. If 'syncSetter' is not defined, but 'asyncSetter' is
     * defined, the new value to be set is passed as a parameter to the callback
     * function of 'asyncSetter'. Otherwise the new value is set without
     * delegating it to another function.
     */
    __.accessor = function(subject) {
        var result,
            syncSetter,
            asyncSetter;
        result = function(newval, fn) {
            //getter
            if (arguments.length === 0) { return subject; }
            //delegate synchronously
            if (typeof syncSetter === 'function') {
                subject = syncSetter(newval);
            }
            //delegate asynchronously
            else if (typeof asyncSetter === 'function') {
                asyncSetter(newval, function(newval) {
                    subject = newval;
                    if (typeof fn === 'function') { fn(); }
                });
            }
            //set new value without delegating it
            else { subject = newval; }
        };
        result.syncSetter = function(fn) {
            if (arguments.length === 0) { return syncSetter; }
            syncSetter = fn;
        };
        result.asyncSetter = function(fn) {
            if (arguments.length === 0) { return asyncSetter; }
            asyncSetter = fn;
        };
        return result;
    };
    /*
     * Loads the specified module (Node or AMD environment) and applies the
     * given callback afterwards.
     */
    __.loadModule = function(path, fn) {
        if (typeof module !== 'undefined') { //node.js
            fn(require('./' + path));
        } else if (typeof define !== 'undefined' && define.amd) { //AMD
            //include relative to require's baseUrl 
            require(['./' + (__.modulePath === '' ?
                path :
                __.modulePath + '/' + path)], function(m) { fn(m); });
        } else { //global scope
            __.err('Include the necessary script.');
        }
    };
    /*
     * Loads the culture module with the specified ID and applies the given
     * callback afterwards (if defined). Note that the module has to be named
     * exactly after the given ID.
     */
    __.loadCulture = function(cultureId, fn) {
        var culture = __.cultures[cultureId];
        if (!culture) {
            try {
                __.loadModule(__.DIR_CULTURES + cultureId, function(culture) {
                    Formatter.validateCulture(culture);
                    __.cultures[cultureId] = culture;
                    if (typeof fn === "function") { fn(culture); }
                });
            } catch (e) {
                __.err('Failed to load culture ' + cultureId + ': ' + e.message);
            }
        } else {
            fn(culture);
        }
    };
    /*
     * Loads the flavor module with the specified ID and applies the given
     * callback. Note that the module file has to be named exactly after the
     * given ID.
     */
    __.loadFlavor = function(flavorId, fn) {
        var flavor = __.flavors[flavorId];
        if (!flavor) {
            try {
                __.loadModule(__.DIR_FLAVORS + flavorId, function(flavor) {
                    __.flavors[flavorId] = flavor;
                    if (typeof fn === "function") { fn(flavor); }
                });
            } catch (e) {
                __.err('Failed to load flavor ' + flavorId + ': ' + e.message);
            }
        } else {
            fn(flavor);
        }
    };
    
    /**
     * Allocates a new formatter with the specified options.
     * @constructor
     * @class Formatter
     * @param {Object} [options]
     * @return {Formatter}
     */
    Formatter = function(options) {
        this.options(options);
    };
    /**
     * Default options for a new Formatter.
     * Overwrite this property if you intend to allocate several formatters with
     * default options different to those initially specified by this module.
     * @static
     * @property defaultOptions
     * @type {Object}
     */
    Formatter.defaultOptions = __.initialDefaultFormatterOptions();
    /**
     * Resets the Formatter.
     * @static
     * @chainable
     * @method reset
     * @return {Formatter}
     */
    Formatter.reset = function() {
        __.initialState.establish();
        return Formatter;
    };
    /**
     * Tests if the given argument represents a valid culture object.
     * Throws an error if it is not valid.
     * @static
     * @chainable
     * @method validateCulture
     * @param {Object} culture
     * @return {Formatter}
     */
    Formatter.validateCulture = function(culture) {
        var msg = 'Invalid culture: ';
        if (typeof culture !== 'object') {
            __.err(msg + 'Not an object');
        }
        if (culture.id === undefined) {
            __.err(msg + 'Missing property "id"');
        }
        /* Numbers */
        if (culture.zeroDigit === undefined) {
            __.err(msg + 'Missing property "zeroDigit"');
        }
        if (culture.decimalSeparator === undefined) {
            __.err(msg + 'Missing property "decimalSeparator"');
        }
        if (culture.groupingSeparator === undefined) {
            __.err(msg + 'Missing property "groupingSeparator"');
        }
        if (culture.groupingSize === undefined) {
            __.err(msg + 'Missing property "groupingSize"');
        }     
        /* Currency */
        if (culture.currencySymbol === undefined) {
            __.err(msg + 'Missing property "currencySymbol"');
        }
        if (culture.currencyToken === undefined) {
            __.err(msg + 'Missing property "currencyToken"');
        }
        /* Weekday names */
        if (culture.weekdays === undefined) {
            __.err(msg + 'Missing property "weekdays"');
        }
        if (culture.weekdaysAbbr === undefined) {
            __.err(msg + 'Missing property "weekdaysAbbr"');
        }
        if (culture.firstDayOfWeek === undefined) {
            __.err(msg + 'Missing property "firstDayOfWeek"');
        }
        /* Month names */
        if (culture.months === undefined) {
            __.err(msg + 'Missing property "months"');
        }
        if (culture.monthsAbbr === undefined) {
            __.err(msg + 'Missing property "monthsAbbr"');
        }
        /* Morning/afternoon tokens */
        if (culture.amToken === undefined) {
            __.err(msg + 'Missing property "amToken"');
        }
        if (culture.pmToken === undefined) {
            __.err(msg + 'Missing property "pmToken"');
        }
        return Formatter;
    };
    /**
     * Sets the Formatter options or returns them if called without an argument.
     * @static
     * @chainable
     * @method options
     * @param {Object} [options] Formatter options:
     * 
      {
          path: './',
          flavorId: 'flavorId',
          cultureId: 'cultureId',
          lineSeparator: '\n'
      }
     * @return {Formatter|Object}
     *      Formatter if called as setter, Formatter options if called as getter
     */
    Formatter.options = function(options, fn) {
        //delegate to the default formatter instance
        if (arguments.length > 0) {
            __.instance.options(options, fn);
            return Formatter;
        }
        return __.instance.options();
    };
    /**
     * Formats the given arguments described by the given formatstring.
     * @static
     * @method format
     * @param {String} fstr Format string
     * @param {any} [data]* Data to be formatted
     * @return {String} Formatted data
     */
    Formatter.format = function(fstr) {
        //delegate to the default formatter instance
        return __.instance.format.apply(__.instance, arguments);
    };
    /**
     * Sets this Formatter's options or returns them if called without an
     * argument.
     * @chainable
     * @method options
     * @param {Object} [options] Formatter options:
              
      {
          path: './',
          flavorId: 'flavorId',
          cultureId: 'cultureId',
          lineSeparator: '\n'
      }
     * @return {Object} This Formatter's options if called as a getter, `this`
     *         if called as a setter
     */
    Formatter.prototype.options = function(options, fn) {
        var key,
            opt,
            nAsyncReturns = 0,
            done = function() {
                if (--nAsyncReturns === 0 && typeof fn === 'function') { fn(); }
            };
        if (arguments.length === 0) { return this._options; }
        if (options) {
            //count options to be delegated asynchronously before setting them
            for (key in options) {
                if (options.hasOwnProperty(key) &&
                        this._options.hasOwnProperty(key) &&
                        this._options[key].asyncSetter() !== undefined) {
                    nAsyncReturns++;
                }
            }
            //only set properties specified by the given options
            for (key in options) {
                if (options.hasOwnProperty(key) &&
                        this._options.hasOwnProperty(key)) {
                    this._options[key](options[key], done);
                }
            }
        } else { //set default options
            opt = __.deepCopy(Formatter.defaultOptions);
            //turn option properties into accessors
            for (key in opt) {
                if (opt.hasOwnProperty(key)) { opt[key] = __.accessor(opt[key]); }
            }
            //hook module loaders for flavor and culture changes
            opt.flavorId.asyncSetter(__.bind(function(flavorId, fn) {
                __.loadFlavor(flavorId, __.bind(function(flavor) {
                    this.options({ flavor: flavor });
                    fn(flavorId);
                }, this));
            }, this));
            opt.cultureId.asyncSetter(__.bind(function(cultureId, fn) {
                __.loadCulture(cultureId, __.bind(function(culture) {
                    this.options({ culture: culture });
                    fn(cultureId);
                }, this));
            }, this));
            this._options = opt;
        }
        return this;
    };
    /**
     * Formats the given arguments described by the given formatstring.
     * @method format
     * @param {String} fstr Format string
     * @param {any} [data]* Data to be formatted
     * @return {String} Formatted data
     */
    Formatter.prototype.format = function(fstr) {
        if (arguments.length === 0) { return undefined; }
        if (!__.cultures[this._options.cultureId()]) {
            __.err('Define a culture first');
        }
        if (!__.flavors[this._options.flavorId()]) {
            __.err('Define a flavor first');
        }
        //format
        return __.flavors[this._options.flavorId()].format(
            typeof fstr === 'string' ? fstr.split('') : fstr,
            Formatter,
            Array.prototype.slice.call(arguments, 1),
            this.options());
    };
    
    /**
     * Utility functions.
     * @static
     * @class Formatter.util
     */
    Formatter.util = {};
    /**
     * Returns true if the given arg is an Array, false otherwise.
     * @static
     * @method isArray
     * @param {any} arg
     * @return {Boolean}
     */
    Formatter.util.isArray = function(arg) {
        return Object.prototype.toString.call(arg) === '[object Array]';
    };
    /**
     * Returns true if the given arg is a String, false otherwise.
     * @static
     * @method isString
     * @param {any} arg
     * @return {Boolean}
     */
    Formatter.util.isString = function(arg) {
        return Object.prototype.toString.call(arg) === '[object String]';
    };
    /**
     * Returns the argument, a character array or an array of length 1
     * containing the argument depending whether the argument is an array,
     * a string or any other value.
     * @static
     * @method toArray
     * @param {any} arg
     * @return {Array}
     */
    Formatter.util.toArray = function(arg) {
        if (this.isArray(arg)) { return arg; }
        if (this.isString(arg)) { return arg.split(''); }
        return [arg];
    };
    /**
     * Concatenates the given argument n-1 times with itself and returns the
     * resulting string.
     * @static
     * @method concat
     * @param {String} arg
     * @param {Number} [n = 1]
     * @return {String}
     */
    Formatter.util.concat = function(arg, n) {
        n = n || 1;
        return new Array(n + 1).join(arg);
    };
    /**
     * Appends or prepends the given character to the given string until the
     * resulting string has the specified length.
     * @static
     * @method pad
     * @param {String} str Append or prepend to this string
     * @param {String} [ch = ' '] Character to be appended or prepended
     * @param {Number} [len = String(str).length] Length of the resulting string
     * @param {Boolean} [left = false] Prepend if true, append otherwise
     * @return {String}
     */
    Formatter.util.pad = function pad(str, ch, len, left) {
        str = String(str);
        ch = ch || ' ';
        len = len || str.length;
        left = Boolean(left);
        var delta = len - str.length;
        if (delta <= 0) { return str; }
        return (left ?
            this.concat(ch, delta) + str :
            str + this.concat(ch, delta));
    };
    /**
     * Prepends the given character to the given string until the resulting
     * string has the specified length.
     * @static
     * @method padLeft
     * @param {String} str Prepend to this string
     * @param {String} [ch = ' '] Character to be prepended
     * @param {Number} [len = String(str).length] Length of the resulting string
     * @return {String}
     */
    Formatter.util.padLeft = function(str, ch, len) {
        return this.pad(str, ch, len, true);
    };
    /**
     * Appends the given character to the given string until the resulting
     * string has the specified length.
     * @static
     * @method padRight
     * @param {String} str Append to this string
     * @param {String} [ch  = ' '] Character to be appended
     * @param {Number} [len = String(str).length] Length of the resulting string
     * @return {String}
     */
    Formatter.util.padRight = function(str, ch, len) {
        return this.pad(str, ch, len);
    };
    
    /**
     * Number utility functions.
     * @static
     * @class Formatter.util.number
     */
    Formatter.util.number = {
        /**
         * Greatest precise integer value in JavaScript.
         * @final
         * @static
         * @property MAX_INT
         * @type {Number}
         */
        MAX_INT: Math.pow(2, 53),
        /**
         * Greatest precise integer value in two's complement range.
         * @final
         * @static
         * @property MAX_SIGNED_INT
         * @type {Number}
         */
        MAX_SIGNED_INT: Math.pow(2, 52) - 1,
        /**
         * Smallest precise integer value in two's complement range.
         * @final
         * @static
         * @property MIN_SIGNED_INT
         * @type {Number}
         */
        MIN_SIGNED_INT: -Math.pow(2, 52)
    };
    /**
     * Returns true if the given number is less than zero or negative zero.
     * @static
     * @method isSigned
     * @param {Number} arg
     * @return {Boolean}
     */
    Formatter.util.number.isSigned = function(arg) {
        if (arg === 0) { return 1/arg < 0; }
        return arg < 0;
    };
    /**
     * Returns the given argument rounded to the given precision.
     * @static
     * @method round
     * @param {Number} arg Number to be rounded
     * @param {Number} [precision=0] Number of precise fractional digits. A
     *        falsy value specifies fractional precision of 0.
     * @return {Number}
     */
    Formatter.util.number.round = function(arg, precision) {
        if (!precision || precision < 0) { precision = 0; }
        var fac = Math.pow(10, precision);
        return Math.round(arg * fac) / fac;
    };
    /**
     * Returns the given argument as a Number within the range
     * [Formatter.util.number.MIN_SIGNED_INT, Formatter.util.number.MAX_SIGNED_INT].
     * @static
     * @method signedInt
     * @param {Number} arg 
     * @return {Number}
     */
    Formatter.util.number.signedInt = function(arg) {
        var r = Number(arg);
        if (r < this.MIN_SIGNED_INT) {
            return this.MIN_SIGNED_INT;
        }
        if (r > this.MAX_SIGNED_INT) {
            return this.MAX_SIGNED_INT;
        }
        return r;
    };
    /**
     * Returns a decimal integer representing two's complement of the given
     * number.
     * 
     * A JavaScript Number is a double-precision floating-point as specified by
     * the IEEE 754 standard. All positive integers up to 2^53 are represented
     * precisely, numbers beyond that threshold get their least significant bits
     * clipped (((Math.pow(2,53) + 1) - Math.pow(2,53) results to 0, not 1).
     * 
     * The argument is therefore interpreted as an integer within the range
     * [-2^52, 2^52-1]. A floating point argument is truncated, an argument out
     * of the expected range is set to the smallest or to the greatest precise
     * value depending on whether the argument is smaller than -2^52 or greater
     * than 2^52-1.
     * 
     * @static
     * @method twosComplement
     * @param {Number} arg
     * @return {Number}
     */
    Formatter.util.number.twosComplement = function(arg) {
        var r = this.signedInt(arg);
        if (r < 0) {
            return r + this.MAX_INT;
        }
        return this.MAX_INT - r;
    };
    /**
     * Returns true if the given argument represents a symbolic number (NaN,
     * POSITIVE_INFINITY, NEGATIVE_INFINITY), false otherwise.
     * @static
     * @method isSymbolicNumber
     * @param {Number} arg
     * @return {Boolean}
     */
    Formatter.util.number.isSymbolicNumber = function(arg) {
        return String(Number(arg)) === "NaN" ||
            Number(arg) === Number.POSITIVE_INFINITY ||
            Number(arg) === Number.NEGATIVE_INFINITY;
    };
    /**
     * Date utility functions for the Gregorian calendar.
     * @static
     * @class Formatter.util.date
     */
    Formatter.util.date = {
        /**
         * Milliseconds per hour.
         * @final
         * @static
         * @property MILLISECONDS_PER_HOUR
         * @type Number
         */
        MILLISECONDS_PER_HOUR: 3600000,
        /**
         * Milliseconds per day.
         * @final
         * @static
         * @property MILLISECONDS_PER_DAY
         * @type Number
         */
        MILLISECONDS_PER_DAY: 86400000,
        /**
         * Milliseconds per week.
         * @final
         * @static
         * @property MILLISECONDS_PER_WEEK
         * @type Number
         */
        MILLISECONDS_PER_WEEK: 604800000
    };
    /**
     * Returns the UNIX timestamp of the given date. The UNIX timestamp
     * describes a UTC date as number of seconds elapsed since the beginning
     * of the UNIX epoche (Midnight, 1970-01-01). Milliseconds of the given
     * date are truncated.
     * @static
     * @method timestamp
     * @param {Date} date Interpreted as a UTC value
     * @return {Number}
     */
     Formatter.util.date.timestamp = function(date) {
        return Math.floor(date.valueOf() / 1000);
    };
    /**
     * Returns the number of days for the specified month.
     * @static
     * @method daysOfMonth
     * @param {Date} date Interpreted as a UTC value
     * @return {Number}
     */
    Formatter.util.date.daysOfMonth = function(date) {
        var d = new Date(date.valueOf());
        d.setUTCDate(1);
        d.setUTCMonth(d.getUTCMonth() + 1);
        d.setUTCDate(0);
        return d.getUTCDate();
    };
    /**
     * Returns the number of days for the specified year.
     * @static
     * @method daysOfYear
     * @param {Date|Number} arg (Date is interpreted as a UTC value)
     * @return {Number}
     */
    Formatter.util.date.daysOfYear = function(arg) {
        return this.isLeapYear(arg) ? 366 : 365;
    };
    /**
     * Returns true if the specified year is a leap year, false otherwise.
     * @static
     * @method isLeapYear
     * @param {Date|Number} arg (Date is interpreted as a UTC value)
     * @return {Boolean}
     */
    Formatter.util.date.isLeapYear = function(arg) {
        if (arg instanceof Date) { arg = arg.getUTCFullYear(); }
        return arg % 4 === 0 && (arg % 100 !== 0 || arg % 400 === 0);
    };
    /**
     * Returns the culture-specific weekday of the given date. The first day
     * of the week corresponds to 0, the last day to 6.
     * @static
     * @method dayOfWeek
     * @param {Date} date Interpreted as a UTC value
     * @param {Object} culture Culture information
     * @return {Number}
     */
    Formatter.util.date.dayOfWeek = function(date, culture) {
        return (date.getUTCDay() + 7 - culture.firstDayOfWeek) % 7;
    };
    /**
     * Returns the day of the year specified by the given date. The first day
     * of the year corresponds to 1.
     * @static
     * @method dayOfYear
     * @param {Date} date Interpreted as a UTC value
     * @return {Number}
     */
    Formatter.util.date.dayOfYear = function(date) {
        var result = date.getUTCDate(),
            year = date.getUTCFullYear(),
            month = date.getUTCMonth() - 1;
            
        while (month >= 0) {
            result += this.daysOfMonth(new Date(Date.UTC(year, month)));
            month--;
        }
        return result;
    };
    /**
     * Returns a date representing the n-th day of the week specified by the
     * given date.
     *
     * @example
         var d = new Date('2012-07-04T00:00Z'), //Wednesday
             c = { firstDayOfWeek: 1 }; //culture with Monday as first weekday
             
         Formatter.util.date.nthDayOfWeek(
             d, c, 0); //Date representing '2012-07-02T00:00'
             
         Formatter.util.date.nthDayOfWeek(
             d, c, 6); //Date representing '2012-07-08T00:00'
     
     * @static
     * @method nthDayOfWeek
     * @param {Date} date Interpreted as a UTC value
     * @param {Object} culture Culture information
     * @param {Number} n In [0,6]
     * @return {Date}
     */
    Formatter.util.date.nthDayOfWeek = function(date, culture, n) {
        var d = new Date(date.valueOf());
        d.setUTCDate(d.getUTCDate() - this.dayOfWeek(date, culture) + n);
        return d;
    };
    /**
     * Returns a date representing the first day of the week specified by
     * the given date.
     * @static
     * @method firstDayOfWeek
     * @param {Date} date Interpreted as a UTC value
     * @param {Object} culture Culture information
     * @return {Date}
     */
    Formatter.util.date.firstDayOfWeek = function(date, culture) {
        return this.nthDayOfWeek(date, culture, 0);
    };
    /**
     * Returns a date representing the first day of the week specified by
     * the given date.
     * @static
     * @method lastDayOfWeek
     * @param {Date} date Interpreted as a UTC value
     * @param {Object} culture Culture information
     * @return {Date}
     */
    Formatter.util.date.lastDayOfWeek = function(date, culture) {
        return this.nthDayOfWeek(date, culture, 6);            
    };
    /**
     * Returns a date representing the first day of the month specified by
     * the given date.
     * @static
     * @method firstDayOfMonth
     * @param {Date} date Interpreted as a UTC value
     * @return {Date}
     */
    Formatter.util.date.firstDayOfMonth = function(date) {
        return new Date(new Date(date.valueOf()).setUTCDate(1));
    };
    /**
     * Returns a date representing the last day of the month specified by the
     * given date.
     * @static
     * @method lastDayOfMonth
     * @param {Date} date Interpreted as a UTC value
     * @return {Date}
     */
    Formatter.util.date.lastDayOfMonth = function(date) {
        var d = new Date(date.valueOf());
        d.setUTCDate(1);
        d.setUTCMonth(d.getUTCMonth() + 1);
        d.setUTCDate(0);
        return d;
    };
    /**
     * Returns a date representing the first day of the specified year.
     * @static
     * @method firstDayOfYear
     * @param {Date|Number} arg (Date is interpreted as a UTC value)
     * @return {Date}
     */
    Formatter.util.date.firstDayOfYear = function(arg) {
        if (!(arg instanceof Date)) { return new Date(Date.UTC(arg, 0, 1)); }
        var d = new Date(arg.valueOf());
        d.setUTCDate(1);
        d.setUTCMonth(0);
        return d;
    };
    /**
     * Returns a date representing the last day of the specified year.
     * @static
     * @method lastDayOfYear
     * @param {Date|Number} arg (Date is interpreted as a UTC value)
     * @return {Date}
     */
    Formatter.util.date.lastDayOfYear = function(arg) {
        var d = (arg instanceof Date ?
            new Date(arg.valueOf()) :
            new Date(Date.UTC(arg, 0)));
        d.setUTCDate(1);
        d.setUTCMonth(0);
        d.setUTCFullYear(d.getUTCFullYear() + 1);
        d.setUTCDate(0);
        return d;
    };
    /**
     * Returns the ISO-8601 week specified by the given date.
     * @static
     * @method isoWeek
     * @param {Date} date Interpreted as a UTC value
     * @return {Number}
     */
    Formatter.util.date.isoWeek = function(date) {
        var d = new Date(Date.UTC(date.getUTCFullYear(), 0, 4)),
            m, //monday, first calendar week 
            result;
        m = this.nthDayOfWeek(d, {firstDayOfWeek: 1}, 0);
        if (date.valueOf() < m.valueOf()) { //date before monday (1-3 Jan.)
            d.setUTCFullYear(d.getUTCFullYear() - 1);
            m = this.nthDayOfWeek(d, {firstDayOfWeek: 1}, 0);
        }
        return Math.floor(
            (date.valueOf() - m.valueOf()) / this.MILLISECONDS_PER_WEEK) + 1;
    };
    /**
     * Returns the century specified by the given date.
     * @static
     * @method century
     * @param {Date} date Interpreted as a UTC value
     * @return {Number}
     */
    Formatter.util.date.century = function(date) {
        return Math.floor(date.getUTCFullYear() / 100) + 1;
    };
    /**
     * Returns the number of past centuries specified by the given date.
     * @static
     * @method pastCenturies
     * @param {Date} date Interpreted as a UTC value
     * @return {Number}
     */
    Formatter.util.date.pastCenturies = function(date) {
        return Math.floor(date.getUTCFullYear() / 100);
    };
    /**
     * Returns true if the time specified by the given date is in the range
     * [00:00, 12:00). Returns false otherwise.
     * @static
     * @method isAM
     * @param {Date} date Interpreted as a UTC value
     * @return {Boolean}
     */
    Formatter.util.date.isAM = function(date) {
        var d = new Date(Date.UTC(
            date.getUTCFullYear(),
            date.getUTCMonth(),
            date.getUTCDate()
        ));
        return (date.valueOf() - d.valueOf()) < (12 * this.MILLISECONDS_PER_HOUR);
    };
    /**
     * Returns true if the time specified by the given date is in the range
     * [12:00, 00:00). Returns false otherwise.
     * @static
     * @method isPM
     * @param {Date} date Interpreted as a UTC value
     * @return {Boolean}
     */
    Formatter.util.date.isPM = function(date) {
        var d = new Date(Date.UTC(
            date.getUTCFullYear(),
            date.getUTCMonth(),
            date.getUTCDate()
        ));
        return (date.valueOf() - d.valueOf()) >= (12 * this.MILLISECONDS_PER_HOUR);
    };
    
    /**
     * Number formatter.
     * @static
     * @class Formatter.number
     */
    Formatter.number = {};
    /**
     * Returns the given number in hexadecimal exponential form.
     * <br/><br/>
     * Details on hexadecimal exponential encoding:
     * <ul>
     * <li><a href="http://en.wikipedia.org/wiki/Hexadecimal#Hexadecimal_exponential_notation">http://en.wikipedia.org/wiki/Hexadecimal#Hexadecimal_exponential_notation</a></li>
     * <li><a href="http://de.wikipedia.org/wiki/IEEE_754">http://de.wikipedia.org/wiki/IEEE_754</a></li>
     * <li><a href="http://www.2ality.com/2012/04/number-encoding.html">http://www.2ality.com/2012/04/number-encoding.html</a></li>
     * <li><a href="http://osr507doc.sco.com/en/topics/FltPtOps_DeNormNums.html">http://osr507doc.sco.com/en/topics/FltPtOps_DeNormNums.html</a></li>
     * </ul>
     * @static
     * @method toHexExp
     * @param {any} arg Number compatibel argument
     * @return {String} Hex exponential form of the given string
     */
    Formatter.number.toHexExp = function(arg) {
        var r,      //result
            b,      //string representing the given number in base 2
            len,    //b's length
            pos,    //floating point position in b
            sign,   //is the given number signed?
            m,      //mantissa
            exp,    //exponent
            i,
            ieee754_64_bias = 1023; //IEEE 754 (double precision) exponent bias
        arg = Number(arg);
        //return NaN, Infinity, -Infinity unchanged
        if (Formatter.util.number.isSymbolicNumber(arg)) { return String(arg); }
        //distinct negative zero from zero
        if (arg === 0) {
            return Formatter.util.number.isSigned(arg) ? '-0x0.0p0' : '0x0.0p0';
        }
        sign = arg < 0;
        arg = Math.abs(arg);
        b = arg.toString(2);
        pos = b.indexOf('.');
        if (pos < 0) { //integer
            exp = b.length - 1;
        } else {
            if (pos === 1 && Number(b.charAt(0)) === 0) { //negative exponent
                //find first fractional 1-bit
                len = b.length;
                i = 2;
                while (i < len && b.charAt(i) !== '1') { i++; }
                //consider exponent bias specified by IEEE 754 (double precision)
                exp = i >= ieee754_64_bias ?
                    -(ieee754_64_bias - 1) :
                    -(i - 1);
            } else { //positive exponent
                exp = b.slice(0, pos).length - 1;
            }
        }
        m = Number(arg / Math.pow(2, exp)).toString(16);
        if (m.indexOf('.') < 0) { m = m + '.0'; }
        r = '0x' + m + 'p' + exp;
        return sign ? '-' + r : r;
    };
    /**
     * Returns a string representing the given number in decimal form. 
     * @static
     * @method toDecimal
     * @param {any} arg Number compatible value to be formatted
     * @param {Object} [options] Formatting options. Default values:
     *
     {
         precision: undefined,   // Number of significant fractional digits. Data
                                 // type limited for falsy values other than 0.
         considerZeroSign: false // Whether to return a sign for negative zero or not
     }
     * @return {String}
     */
    Formatter.number.toDecimal = function(arg, options) {
        options = options || {};
        options.considerZeroSign = Boolean(options.considerZeroSign);
        var r = '',
            numStr,
            sign,
            m,
            exp, //exponent
            pos; //position of 'e' and '.'
        //return NaN, Infinity, -Infinity unchanged
        if (Formatter.util.number.isSymbolicNumber(arg)) { return String(arg); }
        //round to precision fractional digits
        if (options.precision || options.precision === 0) {
            arg = Formatter.util.number.round(Number(arg), options.precision);
        } else {
            arg = Number(arg);
        }
        //signed argument?
        sign = options.considerZeroSign ?
            Formatter.util.number.isSigned(arg) :
            arg < 0;
        //since Number.toString returns decimal notation for small numbers and
        //scientific notation for numbers greater than a certain threshold,
        //parsing is done based on the exponential form of the given number.
        numStr = arg.toExponential();
        pos = numStr.indexOf('e');
        m = numStr.slice((numStr.charAt(0) === '-' ? 1 : 0), pos);
        exp = Number(numStr.substr(pos + 1));
        pos = m.indexOf('.');
        if (pos < 0) { //integer mantissa
            r = m;
            pos = r.length;
        } else {
            r = m.slice(0, pos) + m.substr(pos + 1);
        }
        pos = pos + exp;
        if (0 < pos && pos < r.length) {
            r = r.slice(0, pos) + '.' + r.substr(pos);
        } else if (pos > r.length) {
            r = Formatter.util.padRight(r, '0', pos);
        } else if (pos <= 0) {
            r = '0.' + Formatter.util.padLeft(r, '0', r.length - pos);
            pos = 1;
        }
        //add fractional zero digits if the result's number of fractional digits
        //is less than the given precision
        if (options.precision &&
                options.precision > 0 &&
                r.length - pos - 1 < options.precision) {
            if (pos === r.length) { r += '.'; }
            r = Formatter.util.padRight(r, '0', pos + options.precision + 1);
        }
        //add sign
        if (sign) { r = '-' + r; }
        return r;
    };
    /**
     * Returns a string representing the given number in scientific notation.
     * @static
     * @method toScientific
     * @param {any} arg Number compatible value to be formatted
     * @param {Object} [options] Formatting options. Default values:
     *
     {
         precision: undefined,   // Mantissa precision. Data type limited for falsy
                                 // values other than 0.
         expMinWidth: 1,         // Min width of the exponent (excl. 'e' and sign).
         upperCase: false,       // Whether to use 'e' or 'E' for the exponent.
         considerZeroSign: false // Whether to return a sign for negative zero or not
     }
     * @return {String}
     */
    Formatter.number.toScientific = function(arg, options) {
        options = options || {};
        options.expMinWidth = options.expMinWidth || 1;
        options.upperCase = Boolean(options.upperCase);
        options.considerZeroSign = Boolean(options.considerZeroSign);
        var r,
            numStr,
            sign,
            m, //mantissa
            exp,
            expStr = options.upperCase ? 'E' : 'e',
            len,
            pos,
            i;
        arg = Number(arg);
        numStr = String(arg);
        if (Formatter.util.number.isSymbolicNumber(arg)) {
            return numStr;
        }
        sign = options.considerZeroSign ?
            Formatter.util.number.isSigned(Number(arg)) :
            Number(arg) < 0;
        //mantissa
        pos = numStr.indexOf('e');
        if (pos < 0) {
            numStr = arg.toExponential();
            pos = numStr.indexOf('e');
        }
        m = Number(numStr.slice(0, pos));
        //exponent
        expStr += numStr.charAt(pos + 1); //sign
        exp = numStr.substr(pos + 2); //skip 'e', skip sign
        len = options.expMinWidth - exp.length;
        for (i = len; i > 0; i--) { expStr += '0'; }
        expStr += exp;
        //mantissa precision
        if (options.precision || options.precision === 0) {
            r = String(Formatter.util.number.round(m, options.precision));
            if (options.precision > 0) {
                pos = r.indexOf('.');
                if (pos < 0) {
                    r += '.';
                    r = Formatter.util.padRight(r, '0', r.length +
                        options.precision);
                } else {
                    r = Formatter.util.padRight(r, '0',
                        options.precision + pos + 1);
                }
            }
        } else {
            r = String(m);
        }
        if (m === 0 && sign && options.considerZeroSign) {
            r = '-' + r;
        }
        return r + expStr;
    };
    
    /**
     * Functions ought to format date components.
     * @static
     * @class Formatter.date
     */
    Formatter.date = {};
    
    /**
     * Returns a string representing the specified year.
     * 
     * Negative years are formatted with the prefix '-' by default. Set the
     * option property `bcPrefix` or `bcPostfix` to change the default behavior.
     * Note that setting a non-falsy postfix implies the prefix ''.
     * 
     * The number of digits in the resulting string depends on the option
     * properties `maxDigits` and `leadingZeros`.
     * The resulting year is zero padded if `maxDigits` is greater than the
     * number of year digits and `leadingZeros` is set to true.
     * Most significant digits of the resulting year are truncated if `maxDigits`
     * is less than the number of year digits. In that case leading zeros are
     * also truncated except `leadingZeros` is set to true.
     * 
     * @example
         var d = new Date('2012-01-01T00:00Z');
         Formatter.date.year(d); //'2012'
         Formatter.date.year(d, {maxDigits:3}); //'12'
         Formatter.date.year(d, {maxDigits:3, leadingZeros:true}); //'012'
         Formatter.date.year(d, {maxDigits:5, leadingZeros:true}); //'02012'
         
         d = new Date(Date.UTC(-2012, 1));
         Formatter.date.year(d); //'-2012'
         Formatter.date.year(d, {bcPostfix: ' BC.'}); //'2012 BC.'
         Formatter.date.year(d, {bcPrefix: 'BC.'}); //'BC.2012'
     
     * @static
     * @method year
     * @param {Date} date Interpreted as a UTC value
     * @param {Object} [options] Format options. Default values are:
     * 
        {
            bcPrefix: '-',
            bcPostfix: '',
            leadingZeros: false
            maxDigits: number of year digits
        }
     *
     * @return {String}
     */
    Formatter.date.year = function(date, options) {
        var y = date.getUTCFullYear(),
            bc = (y < 0),
            opt = options || {};
        opt.bcPostfix = opt.bcPostfix || '';
        opt.bcPrefix = (opt.bcPostfix ? '' : (opt.bcPrefix || '-'));
        opt.leadingZeros = opt.leadingZeros || false;
        opt.maxDigits = opt.maxDigits ||
                (bc ? String(y).length - 1 : String(y).length);
        y = String(Math.abs(y % Math.pow(10, opt.maxDigits)));
        if (opt.leadingZeros) {
            y = Formatter.util.padLeft(y, '0', opt.maxDigits);
        }
        return (bc ? [opt.bcPrefix, y, opt.bcPostfix].join('') : y);
    };
    
    /**
     * Returns a string representing the day of the year specified by the
     * given date.
     * @static
     * @method dayOfYear
     * @param {Date} date Interpreted as a UTC value
     * @param {Boolean} leadingZeros Zero padded result?
     * @return {String}
     */
    Formatter.date.dayOfYear = function(date, leadingZeros) {
        return leadingZeros ?
            Formatter.util.padLeft(Formatter.util.date.dayOfYear(date), '0', 3) :
            String(Formatter.util.date.dayOfYear(date));
    };
    /**
     * Returns a string representing the month specified by the given date.
     * @static
     * @method month
     * @param {Date} date Interpreted as a UTC value
     * @param {Boolean} leadingZero Zero padded result?
     * @return {String}
     */
     Formatter.date.month = function(date, leadingZero) {
        var m = String(date.getUTCMonth() + 1);
        if (leadingZero && m.length < 2) { return '0' + m; }
        return m;
    };
    /**
     * Returns a string representing the day of the month specified by the
     * given date.
     * @static
     * @method dayOfMonth
     * @param {Date} date Interpreted as a UTC value
     * @param {Boolean} leadingZero Zero padded result?
     * @return {String}
     */
     Formatter.date.dayOfMonth = function(date, leadingZero) {
        var d = String(date.getUTCDate());
        if (leadingZero && d.length < 2) { return '0' + d; }
        return d;
    };
    /**
     * Returns the culture-specific month name specfied by the given date.
     * @static
     * @method monthName
     * @param {Date} date Interpreted as a UTC value
     * @param {Object} culture Culture information
     * @return {String}
     */
     Formatter.date.monthName = function(date, culture) {
        return culture.months[date.getUTCMonth()];
    };
    /**
     * Returns the culture-specific abbreviated month name specfied by the
     * given date.
     * @static
     * @method abbreviatedMonthName
     * @param {Date} date Interpreted as a UTC value
     * @param {Object} culture Culture information
     * @return {String}
     */
     Formatter.date.abbreviatedMonthName = function(date, culture) {
        return culture.monthsAbbr[date.getUTCMonth()];
    };
    /**
     * Returns the culture-specific weekday name specfied by the given date.
     * @static
     * @method weekdayName
     * @param {Date} date Interpreted as a UTC value
     * @param {Object} culture Culture information
     * @return {String}
     */
     Formatter.date.weekdayName = function(date, culture) {
        return culture.weekdays[date.getUTCDay()];
    };
    /**
     * Returns the culture-specific abbreviated weekday name specfied by
     * the given date.
     * @static
     * @method abbreviatedWeekdayName
     * @param {Date} date Interpreted as a UTC value
     * @param {Object} culture Culture information
     * @return {String}
     */
     Formatter.date.abbreviatedWeekdayName = function(date, culture) {
        return culture.weekdaysAbbr[date.getUTCDay()];
    };
    /**
     * Returns the formatted hours specified by the given date.
     * @static
     * @method hours
     * @param {Date} date Interpreted as a UTC value
     * @param {Boolean} leadingZero Zero padded result?
     * @param {Boolean} h12 Hours in [1,12]?
     * @return {String}
     */
    Formatter.date.hours = function(date, leadingZero, h12) {
        var h = date.getUTCHours();
        if (h12) {
            h = h % 12;
            if (h === 0) { h = 12; }
        }
        return String(leadingZero && h < 10 ? ['0', h].join('') : h);
    };
    /**
     * Returns the formatted minutes specified by the given date.
     * @static
     * @method minutes
     * @param {Date} date Interpreted as a UTC value
     * @param {Boolean} leadingZero Zero padded result?
     * @return {String}
     */
    Formatter.date.minutes = function(date, leadingZero) {
        var m = date.getUTCMinutes();
        return String(leadingZero && m < 10 ? ['0', m].join('') : m);
    };
    /**
     * Returns the formatted seconds specified by the given date.
     * @static
     * @method seconds
     * @param {Date} date Interpreted as a UTC value
     * @param {Boolean} leadingZero Zero padded result?
     * @return {String}
     */
    Formatter.date.seconds = function(date, leadingZero) {
        var s = date.getUTCSeconds();
        return String(leadingZero && s < 10 ? ['0', s].join('') : s);
    };
    /**
     * Returns the formatted milliseconds specified by the given date.
     * @static
     * @method milliseconds
     * @param {Date} date Interpreted as a UTC value
     * @param {Boolean} leadingZeros Zero padded result?
     * @return {String}
     */
    Formatter.date.milliseconds = function(date, leadingZeros) {
        var ms = date.getUTCMilliseconds();
        if (leadingZeros) {
            return Formatter.util.pad(ms, '0', 3, true);
        }
        return String(ms);
    };
    /**
     * Returns a string representing the morning/afternoon designator for the
     * time specified by the given date.
     * @static
     * @method timeDesignator
     * @param {Date} date Interpreted as a UTC value
     * @param {Object} culture Culture information
     * @return {String}
     */
    Formatter.date.timeDesignator = function(date, culture) {
        return (Formatter.util.date.isAM(date) ?
            culture.amToken :
            culture.pmToken);
    };
    /**
     * Returns a string representing the given date's time zone offset from UTC.
     * @static
     * @method timezoneOffset
     * @param {Date} date Interpreted as a date with the same timezone as
     *        provided by the host OS.
     * @return {String}
     */
    Formatter.date.timezoneOffset = function(date) {
        var tz = date.toTimeString().match(/GMT((?:\+|\-)\d{4})/);
        return (Formatter.util.isArray(tz) && tz.length > 1 ? tz[1] : undefined);
    };
    /**
     * Returns a string representing the time zone abbreviation specified by
     * the given date.
     * @static
     * @method abbreviatedTimezone
     * @param {Date} date Interpreted as a date with the same timezone as
     *        provided by the host OS.
     * @return {String}
     */
    Formatter.date.abbreviatedTimezone = function(date) {
        var tz = date.toTimeString().match(/\((\w+)\)/);
        return (Formatter.util.isArray(tz) && tz.length > 1 ? tz[1] : undefined);
    };
    /**
     * Returns a string representing the 24h time specified by the given date.
     * @static
     * @method time
     * @param {Date} date Interpreted as a UTC value
     * @param {Boolean} leadingZeros Zero padded time components?
     * @return {String}
     */
    Formatter.date.time = function(date, leadingZeros) {
        return this.hours(date, leadingZeros) + ':' +
            this.minutes(date, leadingZeros) + ':' +
            this.seconds(date, leadingZeros);
    };
    /**
     * Returns a string representing the 12h time specified by the given date.
     * @static
     * @method time12
     * @param {Date} date Interpreted as a UTC value
     * @param {Boolean} leadingZeros Zero padded time components?
     * @param {Boolean} designator Including time designator (AM/PM)?
     * @return {String}
     */
    Formatter.date.time12 = function(date, leadingZeros, designator, culture) {
        var t = this.hours(date, leadingZeros, true) + ':' +
            this.minutes(date, leadingZeros) + ':' +
            this.seconds(date, leadingZeros);
        if (designator) {
            t += (' ' + this.timeDesignator(date, culture));
        }
        return t;
    };
    
    /*
     * Exports this module to node.
     */
    function exportNode(mod) {
        //load default culture and flavor module
        mod.Formatter.options({
            cultureId: mod.Formatter.options().cultureId(),
            flavorId: mod.Formatter.options().flavorId()
        });
        module.exports = mod;
    }
    /*
     * Exports this module as an AMD module. Initially loading default flavor
     * and culture modules requires them to be specified as dependencies of
     * this module.
     */
    function exportAmd(mod) {
        //load default culture and flavor module
        var culturePath = ['.',
                __.DIR_CULTURES,
                mod.Formatter.options().cultureId()].join('/'),
            flavorPath = ['.',
                __.DIR_FLAVORS,
                mod.Formatter.options().flavorId()].join('/'),
            baseUrl;
        define([culturePath, flavorPath, 'require'],
            function(culture, flavor, require) {
                //extract require's baseUrl
                baseUrl = require.toUrl('.').slice(0, -1);
                //extract the module directory, relative to baseUrl
                __.modulePath = require.toUrl('../').slice(baseUrl.length, -2);
                __.cultures[mod.Formatter.options().cultureId()] = culture;
                __.flavors[mod.Formatter.options().flavorId()] = flavor;
                mod.Formatter.options({
                    cultureId: mod.Formatter.options().cultureId(),
                    flavorId: mod.Formatter.options().flavorId()
                });
                return mod;
        });
    }
    /*
     * Exports this module to the global scope.
     */
    function exportGlobal() {
        if (root.pat && root.pat._private) {
            root.pat.Formatter = Formatter;
            //culture modules available?
            if (typeof root.pat._private.cultures === 'object') {
                __.cultures = root.pat._private.cultures;
                //use default culture if available
                if (__.cultures[Formatter.defaultOptions.cultureId]) {
                    Formatter.options({
                        cultureId: Formatter.defaultOptions.cultureId
                    });
                }
                //use the first culture of the <script> sequence of imported
                //culture scripts. fallback: first culture property as defined
                //by foreach
                else if (__.countOwnProperties(__.cultures) > 0) {
                    Formatter.defaultOptions.cultureId =
                        root.pat._private.preferredCultureId ||
                        __.ownPropertyNames(__.cultures)[0];
                    Formatter.options({
                        cultureId: Formatter.defaultOptions.cultureId
                    });
                }
            }
            //flavor modules available?
            if (typeof root.pat._private.flavors === 'object') {
                __.flavors = root.pat._private.flavors;
                //use default flavor if available
                if (__.flavors[Formatter.defaultOptions.flavorId]) {
                    Formatter.options({
                        flavorId: Formatter.defaultOptions.flavorId
                    });
                }
                //use the first flavor of the <script> sequence of imported
                //flavor scripts. fallback: first flavor property as defined
                //by foreach
                else if (__.countOwnProperties(__.flavors) > 0) {
                    Formatter.options({
                        flavorId: root.pat._private.preferredFlavorId ||
                            __.ownPropertyNames(__.flavors)[0]
                    });
                }
            }
            root.pat._private = mod._private;
        } else {
            __.err('Load culture and flavor first');
        }
    }
    /*
     * Creates the default Formatter instance and exports this module.
     */
    function init() {
        Formatter.defaultOptions = __.initialDefaultFormatterOptions();
        __.instance = new Formatter();
        mod = {
            Formatter: Formatter,
            _private: __
        };
        if (typeof module !== 'undefined') {
            exportNode(mod);
        }
        else if (typeof define !== 'undefined' && define.amd) { //AMD
            exportAmd(mod);
        }
        else if (root) {
            exportGlobal(mod);
        }
        __.initialState.set();        
    }
    
    init();
    
}(this));
