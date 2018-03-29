/**
 * Culture specific information.
 * @module pat
 * @submodule cultures
 */
/**
 * German language. German culture.
 * @static
 * @class cultures.deDE
 */

/*jslint white:true sloppy:true nomen:true*/

(function(root) {
    
    var culture = {
        
        id: 'deDE',
        
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
        months: ['Januar', 'Februar', 'März', 'April', 'Mai', 'Juni', 'Juli', 'August', 'September', 'Oktober', 'November', 'Dezember'],
        monthsAbbr: ['Jan', 'Feb', 'Mrz', 'Apr', 'Mai', 'Jun', 'Jul', 'Aug', 'Sep', 'Okt', 'Nov', 'Dez'],
    
        /* Morning/afternoon tokens (e.g. 'am', 'pm') */
        amToken: 'am',
        pmToken: 'pm'
        
    };
    
    /* Node.js */
    if (typeof module !== 'undefined') {
        module.exports = culture;
    }
    /* AMD */
    else if (typeof define !== 'undefined' && define.amd) { //AMD
        define(culture);
    }
    /* Browser */
    else if (root) {
        if (!root.pat) { root.pat = {}; }
        if (!root.pat._private) { root.pat._private = {}; }
        if (!root.pat._private.cultures) {
            root.pat._private.cultures = {};
            root.pat._private.preferredCultureId = culture.id;
        }
        root.pat._private.cultures[culture.id] = culture;
    }
}(this));
