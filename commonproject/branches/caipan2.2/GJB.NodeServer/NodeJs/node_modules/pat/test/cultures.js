/*global require, describe, it*/
/*jslint sloppy:true nomen:true plusplus:true*/

var fs = require('fs'),
    path = require('path'),
    expect = require('expect.js'),
    Formatter = require('../lib/pat.js').Formatter,
    culturesPath = '../lib/cultures';
    
describe('Cultures path', function() {
    it('should exist', function() {
        fs.stat(culturesPath, function(err, stats) {
            if (err) { throw err; }
            expect(stats.isDirectory()).to.be.ok();
        });
    });
});

(function() {
    var i,
        module;
    function validateCultureModule(module, filename) {
        var p = path.normalize([__dirname, culturesPath, filename].join('/'));
        describe("Culture module defined in '" + p + "'", function() {
            it("should be valid", function() {
                Formatter.validateCulture(module);
            });
        });    
    }
    fs.readdir(culturesPath, function(err, files) {
        if (err) { throw err; }
        for (i = files.length - 1; i >= 0; i--) {
            module = require([culturesPath, files[i]].join('/'));
            validateCultureModule(module, files[i]);
        }
    });
}());
