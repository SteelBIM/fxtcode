/*global require:true, define:true, describe, it*/
/*jslint sloppy:true nomen:true plusplus:true*/

var expect = require('expect.js'),
    sinon = require('sinon'),
    requirejs = require('requirejs'),
    pat = require('../lib/pat.js'),
    _; //unit under test

//use sinon.js assertions with expect.js using 'was' syntax
expect = require('sinon-expect').enhance(expect, sinon, 'was');


describe("Private section -", function() {
    before(function() {
        _ = pat._private;
    });
    beforeEach(function() {
        pat.Formatter.reset();
    });
    
    describe("The initial flavor cache", function() {
        it("should be an object", function() {
            expect(_.flavors).to.be.an('object');
        });
        it("should have length 1", function() {
            expect(Object.getOwnPropertyNames(_.flavors)).to.have.length(1);
        });
        it("should contain the initial flavor", function() {
            expect(_.flavors[pat.Formatter.defaultOptions.flavorId])
                .not.to.be(undefined);
        });
    });
    
    describe("The initial culture cache", function() {
        it("should be an object", function() {
            expect(_.cultures).to.be.an('object');
        });
        it("should have length 1", function() {
            expect(Object.getOwnPropertyNames(_.cultures)).to.have.length(1);
        });
        it("should contain the initial flavor", function() {
            expect(_.cultures[pat.Formatter.defaultOptions.cultureId])
                .not.to.be(undefined);
        });
    });
    
    describe("#countOwnProperties with { a:1, b:2, c:3 }", function() {
        it("should return 3", function() {
            expect(_.countOwnProperties({ a:1, b:2, c:3 })).to.be(3);
        });
    });
    
    describe("#ownPropertyNames with { a:1, b:2, c:3 }", function() {
        it("should return ['a', 'b', 'c']", function() {
            expect(_.ownPropertyNames({ a:1, b:2, c:3 })).to.eql(['a', 'b', 'c']);
        });
    });
    
    describe("#deepCopy: An object and it's deep copy", function() {
        it("should not be identical", function() {
            var obj = {};
            expect(_.deepCopy(obj)).not.to.be(obj);
        });
        it("should not share a property", function() {
            var obj = { obj: {} };
            expect(_.deepCopy(obj).obj).not.to.be(obj.obj);
        });
        it("should be equal", function() {
            var obj = {};
            expect(_.deepCopy(obj)).to.eql(obj);
        });
    });
    
    describe("#accessor: An accessor", function() {
        it("should return the wrapped value", function() {
            var val = 10,
                acc = _.accessor(val);
            expect(acc()).to.be(val);
        });
        it("should set the underlying value", function() {
            var val = 10,
                newval = 20,
                acc = _.accessor(val);
            acc(newval);
            expect(acc()).to.be(newval);
        });
        describe("with a 'syncSetter'", function() {
            it("should call that function with the new value before setting the underlying one", function() {
                var val = 10,
                    acc = _.accessor(val),
                    spy = sinon.spy();
                acc.syncSetter(function(arg) { spy(arg); });
                acc(val);
                expect(spy).was.calledOnce();
                expect(spy).was.calledWithExactly(val);
            });
            it("should set the underlying value to the return value of that function", function() {
                var val = 10,
                    newval = 20,
                    acc = new _.accessor(val);
                acc.syncSetter(function() { return newval; });
                acc(val);
                expect(acc()).to.be(newval);
            });
        });
        describe("with an 'asyncSetter'", function() {
            it("should call that function with the new value before setting the underlying one", function() {
                var val = 10,
                    acc = _.accessor(val),
                    spy = sinon.spy();
                acc.asyncSetter(function(arg) { spy(arg); });
                acc(val);
                expect(spy).was.calledOnce();
                expect(spy).was.calledWithExactly(val);
            });
            it("should set the underlying value to the argument of that function's callback", function() {
                var val = 10,
                    newval = 20,
                    acc = new _.accessor(val);
                acc.asyncSetter(function(arg, fn) {
                    fn(newval);
                });
                acc(val);
                expect(acc()).to.be(newval);
            });
        });
    });
    
    describe("#loadModule('../lib/cultures/deAT') in node context", function() {
        it ("should result in an object with property 'id' set to 'deAT'", function() {
            _.loadModule('../lib/cultures/deAT', function(module) {
                expect(module).to.be.an('object');
                expect(module).to.have.property('id', 'deAT');
            });
        });
    });
    
    /*
     * IMPORTANT:
     * 
     * This test case requires mocha to be executed with the option
     * "--globals module,define" in order to prevent global leak
     * detection for the globals temporarily changed by this test case.
     */
    describe("#loadModule('cultures/deAT') in AMD context", function() {
        it ("should return an object with property 'id' set to 'deAT'", function() {
            var curRequire,
                curDefine;
            //save node's require
            curRequire = require;
            if (typeof define !== 'undefined') { curDefine = define; }
            //simulate AMD context
            define = requirejs.define;
            require = requirejs.require;
            requirejs.config({
                baseUrl: '../lib',
                paths: { cultures: './cultures' }
            });
            _.modulePath = '';
            _.loadModule('cultures/deAT', function(module) {
                expect(module).to.be.an('object');
                expect(module).to.have.property('id', 'deAT');
            });
            if (define !== undefined) { define = curDefine; }
            require = curRequire;
        });
    });
    
    describe("#loadCulture with an ID not representing an existing culture module", function() {
        it ("should throw an error", function() {
            expect(function() { _.loadCulture('__XYZ__'); }).to.throwError();
        });
    });
    
    describe("#loadFlavor with an ID not representing an existing flavor module", function() {
        it ("should throw an error", function() {
            expect(function() { _.loadFlavor('__XYZ__'); }).to.throwError();
        });
    });
});

