/**
 * Tests java flavored format specifiers.
 */

/*global require, describe, it*/
/*jslint sloppy:true nomen:true plusplus:true*/

var expect = require('expect.js'),
    sinon = require('sinon'),
    pat = require(__dirname + '/../../lib/pat.js'),
    flavor = require(__dirname + '/../../lib/flavors/java.js'),
    Scanner = flavor.Scanner,
    Parser = flavor.Parser,
    _, //unit under test
    deAT = {
        id: 'deAT',
        /* Numbers */
        zeroDigit: '0',
        decimalSeparator: ',',
        groupingSeparator: '.',
        groupingSize: 3,
        /* Currency */
        currencySymbol: '€',
        currencyToken: 'EUR',
        /* Weekday names */
        weekdays: ['Sonntag', 'Montag', 'Dienstag', 'Mittwoch', 'Donnerstag', 'Freitag', 'Samstag'],
        weekdaysAbbr: ['So', 'Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa'],
        firstDayOfWeek: 1,
        /* Month names */
        months: ['Jänner', 'Februar', 'März', 'April', 'Mai', 'Juni', 'Juli', 'August', 'September', 'Oktober', 'November', 'Dezember'],
        monthsAbbr: ['Jän', 'Feb', 'Mär', 'Apr', 'Mai', 'Jun', 'Jul', 'Aug', 'Sep', 'Okt', 'Nov', 'Dez'],
        /* Morning/afternoon tokens (e.g. 'am', 'pm') */
        amToken: 'am',
        pmToken: 'pm'
    },
    enUS = {
        id: 'enUS',
        /* Numbers */
        zeroDigit: '0',
        decimalSeparator: '.',
        groupingSeparator: ',',
        groupingSize: 3,
        /* Currency */
        currencySymbol: '$',
        currencyToken: 'USD',
        /* Weekday names */
        weekdays: ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'],
        weekdaysAbbr: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        firstDayOfWeek: 1,
        /* Month names */
        months: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
        monthsAbbr: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        /* Morning/afternoon tokens (e.g. 'am', 'pm') */
        amToken: 'am',
        pmToken: 'pm'
    };

//use sinon.js assertions with expect.js using 'was' syntax
expect = require('sinon-expect').enhance(expect, sinon, 'was');

//set the flavor's formatter
flavor._private.fmt = pat.Formatter;

describe("Java-Formatter -", function() {
    var _ = flavor.Formatter,
        l = _.number.localize;
    
    describe("#number.localize.digits with ['1','2','3'] and Austrian culture", function() {
        var n = ['1','2','3'];
        it("should return the array unchanged", function() {
            l.digits(n, deAT);
            expect(n).to.eql(['1','2','3']);
        });
    });
    
    describe("#number.localize.digits with ['-','1','2'] and Austrian culture", function() {
        var n = ['-','1','2'];
        it("should return the array unchanged", function() {
            l.digits(n, deAT);
            expect(n).to.eql(['-','1','2']);
        });
    });
    
    describe("#number.localize.decimalSeparator with ['0','.','1'] and Austrian culture", function() {
        var n = ['0','.','1'];
        it("should return ['0',',','1']", function() {
            l.decimalSeparator(n, deAT);
            expect(n).to.eql(['0',',','1']);
        });
    });
    
    describe("#number.localize.groupingSeparator with ['1','2','0','0','2','0','0'] and Austrian culture", function() {
        var n = ['1','2','0','0','2','0','0'];
        it("should return ['1','.','2','0','0','.','2','0','0']", function() {
            l.groupingSeparator(n, deAT);
            expect(n).to.eql(['1','.','2','0','0','.','2','0','0']);
        });
    });
    
    describe("#number.localize.zeroPad with ['1','2','0'], width 5, and Austrian culture", function() {
        var n = ['1','2','0'];
        it("should return ['0','0','1','2','0']", function() {
            l.zeroPad(n, 5, deAT);
            expect(n).to.eql(['0','0','1','2','0']);
        });
    });
    
    describe("#number.localize.zeroPad with ['-','0','1','2','0'], width 8, and Austrian culture", function() {
        var n = ['-','0','1','2','0'];
        it("should return ['-','0','0','0','0','1','2','0']", function() {
            l.zeroPad(n, 8, deAT);
            expect(n).to.eql(['-','0','0','0','0','1','2','0']);
        });
    });
    
    describe("#number.localize.sign with ['1'] and { leadingPlusWhenPositive: true }", function() {
        var n = ['1'];
        it("should return ['+','1']", function() {
            l.sign(n, { leadingPlusWhenPositive: true });
            expect(n).to.eql(['+','1']);
        });
    });
    
    
    describe("#number.localize.sign with ['1'] and { leadingSpaceWhenPositive: true }", function() {
        var n = ['1'];
        it("should return [' ','1']", function() {
            l.sign(n, { leadingSpaceWhenPositive: true });
            expect(n).to.eql([' ','1']);
        });
    });
    
});