describe('Formatter.util -', function() {
    before(function() {
        _ = pat.Formatter.util;
    });

    describe("#toArray", function() {
        it("should return the argument if it is an array", function() {
            var arg = [];
            expect(_.toArray(arg)).to.be(arg);
        });
        it("should return a string argument as character array", function() {
            var arg = 'test';
            expect(_.toArray(arg)).to.be.an('array');
            expect(_.toArray(arg)).to.have.length(arg.length);
            expect(_.toArray(arg)[0]).to.be('t');
            expect(_.toArray(arg)[1]).to.be('e');
            expect(_.toArray(arg)[2]).to.be('s');
            expect(_.toArray(arg)[3]).to.be('t');
        });
    });
    
    describe("#concat('test')", function() {
        it("should should return 'test'", function() {
            var arg = 'test';
            expect(_.concat(arg)).to.be(arg);
        });
    });
    
    describe("#concat('test', 2)", function() {
        it("should should return 'test'", function() {
            var arg = 'test';
            expect(_.concat(arg, 2)).to.be(arg + arg);
        });
    });
    
    describe("#pad('test')", function() {
        it("should return 'test'", function() {
            var arg = 'test';
            expect(_.pad(arg)).to.be(arg);
        });
    });
    
    describe("#pad('test', '_', 6)", function() {
        it("should return 'test__'", function() {
            expect(_.pad('test', '_', 6)).to.be('test__');
        });
    });
    
    describe("#pad('test', '_', 6, false)", function() {
        it("should return 'test__'", function() {
            expect(_.pad('test', '_', 6, false)).to.be('test__');
        });
    });
    
    describe("#pad('test', '_', 6, true)", function() {
        it("should return '__test'", function() {
            expect(_.pad('test', '_', 6, true)).to.be('__test');
        });
    });
});

describe('Formatter.util.number', function() {
    before(function() {
        _ = pat.Formatter.util.number;
    });
    
    //round
    
    describe("#round with 0", function() {
        it("should return 0", function() {
            expect(_.round(0)).to.be(0);
        });
    });
    
    describe("#round with 1", function() {
        it("should return 1", function() {
            expect(_.round(1)).to.be(1);
        });
    });
    
    describe("#round with -1", function() {
        it("should return -1", function() {
            expect(_.round(-1)).to.be(-1);
        });
    });
    
    describe("#round with 1.4", function() {
        it("should return 1", function() {
            expect(_.round(1.4)).to.be(1);
        });
    });
    
    describe("#round with 1.5", function() {
        it("should return 2", function() {
            expect(_.round(1.5)).to.be(2);
        });
    });
    
    describe("#round with 1.114 and precision 2", function() {
        it("should return 1.11", function() {
            expect(_.round(1.114, 2)).to.be(1.11);
        });
    });
    
    describe("#round with 1.115 and precision 2", function() {
        it("should return 1.12", function() {
            expect(_.round(1.115, 2)).to.be(1.12);
        });
    });
    
    //signedInt
    
    describe("#signedInt with 0", function() {
        it("should 0", function() {
            expect(_.signedInt(0)).to.be(0);
        });
    });
    
    describe("#signedInt with Formatter.util.number.MAX_SIGNED_INT + 1", function() {
        it("should return Formatter.util.number.MAX_SIGNED_INT", function() {
            expect(_.signedInt(_.MAX_SIGNED_INT + 1)).to.be(_.MAX_SIGNED_INT);
        });
    });
    
    describe("#signedInt with Formatter.util.number.MIN_SIGNED_INT - 1", function() {
        it("should return Formatter.util.number.MIN_SIGNED_INT", function() {
            expect(_.signedInt(_.MIN_SIGNED_INT - 1)).to.be(_.MIN_SIGNED_INT);
        });
    });
});

describe('Formatter.util.date -', function() {
    var mondayFirstCulture,
        sundayFirstCulture;
        
    before(function() {
        _ = pat.Formatter.util.date;
        mondayFirstCulture = { firstDayOfWeek: 1 };
        sundayFirstCulture = { firstDayOfWeek: 0 };
    });

    describe("#timestamp(new Date())", function() {
        it("should return the UNIX timestamp for the given date", function() {
            var d = new Date();
            expect(_.timestamp(d)).to.be(Math.floor(d.valueOf() / 1000));
        });
    });
    
    describe("#daysOfMonth with Date 2012-02-01T00:00Z", function() {
        it("should return 29", function() {
            expect(_.daysOfMonth(new Date('2012-02-01T00:00Z'))).to.be(29);
        });
    });
    
    describe("#daysOfYear with Date 2012-01-01T00:00Z", function() {
        it("should return 366", function() {
            expect(_.daysOfYear(new Date('2012-01-01T00:00Z'))).to.be(366);
        });
    });
    
    describe("#daysOfYear with 2012)", function() {
        it("should return 366", function() {
            expect(_.daysOfYear(2012)).to.be(366);
        });
    });
    
    describe("#isLeapYear with Date 2012-01-01T00:00Z", function() {
        it("should return true", function() {
            expect(_.isLeapYear(new Date('2012-01-01T00:00Z'))).to.be(true);
        });
    });
    
    describe("#isLeapYear with 2012", function() {
        it("should return true", function() {
            expect(_.isLeapYear(2012)).to.be(true);
        });
    });
    
    describe("#isLeapYear with 2100", function() {
        it("should return true", function() {
            expect(_.isLeapYear(2100)).to.be(false);
        });
    });
    
    describe("#isLeapYear with 2000", function() {
        it("should return true", function() {
            expect(_.isLeapYear(2000)).to.be(true);
        });
    });
    
    // Monday date, culture with Monday as first day
    describe("#dayOfWeek with Date 2012-07-02T00:00Z and a culture with Monday as first weekday", function() {
        it("should return 0", function() {
            expect(_.dayOfWeek(new Date('2012-07-02T00:00Z'), mondayFirstCulture)).to.be(0);
        });
    });
    
    // Monday date, culture with Sunday as first day
    describe("#dayOfWeek with Date 2012-07-02T00:00Z and a culture with Sunday as first weekday", function() {
        it("should return 1", function() {
            expect(_.dayOfWeek(new Date('2012-07-02T00:00Z'), sundayFirstCulture)).to.be(1);
        });
    });
    
    describe("#dayOfYear with Date 2012-01-01T00:00Z", function() {
        it("should return 1", function() {
            expect(_.dayOfYear(new Date('2012-01-01T00:00Z'))).to.be(1);
        });
    });
    
    describe("#dayOfYear with Date 2012-03-01T00:00Z", function() {
        it("should return 61", function() {
            expect(_.dayOfYear(new Date('2012-03-01T00:00Z'))).to.be(61);
        });
    });
    
    describe("#dayOfYear with Date 2012-12-31T00:00Z", function() {
        it("should return 366", function() {
            expect(_.dayOfYear(new Date('2012-12-31T00:00Z'))).to.be(366);
        });
    });
    
    describe("#nthDayOfWeek with Date 2012-07-04T00:00Z and a culture with Monday as first weekday", function() {
        var date;
        beforeEach(function() {
            date = new Date('2012-07-04T00:00Z');
        });
        it("should return the Date 2012-07-02T00:00Z for n=0", function() {
            var r = _.nthDayOfWeek(date, mondayFirstCulture, 0);
            expect(r instanceof Date).to.be.ok();
            expect(r.valueOf()).to.be(Date.UTC(2012, 6, 2));
        });
        it("should return the Date 2012-07-08T00:00Z for n=6", function() {
            var r = _.nthDayOfWeek(date, mondayFirstCulture, 6);
            expect(r instanceof Date).to.be.ok();
            expect(r.valueOf()).to.be(Date.UTC(2012, 6, 8));
        });
    });
    
    describe("#firstDayOfMonth with Date 2012-07-15T00:00Z", function() {
        it("should return the Date 2012-07-01T00:00Z", function() {
            var r = _.firstDayOfMonth(new Date('2012-07-15T00:00Z'));
            expect(r instanceof Date).to.be.ok();
            expect(r.valueOf()).to.be(Date.UTC(2012, 6, 1));
        });
    });
    
    describe("#lastDayOfMonth with Date 2012-07-15T00:00Z", function() {
        it("should return the Date 2012-07-31T00:00Z", function() {
            var r = _.lastDayOfMonth(new Date('2012-07-15T00:00Z'));
            expect(r instanceof Date).to.be.ok();
            expect(r.valueOf()).to.be(Date.UTC(2012, 6, 31));
        });
    });
    
    describe("#firstDayOfYear with Date 2012-07-15T00:00Z", function() {
        it("should return the Date 2012-01-01T00:00Z", function() {
            var r = _.firstDayOfYear(new Date('2012-07-15T00:00Z'));
            expect(r instanceof Date).to.be.ok();
            expect(r.valueOf()).to.be(Date.UTC(2012, 0, 1));
        });
    });
    
    describe("#firstDayOfYear with Number 2012", function() {
        it("should return the Date 2012-07-01T00:00Z", function() {
            var r = _.firstDayOfYear(2012);
            expect(r instanceof Date).to.be.ok();
            expect(r.valueOf()).to.be(Date.UTC(2012, 0, 1));
        });
    });
    
    describe("#lastDayOfYear with Date 2012-07-15T00:00Z", function() {
        it("should return the Date 2012-12-31T00:00Z", function() {
            var r = _.lastDayOfYear(new Date('2012-07-15T00:00Z'));
            expect(r instanceof Date).to.be.ok();
            expect(r.valueOf()).to.be(Date.UTC(2012, 11, 31));
        });
    });
    
    describe("#lastDayOfYear with Number 2012", function() {
        it("should return the Date 2012-12-31T00:00Z", function() {
            var r = _.lastDayOfYear(2012);
            expect(r instanceof Date).to.be.ok();
            expect(r.valueOf()).to.be(Date.UTC(2012, 11, 31));
        });
    });
    
    describe("#isoWeek with Date 2012-01-01T00:00Z", function() {
        it("should return 52", function() {
            expect(_.isoWeek(new Date('2012-01-01T00:00Z'))).to.be(52);
        });
    });
    
    describe("#isoWeek with Date 2012-01-02T00:00Z", function() {
        it("should return 1", function() {
            expect(_.isoWeek(new Date('2012-01-02T00:00Z'))).to.be(1);
        });
    });
    
    describe("#centuries with Date 2012-01-01T00:00Z", function() {
        it("should return 21", function() {
            expect(_.century(new Date('2012-01-01T00:00Z'))).to.be(21);
        });
    });
    
    describe("#centuries with Date 2012-01-01T00:00Z", function() {
        it("should return 20", function() {
            expect(_.pastCenturies(new Date('2012-01-01T00:00Z'))).to.be(20);
        });
    });
    
    describe("#isAM with Date 2012-01-01T00:00Z", function() {
        it("should return true", function() {
            expect(_.isAM(new Date('2012-01-01T00:00Z'))).to.be(true);
        });
    });
    
    describe("#isAM with Date 2012-01-01T11:59:59.999Z", function() {
        it("should return true", function() {
            expect(_.isAM(new Date('2012-01-01T11:59:59.999Z'))).to.be(true);
        });
    });
    
    describe("#isAM with Date 2012-01-01T12:00Z", function() {
        it("should return false", function() {
            expect(_.isAM(new Date('2012-01-01T12:00Z'))).to.be(false);
        });
    });
    
    describe("#isPM with Date 2012-01-01T12:00Z", function() {
        it("should return true", function() {
            expect(_.isPM(new Date('2012-01-01T12:00Z'))).to.be(true);
        });
    });
    
    describe("#isPM with Date 2012-01-01T23:59:59.999Z", function() {
        it("should return true", function() {
            expect(_.isPM(new Date('2012-01-01T23:59:59.999Z'))).to.be(true);
        });
    });
    
    describe("#isPM with Date 2012-01-01T00:00Z", function() {
        it("should return false", function() {
            expect(_.isPM(new Date('2012-01-01T00:00Z'))).to.be(false);
        });
    });
});