describe('Java-Scanner -', function() {
    
    before(function() {
        _ = new Scanner();
    });
    
    /*
     * Test expected token properties.
     */
    describe('Property "tokenCategories"', function() {
        it('should be an object', function() {
            expect(Scanner.tokenCategories).to.be.an('object');
        });
        it('should have a number property "general"', function() {
            expect(Scanner.tokenCategories.general).to.be.a('number');
        });
        it('should have a number property "character"', function() {
            expect(Scanner.tokenCategories.character).to.be.a('number');
        });
        it('should have a number property "integral"', function() {
            expect(Scanner.tokenCategories.integral).to.be.a('number');
        });
        it('should have a number property "floatingPoint"', function() {
            expect(Scanner.tokenCategories.floatingPoint).to.be.a('number');
        });
        it('should have a number property "percent"', function() {
            expect(Scanner.tokenCategories.percent).to.be.a('number');
        });
        it('should have a number property "lineSeparator"', function() {
            expect(Scanner.tokenCategories.lineSeparator).to.be.a('number');
        });
        it('should have a number property "datetime"', function() {
            expect(Scanner.tokenCategories.datetime).to.be.a('number');
        });
    });
        
    /*
     * Test weird input.
     */
    describe('Setting falsy input', function() {
        it('should result in an empty input array', function() {
            expect(function() { _.input(false); }).not.to.throwError();
            expect(_.input()).to.eql([]);
            expect(function() { _.input(undefined); }).not.to.throwError();
            expect(_.input()).to.eql([]);
            expect(function() { _.input(null); }).not.to.throwError();
            expect(_.input()).to.eql([]);
            expect(function() { _.input(0); }).not.to.throwError();
            expect(_.input()).to.eql([]);
        });
    });
    
    /**
     * hasNext
     */
    
    describe('Calling #hasNext() for the empty character', function() {
        it('should return false', function() {
            _.input('');
            expect(_.hasNext()).to.be(false);
        });
    });
    
    describe('Calling #next() for the empty character', function() {
        it('should return undefined', function() {
            _.input('');
            expect(_.next()).to.be(undefined);
        });
    });
    
    describe('Calling #hasNext() for a single character (not the percent sign)', function() {
        it('should return true', function() {
            _.input('a');
            expect(_.hasNext()).to.be(true);
        });
    });
    
    describe('Calling #next() for a single character (not the percent sign)', function() {
        it('should return an object', function() {
            _.input('a');
            expect(_.next()).to.be.an('object');
        });
    });
    
    describe('Calling #hasNext() after the first #next() call using a single format specifier "%b"', function() {
        it('should return false', function() {
            _.input('%b');
            expect(_.next()).to.be.an('object');
            expect(_.hasNext()).to.be(false);
        });
    });
    
    describe('Calling #next() for the second time using a single format specifier "%b"', function() {
        it('should return undefined', function() {
            _.input('%b');
            expect(_.next()).to.be.an('object');
            expect(_.next()).to.be(undefined);
        });
    });
    
    /*
     * Test "normal" text (without format specifiers)
     */
    
    describe('Calling #next() with normal text input (string literal)', function() {
        it('should return a text token', function() {
            var token,
                text = 'normal text without any format specifiers';
            _.input(text);
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.category).to.be(Scanner.tokenCategories.text);
            expect(token.value).to.be(text);
        });
    });
    
    describe('Calling #next() with normal text input (string object)', function() {
        it('should return a text token', function() {
            var token,
                text = new String('normal text without any format specifiers');
            _.input(text);
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.category).to.be(Scanner.tokenCategories.text);
            expect(token.value).to.be(text.valueOf());
        });
    });
    
    /*
     * Scanning an incomplete format specifier should throw an error.
     */
    (function() {
        var i,
            incomplete = [
                '%t', '%T', //incomplete datetime
                '%2$', //just the argument index
                '%-', '%#', '%+', '% ', '%0', '%,', '%(', //just the flag
                '%1', //just the width
                '%.' //incomplete precision
        ];
        
        function testInvalidFormatSpecifier(formatSpecifier) {
            describe('Scanning invalid format "' + formatSpecifier + '"', function() {
                it('should throw an error', function() {
                    _.input(formatSpecifier);
                    expect(function() { _.next(); }).to.throwError();
                });
            });
        }
        
        for (i = incomplete.length - 1; i >= 0; i--) {
            testInvalidFormatSpecifier(incomplete[i]);
        }
    }());
    
    /*
     * Scanned format specifiers should have a corresponding token with the
     * correct category.
     */
    
    (function() {
        var categoryName,
            expectedCategory,
            dt = ['H', 'I', 'k', 'l', 'M', 'S', 'L', 'p', 'z', 'Z', 's', 'Q',
                  'B', 'b', 'h', 'A', 'a', 'C', 'Y', 'y', 'j', 'm', 'd', 'e',
                  'R', 'T', 'r', 'D', 'F', 'c'],
            i;
            
        function testTokenCategory(formatSpecifier, categoryName, expectedCategory) {
            describe('Scanning ' + categoryName + ' format "' + formatSpecifier + '"', function() {
                it('should return a token with property "category" set to ' + expectedCategory, function() {
                    var token;
                    _.input(formatSpecifier);
                    token = _.next();
                    expect(token).to.be.an('object');
                    expect(token.category).to.be(expectedCategory);
                });
            });
        }
        
        categoryName = 'general';
        expectedCategory = Scanner.tokenCategories.general;
        testTokenCategory('%b', categoryName, expectedCategory);
        testTokenCategory('%B', categoryName, expectedCategory);
        testTokenCategory('%s', categoryName, expectedCategory);
        testTokenCategory('%S', categoryName, expectedCategory);
        
        categoryName = 'character';
        expectedCategory = Scanner.tokenCategories.character;
        testTokenCategory('%c', categoryName, expectedCategory);
        testTokenCategory('%C', categoryName, expectedCategory);
        
        categoryName = 'integral';
        expectedCategory = Scanner.tokenCategories.integral;
        testTokenCategory('%d', categoryName, expectedCategory);
        testTokenCategory('%o', categoryName, expectedCategory);
        testTokenCategory('%x', categoryName, expectedCategory);
        testTokenCategory('%X', categoryName, expectedCategory);
        
        categoryName = 'floating point';
        expectedCategory = Scanner.tokenCategories.floatingPoint;
        testTokenCategory('%e', categoryName, expectedCategory);
        testTokenCategory('%E', categoryName, expectedCategory);
        testTokenCategory('%f', categoryName, expectedCategory);
        testTokenCategory('%g', categoryName, expectedCategory);
        testTokenCategory('%G', categoryName, expectedCategory);
        testTokenCategory('%a', categoryName, expectedCategory);
        testTokenCategory('%A', categoryName, expectedCategory);
        
        categoryName = 'percent';
        expectedCategory = Scanner.tokenCategories.percent;
        testTokenCategory('%%', categoryName, expectedCategory);
        
        categoryName = 'line separator';
        expectedCategory = Scanner.tokenCategories.lineSeparator;
        testTokenCategory('%n', categoryName, expectedCategory);
        
        categoryName = 'datetime';
        expectedCategory = Scanner.tokenCategories.datetime;
        for (i = dt.length - 1; i >= 0; i--) {
            testTokenCategory('%t' + dt[i], categoryName, expectedCategory);
            testTokenCategory('%T' + dt[i], categoryName, expectedCategory);
        }
    }());
        
    /*
     * Test argument index.
     */
    
    describe('Scanning the format specifier "%b%b%b" (implicit indexing)', function() {
        var token;
        before(function() {
            _.input('%b%b%b');
        });
        it('should return a token with an argument index of 0 when next was called once', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(0);
        });
        it('should return a token with an argument index of 1 when next was called twice', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(1);
        });
        it('should return a token with an argument index of 2 when next was called thrice', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(2);
        });
        it('should return undefined when next was called for the 4th time', function() {
            expect(_.next()).to.be(undefined);
        });
    });
    
    describe('Scanning the format specifier "%5$b%1$b%7$b" (explicit indexing)', function() {
        var token;
        before(function() {
            _.input('%5$b%1$b%7$b');
        });
        it('should return a token with an argument index of 4 when next was called once', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(4);
        });
        it('should return a token with an argument index of 0 when next was called twice', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(0);
        });
        it('should return a token with an argument index of 6 when next was called thrice', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(6);
        });
    });
    
    describe('Scanning the format specifier "%<b" (relative indexing)', function() {
        it('should result in an error', function() {
            _.input('%<b');
            expect(function() { _.next(); }).to.throwError();
        });
    });
    
    describe('Scanning the format specifier "%b%b%<b%<b%b%<b" (mixed indexing)', function() {
        var token;
        before(function() {
            _.input('%b%b%<b%<b%b%<b');
        });
        it('should return a token with an argument index of 0 when next was called once', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(0);
        });
        it('should return a token with an argument index of 1 when next was called twice', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(1);
        });
        it('should return a token with an argument index of 1 when next was called thrice', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(1);
        });
        it('should return a token with an argument index of 1 when next was called for the 4th time', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(1);
        });
        it('should return a token with an argument index of 2 when next was called for the 5th time', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(2);
        });
        it('should return a token with an argument index of 2 when next was called for the 6th time', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(2);
        });
    });
    
    describe('Scanning the format specifier "%5$b%<b%b%<b" (mixed indexing)', function() {
        var token;
        before(function() {
            _.input('%5$b%<b%b%<b');
        });
        it('should return a token with an argument index of 4 when next was called once', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(4);
        });
        it('should return a token with an argument index of 4 when next was called twice', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(4);
        });
        it('should return a token with an argument index of 0 when next was called thrice', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(0);
        });
        it('should return a token with an argument index of 0 when next was called for the 4th time', function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(0);
        });
    });
    
    describe('Scanning the format specifier "%0f" (implicit indexing)', function() { //width
        it('should return a token with argument index 0', function() {
            var token;
            _.input('%0f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(0);
        });
    });
    
    describe('Scanning the format specifier "%+f" (implicit indexing)', function() { //flag
        it('should return a token with argument index 0', function() {
            var token;
            _.input('%+f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(0);
        });
    });
    
    describe('Scanning the format specifier "%05f" (implicit indexing)', function() { //flag, width
        it('should return a token with argument index 0', function() {
            var token;
            _.input('%05f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(0);
        });
    });
    
    describe('Scanning the format specifier "%.3f" (implicit indexing)', function() { //precision
        it('should return a token with argument index 0', function() {
            var token;
            _.input('%.3f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.argumentIndex).to.be(0);
        });
    });
    
    /*
     * Test format specifiers with a single flag.
     */
    
    (function() {
        var flags = {
            leftJustify: '%-f',
            alternateForm: '%#f',
            leadingPlusWhenPositive: '%+f',
            leadingSpaceWhenPositive: '% f',
            zeroPad: '%0f',
            localizedGroupingSeparator: '%,f',
            parenthesesWhenNegative: '%(f'
        },
        key;
        
        function testFlagPropertyRelation(formatSpecifier, property) {
            describe('Scanning format specifier "' + formatSpecifier + '"', function() {
                it('should return a token with property "options.' + property + '", set to true', function() {
                    var token;
                    _.input(formatSpecifier);
                    token = _.next();
                    expect(token).to.be.an('object');
                    expect(token.options).to.be.an('object');
                    expect(token.options[property]).to.be(true);
                });
            });
        }
        for (key in flags) {
            if (flags.hasOwnProperty(key)) {
                testFlagPropertyRelation(flags[key], key);
            }
        }
    }());
    
    /*
     * Test format specifiers with several flags.
     */
    
    describe('Scanning the format specifier "%-#+ 0,(f" (all flags)', function() {
        it('should return a token with all options set', function() {
            var token;
            _.input('%-#+ 0,(f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.options).to.be.an('object');
            expect(token.options.leftJustify).to.be(true);
            expect(token.options.alternateForm).to.be(true);
            expect(token.options.leadingPlusWhenPositive).to.be(true);
            expect(token.options.leadingSpaceWhenPositive).to.be(true);
            expect(token.options.zeroPad).to.be(true);
            expect(token.options.localizedGroupingSeparator).to.be(true);
            expect(token.options.parenthesesWhenNegative).to.be(true);
        });
    });
    
    /*
     * Test width.
     */
    
    describe('Scanning the format specifier "%15f"', function() {
        it('should return a token with property "width" set to 15', function() {
            var token;
            _.input('%15f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.width).to.be(15);
        });
    });
    
    describe('Scanning the format specifier "%1234567890f"', function() {
        it('should return a token with property "width" set to 1234567890', function() {
            var token;
            _.input('%1234567890f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.width).to.be(1234567890);
        });
    });
    
    describe('Scanning the format specifier "%01f"', function() {
        it('should return a token with propery "width" set to 1 and "options.zeroPad" set to true', function() {
            var token;
            _.input('%01f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.width).to.be(1);
            expect(token.options.zeroPad).to.be(true);
        });
    });
    
    describe('Scanning the format specifier "%+1f"', function() {
        it('should return a token with property "width" set to 1 and "options.leadingPlusWhenPositive" set to true', function() {
            var token;
            _.input('%+1f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.width).to.be(1);
            expect(token.options.leadingPlusWhenPositive).to.be(true);
        });
    });
    
    describe('Scanning the format specifier "%4$4f"', function() {
        it('should return a token with property "width" set to 4 and an argument index of 3', function() {
            var token;
            _.input('%4$4f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.width).to.be(4);
            expect(token.argumentIndex).to.be(3);
        });
    });
    
    describe('Scanning the format specifier "%10$01f"', function() {
        it('should return a token with property "width" set to 1, an argument index of 9, and "options.zeroPad" set to true', function() {
            var token;
            _.input('%10$01f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.width).to.be(1);
            expect(token.argumentIndex).to.be(9);
            expect(token.options.zeroPad).to.be(true);
        });
    });
    
    /*
     * Test precision
     */
    
    describe('Scanning the format specifier "%.2f"', function() { //precision only
        it('should return a token with property "precision" set to 2', function() {
            var token;
            _.input('%.2f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.precision).to.be(2);
        });
    });
    
    describe('Scanning the format specifier "%.123456789f"', function() { //longer precision
        it('should return a token with property "precision" set to 123456789', function() {
            var token;
            _.input('%.123456789f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.precision).to.be(123456789);
        });
    });
    
    describe('Scanning the format specifier "%1$.2f"', function() { //argument index, precision
        it('should return a token with property "precision" set to 2 and an argument index 0', function() {
            var token;
            _.input('%1$.2f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.precision).to.be(2);
            expect(token.argumentIndex).to.be(0);
        });
    });
    
    describe('Scanning the format specifier "%+.2f"', function() { //flag, precision
        it('should return a token with property "precision" set to 2 and "options.leadingPlusWhenPositive" set to true', function() {
            var token;
            _.input('%+.2f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.precision).to.be(2);
            expect(token.options.leadingPlusWhenPositive).to.be(true);
        });
    });
    
    describe('Scanning the format specifier "%2$+.2f"', function() { //argument index, flag, precision
        it('should return a token with property "precision" set to 2, an argument index 1, and "options.leadingPlusWhenPositive" set to true', function() {
            var token;
            _.input('%2$+.2f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.precision).to.be(2);
            expect(token.argumentIndex).to.be(1);
            expect(token.options.leadingPlusWhenPositive).to.be(true);
        });
    });
    
    describe('Scanning the format specifier "%10.2f"', function() { //width, precision
        it('should return a token with property "precision" set to 2 and "width" set to 10', function() {
            var token;
            _.input('%10.2f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.precision).to.be(2);
            expect(token.width).to.be(10);
        });
    });
    
    describe('Scanning the format specifier "%3$10.2f"', function() { //argument index, width, precision
        it('should return a token with property "precision" set to 2, "width" set to 10, and an argument index 2', function() {
            var token;
            _.input('%3$10.2f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.precision).to.be(2);
            expect(token.width).to.be(10);
            expect(token.argumentIndex).to.be(2);
        });
    });
    
    describe('Scanning the format specifier "%+10.2f"', function() { //flag, width, precision
        it('should return a token with property "precision" set to 2, "width" set to 10, and "options.leadingPlusWhenPositive" set to true', function() {
            var token;
            _.input('%+10.2f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.precision).to.be(2);
            expect(token.width).to.be(10);
            expect(token.options.leadingPlusWhenPositive).to.be(true);
        });
    });
    
    describe('Scanning the format specifier "%3$+10.2f"', function() { //argument index, flag, width, precision
        it('should return a token with property "precision" set to 2, "width" set to 10, argument index 2 and "options.leadingPlusWhenPositive" set to true', function() {
            var token;
            _.input('%3$+10.2f');
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.precision).to.be(2);
            expect(token.width).to.be(10);
            expect(token.argumentIndex).to.be(2);
            expect(token.options.leadingPlusWhenPositive).to.be(true);
        });
    });
    
    //
    // Combinations
    //
    
    describe("Scanning '%bTest'", function() {
        var token;
        before(function() {
            _.input('%bTest');
        })
        it("should return a token of general category first", function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.category).to.be(Scanner.tokenCategories.general);
        });
        it("should then return a text token with value 'Test'", function() {
            token = _.next();
            expect(token).to.be.an('object');
            expect(token.category).to.be(Scanner.tokenCategories.text);
            expect(token.value).to.be('Test');
        });
    });
});

describe("Java-Parser -", function() {
    
    before(function() {
        pat._private.fmt = pat.Formatter;
        _ = new Parser();
    });
    
    describe("A new Parser", function() {
        it("should have a Scanner property '_scanner'", function() {
            expect(_._scanner instanceof Scanner).to.be.ok();
        });
        it("should have an empty Array property '_target'", function() {
            expect(_._target).to.be.an('array');
            expect(_._target).to.be.empty();
        });
    });
    
    describe("Parsing the empty string", function() {
        it("should the empty string", function() {
            expect(_.parse('')).to.be('');
        });
    });
    
    describe("Parsing 'any'", function() {
        it("should return 'any'", function() {
            expect(_.parse('any')).to.be('any');
        });
    });
    
    describe("Parsing '%%'", function() {
        it("should return '%'", function() {
            expect(_.parse('%%')).to.be('%');
        });
    });
    
    describe("Parsing '%n' with '\\n' specified as line separator", function() {
        it("should return '\\n'", function() {
            expect(_.parse('%n', undefined, {
                lineSeparator: function() { return '\n'; }
            })).to.be('\n');
        });
    });
    
    describe("Parsing '%n' with '\\r\\n' specified as line separator", function() {
        it("should return '\\r\\n'", function() {
            expect(_.parse('%n', undefined, {
                lineSeparator: function() { return '\r\n'; }
            })).to.be('\r\n');
        });
    });
    
    describe("Parsing '%n' with '<br/>' specified as line separator", function() {
        it("should return '<br/>'", function() {
            expect(_.parse('%n', undefined, {
                lineSeparator: function() { return '<br/>'; }
            })).to.be('<br/>');
        });
    });
    
    describe("Parsing general format", function() {
        
        //%b with falsy values
        
        describe("'%b' with false", function() {
            it("should return 'false'", function() {
                expect(_.parse('%b', [false])).to.be('false');
            });
        });
        
        describe("'%b' with undefined", function() {
            it("should return 'false'", function() {
                expect(_.parse('%b', [undefined])).to.be('false');
            });
        });
        
        describe("'%b' with null", function() {
            it("should return 'false'", function() {
                expect(_.parse('%b', [null])).to.be('false');
            });
        });
        
        describe("'%b' with 0", function() {
            it("should return 'false'", function() {
                expect(_.parse('%b', [0])).to.be('false');
            });
        });
        
        describe("'%b' with the empty string", function() {
            it("should return 'false'", function() {
                expect(_.parse('%b', [''])).to.be('false');
            });
        });
        
        //%b with truthy values
        
        describe("'%b' with true", function() {
            it("should return 'true'", function() {
                expect(_.parse('%b', [true])).to.be('true');
            });
        });
        
        describe("'%b' with 1", function() {
            it("should return 'true'", function() {
                expect(_.parse('%b', [1])).to.be('true');
            });
        });
        
        describe("'%b' with 'any'", function() {
            it("should return 'true'", function() {
                expect(_.parse('%b', ['any'])).to.be('true');
            });
        });
        
        //%b upper case variant
        
        describe("'%B' with true", function() {
            it("should return 'TRUE'", function() {
                expect(_.parse('%B', [true])).to.be('TRUE');
            });
        });
        
        describe("'%B' with false", function() {
            it("should return 'FALSE'", function() {
                expect(_.parse('%B', [false])).to.be('FALSE');
            });
        });
        
        //%b with width, flag, and precision
        
        describe("'%6b' with true", function() {
            it("should return '  true'", function() {
                expect(_.parse('%6b', [true])).to.be('  true');
            });
        });
        
        describe("'%-6b' with true", function() {
            it("should return 'true  '", function() {
                expect(_.parse('%-6b', [true])).to.be('true  ');
            });
        });
        
        describe("'%.2b' with true", function() {
            it("should return 'tr'", function() {
                expect(_.parse('%.2b', [true])).to.be('tr');
            });
        });
        
        describe("'%4.2b' with true", function() {
            it("should return '  tr'", function() {
                expect(_.parse('%4.2b', [true])).to.be('  tr');
            });
        });
        
        describe("%b with inapplicable flag", function() {
            
            describe("'#' and argument true", function() {
                it("should return 'true'", function() {
                    expect(_.parse('%#b', [true])).to.be('true');
                });
            });
            
            describe("' ' and argument true", function() {
                it("should return 'true'", function() {
                    expect(_.parse('% b', [true])).to.be('true');
                });
            });
            
            describe("'0' and argument true", function() {
                it("should return 'true'", function() {
                    expect(_.parse('%0b', [true])).to.be('true');
                });
            });
            
            describe("',' and argument true", function() {
                it("should return 'true'", function() {
                    expect(_.parse('%,b', [true])).to.be('true');
                });
            });
            
            describe("'(' and argument true", function() {
                it("should return 'true'", function() {
                    expect(_.parse('%(b', [true])).to.be('true');
                });
            });
        });
        
        //%s, %S
        
        describe("'%s' with 'any'", function() {
            it("should return 'any'", function() {
                expect(_.parse('%s', ['any'])).to.be('any');
            });
        });
        
        describe("'%s' with the empty string", function() {
            it("should return the empty string", function() {
                expect(_.parse('%s', [''])).to.be('');
            });
        });
        
        describe("'%s' with undefined", function() {
            it("should return 'undefined'", function() {
                expect(_.parse('%s', [undefined])).to.be('undefined');
            });
        });
        
        describe("'%s' with null", function() {
            it("should return 'null'", function() {
                expect(_.parse('%s', [null])).to.be('null');
            });
        });
        
        describe("'%s' with 1", function() {
            it("should return '1'", function() {
                expect(_.parse('%s', [1])).to.be('1');
            });
        });
        
        describe("'%s' with {}", function() {
            it("should return String({})", function() {
                expect(_.parse('%s', [{}])).to.be(String({}));
            });
        });
        
        describe("'%s' with 'any'", function() {
            it("should return 'ANY'", function() {
                expect(_.parse('%S', ['any'])).to.be('ANY');
            });
        });
        
        describe("%s with inapplicable flag", function() {
            
            describe("'#' and argument 'any'", function() {
                it("should return 'any'", function() {
                    expect(_.parse('%#s', ['any'])).to.be('any');
                });
            });
            
            describe("' ' and argument 'any'", function() {
                it("should return 'any'", function() {
                    expect(_.parse('% s', ['any'])).to.be('any');
                });
            });
            
            describe("'0' and argument 'any'", function() {
                it("should return 'any'", function() {
                    expect(_.parse('%0s', ['any'])).to.be('any');
                });
            });
            
            describe("',' and argument 'any'", function() {
                it("should return 'any'", function() {
                    expect(_.parse('%,s', ['any'])).to.be('any');
                });
            });
            
            describe("'(' and argument 'any'", function() {
                it("should return 'any'", function() {
                    expect(_.parse('%(s', ['any'])).to.be('any');
                });
            });
        });
    });
    
    describe("Parsing character format", function() {
        
        describe("'%c' with 'z'", function() {
            it("should return 'z'", function() {
                expect(_.parse('%c', ['z'])).to.be('z');
            });
        });
        
        describe("'%C' with 'z'", function() {
            it("should return 'Z'", function() {
                expect(_.parse('%C', ['z'])).to.be('Z');
            });
        });
        
        describe("'%c' with 'zone'", function() {
            it("should return 'zone'", function() {
                expect(_.parse('%c', ['zone'])).to.be('z');
            });
        });
        
        describe("'%c' with 0x27EB", function() {
            it("should return String.fromCharCode(0x27EB)", function() {
                expect(_.parse('%c', [0x27EB])).to.be(String.fromCharCode(0x27EB));
            });
        });
        
        describe("'%2c' with 'z'", function() {
            it("should return ' z'", function() {
                expect(_.parse('%2c', ['z'])).to.be(' z');
            });
        });
        
        describe("'%-2c' with 'z'", function() {
            it("should return 'z '", function() {
                expect(_.parse('%-2c', ['z'])).to.be('z ');
            });
        });
        
        describe("'%.2c' with 'z'", function() {
            it("should ignore the precision and return 'z'", function() {
                expect(_.parse('%.2c', ['z'])).to.be('z');
            });
        });
    });
    
    describe("Parsing integral format", function() {
          
        describe("(decimal, deAT)", function() {
            
            describe("'%d' with -0 and Austrian culture", function() {
                it("should return '0'", function() {
                    expect(_.parse('%d', [-0], {
                        culture: function() { return deAT; }
                    })).to.be('0');
                });
            });
        
            describe("'%d' with 10 and Austrian culture", function() {
                it("should return '10'", function() {
                    expect(_.parse('%d', [10], {
                        culture: function() { return deAT; }
                    })).to.be('10');
                });
            });
            
            describe("'%d' with -10 and Austrian culture", function() {
                it("should return '-10'", function() {
                    expect(_.parse('%d', [-10], {
                        culture: function() { return deAT; }
                    })).to.be('-10');
                });
            });
            
            describe("'%+d' with 10 and Austrian culture", function() {
                it("should return '+10'", function() {
                    expect(_.parse('%+d', [10], {
                        culture: function() { return deAT; }
                    })).to.be('+10');
                });
            });
            
            describe("'%+d' with -10 and Austrian culture", function() {
                it("should return '-10'", function() {
                    expect(_.parse('%+d', [-10], {
                        culture: function() { return deAT; }
                    })).to.be('-10');
                });
            });
            
            describe("'% d' with 10 and Austrian culture", function() {
                it("should return ' 10'", function() {
                    expect(_.parse('% d', [10], {
                        culture: function() { return deAT; }
                    })).to.be(' 10');
                });
            });
            
            describe("'% d' with -10 and Austrian culture", function() {
                it("should return '-10'", function() {
                    expect(_.parse('% d', [-10], {
                        culture: function() { return deAT; }
                    })).to.be('-10');
                });
            });
            
            describe("'%(d' with -10 and Austrian culture", function() {
                it("should return '(10)'", function() {
                    expect(_.parse('%(d', [-10], {
                        culture: function() { return deAT; }
                    })).to.be('(10)');
                });
            });
            
            describe("'%(d' with 10 and Austrian culture", function() {
                it("should return '10'", function() {
                    expect(_.parse('%(d', [10], {
                        culture: function() { return deAT; }
                    })).to.be('10');
                });
            });
            
            describe("'%,d' with 5200400600 and Austrian culture", function() {
                it("should return '5.200.400.600'", function() {
                    expect(_.parse('%,d', [5200400600], {
                        culture: function() { return deAT; }
                    })).to.be('5.200.400.600');
                });
            });
            
            describe("'%,d' with -5200400600 and Austrian culture", function() {
                it("should return '-5.200.400.600'", function() {
                    expect(_.parse('%,d', [-5200400600], {
                        culture: function() { return deAT; }
                    })).to.be('-5.200.400.600');
                });
            });
            
            describe("'%4d' with 10 and Austrian culture", function() {
                it("should return '  10'", function() {
                    expect(_.parse('%4d', [10], {
                        culture: function() { return deAT; }
                    })).to.be('  10');
                });
            });
            
            describe("'%4d' with -10 and Austrian culture", function() {
                it("should return ' -10'", function() {
                    expect(_.parse('%4d', [-10], {
                        culture: function() { return deAT; }
                    })).to.be(' -10');
                });
            });
            
            describe("'%-4d' with 10 and Austrian culture", function() {
                it("should return '10  '", function() {
                    expect(_.parse('%-4d', [10], {
                        culture: function() { return deAT; }
                    })).to.be('10  ');
                });
            });
            
            describe("'%-4d' with -10 and Austrian culture", function() {
                it("should return '-10 '", function() {
                    expect(_.parse('%-4d', [-10], {
                        culture: function() { return deAT; }
                    })).to.be('-10 ');
                });
            });
            
            describe("'%04d' with 10 and Austrian culture", function() {
                it("should return '0010'", function() {
                    expect(_.parse('%04d', [10], {
                        culture: function() { return deAT; }
                    })).to.be('0010');
                });
            });
            
            describe("'%04d' with -10 and Austrian culture", function() {
                it("should return '-010'", function() {
                    expect(_.parse('%04d', [-10], {
                        culture: function() { return deAT; }
                    })).to.be('-010');
                });
            });
            
            //integer converted to number string in scientific notation by
            //JavaScripts Number.toString() 
            describe("'%d' with 1e200", function() {
                var a = new Array(201),
                    i,
                    expectedStr;
                for (i = 200; i > 0; i--) { a[i] = 0; }
                a[0] = 1;
                expectedStr = a.join('');
                it("should return '" + expectedStr + "'", function() {
                    expect(_.parse('%d', [1e200], {
                        culture: function() { return deAT; }
                    })).to.be(expectedStr);
                });
            });
            
            //NaN, infinity
            
            describe("'%d' with 'not-a-number'", function() {
                it("should return 'NaN'", function() {
                    expect(_.parse('%d', ['not-a-number'])).to.be('NaN');
                });
            });
            
            describe("'%d' with Number(1e1000)", function() {
                it("should return 'Infinity'", function() {
                    expect(_.parse('%d', [Number(1e1000)])).to.be('Infinity');
                });
            });
            
            describe("'%d' with Number(-1e1000)", function() {
                it("should return '-Infinity'", function() {
                    expect(_.parse('%d', [Number(-1e1000)])).to.be('-Infinity');
                });
            });
            
            describe("'%(d' with Number(-1e1000)", function() {
                it("should return '(Infinity)'", function() {
                    expect(_.parse('%(d', [Number(-1e1000)])).to.be('(Infinity)');
                });
            });
        });
        
        describe("(octal)", function() {
        
            describe("'%o' with 10", function() {
                it("should return '12'", function() {
                    expect(_.parse('%o', [10])).to.be('12');
                });
            });
            
            describe("'%#o' with 10", function() {
                it("should return '012'", function() {
                    expect(_.parse('%#o', [10])).to.be('012');
                });
            });
            
            describe("'%4o' with 10", function() {
                it("should return '  12'", function() {
                    expect(_.parse('%4o', [10])).to.be('  12');
                });
            });
            
            describe("'%04o' with 10", function() {
                it("should return '0012'", function() {
                    expect(_.parse('%04o', [10])).to.be('0012');
                });
            });
            
            describe("'%1o' with 10", function() {
                it("should return '12'", function() {
                    expect(_.parse('%1o', [10])).to.be('12');
                });
            });
            
            describe("'%#3o' with 10", function() {
                it("should return '012'", function() {
                    expect(_.parse('%#3o', [10])).to.be('012');
                });
            });
            
            describe("'%#4o' with 10", function() {
                it("should return ' 012'", function() {
                    expect(_.parse('%#4o', [10])).to.be(' 012');
                });
            });
            
            describe("'%o' with -10", function() {
                it("should return a string representing two's complement of 10 in base 8", function() {
                    expect(_.parse('%o', [-10])).to.be(Number(
                        pat.Formatter.util.number.twosComplement(10)).toString(8));
                });
            });
        });
        
        describe("(hexadecimal)", function() {
        
            describe("'%x' with 10", function() {
                it("should return 'a'", function() {
                    expect(_.parse('%x', [10])).to.be('a');
                });
            });
            
            describe("'%#x' with 10", function() {
                it("should return '0xa'", function() {
                    expect(_.parse('%#x', [10])).to.be('0xa');
                });
            });
            
            describe("'%X' with 10", function() {
                it("should return 'A'", function() {
                    expect(_.parse('%X', [10])).to.be('A');
                });
            });
            
            describe("'%#X' with 10", function() {
                it("should return '0XA'", function() {
                    expect(_.parse('%#X', [10])).to.be('0XA');
                });
            });
            
            describe("'%3x' with 10", function() {
                it("should return '  a'", function() {
                    expect(_.parse('%3x', [10])).to.be('  a');
                });
            });
            
            describe("'%03x' with 10", function() {
                it("should return '00a'", function() {
                    expect(_.parse('%03x', [10])).to.be('00a');
                });
            });
            
            describe("'%02x' with 10", function() {
                it("should return '0a'", function() {
                    expect(_.parse('%02x', [10])).to.be('0a');
                });
            });
            
            describe("'%#3x' with 10", function() {
                it("should return '0xa'", function() {
                    expect(_.parse('%#3x', [10])).to.be('0xa');
                });
            });
            
            describe("'%#4x' with 10", function() {
                it("should return ' 0xa'", function() {
                    expect(_.parse('%#4x', [10])).to.be(' 0xa');
                });
            });
            
            describe("'%x' with -10", function() {
                it("should return a string representing two's complement of 10 in base 16", function() {
                    expect(_.parse('%x', [-10])).to.be(Number(
                        pat.Formatter.util.number.twosComplement(10)).toString(16));
                });
            });
        });
    });
    
    describe("Parsing floating point format", function() {
        
        describe("(scientific, enUS)", function() {
        
            describe("'%e' with 0", function() {
                it("should return '0.000000e+00'", function() {
                    expect(_.parse('%e', [0], {
                        culture: function() { return enUS; }
                    })).to.be('0.000000e+00');
                });
            });
        
            describe("'%e' with -0", function() {
                it("should return '-0.000000e+00'", function() {
                    expect(_.parse('%e', [-0], {
                        culture: function() { return enUS; }
                    })).to.be('-0.000000e+00');
                });
            });
            
            describe("'%e' with 1", function() {
                it("should return '1.000000e+00'", function() {
                    expect(_.parse('%e', [1], {
                        culture: function() { return enUS; }
                    })).to.be('1.000000e+00');
                });
            });
            
            describe("'%e' with -1", function() {
                it("should return '-1.000000e+00'", function() {
                    expect(_.parse('%e', [-1], {
                        culture: function() { return enUS; }
                    })).to.be('-1.000000e+00');
                });
            });
            
            describe("'%e' with 0.1", function() {
                it("should return '1.000000e-01'", function() {
                    expect(_.parse('%e', [0.1], {
                        culture: function() { return enUS; }
                    })).to.be('1.000000e-01');
                });
            });
            
            describe("'%e' with -0.1", function() {
                it("should return '-1.000000e-01'", function() {
                    expect(_.parse('%e', [-0.1], {
                        culture: function() { return enUS; }
                    })).to.be('-1.000000e-01');
                });
            });
            
            describe("'%e' with 10000000000", function() {
                it("should return '1.000000e+10'", function() {
                    expect(_.parse('%e', [10000000000], {
                        culture: function() { return enUS; }
                    })).to.be('1.000000e+10');
                });
            });
            
            //default precision: round up
            describe("'%e' with 1.1234565", function() {
                it("should return '1.123457e+00'", function() {
                    expect(_.parse('%e', [1.1234565], {
                        culture: function() { return enUS; }
                    })).to.be('1.123457e+00');
                });
            });
            
            //default precision: round down
            describe("'%e' with 1.1234564", function() {
                it("should return '1.123456e+00'", function() {
                    expect(_.parse('%e', [1.1234564], {
                        culture: function() { return enUS; }
                    })).to.be('1.123456e+00');
                });
            });
            
            //precision: round up
            describe("'%.1e' with 1.15", function() {
                it("should return '1.2e+00'", function() {
                    expect(_.parse('%.1e', [1.15], {
                        culture: function() { return enUS; }
                    })).to.be('1.2e+00');
                });
            });
            
            //precision: round down
            describe("'%.1e' with 1.14", function() {
                it("should return '1.1e+00'", function() {
                    expect(_.parse('%.1e', [1.14], {
                        culture: function() { return enUS; }
                    })).to.be('1.1e+00');
                });
            });
            
            //zero pad
            
            describe("'%013e' with 1 and Austrian culture", function() {
                it("should return '01.000000e+00'", function() {
                    expect(_.parse('%013e', [1], {
                        culture: function() { return enUS; }
                    })).to.be('01.000000e+00');
                });
            });
            
            describe("'%010.0e' with 1 and Austrian culture", function() {
                it("should return '000001e+00'", function() {
                    expect(_.parse('%010.0e', [1], {
                        culture: function() { return enUS; }
                    })).to.be('000001e+00');
                });
            });
            
            //sign
            
            describe("'%(1e' with -1", function() {
                it("should return '(1.000000e+00)'", function() {
                    expect(_.parse('%(1e', [-1], {
                        culture: function() { return enUS; }
                    })).to.be('(1.000000e+00)');
                });
            });
            
            describe("'% 1e' with 1", function() {
                it("should return ' 1.000000e+00'", function() {
                    expect(_.parse('% 1e', [1], {
                        culture: function() { return enUS; }
                    })).to.be(' 1.000000e+00');
                });
            });
            
            describe("'%+1e' with 1", function() {
                it("should return '+1.000000e+00'", function() {
                    expect(_.parse('%+1e', [1], {
                        culture: function() { return enUS; }
                    })).to.be('+1.000000e+00');
                });
            });
            
            //width
            
            describe("'%6.0e' with 1", function() {
                it("should return ' 1e+00'", function() {
                    expect(_.parse('%6.0e', [1], {
                        culture: function() { return enUS; }
                    })).to.be(' 1e+00');
                });
            });
            
            describe("'%-6.0e' with 1", function() {
                it("should return '1e+00 '", function() {
                    expect(_.parse('%-6.0e', [1], {
                        culture: function() { return enUS; }
                    })).to.be('1e+00 ');
                });
            });
        });
        
        describe("(decimal)", function() {
            
            describe("'%f' with 0", function() {
                it("should return '0.000000'", function() {
                    expect(_.parse('%f', [0], {
                        culture: function() { return enUS; }
                    })).to.be('0.000000');
                });
            });
            
            describe("'%f' with 1", function() {
                it("should return '1.000000'", function() {
                    expect(_.parse('%f', [1], {
                        culture: function() { return enUS; }
                    })).to.be('1.000000');
                });
            });
            
            describe("'%f' with -1", function() {
                it("should return '-1.000000'", function() {
                    expect(_.parse('%f', [-1], {
                        culture: function() { return enUS; }
                    })).to.be('-1.000000');
                });
            });
            
            describe("'%f' with 1e25", function() {
                it("should return '10000000000000000000000000.000000'", function() {
                    expect(_.parse('%f', [1e25], {
                        culture: function() { return enUS; }
                    })).to.be('10000000000000000000000000.000000');
                });
            });
            
            describe("'%f' with -1e25", function() {
                it("should return '-10000000000000000000000000.000000'", function() {
                    expect(_.parse('%f', [-1e25], {
                        culture: function() { return enUS; }
                    })).to.be('-10000000000000000000000000.000000');
                });
            });
            
            describe("'%f' with 1e-25", function() {
                it("should return '0.000000'", function() {
                    expect(_.parse('%f', [1e-25], {
                        culture: function() { return enUS; }
                    })).to.be('0.000000');
                });
            });
            
            describe("'%f' with -1e-25", function() {
                it("should return '-0.000000'", function() {
                    expect(_.parse('%f', [-1e-25], {
                        culture: function() { return enUS; }
                    })).to.be('-0.000000');
                });
            });
            
            describe("'%6.2f' with 45.454", function() {
                it("should return ' 45.45'", function() {
                    expect(_.parse('%6.2f', [45.454], {
                        culture: function() { return enUS; }
                    })).to.be(' 45.45');
                });
            });
            
            describe("'%6.2f' with 45.4", function() {
                it("should return ' 45.40'", function() {
                    expect(_.parse('%6.2f', [45.4], {
                        culture: function() { return enUS; }
                    })).to.be(' 45.40');
                });
            });
            
            describe("'%06.2f' with 45.4 and Austrian culture", function() {
                it("should return '045.40'", function() {
                    expect(_.parse('%06.2f', [45.4], {
                        culture: function() { return enUS; }
                    })).to.be('045.40');
                });
            });
            
            describe("'%.0f' with 1", function() {
                it("should return '1'", function() {
                    expect(_.parse('%.0f', [1], {
                        culture: function() { return enUS; }
                    })).to.be('1');
                });
            });
            
            describe("'%+.0f' with 1", function() {
                it("should return '+1'", function() {
                    expect(_.parse('%+.0f', [1], {
                        culture: function() { return enUS; }
                    })).to.be('+1');
                });
            });
            
            describe("'% .0f' with 1", function() {
                it("should return ' 1'", function() {
                    expect(_.parse('% .0f', [1], {
                        culture: function() { return enUS; }
                    })).to.be(' 1');
                });
            });
            
            describe("'%(.0f' with -1", function() {
                it("should return '(1)'", function() {
                    expect(_.parse('%(.0f', [-1], {
                        culture: function() { return enUS; }
                    })).to.be('(1)');
                });
            });
            
            describe("'%4f' with 1", function() {
                it("should return ' 1.000000'", function() {
                    expect(_.parse('%4f', [1], {
                        culture: function() { return enUS; }
                    })).to.be('1.000000');
                });
            });
        });
        
        describe("(general scientific, enUS)", function() {
            
            //default precision
            
            //C-style would return 0 but Java does not support alternate
            //form for %g, so it returns 0 with precision - 1 fractional
            //digits
            describe("'%g' with 0", function() {
                it("should return '0.00000'", function() {
                    expect(_.parse('%g', [0], {
                        culture: function() { return enUS; }
                    })).to.be('0.00000');
                });
            });
            
            describe("'%g' with 1", function() {
                it("should return '1.00000'", function() {
                    expect(_.parse('%g', [1], {
                        culture: function() { return enUS; }
                    })).to.be('1.00000');
                });
            });
            
            describe("'%g' with -1", function() {
                it("should return '-1.00000'", function() {
                    expect(_.parse('%g', [-1], {
                        culture: function() { return enUS; }
                    })).to.be('-1.00000');
                });
            });
            
            //explicit precision for values not within (-1; 0) nor in (0; 1)
            
            //zero pad
            describe("'%.2g' with 1", function() {
                it("should return '1.0'", function() {
                    expect(_.parse('%.2g', [1], {
                        culture: function() { return enUS; }
                    })).to.be('1.0');
                });
            });
            
            //zero pad
            describe("'%.2g' with -1", function() {
                it("should return '-1.0'", function() {
                    expect(_.parse('%.2g', [-1], {
                        culture: function() { return enUS; }
                    })).to.be('-1.0');
                });
            });
            
            //round
            describe("'%.2g' with 1.45", function() {
                it("should return '1.5'", function() {
                    expect(_.parse('%.2g', [1.45], {
                        culture: function() { return enUS; }
                    })).to.be('1.5');
                });
            });
            
            //round
            describe("'%.2g' with -1.45", function() {
                it("should return '-1.4'", function() {
                    expect(_.parse('%.2g', [-1.45], {
                        culture: function() { return enUS; }
                    })).to.be('-1.4');
                });
            });
            
            //round
            describe("'%.2g' with 0.0001", function() {
                it("should return '0.00'", function() {
                    expect(_.parse('%.2g', [0.0001], {
                        culture: function() { return enUS; }
                    })).to.be('0.00');
                });
            });
            
            //explicit precision for values within (-1; 0) or within (0; 1)
            
            //zero pad
            describe("'%.3g' with 0.1", function() {
                it("should return '0.100'", function() {
                    expect(_.parse('%.3g', [0.1], {
                        culture: function() { return enUS; }
                    })).to.be('0.100');
                });
            });
            
            //zero pad
            describe("'%.3g' with -0.1", function() {
                it("should return '-0.100'", function() {
                    expect(_.parse('%.3g', [-0.1], {
                        culture: function() { return enUS; }
                    })).to.be('-0.100');
                });
            });
            
            //round
            describe("'%.3g' with 0.2345", function() {
                it("should return '0.235'", function() {
                    expect(_.parse('%.3g', [0.2345], {
                        culture: function() { return enUS; }
                    })).to.be('0.235');
                });
            });
            
            //round
            describe("'%.3g' with -0.2345", function() {
                it("should return '-0.234'", function() {
                    expect(_.parse('%.3g', [-0.2345], {
                        culture: function() { return enUS; }
                    })).to.be('-0.234');
                });
            });
            
            //test bounds for decimal/exponential form
            //[10^-4 ; 10^precision) => decimal form
            //otherwise => exponential form
            
            //upper bound (10^precision)
            
            describe("'%.2g' with 99.4", function() {
                it("should return '99'", function() {
                    expect(_.parse('%.2g', [99.4], {
                        culture: function() { return enUS; }
                    })).to.be('99');
                });
            });
            
            describe("'%.2g' with -99.5", function() {
                it("should return '-99'", function() {
                    expect(_.parse('%.2g', [-99.5], {
                        culture: function() { return enUS; }
                    })).to.be('-99');
                });
            });
            
            describe("'%.2g' with 99.5", function() {
                it("should return '1.0e+02'", function() {
                    expect(_.parse('%.2g', [99.5], {
                        culture: function() { return enUS; }
                    })).to.be('1.0e+02');
                });
            });
            
            describe("'%.2g' with -99.6", function() {
                it("should return '-1.0e+02'", function() {
                    expect(_.parse('%.2g', [-99.6], {
                        culture: function() { return enUS; }
                    })).to.be('-1.0e+02');
                });
            });
            
            //lower bound (10^-4)
            
            describe("'%.4g' with 0.0001", function() {
                it("should return '0.0001'", function() {
                    expect(_.parse('%.4g', [0.0001], {
                        culture: function() { return enUS; }
                    })).to.be('0.0001');
                });
            });
            
            describe("'%.4g' with 0.00009", function() {
                it("should return '0.0001'", function() {
                    expect(_.parse('%.4g', [0.00009], {
                        culture: function() { return enUS; }
                    })).to.be('0.0001');
                });
            });
            
            describe("'%.5g' with 0.00009", function() {
                it("should return '9.0000e-05'", function() {
                    expect(_.parse('%.5g', [0.00009], {
                        culture: function() { return enUS; }
                    })).to.be('9.0000e-05');
                });
            });
            
            //sign
            
            describe("'%+.0g' with 1", function() {
                it("should return '+1'", function() {
                    expect(_.parse('%+.0g', [1], {
                        culture: function() { return enUS; }
                    })).to.be('+1');
                });
            });
            
            describe("'% .0g' with 1", function() {
                it("should return ' 1'", function() {
                    expect(_.parse('% .0g', [1], {
                        culture: function() { return enUS; }
                    })).to.be(' 1');
                });
            });
            
            describe("'%(.0g' with -1", function() {
                it("should return '(1)'", function() {
                    expect(_.parse('%(.0g', [-1], {
                        culture: function() { return enUS; }
                    })).to.be('(1)');
                });
            });
            
            //width
            
            describe("'%3.0g' with 1", function() {
                it("should return '  1'", function() {
                    expect(_.parse('%3.0g', [1], {
                        culture: function() { return enUS; }
                    })).to.be('  1');
                });
            });
            
            describe("'%-3.0g' with 1", function() {
                it("should return '1  '", function() {
                    expect(_.parse('%-3.0g', [1], {
                        culture: function() { return enUS; }
                    })).to.be('1  ');
                });
            });
            
            describe("'%03.0g' with 1", function() {
                it("should return '001'", function() {
                    expect(_.parse('%03.0g', [1], {
                        culture: function() { return enUS; }
                    })).to.be('001');
                });
            });
            
            //grouping
            describe("'%,g' with 100000", function() {
                it("should return '100,000'", function() {
                    expect(_.parse('%,g', [100000], {
                        culture: function() { return enUS; }
                    })).to.be('100,000');
                });
            });
            
            //upper case
            
            describe("'%.0G' with 10", function() {
                it("should return '1E+01'", function() {
                    expect(_.parse('%.0G', [10], {
                        culture: function() { return enUS; }
                    })).to.be('1E+01');
                });
            });
        });
    });
    
    describe("Parsing date format", function() {
        var d1 = '2012-01-01T20:02:05.025Z',
            d2 = '2012-01-01T08:02:05.025Z';
            
        //time
        
        describe("'%tH' with Date " + d1, function() {
            it("should return '20'", function() {
                expect(_.parse('%tH', [new Date(d1)])).to.be('20');
            });
        });
        
        describe("'%#tH' (host OS timezone) with Date " + d1, function() {
            it("should return the hour converted to the host OS' timezone", function() {
                var date = new Date(d1),
                    r = _.parse('%#tH', [date]);
                expect(r).to.be.a('string');
                expect(Number(r)).to.be(date.getHours());
            });
        });
        
        describe("'%tI' with Date " + d1, function() {
            it("should return '08'", function() {
                expect(_.parse('%tI', [new Date(d1)])).to.be('08');
            });
        });
        
        describe("'%tk' with Date " + d2, function() {
            it("should return '8'", function() {
                expect(_.parse('%tk', [new Date(d2)])).to.be('8');
            });
        });
        
        describe("'%tl' with Date " + d1, function() {
            it("should return '8'", function() {
                expect(_.parse('%tl', [new Date(d1)])).to.be('8');
            });
        });
        
        describe("'%tM' with Date " + d1, function() {
            it("should return '02'", function() {
                expect(_.parse('%tM', [new Date(d1)])).to.be('02');
            });
        });
        
        describe("'%tS' with Date " + d1, function() {
            it("should return '05'", function() {
                expect(_.parse('%tS', [new Date(d1)])).to.be('05');
            });
        });
        
        describe("'%tL' with Date " + d1, function() {
            it("should return '025'", function() {
                expect(_.parse('%tL', [new Date(d1)])).to.be('025');
            });
        });
        
        describe("'%tp' with Date " + d1, function() {
            it("should return the culture specific afternoon designator", function() {
                expect(_.parse('%tp', [new Date(d1)], {
                    culture: function() {
                        return { pmToken: 'pm' };
                    }
                })).to.be('pm');
            });
        });
        
        describe("'%Tp' with Date " + d1, function() {
            it("should return the culture specific afternoon designator with upper case letters", function() {
                expect(_.parse('%Tp', [new Date(d1)], {
                    culture: function() {
                        return { pmToken: 'pm' };
                    }
                })).to.be('PM');
            });
        });
        
        describe("'%tz' (UTC date) with Date " + d1, function() {
            it("should return '+0000'", function() {
                expect(_.parse('%tz', [new Date(d1)])).to.be('+0000');
            });
        });
        
        describe("'%#tz' (date with host OS timezone) with Date " + d1, function() {
            it("should return the UTC offset", function() {
                var date = new Date(d1);
                expect(_.parse('%#tz', [date]))
                    .to.be(pat.Formatter.date.timezoneOffset(date));
            });
        });
        
        describe("'%tZ' (UTC date) with Date " + d1, function() {
            it("should return 'GMT'", function() {
                expect(_.parse('%tZ', [new Date(d1)])).to.be('GMT');
            });
        });
        
        describe("'%#tZ' (date with host OS timezone) with Date " + d1, function() {
            it("should return the abbreviated timezone", function() {
                var date = new Date(d1);
                expect(_.parse('%#tZ', [date]))
                    .to.be(pat.Formatter.date.abbreviatedTimezone(date));
            });
        });
        
        describe("'%#TZ' (date with host OS timezone) with Date " + d1, function() {
            it("should return the abbreviated timezone with upper case letters", function() {
                var date = new Date(d1);
                expect(_.parse('%#TZ', [date])).to.be(String(
                    pat.Formatter.date.abbreviatedTimezone(date)).toUpperCase());
            });
        });
        
        describe("'%ts' with Date " + d1, function() {
            it("should return the date's UNIX timestamp", function() {
                var date = new Date(d1);
                expect(_.parse('%ts', [date]))
                    .to.be(String(pat.Formatter.util.date.timestamp(date)));
            });
        });
        
        describe("'%tQ' with Date " + d1, function() {
            it("should return the date's UNIX timestamp in milliseconds", function() {
                var date = new Date(d1);
                expect(_.parse('%tQ', [date]))
                    .to.be(String(date.valueOf()));
            });
        });
        
        //date
        
        describe("'%tB' with Date " + d1, function() {
            it("should return the culture specific month name", function() {
                expect(_.parse('%tB', [new Date(d1)], {
                    culture: function() { return deAT; }
                })).to.be(deAT.months[0]);
            });
        });
        
        describe("'%TB' with Date " + d1, function() {
            it("should return the culture specific month name", function() {
                expect(_.parse('%TB', [new Date(d1)], {
                    culture: function() { return deAT; }
                })).to.be(String(deAT.months[0]).toUpperCase());
            });
        });
        
        describe("'%tb' with Date " + d1, function() {
            it("should return the culture specific abbreviated month name", function() {
                expect(_.parse('%tb', [new Date(d1)], {
                    culture: function() { return deAT; }
                })).to.be(deAT.monthsAbbr[0]);
            });
        });
        
        describe("'%Tb' with Date " + d1, function() {
            it("should return the culture specific abbreviated month name", function() {
                expect(_.parse('%Tb', [new Date(d1)], {
                    culture: function() { return deAT; }
                })).to.be(String(deAT.monthsAbbr[0]).toUpperCase());
            });
        });
        
        describe("'%th' with Date " + d1, function() {
            it("should return the culture specific abbreviated month name", function() {
                expect(_.parse('%th', [new Date(d1)], {
                    culture: function() { return deAT; }
                })).to.be(deAT.monthsAbbr[0]);
            });
        });
        
        describe("'%Th' with Date " + d1, function() {
            it("should return the culture specific abbreviated month name", function() {
                expect(_.parse('%Th', [new Date(d1)], {
                    culture: function() { return deAT; }
                })).to.be(String(deAT.monthsAbbr[0]).toUpperCase());
            });
        });
        
        describe("'%tA' with Date " + d1, function() {
            it("should return the culture specific weekday name", function() {
                expect(_.parse('%tA', [new Date(d1)], {
                    culture: function() { return deAT; }
                })).to.be(deAT.weekdays[0]);
            });
        });
        
        describe("'%TA' with Date " + d1, function() {
            it("should return the culture specific weekday name", function() {
                expect(_.parse('%TA', [new Date(d1)], {
                    culture: function() { return deAT; }
                })).to.be(String(deAT.weekdays[0]).toUpperCase());
            });
        });
        
        describe("'%ta' with Date " + d1, function() {
            it("should return the culture specific abbreviated weekday name", function() {
                expect(_.parse('%ta', [new Date(d1)], {
                    culture: function() { return deAT; }
                })).to.be(deAT.weekdaysAbbr[0]);
            });
        });
        
        describe("'%Ta' with Date " + d1, function() {
            it("should return the culture specific abbreviated weekday name", function() {
                expect(_.parse('%Ta', [new Date(d1)], {
                    culture: function() { return deAT; }
                })).to.be(String(deAT.weekdaysAbbr[0]).toUpperCase());
            });
        });
        
        describe("'%tC' with Date " + d1, function() {
            it("should return '20'", function() {
                expect(_.parse('%tC', [new Date(d1)])).to.be('20');
            });
        });
        
        describe("'%tC' with Date '500-01-01T00:00Z'", function() {
            it("should return '05'", function() {
                expect(_.parse('%tC', [new Date(Date.UTC(500,0))])).to.be('05');
            });
        });
        
        describe("'%tY' with Date " + d1, function() {
            it("should return '2012'", function() {
                expect(_.parse('%tY', [new Date(d1)])).to.be('2012');
            });
        });
        
        describe("'%tY' with Date '500-01-01T00:00Z'", function() {
            it("should return '0500'", function() {
                expect(_.parse('%tY', [new Date(Date.UTC(500,0))])).to.be('0500');
            });
        });
        
        describe("'%ty' with Date " + d1, function() {
            it("should return '12'", function() {
                expect(_.parse('%ty', [new Date(d1)])).to.be('12');
            });
        });
        
        describe("'%ty' with Date " + d1, function() {
            it("should return '00'", function() {
                expect(_.parse('%ty', [new Date(Date.UTC(500,0))])).to.be('00');
            });
        });
        
        describe("'%tj' with Date " + d1, function() {
            it("should return '001'", function() {
                expect(_.parse('%tj', [new Date(d1)])).to.be('001');
            });
        });
        
        describe("'%tm' with Date " + d1, function() {
            it("should return '01'", function() {
                expect(_.parse('%tm', [new Date(d1)])).to.be('01');
            });
        });
        
        describe("'%td' with Date " + d1, function() {
            it("should return '01'", function() {
                expect(_.parse('%td', [new Date(d1)])).to.be('01');
            });
        });
        
        describe("'%te' with Date " + d1, function() {
            it("should return '1'", function() {
                expect(_.parse('%te', [new Date(d1)])).to.be('1');
            });
        });
        
        describe("'%tV' with Date " + d1, function() {
            it("should return '52'", function() {
                expect(_.parse('%tV', [new Date(d1)])).to.be('52');
            });
        });
        
        //compositions
        
        describe("'%tR' with Date " + d1, function() {
            it("should return '20:02'", function() {
                expect(_.parse('%tR', [new Date(d1)])).to.be('20:02');
            });
        });
        
        describe("'%tT' with Date " + d1, function() {
            it("should return '20:02:05'", function() {
                expect(_.parse('%tT', [new Date(d1)])).to.be('20:02:05');
            });
        });
        
        describe("'%tr' with Date " + d1, function() {
            it("should return '08:02:05'", function() {
                expect(_.parse('%tr', [new Date(d1)], {
                    culture: function() { return deAT; }
                })).to.be('08:02:05 ' + deAT.pmToken.toUpperCase());
            });
        });
        
        describe("'%tD' with Date " + d1, function() {
            it("should return '01/01/12'", function() {
                expect(_.parse('%tD', [new Date(d1)])).to.be('01/01/12');
            });
        });
        
        describe("'%tD' with Date '2012-12-20'", function() {
            it("should return '12/20/12'", function() {
                expect(_.parse('%tD', [new Date('2012-12-20')])).to.be('12/20/12');
            });
        });
        
        describe("'%tF' with Date " + d1, function() {
            it("should return '2012-01-01'", function() {
                expect(_.parse('%tF', [new Date(d1)])).to.be('2012-01-01');
            });
        });
        
        describe("'%tc' with Date " + d1, function() {
            it("should return '1'", function() {
                expect(_.parse('%tc', [new Date(d1)], {
                    culture: function() { return deAT; }
                })).to.be([deAT.weekdaysAbbr[0],
                    deAT.monthsAbbr[0],
                    '01',
                    '20:02:05',
                    'GMT',
                    '2012'].join(' '));
            });
        });
        
        //width
        
        describe("'%4tH' with date " + d1, function() {
            it("should return '  20'", function() {
                expect(_.parse('%4tH', [new Date(d1)])).to.be('  20');
            });
        });
        
        describe("'%-4tH' with date " + d1, function() {
            it("should return '20  '", function() {
                expect(_.parse('%-4tH', [new Date(d1)])).to.be('20  ');
            });
        });
    });
});

describe("Java format -", function() {
    
    var fmtOpt;
    
    beforeEach(function() {
        pat.Formatter.reset();
        fmtOpt = {
            culture: function() { return enUS; },
            lineSeparator: function() { return '\n'; }
        };
    });
    
    function f(fstr) {
        return flavor.format(fstr,
            pat.Formatter,
            Array.prototype.slice.call(arguments, 1),
            fmtOpt);
    }
    
    describe("Doc examples", function() {
        
        var d = '2012-01-01T20:15:00Z';
        
        describe("format('Another %s', 'string')", function() {
            it("should return 'Another string'", function() {
                expect(f('Another %s', 'string')).to.be('Another string');
            });            
        });
        
        describe("format('%.2f', 1/3)", function() {
            it("should return '0.33'", function() {
                expect(f('%.2f', 1/3)).to.be('0.33');
            });            
        });
        
        describe("format('%.1B', undefined)", function() {
            it("should return 'F'", function() {
                expect(f('%.1B', undefined)).to.be('F');
            });            
        });
        
        describe("format('%1$tH:%1$tM', new Date(" + d + "))", function() {
            it("should return '20:15'", function() {
                expect(f('%1$tH:%1$tM', new Date(d))).to.be('20:15');
            });            
        });
        
        describe("format('%1$tH:%<tM', new Date(" + d + "))", function() {
            it("should return '20:15'", function() {
                expect(f('%1$tH:%<tM', new Date(d))).to.be('20:15');
            });            
        });
        
        describe("format('%tH:%<tM', new Date(" + d + "))", function() {
            it("should return '20:15'", function() {
                expect(f('%tH:%<tM', new Date(d))).to.be('20:15');
            });            
        });
        
        describe("format('%2$s %s %<s %s %3$s', 'One', 'Two', 'Three'))", function() {
            it("should return 'Two One One Two Three'", function() {
                expect(f('%2$s %s %<s %s %3$s', 'One', 'Two', 'Three'))
                    .to.be('Two One One Two Three');
            });            
        });
        
        describe("format('%d', 3.5)", function() {
            it("should return '3'", function() {
                expect(f('%d', 3.5)).to.be('3');
            });            
        });
        
        describe("format('%o', 3.5)", function() {
            it("should return '3'", function() {
                expect(f('%o', 3.5)).to.be('3');
            });            
        });
        
        describe("format('%x', 255)", function() {
            it("should return 'ff'", function() {
                expect(f('%x', 255)).to.be('ff');
            });            
        });
        
        describe("format('%06.3f', 3.14159)", function() {
            it("should return '03.142'", function() {
                expect(f('%06.3f', 3.14159)).to.be('03.142');
            });            
        });
        
        describe("format('%e', 1400)", function() {
            it("should return '1.400000e+03'", function() {
                expect(f('%e', 1400)).to.be('1.400000e+03');
            });            
        });
        
        describe("format('%tFT%tTZ', new Date('2012-07-21T20:15:00Z'))", function() {
            it("should return '2012-07-21T20:15:00Z'", function() {
                expect(f('%1$tFT%1$tTZ', new Date('2012-07-21T20:15:00Z')))
                    .to.be('2012-07-21T20:15:00Z');
            });            
        });
    });
    
});