describe('Formatter.number -', function() {
    
    before(function() {
        _ = pat.Formatter.number;
    });
    
    //toDecimal
    
    describe("#toDecimal with 0", function() {
        it("should return '0'", function() {
            expect(_.toDecimal(0)).to.be('0');
        });
    });
    
    describe("#toDecimal with 1", function() {
        it("should return '1'", function() {
            expect(_.toDecimal(1)).to.be('1');
        });
    });
    
    describe("#toDecimal with -1", function() {
        it("should return '-1'", function() {
            expect(_.toDecimal(-1)).to.be('-1');
        });
    });
    
    describe("#toDecimal with 0.1", function() {
        it("should return '0.1'", function() {
            expect(_.toDecimal(0.1)).to.be('0.1');
        });
    });
    
    describe("#toDecimal with 1.1", function() {
        it("should return '1.1'", function() {
            expect(_.toDecimal(1.1)).to.be('1.1');
        });
    });
    
    describe("#toDecimal with -1.1", function() {
        it("should return '-1.1'", function() {
            expect(_.toDecimal(-1.1)).to.be('-1.1');
        });
    });
    
    describe("#toDecimal with 1e25", function() {
        it("should return '10000000000000000000000000'", function() {
            expect(_.toDecimal(1e25)).to.be('10000000000000000000000000');
        });
    });
    
    describe("#toDecimal with -1e25", function() {
        it("should return '-10000000000000000000000000'", function() {
            expect(_.toDecimal(-1e25)).to.be('-10000000000000000000000000');
        });
    });
    
    describe("#toDecimal with 1e-25", function() {
        it("should return '0.0000000000000000000000001'", function() {
            expect(_.toDecimal(1e-25)).to.be('0.0000000000000000000000001');
        });
    });
    
    describe("#toDecimal with -1e-25", function() {
        it("should return '-0.0000000000000000000000001'", function() {
            expect(_.toDecimal(-1e-25)).to.be('-0.0000000000000000000000001');
        });
    });
    
    describe("#toDecimal with 12222222222e-10", function() {
        it("should return '1.2222222222'", function() {
            expect(_.toDecimal(12222222222e-10)).to.be('1.2222222222');
        });
    });
    
    describe("#toDecimal with 122222222223e-10", function() {
        it("should return '12.2222222223'", function() {
            expect(_.toDecimal(122222222223e-10)).to.be('12.2222222223');
        });
    });
    
    //JavaScript (IEEE 754 double precision) guarantees precision of 15 fractional
    //digits (up to max. 17 digits for some values)
    
    //15 fractional mantissa digits
    describe("#toDecimal with 1.222222222233333e25", function() {
        it("should return '12222222222333330000000000'", function() {
            expect(_.toDecimal(1.222222222233333e25)).to.be('12222222222333330000000000');
        });
    });
    
    //more than 17 fractional mantissa digits
    describe("#toDecimal with 1.2222222222333333333344444e25", function() {
        var n = String(1.2222222222333333333344444),
            i;
        n = n.substr(2); //remove comma
        for (i = 25 - n.length; i > 0; i--) { n = n + '0'; }
        n = '1' + n;
        it("should return '" + n + "'", function() {
            expect(_.toDecimal(1.2222222222333333333344444e25)).to.be(n);
        });
    });
    
    //precision
    
    describe("#toDecimal with 1.5 and precision 0", function() {
        it("should return '2'", function() {
            expect(_.toDecimal(1.5, {
                precision: 0
            })).to.be('2');
        });
    });
    
    describe("#toDecimal with 0.1 and precision 2", function() {
        it("should return '0.10'", function() {
            expect(_.toDecimal(0.1, {
                precision: 2
            })).to.be('0.10');
        });
    });
    
    describe("#toDecimal with 1.12345 and precision 2", function() {
        it("should return '1.12'", function() {
            expect(_.toDecimal(1.12345, {
                precision: 2
            })).to.be('1.12');
        });
    });
    
    //signed zero
    
    describe("#toDecimal with -0 and considerSignedZero = true", function() {
        it("should return '-0'", function() {
            expect(_.toDecimal(-0, {
                considerZeroSign: true
            })).to.be('-0');
        });
    });
    
    describe("#toDecimal with +0 and considerSignedZero = true", function() {
        it("should return '0'", function() {
            expect(_.toDecimal(+0, {
                considerZeroSign: true
            })).to.be('0');
        });
    });
    
    //toScientific
    
    describe("#toScientific with 0", function() {
        it("should return '0e+0'", function() {
            expect(_.toScientific(0)).to.be('0e+0');
        });
    });
    
    describe("#toScientific with 1", function() {
        it("should return '1e+0'", function() {
            expect(_.toScientific(1)).to.be('1e+0');
        });
    });
    
    describe("#toScientific with -1", function() {
        it("should return '-1e+0'", function() {
            expect(_.toScientific(-1)).to.be('-1e+0');
        });
    });
        
    describe("#toScientific with 0.1", function() {
        it("should return '1e-1'", function() {
            expect(_.toScientific(0.1)).to.be('1e-1');
        });
    });
    
    describe("#toScientific with -0.1", function() {
        it("should return '-1e-1'", function() {
            expect(_.toScientific(-0.1)).to.be('-1e-1');
        });
    });
    
    describe("#toScientific with 10000000000", function() {
        it("should return '1e+10'", function() {
            expect(_.toScientific(10000000000)).to.be('1e+10');
        });
    });
    
    describe("#toScientific with 15.1 and precision = 0", function() {
        it("should return '2e+1'", function() {
            expect(_.toScientific(15.1, {
                precision: 0
            })).to.be('2e+1');
        });
    });
    
    describe("#toScientific with 12.34 and precision = 2", function() {
        it("should return '1.23e+1'", function() {
            expect(_.toScientific(12.34, {
                precision: 2
            })).to.be('1.23e+1');
        });
    });
    
    describe("#toScientific with 12.35 and precision = 2", function() {
        it("should return '1.24e+1'", function() {
            expect(_.toScientific(12.35, {
                precision: 2
            })).to.be('1.24e+1');
        });
    });
    
    describe("#toScientific with 1 and exponent min width set to 2", function() {
        it("should return '1e+00'", function() {
            expect(_.toScientific(1, {
                expMinWidth: 2
            })).to.be('1e+00');
        });
    });
    
    describe("#toScientific with 1, exponent min width 1, and upperCase flag set", function() {
        it("should return '1E+0'", function() {
            expect(_.toScientific(1, {
                expMinWidth: 1,
                upperCase: true
            })).to.be('1E+0');
        });
    });
    
    describe("#toScientific with 1400, exponent min width 2, and precision 6", function() {
        it("should return '1.400000e+03'", function() {
            expect(_.toScientific(1400, {
                expMinWidth: 2,
                precision: 6
            })).to.be('1.400000e+03');
        });
    });
    
    //signed zero
    
    describe("#toScientific with -0 and considerSignedZero = true", function() {
        it("should return '-0e+0'", function() {
            expect(_.toScientific(-0, {
                considerZeroSign: true
            })).to.be('-0e+0');
        });
    });
    
    describe("#toScientific with +0 and considerSignedZero = true", function() {
        it("should return '0e+0'", function() {
            expect(_.toScientific(+0, {
                considerZeroSign: true
            })).to.be('0e+0');
        });
    });
    
    //toHexExp
    
    describe("#toHexExp with NaN", function() {
        it("should return 'NaN'", function() {
            expect(_.toHexExp(NaN)).to.be('NaN');
        });
    });
    
    describe("#toHexExp with Infinity", function() {
        it("should return 'Infinity'", function() {
            expect(_.toHexExp(Infinity)).to.be('Infinity');
        });
    });
    
    describe("#toHexExp with -Infinity", function() {
        it("should return '-Infinity'", function() {
            expect(_.toHexExp(-Infinity)).to.be('-Infinity');
        });
    });
    
    describe("#toHexExp with 0", function() {
        it("should return '0x0.0p0'", function() {
            expect(_.toHexExp(0)).to.be('0x0.0p0');
        });
    });
    
    describe("#toHexExp with -0", function() {
        it("should return '-0x0.0p0'", function() {
            expect(_.toHexExp(-0)).to.be('-0x0.0p0');
        });
    });
    
    describe("#toHexExp with 1", function() {
        it("should return '0x1.0p0'", function() {
            expect(_.toHexExp(1)).to.be('0x1.0p0');
        });
    });
    
    describe("#toHexExp with -1", function() {
        it("should return '-0x1.0p0'", function() {
            expect(_.toHexExp(-1)).to.be('-0x1.0p0');
        });
    });
    
    describe("#toHexExp with 1.4", function() {
        it("should return '0x1.6666666666666p0'", function() {
            expect(_.toHexExp(1.4)).to.be('0x1.6666666666666p0');
        });
    });
    
    describe("#toHexExp with -1.4", function() {
        it("should return '-0x1.6666666666666p0'", function() {
            expect(_.toHexExp(-1.4)).to.be('-0x1.6666666666666p0');
        });
    });
    
    describe("#toHexExp with 0.1", function() {
        it("should return '0x1.999999999999ap-4'", function() {
            expect(_.toHexExp(0.1)).to.be('0x1.999999999999ap-4');
        });
    });
    
    describe("#toHexExp with -0.1", function() {
        it("should return '-0x1.999999999999ap-4'", function() {
            expect(_.toHexExp(-0.1)).to.be('-0x1.999999999999ap-4');
        });
    });
    
    describe("#toHexExp with 123.123", function() {
        it("should return '0x1.ec7df3b645a1dp6'", function() {
            expect(_.toHexExp(123.123)).to.be('0x1.ec7df3b645a1dp6');
        });
    });
    
    describe("#toHexExp with -123.123", function() {
        it("should return '-0x1.ec7df3b645a1dp6'", function() {
            expect(_.toHexExp(-123.123)).to.be('-0x1.ec7df3b645a1dp6');
        });
    });
    
    //smallest normalized number
    
    describe("#toHexExp with 2^-1022", function() {
        it("should return '0x1.0p-1022'", function() {
            expect(_.toHexExp(Math.pow(2, -1022))).to.be('0x1.0p-1022');
        });
    });
    
    //largest denormalized number
    
    describe("#toHexExp with 2^-1023", function() {
        it("should return '0x0.8p-1022'", function() {
            expect(_.toHexExp(Math.pow(2, -1023))).to.be('0x0.8p-1022');
        });
    });
});

describe('Formatter.date -', function() {
    var deAT = {
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
    };
    
    before(function() {
        _ = pat.Formatter.date;
    });
    
    describe("#year with Date 2012-01-01T00:00Z", function() {
        it("should return '2012'", function() {
            expect(_.year(new Date('2012-01-01T00:00Z'))).to.be('2012');
        });
    });

    describe("#year with Date 2012-01-01T00:00Z and maxDigits=3", function() {
        it("should return '12'", function() {
            expect(_.year(new Date('2012-01-01T00:00Z'), {
                maxDigits: 3
            })).to.be('12');
        });
    });

    describe("#year with Date 2012-01-01T00:00Z, maxDigits=3, and leadingZeros=true", function() {
        it("should return '012'", function() {
            expect(_.year(new Date('2012-01-01T00:00Z'), {
                maxDigits: 3,
                leadingZeros: true
            })).to.be('012');
        });
    });

    describe("#year with Date 2000-01-01T00:00Z and maxDigits=3", function() {
        it("should return '0'", function() {
            expect(_.year(new Date('2000-01-01T00:00Z'), {
                maxDigits: 3
            })).to.be('0');
        });
    });

    describe("#year with Date 2000-01-01T00:00Z and maxDigits=2, and leadingZeros=true", function() {
        it("should return '00'", function() {
            expect(_.year(new Date('2000-01-01T00:00Z'), {
                maxDigits: 2,
                leadingZeros: true
            })).to.be('00');
        });
    });

    describe("#year with Date 2012-01-01T00:00Z, maxDigits=5", function() {
        it("should return '2012'", function() {
            expect(_.year(new Date('2012-01-01T00:00Z'), {
                maxDigits: 5
            })).to.be('2012');
        });
    });

    describe("#year with Date 2012-01-01T00:00Z, maxDigits=5, and leadingZeros=true", function() {
        it("should return '02012'", function() {
            expect(_.year(new Date('2012-01-01T00:00Z'), {
                maxDigits: 5,
                leadingZeros: true
            })).to.be('02012');
        });
    });
    
    describe("#year with Date BC 2012-01-01T00:00Z", function() {
        it("should return '-2012'", function() {
            expect(_.year(new Date(Date.UTC(-2012, 1)))).to.be('-2012');
        });
    });

    describe("#year with Date BC 2012-01-01T00:00Z and maxDigits=3", function() {
        it("should return '-12'", function() {
            expect(_.year(new Date(Date.UTC(-2012, 1)), {
                maxDigits: 3
            })).to.be('-12');
        });
    });

    describe("#year with Date BC 2012-01-01T00:00Z, maxDigits=3, and leadingZeros=true", function() {
        it("should return '-012'", function() {
            expect(_.year(new Date(Date.UTC(-2012, 1)), {
                maxDigits: 3,
                leadingZeros: true
            })).to.be('-012');
        });
    });

    describe("#year with Date BC 2000-01-01T00:00Z and maxDigits=3", function() {
        it("should return '-0'", function() {
            expect(_.year(new Date(Date.UTC(-2000, 1)), {
                maxDigits: 3
            })).to.be('-0');
        });
    });

    describe("#year with Date BC 2000-01-01T00:00Z and maxDigits=2, and leadingZeros=true", function() {
        it("should return '-00'", function() {
            expect(_.year(new Date(Date.UTC(-2000, 1)), {
                maxDigits: 2,
                leadingZeros: true
            })).to.be('-00');
        });
    });

    describe("#year with Date BC 2012-01-01T00:00Z, maxDigits=5", function() {
        it("should return '-2012'", function() {
            expect(_.year(new Date(Date.UTC(-2012, 1)), {
                maxDigits: 5
            })).to.be('-2012');
        });
    });

    describe("#year with Date BC 2012-01-01T00:00Z, maxDigits=5, and leadingZeros=true", function() {
        it("should return '-02012'", function() {
            expect(_.year(new Date(Date.UTC(-2012, 1)), {
                maxDigits: 5,
                leadingZeros: true
            })).to.be('-02012');
        });
    });

    describe("#year with Date BC 2012-01-01T00:00Z, and bcPrefix='B.C. '", function() {
        it("should return 'B.C. 2012'", function() {
            expect(_.year(new Date(Date.UTC(-2012, 1)), {
                bcPrefix: 'B.C. '
            })).to.be('B.C. 2012');
        });
    });

    describe("#year with Date BC 2012-01-01T00:00Z, and bcPostfix=' B.C.'", function() {
        it("should return '2012 B.C.'", function() {
            expect(_.year(new Date(Date.UTC(-2012, 1)), {
                bcPostfix: ' B.C.'
            })).to.be('2012 B.C.');
        });
    });
    
    describe("#dayOfYear with Date 2012-01-01T00:00Z", function() {
        it("should return '1'", function() {
            expect(_.dayOfYear(new Date('2012-01-01T00:00Z'))).to.be('1');
        });
    });
    
    describe("#dayOfYear with Date 2012-01-01T00:00Z and leading zeros", function() {
        it("should return '001'", function() {
            expect(_.dayOfYear(new Date('2012-01-01T00:00Z'), true)).to.be('001');
        });
    });
    
    describe("#month with Date 2012-01-01T00:00Z", function() {
        it("should return '1'", function() {
            expect(_.month(new Date('2012-01-01T00:00Z'))).to.be('1');
        });
    });
    
    describe("#month with Date 2012-01-01T00:00Z and leading zero", function() {
        it("should return '01'", function() {
            expect(_.month(new Date('2012-01-01T00:00Z'), true)).to.be('01');
        });
    });
    
    describe("#dayOfMonth with Date 2012-01-08T00:00Z", function() {
        it("should return '8'", function() {
            expect(_.dayOfMonth(new Date('2012-01-08T00:00Z'))).to.be('8');
        });
    });
    
    describe("#dayOfMonth with Date 2012-01-08T00:00Z and leading zero", function() {
        it("should return '08'", function() {
            expect(_.dayOfMonth(new Date('2012-01-08T00:00Z'), true)).to.be('08');
        });
    });
    
    describe("#monthName with Date 2012-01-01T00:00Z", function() {
        it("should return '" + deAT.months[0] + "'", function() {
            expect(_.monthName(new Date('2012-01-01T00:00Z'), deAT))
                .to.be(deAT.months[0]);
        });
    });
    
    describe("#abbreviatedMonthName with Date 2012-01-01T00:00Z", function() {
        it("should return '" + deAT.monthsAbbr[0] + "'", function() {
            expect(_.abbreviatedMonthName(new Date('2012-01-01T00:00Z'), deAT))
                .to.be(deAT.monthsAbbr[0]);
        });
    });
    
    describe("#weekdayName with Date 2012-01-01T00:00Z", function() {
        it("should return '" + deAT.weekdays[0] + "'", function() {
            expect(_.weekdayName(new Date('2012-01-01T00:00Z'), deAT))
                .to.be(deAT.weekdays[0]);
        });
    });
    
    describe("#abbreviatedWeekdayName with Date 2012-01-01T00:00Z", function() {
        it("should return '" + deAT.weekdaysAbbr[0] + "'", function() {
            expect(_.abbreviatedWeekdayName(new Date('2012-01-01T00:00Z'), deAT))
                .to.be(deAT.weekdaysAbbr[0]);
        });
    });
    
    describe("#hours with Date 2012-01-01T08:00Z", function() {
        it("should return '8'", function() {
            expect(_.hours(new Date('2012-01-01T08:00Z'))).to.be('8');
        });
        it("should return '08' when called with leadingZero=true", function() {
            expect(_.hours(new Date('2012-01-01T08:00Z'), true)).to.be('08');
        });
    });
    
    describe("#hours with Date 2012-01-01T20:00Z", function() {
        it("should return '20'", function() {
            expect(_.hours(new Date('2012-01-01T20:00Z'))).to.be('20');
        });
        it("should return '8' when called with leadingZero=false, and h12=true", function() {
            expect(_.hours(new Date('2012-01-01T20:00Z'), false, true)).to.be('8');
        });
        it("should return '08' when called with leadingZero=true, and h12=true", function() {
            expect(_.hours(new Date('2012-01-01T20:00Z'), true, true)).to.be('08');
        });
    });
    
    describe("#minutes with Date 2012-01-01T00:05Z", function() {
        it("should return '5'", function() {
            expect(_.minutes(new Date('2012-01-01T00:05Z'))).to.be('5');
        });
        it("should return '05' when called with leadingZero=true", function() {
            expect(_.minutes(new Date('2012-01-01T00:05Z'), true)).to.be('05');
        });
    });
    
    describe("#seconds with Date 2012-01-01T00:00:05Z", function() {
        it("should return '5'", function() {
            expect(_.seconds(new Date('2012-01-01T00:00:05Z'))).to.be('5');
        });
        it("should return '05' when called with leadingZero=true", function() {
            expect(_.seconds(new Date('2012-01-01T00:00:05Z'), true)).to.be('05');
        });
    });
    
    describe("#milliseconds with Date 2012-01-01T00:00:00.010Z", function() {
        it("should return '10'", function() {
            expect(_.milliseconds(new Date('2012-01-01T00:00:00.010Z')))
                .to.be('10');
        });
        it("should return '010' when called with leadingZero=true", function() {
            expect(_.milliseconds(new Date('2012-01-01T00:00:00.010Z'), true))
                .to.be('010');
        });
    });
    
    describe("#timeDesignator with Date 2012-01-01T00:00Z", function() {
        it("should return '" + deAT.amToken + "'", function() {
            expect(_.timeDesignator(new Date('2012-01-01T00:00Z'), deAT))
                .to.be(deAT.amToken);
        });
    });
    
    describe("#timeDesignator with Date 2012-01-01T12:00Z", function() {
        it("should return '" + deAT.pmToken + "'", function() {
            expect(_.timeDesignator(new Date('2012-01-01T12:00Z'), deAT))
                .to.be(deAT.pmToken);
        });
    });
    
    describe("#timezoneOffset with Date 2012-01-01T20:15", function() {
        var date,
            offset;
        beforeEach(function() {
            date = new Date('2012-01-01T20:15');
            offset = _.timezoneOffset(date);
        });
        it("should return a string with length 5", function() {
            expect(offset).to.be.a('string');
            expect(offset).to.have.length(5);
        });
        it("should return a string with the first character equal to '+' or '-'", function() {
            expect(offset.charAt(0) === '+' || offset.charAt(0) === '-').to.be.ok();
        });
        it("should return a string where the first character is the inverted sign " +
                "returned by Date.getTimezoneOffset for the specified date", function() {
            var sign = (date.getTimezoneOffset() < 0 ? '+' : '-');
            expect(offset.charAt(0)).to.be(sign);
        });
        it("should return a string with the first two characters after the sign " +
                "equal to the hours of Date.getTimezoneOffset for the specified date", function() {
            var hours = String(Math.floor(date.getTimezoneOffset() / -60));
            if (hours.length === 1) { hours = '0' + hours; }
            expect(offset.substr(1, 2)).to.be(hours);
        });
        it("should return a string with the last two characters equal to the " +
                "minutes of Date.getTimezoneOffset for the specified date", function() {
            var min = String(date.getTimezoneOffset() % -60);
            if (min.length === 1) { min = '0' + min; }
            expect(offset.substr(3)).to.be(min);
        });
    });
    
    describe("#abbreviatedTimezone with Date 'Mon Jul 09 2012 17:29:37 GMT+0200 (CEST)'", function() {
        it("should return 'CEST'", function() {
            expect(_.abbreviatedTimezone(new Date(
                'Mon Jul 09 2012 17:29:37 GMT+0200 (CEST)'))).to.be('CEST');
        });
    });
    
    describe("#time with Date 2012-01-01T05:30Z", function() {
        var date;
        beforeEach(function() {
            date = new Date('2012-01-01T05:30Z');
        });
        it("should return '5:30:0'", function() {
            expect(_.time(date)).to.be('5:30:0');
        });
        it("should return '05:30:00'", function() {
            expect(_.time(date, true)).to.be('05:30:00');
        });
    });
    
    describe("#time12 with Date 2012-01-01T20:15Z", function() {
        var date;
        beforeEach(function() {
            date = new Date('2012-01-01T20:15Z');
        });
        it("should return '8:15:0'", function() {
            expect(_.time12(date)).to.be('8:15:0');
        });
        it("should return '08:15:00' when called with leadingZeros=true", function() {
            expect(_.time12(date, true)).to.be('08:15:00');
        });
        it("should return '08:15:00 " + deAT.pmToken + "'", function() {
            expect(_.time12(date, true, true, deAT)).to.be('08:15:00 ' + deAT.pmToken);
        });
    });
});

describe("Formatter -", function() {
    var loadModuleSpy,
        nonDefaultCulture = 'deDE';
    
    before(function() {
        _ = pat.Formatter;
        loadModuleSpy = sinon.spy(pat._private, 'loadModule');
    });
    
    describe("The Formatter's initial default options", function() {
        it("should have the 'flavorId' property set to 'java'", function() {
            expect(_.defaultOptions).to.be.an('object');
            expect(_.defaultOptions).to.have.property('flavorId', 'java');
        });
        it("should have the 'cultureId' property set to 'enUS'", function() {
            expect(_.defaultOptions).to.be.an('object');
            expect(_.defaultOptions).to.have.property('cultureId', 'enUS');
        });
        it("should have the 'lineSeparator' property set to '\\n'", function() {
            expect(_.defaultOptions).to.be.an('object');
            expect(_.defaultOptions).to.have.property('lineSeparator', '\n');
        });
    });
    
    describe("#options()", function() {
        it("should return an object", function() {
            expect(_.options()).to.be.an('object');
        });
        it("should have a function property named 'flavor'", function() {
            expect(_.options()).to.have.property('flavor');
        });
        it("should have a function property named 'culture'", function() {
            expect(_.options()).to.have.property('culture');
        });
        it("should have a function property named 'lineSeparator'", function() {
            expect(_.options()).to.have.property('lineSeparator');            
        });
    });
    
    describe("Resetting the Formatter and setting a culture not yet loaded", function() {
        beforeEach(function() {
            _.reset();
            loadModuleSpy.reset();
            _.options({ cultureId: nonDefaultCulture });
        });
        it("should call pat._private.loadModule once after resetting the Formatter", function() {
            expect(loadModuleSpy).was.calledOnce();
        });
        it("should result in the culture cache to have a property named after " +
                "the default culture", function() {
            expect(pat._private.cultures[_.defaultOptions.cultureId])
                .not.to.be(undefined);
        });
        it("should result in the culture cache to have a property named after " +
                "the current culture", function() {
            expect(pat._private.cultures[_.options().cultureId()])
                .not.to.be(undefined);
        });
    });
    
    describe("Setting the initial flavor again", function() {
        it("should not load the module again", function() {
            _.reset();
            loadModuleSpy.reset();
            _.options({ flavor: _.defaultOptions.flavor });
            expect(loadModuleSpy).was.notCalled();
        });
    });
    
    describe("Setting the initial culture again", function() {
        it("should not load the module again", function() {
            _.reset();
            loadModuleSpy.reset();
            _.options({ culture: _.defaultOptions.culture });
            expect(loadModuleSpy).was.notCalled();
        });
    });
    
    describe("Overwriting the initial default formatter options with an " +
            "alternative culture", function() {
        it("should result in a new formatter to have that default culture", function() {
            var f;
            _.reset();
            _.defaultOptions.culture = nonDefaultCulture;
            f = new pat.Formatter();
            expect(f.options().culture()).to.be(nonDefaultCulture);
        });
    });
});

/*
 * IMPORTANT:
 * 
 * This test case requires mocha to be executed with the option
 * "--globals module,define" in order to prevent global leak
 * detection for the globals temporarily changed by this test case.
 */
describe("Integration as an AMD module", function() {
    var curRequire,
        curDefine,
        curModule;
        
    beforeEach(function() {
        //save node state
        curModule = module;
        curRequire = require;
        if (typeof define !== 'undefined') { curDefine = define; }
        //simulate AMD context
        module = null;
        define = requirejs.define;
        //flush module cashes
        pat = require('../lib/pat.js');//pat.Formatter.reset();
        pat._private.cultures = {};
        pat._private.flavors = {};
    });
    
    afterEach(function() {
        if (curDefine !== undefined) { define = curDefine; }
        require = curRequire;
        module = curModule;
    });
    
    function checkInitState(pat) {
        var defOpt,
            opt,
            cult,
            flav,
            priv;
        expect(pat).to.be.an('object');
        //pat._private
        expect(pat._private).to.be.an('object');
        priv = pat._private;
        expect(priv.initialDefaultFormatterOptions).to.be.a('function');
        defOpt = priv.initialDefaultFormatterOptions();
        expect(defOpt).to.be.an('object');
        expect(defOpt).to.have.property('cultureId');
        expect(defOpt).to.have.property('flavorId');
        //pat.Formatter
        expect(pat.Formatter).to.be.a('function');
        expect(pat.Formatter.options).to.be.a('function');
        opt = pat.Formatter.options();
        expect(opt).to.be.an('object');
        //culture
        expect(opt.cultureId).to.be.a('function');
        expect(opt.cultureId()).to.be(defOpt.cultureId);
        expect(opt.culture).to.be.a('function');
        expect(opt.culture()).to.be.an('object');
        expect(opt.culture()).to.have.property('id', opt.cultureId());
        //flavor
        expect(opt.flavorId).to.be.a('function');
        expect(opt.flavorId()).to.be(defOpt.flavorId);
        expect(opt.flavor).to.be.a('function');
        expect(opt.flavor()).to.be.an('object');
        //pat._private
        expect(priv.cultures).to.be.an('object');
        expect(priv.cultures).to.have.property(defOpt.cultureId, opt.culture());
        expect(priv.flavors).to.be.an('object');
        expect(priv.flavors).to.have.property(defOpt.flavorId, opt.flavor());
    }
    
    describe("using requirejs.config with baseUrl", function() {
        it ("should load pat", function() {
            requirejs.config({
                baseUrl: '../lib'
            });
            requirejs(['pat'], function(pat) {
                checkInitState(pat);
            });
        });
    });
    
    describe("using requirejs.config with baseUrl and paths settings", function() {
        it ("should load pat", function() {
            requirejs.config({
                baseUrl: './',
                paths: { pat: '../lib' }
            });
            requirejs(['pat/pat'], function(pat) {
                checkInitState(pat);
            });
        });
    });
});
